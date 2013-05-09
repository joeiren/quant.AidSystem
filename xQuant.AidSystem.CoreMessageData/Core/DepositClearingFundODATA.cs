using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 县级行社上存清算资金查询
    /// </summary>
    public class DepositClearingFundODATA : IMessageRespHandler
    {
        public UInt32 TOTAL_WIDTH
        {
            get 
            {
                return (UInt32)(CoreDataBlockHeader.TOTAL_WIDTH + DepositClearingFundODATAItem.TOTAL_WIDTH * BalanceInfoList.Count);
            }
        }

        public List<DepositClearingFundODATAItem> BalanceInfoList
        {
            get;
            set;
        }

        public DepositClearingFundODATA()
        {
            BalanceInfoList = new List<DepositClearingFundODATAItem>();
        }
        
        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            if (messagebytes.Length >= CoreDataBlockHeader.TOTAL_WIDTH)
            {
                CoreDataBlockHeader dbhdr = new CoreDataBlockHeader();
                int offset = 0;
                while (offset < messagebytes.Length)
                {
                    byte[] dbbytes = CommonDataHelper.SubBytes(messagebytes, offset, messagebytes.Length - offset);
                    dbhdr = (CoreDataBlockHeader)dbhdr.FromBytes(dbbytes);
                    if (dbhdr == null)
                    {
                        offset += CoreDataBlockHeader.TOTAL_WIDTH;
                        continue;
                    }
                    DepositClearingFundODATAItem item = new DepositClearingFundODATAItem();
                    item = (DepositClearingFundODATAItem)item.FromBytes(dbbytes);
                    offset += DepositClearingFundODATAItem.TOTAL_WIDTH;
                    BalanceInfoList.Add(item);
                }
            }
            return this;
        }

        #endregion
    }

    public class DepositClearingFundODATAItem : IMessageRespHandler
    {
        public const int TOTAL_WIDTH = 119 + CoreDataBlockHeader.TOTAL_WIDTH;
        #region Property
        /// <summary>
        /// 机构号,6
        /// </summary>
        public String OrgNO
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
        /// 科目,8
        /// </summary>
        public String Subject
        {
            get;
            set;
        }
        /// <summary>
        /// 上日余额,17
        /// </summary>
        public String PerviousBalance
        {
            get;
            set;
        }
        /// <summary>
        /// 本日借方发生额,17
        /// </summary>
        public String DebitAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 本日贷方发生额,17
        /// </summary>
        public String CreditAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 当前余额,17
        /// </summary>
        public String CurrentBalance
        {
            get;
            set;
        }
        /// <summary>
        /// 下限金额,17
        /// </summary>
        public String FloorAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 轧差金额,17
        /// </summary>
        public String OffsetBalance
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
                CoreDataBlockHeader dbhdr1 = new CoreDataBlockHeader();
                dbhdr1 = (CoreDataBlockHeader)dbhdr1.FromBytes(messagebytes);
                messagebytes = CommonDataHelper.SubBytes(messagebytes, CoreDataBlockHeader.TOTAL_WIDTH, messagebytes.Length - CoreDataBlockHeader.TOTAL_WIDTH);
                OrgNO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 6).TrimEnd();
                Currency = CommonDataHelper.GetValueFromBytes(ref messagebytes, 3).TrimEnd();
                Subject = CommonDataHelper.GetValueFromBytes(ref messagebytes, 8).TrimEnd();
                PerviousBalance = CommonDataHelper.GetValueFromBytes(ref messagebytes, 17).TrimEnd();
                DebitAmount = CommonDataHelper.GetValueFromBytes(ref messagebytes, 17).TrimEnd();
                CreditAmount = CommonDataHelper.GetValueFromBytes(ref messagebytes, 17).TrimEnd();
                CurrentBalance = CommonDataHelper.GetValueFromBytes(ref messagebytes, 17).TrimEnd();
                FloorAmount = CommonDataHelper.GetValueFromBytes(ref messagebytes, 17).TrimEnd();
                OffsetBalance = CommonDataHelper.GetValueFromBytes(ref messagebytes, 17).TrimEnd();
            }
            return this;
        }

        #endregion
    }

}
