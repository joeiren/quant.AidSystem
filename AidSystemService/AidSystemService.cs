using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using xQuant.MQ;
using xQuant.BizCore;
using System.Windows.Forms;
using System.Threading;
using System.Timers;
using xQuant.AidSystem.Communication;

namespace AidSystemService
{
    public partial class AidSystemService : ServiceBase
    {
        System.Timers.Timer _timer = new System.Timers.Timer();
        private const String EventSourceName = "xQuant.AidService";
        public AidSystemService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            _timer.Interval = 10000;
            _timer.Enabled = true;
            _timer.AutoReset = false;
            
        }

        protected override void OnStop()
        {
            _timer.Stop();
            _timer.Close();
        }
        private void timer_Elapsed(object source, ElapsedEventArgs e)
        {
            MQHandler handler = null;
            MQHandlerSync handlerSync = null;
            try
            {
                if (InitMQConnection())
                {
                    handler = MQHandler.GetSingleton();
                    handler.Init();
                    handlerSync = MQHandlerSync.GetSingleton();
                    handlerSync.Init();
                }
            }
            catch (Exception ex)
            {
                //CommonHelper.AddEventLog(EventSourceName, "Windows服务启动后，初始化MQ连接，MQHandler，MQHandlerSync发生异常！Message:" + ex.Message);
                xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Error, "Windows服务启动后，初始化MQ连接，MQHandler，MQHandlerSync发生异常！Message:" + ex.Message);
            }
            //try
            //{
            //    LoadResendLog resend = new LoadResendLog();
            //    if (handler != null)
            //    {
            //        if (resend.InitMQ(handler.GetMQSender(), handler.GetMQReceiver()))
            //        {
            //            resend.GetResendMessages();
            //            resend.OnSend();
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    AddEventLog("AidSystem服务启动中重发数据库中的消息发生了异常！Message:" + ex.Message);
            //}
        }
        /// <summary>
        /// 初始化MQ连接
        /// </summary>
        private bool InitMQConnection()
        {
            if (!MQConnection.Default.Connected)
            {
                try
                {
                    MQConnection.Default.Init(MQRepository.GetSingleton().MQClientIdentifier, MQRepository.GetSingleton().MQHost, MQRepository.GetSingleton().MQPort, MQRepository.GetSingleton().MQName);
                    MQConnection.Default.Open();
                }
                catch(Exception ex)
                {
                    //CommonHelper.AddEventLog(EventSourceName, "Message:" + ex.Message + "StackTrace:" + ex.StackTrace + "InnerException:" + ex.InnerException.Message + "InnerException.StackTrace:" + ex.InnerException.StackTrace);
                    xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Error, "Message:" + ex.Message + "StackTrace:" + ex.StackTrace + "InnerException:" + ex.InnerException.Message + "InnerException.StackTrace:" + ex.InnerException.StackTrace);
                }
            }
            if (!MQConnection.Default.Connected)
            {
                xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Error, "mq连接失败！");
                //CommonHelper.AddEventLog(EventSourceName, "mq连接失败！");
                return false;
            }
            return true;
        }
    }
}
