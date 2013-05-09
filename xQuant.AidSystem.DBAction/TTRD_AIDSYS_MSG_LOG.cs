using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.DBAction
{
    public class TTRD_AIDSYS_MSG_LOG
    {
        /// <summary>
        /// 消息GUID
        /// </summary>
        public string M_ID
        { get; set; }
        /// <summary>
        /// 发送内容
        /// </summary>
        public byte[] M_S_CONTENT
        { get; set; }
        /// <summary>
        /// 接收内容
        /// </summary>
        public byte[] M_R_CONTENT
        { get; set; }
        /// <summary>
        /// 柜员号
        /// </summary>
        public string U_ID
        { get; set; }
        /// <summary>
        /// 机构号
        /// </summary>
        public string I_ID
        { get; set; }
        /// <summary>
        /// 平台类型
        /// </summary>
        public int M_PLATTYPE
        { get; set; }
        /// <summary>
        /// 子包号
        /// </summary>
        public int M_SUBID
        { get; set; }
        /// <summary>
        /// 是否单包
        /// </summary>
        public string M_ISSINGLE
        { get; set; }
        /// <summary>
        /// 消息业务流水号
        /// </summary>
        public string M_SERIALNO
        { get; set; }
        /// <summary>
        /// 失败信息
        /// </summary>
        public string M_ERROR
        { get; set; }
        /// <summary>
        /// 初始发送日期
        /// </summary>
        public string M_SENDDATE
        { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string M_STATE
        { get; set; }
    }
}
