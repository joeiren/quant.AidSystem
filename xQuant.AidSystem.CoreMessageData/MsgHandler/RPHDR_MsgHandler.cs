using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class RPHDR_MsgHandler : IMessageRespHandler
    {
        public const UInt16 TOTAL_WIDTH = 36;

        #region
        /// <summary>
        /// 信息类型，3
        /// </summary>
        public String MSG_ID
        {
            get
            {
                return "REP";
            }
            set
            {

            }
        }
        /// <summary>
        /// 会话控制，1
        /// </summary>
        public String SESS_CTL
        {
            get
            {
                return "0";
            }
            set { }
        }
        /// <summary>
        /// 输出浏览控制，1
        /// </summary>
        public String BR_CTL
        {
            get
            {
                return "0";
            }
            set { }
        }
        /// <summary>
        /// 翻页模式,1
        /// </summary>
        public String PG_CTL
        {
            get
            {
                return "0";
            }
            set
            {

            }
        }
        /// <summary>
        /// 预留,3
        /// </summary>
        public String RES
        {
            get
            {
                return CommonDataHelper.SpaceString(3);
            }
            set { }
        }
        /// <summary>
        /// 此交易状态,1
        /// </summary>
        public String STATUS
        {
            get;
            set;
        }
        /// <summary>
        /// 抹账交易返回码,1
        /// </summary>
        public String REV_STS
        {
            get;
            set;
        }
        /// <summary>
        /// 被抹账交易码,6
        /// </summary>
        public String REV_BTX_ID
        {
            get
            {
                return CommonDataHelper.SpaceString(6);
            }
            set { }
        }
        /// <summary>
        /// 响应码,6
        /// </summary>
        public String RP_CDE
        {
            get;
            set;
        }
        /// <summary>
        /// 响应子码,2
        /// </summary>
        public String RPS_CDE
        {
            get
            {
                return CommonDataHelper.SpaceString(2);
            }
            set
            { }
        }
        /// <summary>
        /// 此交易流水号,11
        /// </summary>
        public String SEQ_NO
        {
            get;
            set;
        }

        #endregion
        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            if (messagebytes.Length >= TOTAL_WIDTH)
            {
                MSG_ID = CommonDataHelper.GetValueFromBytes(ref messagebytes, 3).TrimEnd();
                SESS_CTL = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                BR_CTL = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                PG_CTL = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                RES = CommonDataHelper.GetValueFromBytes(ref messagebytes, 3).TrimEnd();
                STATUS = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                REV_STS = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                REV_BTX_ID = CommonDataHelper.GetValueFromBytes(ref messagebytes, 6).TrimEnd();
                RP_CDE = CommonDataHelper.GetValueFromBytes(ref messagebytes, 6).TrimEnd();
                RPS_CDE = CommonDataHelper.GetValueFromBytes(ref messagebytes, 2).TrimEnd();
                SEQ_NO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 11).TrimEnd();                
            }
            return this;
        }

        #endregion
    }
}
