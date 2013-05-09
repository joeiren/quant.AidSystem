using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class RQHDR_MsgHandler : IMessageReqHandler
    {
        public const UInt16 TOTAL_WIDTH = 132;
        #region Property
        /// <summary>
        /// 信息类型
        /// </summary>
        public String MSG_ID
        {
            get
            {
                return "REQ";
            }
            set
            { }
        }
        /// <summary>
        /// 交互控制
        /// </summary>
        public String CONV_CTL
        {
            get
            {
                return "0";
            }
            set
            { }
        }
        /// <summary>
        /// 翻页模式
        /// </summary>
        public String PG_CTL
        {
            get
            {
                return "0";
            }
            set
            { }
        }
        /// <summary>
        /// 预留 3位
        /// </summary>
        public String RES
        {
            get
            {
                return CommonDataHelper.SpaceString(3);
            }
            set
            { }
        }
        /// <summary>
        /// 渠道代码 2位
        /// </summary>
        public String CH_PID
        {
            get
            {
                return "KP";
            }
            set { }
        }
        /// <summary>
        /// 渠道分析代码 2位
        /// </summary>
        public String CH_AID
        {
            get
            {
                return "KP";
            }
            set
            { }
        }
        /// <summary>
        /// 客户端应用种类代码 1位
        /// </summary>
        public String CT_ID
        {
            get
            {
                return CommonDataHelper.SpaceString(1);
            }
            set
            { }
        }
        /// <summary>
        /// 柜员输入交易码 6位
        /// </summary>
        public virtual String TEL_TXID
        {
            get
            {
                return string.Format("{0}0", SYS_TXID.Substring(0, 5));
            }
            set
            { }
        }
        /// <summary>
        /// 主机启动原交易码 6位
        /// </summary>
        public String SYS_TXID
        {
            get;
            set;
        }
        /// <summary>
        /// 业务交易日 10位
        /// </summary>
        public String TX_DTE
        {
            get;
            set;
        }
        /// <summary>
        /// 系统日期 10位
        /// </summary>
        public String SYS_DTE
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd");
            }
            set
            { }
        }
        /// <summary>
        /// 系统时间 8位
        /// </summary>
        public String SYS_TME
        {
            get
            {
                return DateTime.Now.ToString("HH.mm.ss");
            }
            set
            { }
        }
        /// <summary>
        /// 交易机构号 6位
        /// </summary>
        public String TX_OUNO
        {
            get;
            set;
        }
        /// <summary>
        /// 交易部门 3位
        /// </summary>
        public String TX_DPT
        {
            get
            {
                return CommonDataHelper.SpaceString(3);
            }
            set
            { }
        }
        /// <summary>
        /// 交易地区号 2位
        /// </summary>
        public String TX_DIS
        {
            get
            {
                return CommonDataHelper.SpaceString(2);
            }
            set
            { }
        }
        /// <summary>
        /// 前端unix服务器号 6位
        /// </summary>
        public String SRV_UNIX_ID
        {
            get
            {
                return CommonDataHelper.SpaceString(6);
            }
            set
            { }
        }
        /// <summary>
        /// 终端号 8位
        /// </summary>
        public String TRM_ID
        {
            get
            {
                return CommonDataHelper.SpaceString(8);
            }
            set
            { }
        }
        /// <summary>
        /// 柜员号 7位
        /// </summary>
        public String TEL_ID
        {
            get;
            set;
        }
        /// <summary>
        /// 静态授权柜员号 7位
        /// </summary>
        public String ATEL_ID
        {
            get
            {
                return CommonDataHelper.SpaceString(7);
            }
            set
            { }
        }
        /// <summary>
        /// 已预警、复核或授权标志 1位
        /// </summary>
        public String APRC_FLG
        {
            get
            {
                return "0";
            }
            set
            { }
        }
        /// <summary>
        /// 交易性质 1 位
        /// </summary>
        public String TX_MODE
        {
            get;
            set;
        }
        /// <summary>
        /// 发起方系统代码 2 位
        /// </summary>
        public String SRS_ID
        {
            // 从配置读
            get
            {
                return "18";
            }
            set { }
        }
        /// <summary>
        /// 老系统标志 1位
        /// </summary>
        public String ORG_SYS
        {
            get
            {
                return "0";
            }
            set
            { }
        }
        /// <summary>
        /// 渠道交易流水号 12位
        /// </summary>
        public String SRV_JNO
        {
            get;
            set;
        }
        /// <summary>
        /// 抹帐用前端（渠道）交易流水号 12位
        /// </summary>
        public String SRV_REV_JNO
        {
            get;
            set;
        }
        /// <summary>
        /// 抹帐用核心交易流水号 11位
        /// </summary>
        public String HOST_JNO
        {
            get;
            set;
        }


        #endregion

        #region IMessageReqHandler Members

        public byte[] ToBytes()
        {
            StringBuilder sb = new StringBuilder();
            sb = sb.Append(MSG_ID);
            sb = sb.Append(CONV_CTL);
            sb = sb.Append(PG_CTL);
            sb = sb.Append(RES);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(CH_PID, 2));
            sb = sb.Append(CH_AID);
            sb = sb.Append(CT_ID);
            sb = sb.Append(TEL_TXID);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(SYS_TXID, 6));
            sb = sb.Append(TX_DTE);
            sb = sb.Append(SYS_DTE);
            sb = sb.Append(SYS_TME);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(TX_OUNO, 6));
            sb = sb.Append(TX_DPT);
            sb = sb.Append(TX_DIS);
            sb = sb.Append(SRV_UNIX_ID);
            sb = sb.Append(TRM_ID);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(TEL_ID, 7));
            sb = sb.Append(ATEL_ID);
            sb = sb.Append(APRC_FLG);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(TX_MODE, 1));
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(SRS_ID, 2));
            sb = sb.Append(ORG_SYS);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(SRV_JNO, 12));
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(SRV_REV_JNO, 12));
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(HOST_JNO, 11));
            byte [] bytes = new byte[TOTAL_WIDTH];
            int len = EBCDICEncoder.WideCharToEBCDIC(EBCDICEncoder.CCSID_IBM_1388, sb.ToString(), sb.Length, bytes, bytes.Length);
            if (len != bytes.Length)
            {
                return CommonDataHelper.SubBytes(bytes, 0, len);
            }
            return bytes;
        }

        #endregion
    }
}
