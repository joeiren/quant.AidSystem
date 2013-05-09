using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class AcctRecordMulti2OneODATA : IMessageRespHandler
    {
        public const UInt16 TOTAL_WIDTH = CoreDataBlockHeader.TOTAL_WIDTH;

        private List<AcctRecordMulti2OneODATA_Item> _odataItemList;
        public List<AcctRecordMulti2OneODATA_Item> OdataItemList
        {
            get
            {
                return _odataItemList;
            }
            set
            {
                _odataItemList = value;
            }
        }

        private List<AcctRecordMulti2OneODATA_PendingItem> _odataPendingList;
        public List<AcctRecordMulti2OneODATA_PendingItem> OdataPendingList
        {
            get
            {
                return _odataPendingList;
            }
            set
            {
                _odataPendingList = value;
            }
        }

        public String RespOdata
        {
            get;
            set;
        }

        public AcctRecordMulti2OneODATA()
        {
            _odataItemList = new List<AcctRecordMulti2OneODATA_Item>();
            _odataPendingList = new List<AcctRecordMulti2OneODATA_PendingItem>();
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
                    switch (dbhdr.DBH_DB_ID.Trim())
                    {
                        case "BXO00008":
                            AcctRecordMulti2OneODATA_Item item = new AcctRecordMulti2OneODATA_Item();
                            item = (AcctRecordMulti2OneODATA_Item)item.FromBytes(dbbytes);
                            offset += AcctRecordMulti2OneODATA_Item.TOTAL_WIDTH;
                            _odataItemList.Add(item);
                            break;

                        case "BG203300":
                            AcctRecordMulti2OneODATA_PendingItem pending = new AcctRecordMulti2OneODATA_PendingItem();
                            pending = (AcctRecordMulti2OneODATA_PendingItem)pending.FromBytes(dbbytes);
                            offset += AcctRecordMulti2OneODATA_PendingItem.TOTAL_WIDTH;
                            _odataPendingList.Add(pending);
                            break;
                        default:
                            offset += (int)dbhdr.DBH_DB_LENGTH;
                            break;
                    }
                }
            }
            else
            {
                RespOdata = CommonDataHelper.GetValueFromBytes(ref messagebytes, (UInt16)messagebytes.Length).TrimEnd();
            }
            return this;
        }

        #endregion
    }

    public class AcctRecordMulti2OneODATA_Item : IMessageRespHandler
    {
        public const UInt16 TOTAL_WIDTH = 65;
        /// <summary>
        /// 账号,20
        /// </summary>
        public String ACCT
        {
            get;
            set;
        }
        /// <summary>
        /// 科目号,8
        /// </summary>
        public String GL_NO
        {
            get;
            set;
        }
        /// <summary>
        /// 币种,3
        /// </summary>
        public String CCY
        {
            get;
            set;
        }
        /// <summary>
        /// 借贷标志,1
        /// </summary>
        public String CD_IND
        {
            get;
            set;
        }
        /// <summary>
        /// 金额,17
        /// </summary>
        public String AMT
        {
            get;
            set;
        }
        /// <summary>
        /// 分录序号,2
        /// </summary>
        public String GL_SEQ
        {
            get;
            set;
        }

        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            if (messagebytes.Length >= TOTAL_WIDTH)
            {
                CoreDataBlockHeader dbhdr = new CoreDataBlockHeader();
                dbhdr = (CoreDataBlockHeader)dbhdr.FromBytes(messagebytes);
                if (dbhdr.DBH_DB_ID.Trim() == "BXO00008")
                {
                    messagebytes = CommonDataHelper.SubBytes(messagebytes, CoreDataBlockHeader.TOTAL_WIDTH, messagebytes.Length - CoreDataBlockHeader.TOTAL_WIDTH);
                    ACCT = CommonDataHelper.GetValueFromBytes(ref messagebytes, 20).TrimEnd();
                    GL_NO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 8).TrimEnd();
                    CCY = CommonDataHelper.GetValueFromBytes(ref messagebytes, 3).TrimEnd();
                    CD_IND = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                    AMT = CommonDataHelper.GetValueFromBytes(ref messagebytes, 17).TrimEnd();
                    GL_SEQ = CommonDataHelper.GetValueFromBytes(ref messagebytes, 2).TrimEnd();
                }
            }
            return this;
        }

        #endregion
    }

    public class AcctRecordMulti2OneODATA_PendingItem : IMessageRespHandler
    {
        public const UInt16 TOTAL_WIDTH = 85;

        /// <summary>
        /// 资金业务系统流水号
        /// </summary>
        public String FlowNO
        {
            get;
            set;
        }

        /// <summary>
        /// 套内序号,2
        /// </summary>
        public String InnerSN
        {
            get;
            set;
        }
        /// <summary>
        /// 挂账序号,14
        /// </summary>
        public String PendingSN
        {
            get;
            set;
        }
        /// <summary>
        /// 挂账账号,20
        /// </summary>
        public String PendingAccount
        {
            get;
            set;
        }
        /// <summary>
        /// 挂账金额,17
        /// </summary>
        public String PendingAmount
        {
            get;
            set;
        }
        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            if (messagebytes.Length >= TOTAL_WIDTH)
            {
                CoreDataBlockHeader dbhdr = new CoreDataBlockHeader();
                dbhdr = (CoreDataBlockHeader)dbhdr.FromBytes(messagebytes);
                if (dbhdr.DBH_DB_ID.Trim() == "BG203300")
                {
                    messagebytes = CommonDataHelper.SubBytes(messagebytes, CoreDataBlockHeader.TOTAL_WIDTH, messagebytes.Length - CoreDataBlockHeader.TOTAL_WIDTH);
                    FlowNO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 18).TrimEnd();
                    InnerSN = CommonDataHelper.GetValueFromBytes(ref messagebytes, 2).TrimEnd();
                    PendingSN = CommonDataHelper.GetValueFromBytes(ref messagebytes, 14).TrimEnd();
                    PendingAccount = CommonDataHelper.GetValueFromBytes(ref messagebytes, 20).TrimEnd();
                    PendingAmount = CommonDataHelper.GetValueFromBytes(ref messagebytes, 17).TrimEnd();
                }
            }
            return this;
        }

        #endregion
    }
}
