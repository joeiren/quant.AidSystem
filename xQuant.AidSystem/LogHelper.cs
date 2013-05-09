/*
* 版权所有：杭州衡泰软件
* 作    者：林尚都(linshangdou@yahoo.com.cn)
* 创建时间：2010-12-31 10:15:24
* 版 本 号：1.0.0
* 模块说明：
* ----------------------------------
* 修改记录：
* 日    期：
* 版 本 号：
* 修 改 人：
* 修改内容：
* 
*/


using System;
using System.Configuration;

namespace xQuant.AidSystem.Communication
{
    /// <summary>
    /// 日志帮助类
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// 把日志写入文件
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        public static void Write(string logLevel, string message)
        {
            try
            {
                if (logLevel == Log4.LogLevel.Debug && GlobalConfig.DebugLog)
                {
                    Log4.LogHelper.Write(logLevel, message);
                }
                else if (logLevel == Log4.LogLevel.Error && GlobalConfig.ErrorLog)
                {
                    Log4.LogHelper.Write(logLevel, message);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }

    /// <summary>
    /// web config 配置信息
    /// </summary>
    public class GlobalConfig
    {
        /// <summary>
        /// 是否写DebugLog
        /// </summary>
        public static readonly bool DebugLog = Convert.ToBoolean(ConfigurationManager.AppSettings["DebugLog"]);

        /// <summary>
        /// 是否写ErrorLog
        /// </summary>
        public static readonly bool ErrorLog = Convert.ToBoolean(ConfigurationManager.AppSettings["ErrorLog"]);
    }
}
