using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class AcctCheckOBData : IMessageRespHandler
    {
        public const UInt32 OBDataHeadLength = 24;

        /// <summary>
        /// 记录格式ID,8
        /// </summary>
        public String RecordFormat
        {
            get;
            set;
        }
        /// <summary>
        /// 记录条数，8
        /// </summary>
        public UInt16 RecordCount
        {
            get;
            set;
        }
        /// <summary>
        /// 记录长度,8
        /// </summary>
        public UInt32 RecodeLength
        {
            get;
            set;
        }

        public UInt32 TOTAL_WIDTH
        {
            get
            {
                return (UInt32)AcctCheckOBDataItem.TOTAL_WIDTH * RecordCount + 24;
            }
        }

        public List<AcctCheckOBDataItem> _obDataItemList = null;
        public List<AcctCheckOBDataItem> OBDataItemList
        {
            get
            {
                return _obDataItemList;
            }
            set
            {
                _obDataItemList = value;
            }
        }


        public AcctCheckOBData()
        {
            _obDataItemList = new List<AcctCheckOBDataItem>();
        }

        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            if (messagebytes.Length >= TOTAL_WIDTH)
            {
                RecordFormat = CommonDataHelper.GetValueFromBytes(ref messagebytes, 8).TrimEnd();
                string strlen = CommonDataHelper.GetValueFromBytes(ref messagebytes, 8).TrimEnd();
                int length = 0;
                if (int.TryParse(strlen, out length))
                {
                    RecodeLength = (UInt32)length;
                }
                string strcount = CommonDataHelper.GetValueFromBytes(ref messagebytes, 8).TrimEnd();
                int count = 0;
                if (int.TryParse(strcount, out count))
                {
                    RecordCount = (UInt16)count;
                }
                
                //byte[] itembyte = CommonDataHelper.SubBytes(messagebytes, (int)AcctCheckOBData.OBDataHeadLength, (int)(messagebytes.Length - AcctCheckOBData.OBDataHeadLength));
                int offset = 0;
                int len = messagebytes.Length;
                byte[] itembyte = messagebytes;
                while (len > 0)
                {
                    AcctCheckOBDataItem obitem = new AcctCheckOBDataItem();
                    obitem = (AcctCheckOBDataItem)obitem.FromBytes(itembyte);
                    _obDataItemList.Add(obitem);
                    offset += AcctCheckOBDataItem.TOTAL_WIDTH;
                    len -= AcctCheckOBDataItem.TOTAL_WIDTH;
                    if (messagebytes.Length - offset >= AcctCheckOBDataItem.TOTAL_WIDTH)
                    {
                        itembyte = CommonDataHelper.SubBytes(messagebytes, offset, AcctCheckOBDataItem.TOTAL_WIDTH);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return this;
        }

        #endregion
    }


    public class AcctCheckOBDataItem : IMessageRespHandler
    {
        public const int TOTAL_WIDTH = 119;
        #region Property
        /// <summary>
        /// 交易日期,8
        /// </summary>
        public String TradeDate
        {
            get;
            set;
        }
        /// <summary>
        /// 资金业务流水号,18
        /// </summary>
        public String BizFlowNO
        {
            get;
            set;
        }
        /// <summary>
        /// 交易机构,6
        /// </summary>
        public String OrgNO
        {
            get;
            set;
        }
        /// <summary>
        /// 交易柜员,7
        /// </summary>
        public String TellerNO
        {
            get;
            set;
        }
        /// <summary>
        /// 柜员流水号,11
        /// </summary>
        public String TellerFlowNO
        {
            get;
            set;
        }
        /// <summary>
        /// 交易账号,6
        /// </summary>
        public String TradeAcctNO
        {
            get;
            set;
        }
        /// <summary>
        /// 账号归属机构,6
        /// </summary>
        public String OrgNOWithinAcct
        {
            get;
            set;
        }
        /// <summary>
        /// 币种,3
        /// </summary>
        public String Currency
        {
            get;
            set;
        }
        /// <summary>
        /// 核算码,8
        /// </summary>
        public String CheckCode
        {
            get;
            set;
        }
        /// <summary>
        /// 借贷标志,1;1-借，2-贷
        /// </summary>
        public String DCFlag
        {
            get;
            set;
        }
        /// <summary>
        /// 红蓝字标志,1;1-正常，2-红字，3-蓝字
        /// </summary>
        public String RedBlueFlag
        {
            get;
            set;
        }
        /// <summary>
        /// 金额，17
        /// </summary>
        public String Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 分录状态,1;  1-正常，2-抹帐
        /// </summary>
        public String Status
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
                TradeDate = CommonDataHelper.GetValueFromBytes(ref messagebytes, 8).TrimEnd();
                BizFlowNO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 18).TrimEnd();
                OrgNO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 6).TrimEnd();
                TellerNO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 7).TrimEnd();
                TellerFlowNO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 11).TrimEnd();
                TradeAcctNO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 32).TrimEnd();
                OrgNOWithinAcct = CommonDataHelper.GetValueFromBytes(ref messagebytes, 6).TrimEnd();
                Currency = CommonDataHelper.GetValueFromBytes(ref messagebytes, 3).TrimEnd();
                CheckCode = CommonDataHelper.GetValueFromBytes(ref messagebytes, 8).TrimEnd();
                DCFlag = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                RedBlueFlag = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromBytes(ref messagebytes, 1), null);
                Amount = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromBytes(ref messagebytes, 17), null);
                Status = CommonDataHelper.StrTrimer(CommonDataHelper.GetValueFromBytes(ref messagebytes, 1), null);
            }
            return this;
        }

        #endregion
    }
}
