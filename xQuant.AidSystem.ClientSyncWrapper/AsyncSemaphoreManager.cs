using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Threading;

namespace xQuant.AidSystem.ClientSyncWrapper
{
    public sealed class AsyncSemaphoreManager
    {
        #region 属性
        private HybridDictionary _semaphoreList;
        public HybridDictionary SemaphoreList
        {
            get 
            {
                return _semaphoreList;
            }
        }
        private HybridDictionary _asyncResultList;
        public HybridDictionary AsyncResultList
        {
            get 
            {
                return _asyncResultList;
            }
        }

        #endregion
        
        public AsyncSemaphoreManager()
        {
            _semaphoreList = new HybridDictionary();
            _asyncResultList = new HybridDictionary();
        }

        #region 公开方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="endstate">信号量终止状态，true：终止(允许一个或多个等待线程继续);false:非终止(导致线程阻止);</param>
        /// <returns></returns>
        public bool InsertSemaphore(object id, bool endstate)
        {
            if (_semaphoreList == null)
            {
                _semaphoreList = new HybridDictionary();
            }
            if (_semaphoreList.Contains(id))
            {
                //xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Debug, string.Format("添加到信号量队列失败，_semaphoreList中已经存在该id={0}！", id));
                return false;
            }
            else
            {
                lock (_semaphoreList.SyncRoot)
                {
                    AutoResetEvent semaphore = new AutoResetEvent(endstate);
                    _semaphoreList.Add(id, semaphore);
                }
                return true;
            }
        }

        public bool RemoveSemaphore(object id)
        {
            if (_semaphoreList == null)
            {
                //xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Debug, "从队列中移除信号量失败，_semaphoreList=null！");
                return false;
            }
            if (_semaphoreList.Contains(id))
            {
                lock (_semaphoreList.SyncRoot)
                {
                    GetSemaphore(id).Close();
                    _semaphoreList.Remove(id);
                }
                return true;
            }
            else
            {
                //xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Debug, string.Format("从队列中移除信号量失败，_semaphoreList中没有该id={0}！", id));
                return false;
            }
        }
        
        public bool WaitSemaphore(object id, int timeout)
        {
            AutoResetEvent semaphore = GetSemaphore(id);
            if (semaphore != null)
            {
               return semaphore.WaitOne(timeout);
            }
            else
            {
                return false;
            }
        }
        
        public void ReleaseSemaphore(object id)
        {
            AutoResetEvent semaphore = GetSemaphore(id);
            if (semaphore != null)
            { 
                semaphore.Set();
            }
        }

        public bool InsertAsyncResult(object id, object data)
        {
            if (_asyncResultList == null)
            {
                _asyncResultList = new HybridDictionary();
            }
            if (_asyncResultList.Contains(id))
            {
                //xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Debug, string.Format("添加到异步结果队列失败，_asyncResultList中已经存在该id={0}！", id));
                return false;
            }
            else
            {
                lock (_asyncResultList.SyncRoot)
                {
                    _asyncResultList[id] = data;
                }
                return true;
            }
        }
        
        public bool RemoveAsyncResult(object id)
        {
            if (_asyncResultList == null)
            {
                //xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Debug, "从队列中移除异步结果对象失败，_asyncResultList=null！");
                return false;
            }
            if (_asyncResultList.Contains(id))
            {
                lock (_asyncResultList.SyncRoot)
                {
                    _asyncResultList.Remove(id);
                    return true;
                }
            }
            else
            {
                //xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Debug, string.Format("从队列中移除异步结果对象失败，_asyncResultList中没有该id={0}！", id));
                return false;
            }
        }
        
        public object GetAsyncResult(object id)
        {
            if (_asyncResultList != null && _asyncResultList.Contains(id))
            {
                return _asyncResultList[id];
            }
            else
            {
                string error = string.Format("获取异步结果队列中的对象失败，_asyncResultList中不存在该id={0}！", id);
                xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Debug, error);
                return null;
            }
        }

        public AutoResetEvent GetSemaphore(object id)
        {
            if (_semaphoreList != null && _semaphoreList.Contains(id))
            {
                return _semaphoreList[id] as AutoResetEvent;
            }
            else
            {
                string error = string.Format("获取信号量队列中的对象失败，_semaphoreList中不存在该id={0}！", id);
                xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Debug, error);
                return null;
            }
        }
        #endregion
    }
}
