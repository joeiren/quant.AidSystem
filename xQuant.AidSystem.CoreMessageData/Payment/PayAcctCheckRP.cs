using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class PayAcctCheckRP : IMessageRespHandler
    {

        public uint TOTAL_WIDTH
        {
            get
            {
                return (uint)(98 + (PayAcctCheckItemRP.TOTAL_WIDTH + 1) * AccoutCount);
            }
            
        }
        #region Property
        /// <summary>
        /// 交易结果,10;00—成功,其他—失败,X2
        /// </summary>
        public String RetCode
        {
            get;
            set;
        }
        /// <summary>
        /// 交易结果描述,X80
        /// </summary>
        public String RetMsg
        {
            get;
            set;
        }
        /// <summary>
        /// 记录条数
        /// </summary>
        public int AccoutCount
        {
            get;
            set;
        }

        public List<PayAcctCheckItemRP> AccountList
        {
            get;
            set;
        }
        #endregion
        public PayAcctCheckRP()
        {
            AccountList = new List<PayAcctCheckItemRP>();
        }

        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            if (messagebytes.Length >= TOTAL_WIDTH - PaymentBizMsgDataBase.HEADER_WIDTH)
            {
                RetCode = CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 10);
                RetMsg = CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 80);
                string number = CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 8);
                int count = 0;
                if (int.TryParse(number, out count))
                {
                    AccoutCount = count;
                }

                int offset = 0;
                while (offset < messagebytes.Length && messagebytes.Length - offset > PayAcctCheckItemRP.TOTAL_WIDTH && AccountList.Count < AccoutCount)
                {
                    byte[] dbbytes = CommonDataHelper.SubBytes(messagebytes, offset, PayAcctCheckItemRP.TOTAL_WIDTH + 1);
                    PayAcctCheckItemRP item = new PayAcctCheckItemRP();
                    item.FromBytes(dbbytes);
                    AccountList.Add(item);
                    offset += PayAcctCheckItemRP.TOTAL_WIDTH + 1;
                }
            }
            return this;
        }

        #endregion
    }

    public class PayAcctCheckItemRP : IMessageRespHandler
    {
        public const int TOTAL_WIDTH = 634;
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
        /// 平台流水号,8
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
        /// 交易时间,20
        /// </summary>
        public String TradeTime
        {
            get;
            set;
        }
        #endregion

        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            if (messagebytes.Length > TOTAL_WIDTH)
            {
                PlatDate = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 8), null);
                SeqNO = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 8), null);
                ChanelFlag = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 1), null);
                SysSeqNO = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 22), null);
                OrganNO = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 8), null);
                MsgNO = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 3), null);
                PayAccount = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 32), null);
                PayName = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 60), null);
                PayBankNO = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 12), null);
                PayBankName = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 60), null);
                RecvAccount = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 32), null);
                RecvName = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 60), null);
                RecvBankNO = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 12), null);
                RecvBankName = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 60), null);
                Currency = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 3), null);
                Amount = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 15), null);
                IBORate = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 7), null);
                IBOLimit = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 5), null);
                BizType = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 2), null);
                Note = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 60), null);
                Operator = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 7), null);
                UploadChannel = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 2), null);
                AccountFlag = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 2), null);
                HostFlowNO = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 12), null);
                HostTradeDate = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 8), null);
                Fee = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 15), null);
                HostRespCode = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 10), null);
                HostRespMsg = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 80), null);
                TradeDate = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 8), null);
                TradeTime = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 20), null);
            }
            return this;
        }

        #endregion
    }
}
