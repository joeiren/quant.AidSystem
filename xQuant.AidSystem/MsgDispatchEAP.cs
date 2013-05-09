using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Threading;

namespace xQuant.AidSystem.Communication
{
 
    public delegate void DispatchCompletedEventHandler(object sender, TransmitCompletedEventArgs e);

    public class MsgDispatchEAP
    {
        private delegate void DispatchEventHandler(MessageData message, AsyncOperation operation);
        private SendOrPostCallback onCompletedDelegate;

        private HybridDictionary _userStateToLifetime = new HybridDictionary();
        public event DispatchCompletedEventHandler DispatchCompleted;
        private ICommunicationHandler _messageHandler;
        public ICommunicationHandler MsgHandler
        {
            get 
            {
                return _messageHandler;
            }
            set 
            {
                _messageHandler = value;
            }
        }

        public MsgDispatchEAP(ICommunicationHandler handler)
        {
            onCompletedDelegate = new SendOrPostCallback(PostCompleted);
            _messageHandler = handler;
        }

        public virtual void OnDispatchMsgAsync(MessageData message, object taskid)
        {
            AsyncOperation asyncOper = AsyncOperationManager.CreateOperation(taskid);

            // Multiple threads will access the task dictionary, so it must be locked to serialize access.
            lock (_userStateToLifetime.SyncRoot)
            {
                if (_userStateToLifetime.Contains(taskid))
                {
                    throw new ArgumentException("Task ID parameter must be unique", "OnDispatchMsgAsync:taskid");
                }
                _userStateToLifetime[taskid] = asyncOper;
            }

            // Start the asynchronous operation.
            DispatchEventHandler disptchHandler = new DispatchEventHandler(OnDispatch);
            disptchHandler.BeginInvoke(message, asyncOper, null, null);

        }

        private void DispatchCompleting(MessageData responsemsg, Exception ex, bool canceled, AsyncOperation asyncOper)
        {
            if (!canceled)
            {
                lock (_userStateToLifetime.SyncRoot)
                {
                    _userStateToLifetime.Remove(asyncOper.UserSuppliedState);
                }
            }
            TransmitCompletedEventArgs eventArgs = new TransmitCompletedEventArgs(responsemsg, ex, canceled, asyncOper.UserSuppliedState);
            asyncOper.PostOperationCompleted(onCompletedDelegate, eventArgs);
        }

        private bool TaskCanceled(object taskId)
        {
            return (_userStateToLifetime[taskId] == null);
        }

        public void CancelAsync(object taskId)
        {
            AsyncOperation asyncOp = _userStateToLifetime[taskId] as AsyncOperation;
            if (asyncOp != null)
            {
                lock (_userStateToLifetime.SyncRoot)
                {
                    _userStateToLifetime.Remove(taskId);
                }
            }
        }

        #region internal Callback and Delegate methods
        private void OnDispatch(MessageData requestmsg, AsyncOperation asyncOper)
        {
            Exception ex = null;

            MessageData responseMsg = new MessageData();

            // Check that the task is still active.The operation may have been canceled before
            // the thread was scheduled.
            if (!TaskCanceled(asyncOper.UserSuppliedState))
            {
                try
                {
                    responseMsg = _messageHandler.MessageHandler(requestmsg);
                }
                catch (Exception e)
                {
                    responseMsg = requestmsg;
                    ex = e;
                }
            }

            DispatchCompleting(responseMsg, ex, TaskCanceled(asyncOper.UserSuppliedState), asyncOper);
        }

        /// <summary>
        /// Using SendOrPostCallback to finish completing
        /// </summary>
        /// <param name="eventargs"></param>
        private void PostCompleted(object eventargs)
        {
            TransmitCompletedEventArgs eventArgs = eventargs as TransmitCompletedEventArgs;
            OnDispatchCompleted(eventArgs);
        }

        private void OnDispatchCompleted(TransmitCompletedEventArgs eventargs)
        {
            if (DispatchCompleted != null)
            {
                DispatchCompleted(this, eventargs);
            }
        }
        #endregion
    }

    #region TransmitCompletedEventArgs Class
    public class TransmitCompletedEventArgs : AsyncCompletedEventArgs
    {
        private MessageData _messageData;
        public MessageData MessageData
        {
            get
            {
                return _messageData;
            }
        }

        public TransmitCompletedEventArgs(MessageData message, Exception ex, bool canceled, object usertoken)
            : base(ex, canceled, usertoken)
        {
            _messageData = message;
        }
    }
    #endregion

}
