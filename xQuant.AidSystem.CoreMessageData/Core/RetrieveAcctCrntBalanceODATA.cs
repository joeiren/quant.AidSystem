using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 资金业务账号余额查询
    /// </summary>
    public class RetrieveAcctCrntBalanceODATA :　IMessageRespHandler
    {
        public UInt32 TOTAL_WIDTH
        {
            get 
            {
                return (UInt32)(CoreDataBlockHeader.TOTAL_WIDTH + RetrieveAcctCrntBalanceODATA_Item.TOTAL_WIDTH * BalanceList.Count);
            }
        }

        public List<RetrieveAcctCrntBalanceODATA_Item> BalanceList
        {
            get;
            set;
        }

        public RetrieveAcctCrntBalanceODATA()
        {
            BalanceList = new List<RetrieveAcctCrntBalanceODATA_Item>();
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
                    RetrieveAcctCrntBalanceODATA_Item item = new RetrieveAcctCrntBalanceODATA_Item();
                    item = (RetrieveAcctCrntBalanceODATA_Item)item.FromBytes(dbbytes);
                    offset += (RetrieveAcctCrntBalanceODATA_Item.TOTAL_WIDTH + CoreDataBlockHeader.TOTAL_WIDTH);
                    BalanceList.Add(item);
                }
            }
            return this;
        }

        #endregion
    }

    public class RetrieveAcctCrntBalanceODATA_Item : IMessageRespHandler
    {
        public const int TOTAL_WIDTH = 40;
        #region Property
        /// <summary>
        /// 账号,20
        /// </summary>
        public String AcctNO
        {
            get;
            set;
        }
        /// <summary>
        /// 账号性质,1;1-表内内部账，2-存款账号
        /// </summary>
        public String AcctProperty
        {
            get;
            set;
        }
        /// <summary>
        /// 结果标志,1;1-成功，2-账号不存在或已销户
        /// </summary>
        public String ResultFlag
        {
            get;
            set;
        }
        /// <summary>
        /// 余额,17
        /// </summary>
        public String Balance
        {
            get;
            set;
        }
        /// <summary>
        /// 余额方向,1
        /// </summary>
        public String BalanceDirection
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
                AcctNO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 20).TrimEnd();
                AcctProperty = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                ResultFlag = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                Balance = CommonDataHelper.GetValueFromBytes(ref messagebytes, 17).TrimEnd();
                BalanceDirection = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
            }
            return this;
        }

        #endregion
    }
}
