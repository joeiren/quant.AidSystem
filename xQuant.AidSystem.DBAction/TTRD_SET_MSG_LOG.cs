using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.DBAction
{
    public class TTRD_SET_MSG_LOG
    {
        /// <summary>
        /// 消息GUID
        /// </summary>
        public string GUID
        { get; set; }
        /// <summary>
        /// 资金系统流水号
        /// </summary>
        public string FLOW_NO
        { get; set; }
        /// <summary>
        /// 后台系统流水号
        /// </summary>
        public string HOSTFLOW_NO
        { get; set; }
        /// <summary>
        /// 平台类型
        /// </summary>
        public int PLATFORMTYPE
        { get; set; }
        /// <summary>
        /// 消息类别
        /// </summary>
        public int MSGTYPE
        { get; set; }
        /// <summary>
        /// 发送内容
        /// </summary>
        public byte[] SEND_CONTENT
        { get; set; }
        /// <summary>
        /// 接收内容
        /// </summary>
        public byte[] RECV_CONTENT
        { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public string SEND_TIME
        { get; set; }
        /// <summary>
        /// 应答时间
        /// </summary>
        public string RESP_TIME
        { get; set; }
        /// <summary>
        /// 机构号
        /// </summary>
        public string INS_ID
        { get; set; }
        /// <summary>
        /// 柜员号
        /// </summary>
        public string USER_CODE
        { get; set; }
        /// <summary>
        /// 是否多包
        /// </summary>
        public string IS_MUL_PKG
        { get; set; }
        /// <summary>
        /// 日志状态
        /// </summary>
        public int STATE
        { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ERRINFO
        { get; set; }
      
       
    }
}
