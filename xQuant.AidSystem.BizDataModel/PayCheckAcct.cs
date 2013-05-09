using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.BizDataModel
{
    public class PayCheckAcct
    {
        #region Property
        /// <summary>
        /// 平台日期,8
        /// </summary>
        public String PlatDate
        {
            get;
            set;
        }
        /// <summary>
        /// 平台流水号,6
        /// </summary>
        public String SeqNO
        {
            get;
            set;
        }
        /// <summary>
        ///渠道标志,1; 0：大额1：农信银
        /// </summary>
        public String ChanelFlag
        {
            get;
            set;
        }
        /// <summary>
        /// 资金流水号,22
        /// </summary>
        public String SysSeqNO
        {
            get;
            set;
        }
        /// <summary>
        /// 交易机构号,8
        /// </summary>
        public String OrganNO
        {
            get;
            set;
        }
        /// <summary>
        /// 报文编号,3
        /// </summary>
        public String MsgNO
        {
            get;
            set;
        }
        /// <summary>
        /// 付款账号,32
        /// </summary>
        public String PayAccount
        {
            get;
            set;
        }
        /// <summary>
        /// 付款方户名,60
        /// </summary>
        public String PayName
        {
            get;
            set;
        }
        /// <summary>
        /// 付款行行号,12
        /// </summary>
        public String PayBankNO
        {
            get;
            set;
        }
        /// <summary>
        /// 付款行名称,60
        /// </summary>
        public String PayBankName
        {
            get;
            set;
        }
        /// <summary>
        /// 收款账号,32
        /// </summary>
        public String RecvAccount
        {
            get;
            set;
        }
        /// <summary>
        /// 收款方户名,60
        /// </summary>
        public String RecvName
        {
            get;
            set;

        }
        /// <summary>
        /// 收款行行号,12
        /// </summary>
        public String RecvBankNO
        {
            get;
            set;
        }
        /// <summary>
        /// 收款行名称,60
        /// </summary>
        public String RecvBankName
        {
            get;
            set;
        }
        /// <summary>
        ///币种代码 ,3
        /// </summary>
        public String Currency
        {
            get;
            set;
        }
        /// <summary>
        /// 交易金额,15
        /// </summary>
        public String Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 拆借利率,7
        /// </summary>
        public String IBORate
        {
            get;
            set;
        }
        /// <summary>
        /// 拆借期限,5
        /// </summary>
        public String IBOLimit
        {
            get;
            set;
        }
        /// <summary>
        /// 业务种类,2
        /// </summary>
        public String BizType
        {
            get;
            set;
        }
        /// <summary>
        /// 备注,60
        /// </summary>
        public String Note
        {
            get;
            set;

        }
        /// <summary>
        /// 操作员,7
        /// </summary>
        public String Operator
        {
            get;
            set;
        }
        /// <summary>
        /// 上送渠道,2
        /// </summary>
        public String UploadChannel
        {
            get;
            set;
        }
        /// <summary>
        /// 上主机记账标志,2;01：记账成功 03：记账超时
        /// </summary>
        public String AccountFlag
        {
            get;
            set;
        }
        /// <summary>
        /// 主机流水号,12
        /// </summary>
        public String HostFlowNO
        {
            get;
            set;
        }
        /// <summary>
        /// 主机交易日期,8
        /// </summary>
        public String HostTradeDate
        {
            get;
            set;
        }
        /// <summary>
        /// 手续费,15
        /// </summary>
        public String Fee
        {
            get;
            set;
        }
        /// <summary>
        /// 主机响应码,10
        /// </summary>
        public String HostRespCode
        {
            get;
            set;
        }
        /// <summary>
        /// 主机响应信息,80
        /// </summary>
        public String HostRespMsg
        {
            get;
            set;
        }
        /// <summary>
        /// 交易日期,8
        /// </summary>
        public String TradeDate
        {
            get;
            set;
        }
        /// <summary>
        /// 交易时间,8
        /// </summary>
        public String TradeTime
        {
            get;
            set;
        }
        #endregion
    }
}
