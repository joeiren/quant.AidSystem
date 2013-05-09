using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// </summary>
    public class AcctRetrieveODATA : IMessageRespHandler
    {
        //ODATA总长
        public UInt32 TOTAL_WIDTH
        {
            get
            {
                return (UInt32)(CoreDataBlockHeader.TOTAL_WIDTH + AcctRetrieveODATA_DB1.TOTAL_WIDTH + DB_BGO30801_List.Count * AcctRetrieveODATA_DB2.TOTAL_WIDTH +
                    AcctRetrieveODATA_DB3.TOTAL_WIDTH + DB_BGO30803_List.Count * AcctRetrieveODATA_DB4.TOTAL_WIDTH);
            }
            
        }

        public AcctRetrieveODATA_DB1 DB_BGO30800
        {
            get;
            set;
        }

        public List<AcctRetrieveODATA_DB2> DB_BGO30801_List
        {
            get;
            set;
        }

        public AcctRetrieveODATA_DB3 DB_BGO30802
        {
            get;
            set;
        }

        public List<AcctRetrieveODATA_DB4> DB_BGO30803_List
        {
            get;
            set;
        }

        public AcctRetrieveODATA()
        {
            DB_BGO30800 = new AcctRetrieveODATA_DB1();
            DB_BGO30802 = new AcctRetrieveODATA_DB3();
            DB_BGO30801_List = new List<AcctRetrieveODATA_DB2>();
            DB_BGO30803_List = new List<AcctRetrieveODATA_DB4>();
        }

        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            if (messagebytes.Length >= CoreDataBlockHeader.TOTAL_WIDTH)
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
                        case "BGO30800":
                            DB_BGO30800 = (AcctRetrieveODATA_DB1)DB_BGO30800.FromBytes(dbbytes);
                            offset += AcctRetrieveODATA_DB1.TOTAL_WIDTH;
                            break;

                        case "BGO30802":
                            DB_BGO30802 = (AcctRetrieveODATA_DB3)DB_BGO30802.FromBytes(dbbytes);
                            offset += AcctRetrieveODATA_DB3.TOTAL_WIDTH;
                            break;

                        case "BGO30801":
                            AcctRetrieveODATA_DB2 db2 = new AcctRetrieveODATA_DB2();
                            db2 = (AcctRetrieveODATA_DB2)db2.FromBytes(dbbytes);
                            offset += AcctRetrieveODATA_DB2.TOTAL_WIDTH;
                            DB_BGO30801_List.Add(db2);
                            break;

                        case "BGO30803":
                            AcctRetrieveODATA_DB4 db4 = new AcctRetrieveODATA_DB4();
                            db4 = (AcctRetrieveODATA_DB4)db4.FromBytes(dbbytes);
                            offset += AcctRetrieveODATA_DB4.TOTAL_WIDTH;
                            DB_BGO30803_List.Add(db4);
                            break;
                        default:
                            offset += CoreDataBlockHeader.TOTAL_WIDTH;
                            break;

                    }
                }
            }
            return this;
            
        }

        #endregion
    }

    //BGO30800
    public class AcctRetrieveODATA_DB1 : IMessageRespHandler
    {

        public const UInt16 TOTAL_WIDTH = 28 + CoreDataBlockHeader.TOTAL_WIDTH;

        //核心交易流水号,12
        public String CoreTradeSN
        {
            get;
            set;
        }
        //发起方系统代码,2
        public String SrcSystemCode
        {
            get;
            set;
        }
        //有会计流水标志,1
        public String AcctSNFlag
        {
            get;
            set;
        }
        //外系统跟踪号,12
        public String OutsideSN
        {
            get;
            set;
        }
        //被抹账标志,1
        public String ErasedFlag
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
                if (dbhdr.DBH_DB_ID.Trim() == "BGO30800")
                {
                    messagebytes = CommonDataHelper.SubBytes(messagebytes, CoreDataBlockHeader.TOTAL_WIDTH, messagebytes.Length - CoreDataBlockHeader.TOTAL_WIDTH);
                    CoreTradeSN = CommonDataHelper.GetValueFromBytes(ref messagebytes, 12).TrimEnd();
                    SrcSystemCode = CommonDataHelper.GetValueFromBytes(ref messagebytes, 2).TrimEnd();
                    AcctSNFlag = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                    OutsideSN = CommonDataHelper.GetValueFromBytes(ref messagebytes, 12).TrimEnd();
                    ErasedFlag = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                }
            }

            return this;
        }

        #endregion
    }

    //BGO30801
    public class AcctRetrieveODATA_DB2 : IMessageRespHandler
    {
        public const UInt16 TOTAL_WIDTH = CoreDataBlockHeader.TOTAL_WIDTH + 154;
        //核心交易流水号,12
        public String CoreTradeSN
        {
            get;
            set;
        }

        //交易账号,20
        public String TradeAccount
        {
            get;
            set;
        }
        //户名,80
        public String AccountName
        {
            get;
            set;
        }
        //现转标志,1
        public String CashFalg
        {
            get;
            set;
        }
        //借贷标志,1
        public String LoanFlag
        {
            get;
            set;
        }
        //金额,17
        //private Decimal _amount = new Decimal(0);
        public String Amount
        {
            get;
            //{
            //    return Decimal.Round(_amount, 2);
            //}
            set;
            //{
            //    _amount = value;
            //}
        }

        //凭证号码,20
        public String VoucherNO
        {
            get;
            set;
        }
        //发起方系统代码,2
        public String SrcSystemCode
        {
            get;
            set;
        }
        //入账配钞标志,1
        public String PostingCashFlag
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
                if (dbhdr.DBH_DB_ID.Trim() == "BGO30801")
                {
                    messagebytes = CommonDataHelper.SubBytes(messagebytes, CoreDataBlockHeader.TOTAL_WIDTH, messagebytes.Length - CoreDataBlockHeader.TOTAL_WIDTH);
                    CoreTradeSN = CommonDataHelper.GetValueFromBytes(ref messagebytes, 12).TrimEnd();
                    TradeAccount = CommonDataHelper.GetValueFromBytes(ref messagebytes, 20).TrimEnd();
                    AccountName = CommonDataHelper.GetValueFromBytes(ref messagebytes, 80).TrimEnd();
                    CashFalg = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                    LoanFlag = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                    //string value = CommonDataHelper.GetValueFromBytes(ref messagebytes, 17);
                    //decimal amount;
                    //if (Decimal.TryParse(value, out amount))
                    //{
                    //    Amount = amount;
                    //}
                    Amount = CommonDataHelper.GetValueFromBytes(ref messagebytes, 17).TrimEnd();
                    VoucherNO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 20).TrimEnd();
                    SrcSystemCode = CommonDataHelper.GetValueFromBytes(ref messagebytes, 2).TrimEnd();
                    PostingCashFlag = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                }

            }

            return this;
        }

        #endregion
    }

    //BGO30802
    public class AcctRetrieveODATA_DB3 : IMessageRespHandler
    {
        public const UInt16 TOTAL_WIDTH = 3 + CoreDataBlockHeader.TOTAL_WIDTH;
        //是否配钞 ,1
        public String IsQuotaMoney
        {
            get;
            set;
        }
        //配钞笔数,2
        public UInt16 QuotaMoneyCount
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
                if (dbhdr.DBH_DB_ID.Trim() == "BGO30802")
                {
                    messagebytes = CommonDataHelper.SubBytes(messagebytes, CoreDataBlockHeader.TOTAL_WIDTH, messagebytes.Length - CoreDataBlockHeader.TOTAL_WIDTH);
                    IsQuotaMoney = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                    String val = CommonDataHelper.GetValueFromBytes(ref messagebytes, 2);
                    UInt16 count;
                    if (UInt16.TryParse(val, out count))
                    {
                        QuotaMoneyCount = count;
                    }
                }
            }

            return this;
        }

        #endregion
    }

    //BGO30803
    public class AcctRetrieveODATA_DB4 : IMessageRespHandler
    {
        public const UInt16 TOTAL_WIDTH = 20 + CoreDataBlockHeader.TOTAL_WIDTH;
        //币种,3
        public String Currency
        {
            get;
            set;
        }
        //配钞金额,17
        //private Decimal _quotaAmount = new Decimal(0);
        public String QuotaAmount
        {
            get;
            //{
            //    return Decimal.Round(_quotaAmount, 2);
            //}
            set;
            //{
            //    _quotaAmount = value;
            //}
        }
        
        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            if (messagebytes.Length >= TOTAL_WIDTH)
            {
                CoreDataBlockHeader dbhdr = new CoreDataBlockHeader();
                dbhdr = (CoreDataBlockHeader)dbhdr.FromBytes(messagebytes);
                if (dbhdr.DBH_DB_ID.Trim() == "BGO30803")
                {
                    messagebytes = CommonDataHelper.SubBytes(messagebytes, CoreDataBlockHeader.TOTAL_WIDTH, messagebytes.Length - CoreDataBlockHeader.TOTAL_WIDTH);
                    Currency = CommonDataHelper.GetValueFromBytes(ref messagebytes, 3);
                    //string amount = CommonDataHelper.GetValueFromBytes(ref messagebytes, 17);
                    //decimal quotaAmount;
                    //if (Decimal.TryParse(amount, out quotaAmount))
                    //{
                    //    QuotaAmount = quotaAmount;
                    //}
                    QuotaAmount = CommonDataHelper.GetValueFromBytes(ref messagebytes, 17);
                }
                
            }
            
            return this;
        }

        #endregion
    }
}
