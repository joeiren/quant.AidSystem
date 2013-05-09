using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class AcctEraseODATA : IMessageRespHandler
    {
        //ODATA总长
        public UInt16 TOTAL_WIDTH
        {
            get
            {
                return (UInt16)(CoreDataBlockHeader.TOTAL_WIDTH + AcctEraseODATA_DB1.TOTAL_WIDTH + DB_BY999001_List.Count * AcctEraseODATA_DB2.TOTAL_WIDTH);
            }
        }

        public AcctEraseODATA_DB1 DB_BY999000
        {
            get;
            set;
        }

        public List<AcctEraseODATA_DB2> DB_BY999001_List
        {
            get;
            set;
        }

        public AcctEraseODATA()
        {
            DB_BY999001_List = new List<AcctEraseODATA_DB2>();
            DB_BY999000 = new AcctEraseODATA_DB1();
        }
        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            if (messagebytes.Length > CoreDataBlockHeader.TOTAL_WIDTH)
            {
                CoreDataBlockHeader dbhdr = new CoreDataBlockHeader();
                //dbhdr = (CoreDataBlockHeader)dbhdr.FromBytes(messagebytes);
                //messagebytes = CommonDataHelper.SubBytes(messagebytes, CoreDataBlockHeader.TOTAL_WIDTH, messagebytes.Length - CoreDataBlockHeader.TOTAL_WIDTH);
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
                        case "BY999000":
                            DB_BY999000 = (AcctEraseODATA_DB1)DB_BY999000.FromBytes(dbbytes);
                            if (dbhdr.DBH_DB_LENGTH > 0)
                            {
                                offset += (int)dbhdr.DBH_DB_LENGTH;                                
                            }
                            else
                            {
                                offset += AcctEraseODATA_DB1.TOTAL_WIDTH;
                            }
                            break;

                        case "BY999001":
                            AcctEraseODATA_DB2 db = new AcctEraseODATA_DB2();
                            db = (AcctEraseODATA_DB2)db.FromBytes(dbbytes);
                            DB_BY999001_List.Add(db);
                            if (dbhdr.DBH_DB_LENGTH > 0)
                            {
                                offset += (int)dbhdr.DBH_DB_LENGTH;
                            }
                            else
                            {
                                offset += AcctEraseODATA_DB2.TOTAL_WIDTH;
                            }
                            break;

                        default:
                            if (dbhdr.DBH_DB_LENGTH > 0)
                            {
                                offset += (int)dbhdr.DBH_DB_LENGTH;
                            }
                            else
                            {
                                offset += CoreDataBlockHeader.TOTAL_WIDTH;
                            }
                            break;
                    }
                }
            }
            return this;
        }

        #endregion
    }
    
    /// <summary>
    /// BY999000
    /// </summary>
    public class AcctEraseODATA_DB1 : IMessageRespHandler
    { 
        public const UInt16 TOTAL_WIDTH = 61 + CoreDataBlockHeader.TOTAL_WIDTH;
        /// <summary>
        /// 反交易时间,8
        /// </summary>
        public String TIME
        {
            get;
            set;
        }
        /// <summary>
        /// 反交易流水号,12
        /// </summary>
        public String JNO
        {
            get;
            set;
        }
        /// <summary>
        /// 交易日期,8
        /// </summary>
        public String Date
        {
            get;
            set;
        }
        /// <summary>
        /// 交易时间,8
        /// </summary>
        public String TX_TIME
        {
            get;
            set;
        }
        /// <summary>
        /// 交易柜员号,7
        /// </summary>
        public String TEL_NO
        {
            get;
            set;
        }
        /// <summary>
        /// 交易码,6
        /// </summary>
        public String TX_ID
        {
            get;
            set;
        }
        /// <summary>
        /// 交易流水号,12
        /// </summary>
        public String TX_JNO
        {
            get;
            set;
        }

        #region IMessageRespHandler Members

        public object  FromBytes(byte[] messagebytes)
        {
            if (messagebytes.Length >= TOTAL_WIDTH)
            {
                CoreDataBlockHeader dbhdr = new CoreDataBlockHeader();
                dbhdr = (CoreDataBlockHeader)dbhdr.FromBytes(messagebytes);
                if (dbhdr.DBH_DB_ID !=null && dbhdr.DBH_DB_ID.Trim() == "BY999000")
                {
                    messagebytes = CommonDataHelper.SubBytes(messagebytes, CoreDataBlockHeader.TOTAL_WIDTH, messagebytes.Length - CoreDataBlockHeader.TOTAL_WIDTH);
                    TIME = CommonDataHelper.GetValueFromBytes(ref messagebytes, 8).TrimEnd();
                    JNO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 12).TrimEnd();
                    Date = CommonDataHelper.GetValueFromBytes(ref messagebytes, 8).TrimEnd();
                    TX_TIME = CommonDataHelper.GetValueFromBytes(ref messagebytes, 8).TrimEnd();
                    TEL_NO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 7).TrimEnd();
                    TX_ID = CommonDataHelper.GetValueFromBytes(ref messagebytes, 6).TrimEnd();
                    TX_JNO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 12).TrimEnd();                    
                }
            }

            return this;
        }

        #endregion
    }

    /// <summary>
    /// BY999001
    /// </summary>
    public class AcctEraseODATA_DB2 : IMessageRespHandler
    {
        public const UInt16 TOTAL_WIDTH = 123 + CoreDataBlockHeader.TOTAL_WIDTH;

        /// <summary>
        /// 分录顺序号,2
        /// </summary>
        public String GL_SEQ
        {
            get;
            set;
        }

        /// <summary>
        /// 账号,20
        /// </summary>
        public String ACC_NO
        {
            get;
            set;
        }
        /// <summary>
        /// 户名,80
        /// </summary>
        public String ACC_NAME
        {
            get;
            set;
        }
        /// <summary>
        /// 借/贷标志,1
        /// </summary>
        public String DRCR_IND
        {
            get;
            set;
        }
        /// <summary>
        /// 交易金额,17
        /// </summary>
        //private Decimal _tradeAmount = new Decimal(0);
        public String TX_AMT
        {
            get;
            //{
            //    return Decimal.Round(_tradeAmount, 2);
            //}
            set;
            //{
            //    _tradeAmount = value;
            //}
        }
        /// <summary>
        /// 交易币种,3
        /// </summary>
        public String TX_CCY
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
                if (dbhdr.DBH_DB_ID != null && dbhdr.DBH_DB_ID.Trim() == "BY999001")
                {
                    messagebytes = CommonDataHelper.SubBytes(messagebytes, CoreDataBlockHeader.TOTAL_WIDTH, messagebytes.Length - CoreDataBlockHeader.TOTAL_WIDTH);
                    GL_SEQ = CommonDataHelper.GetValueFromBytes(ref messagebytes, 2).TrimEnd();
                    ACC_NO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 20).TrimEnd();
                    ACC_NAME = CommonDataHelper.GetValueFromBytes(ref messagebytes, 80).TrimEnd();
                    DRCR_IND = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                    TX_AMT = CommonDataHelper.GetValueFromBytes(ref messagebytes, 17).TrimEnd();
                    //string amount = CommonDataHelper.GetValueFromBytes(ref messagebytes, 17);
                    //decimal tradeAmt;
                    //if (Decimal.TryParse(amount, out tradeAmt))
                    //{
                    //    TX_AMT = tradeAmt;
                    //}
                    TX_CCY = CommonDataHelper.GetValueFromBytes(ref messagebytes, 3).TrimEnd();
                }
            }

            return this;
        }

        #endregion
    }
}
