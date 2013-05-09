using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xQuant.AidSystem.BizDataModel;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 单个同业存放结息接口的请求数据对象（RQHDR+RQDTL+IBDATA）
    /// </summary>
    public class InterBankInterestData : CoreBizMsgMultiReqDataBase
    {

        public InterBankInterestData()
            : base()
        {
            RQDTL = new InterBankInterestRQDTL();
        }
        public InterBankInterestRQDTL RQDTL
        {
            get;
            set;
        }

        #region Base Method

        protected override byte[] RQDTL_ToBytes(byte[] dest)
        {
            Array.Copy(RQDTL.ToBytes(), 0, dest, CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH, RQDTL.TOTAL_WIDTH);
            return dest;
        }

        protected override void ODATA_FromBytes(byte[] buffer)
        {
            return;
        }

        protected override ushort GetRQDTLLen()
        {
            return (ushort)RQDTL.TOTAL_WIDTH;
        }

        protected override ushort GetODATALen()
        {
            return 0;
        }

        public override uint RQ_TOTAL_WIDTH
        {
            get
            {
                return (uint)(CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH + RQDTL.TOTAL_WIDTH);
            }
        }
        #endregion

        public void SetHeader(RQHDR_MsgHandler hdr)
        {
            RQhdrHandler.SYS_TXID = hdr.SYS_TXID; //
            RQhdrHandler.TX_OUNO = hdr.TX_OUNO;
            RQhdrHandler.TEL_ID = hdr.TEL_ID;
            RQhdrHandler.TX_MODE = hdr.TX_MODE;
            RQhdrHandler.TX_DTE = hdr.TX_DTE;
            RQhdrHandler.SRV_JNO = hdr.SRV_JNO;
            RQhdrHandler.SRV_REV_JNO = hdr.SRV_JNO;
            RQhdrHandler.HOST_JNO = hdr.SRV_JNO;
        }

        public void SetRQDTL(InterBankInterestSummaryInfo summary)
        {
            if (RQDTL == null)
            {
                return;
            }
            RQDTL.KPSN = summary.KPSN;
            RQDTL.BatchName = summary.BatchName;
            RQDTL.TotalAmount = new decimal(summary.TotalAmount).ToString();
            RQDTL.TotalCount = summary.TotalCount;
            RQDTL.Reserve = "";
        }

        public void SetIBData(List<InterBankInterestSettleInfo> collection)
        {
            if (collection == null)
            {
                return;
            }
            int offset = 0;
            int total = collection.Count;
            if (IBDATACollection == null)
            {
                IBDATACollection = new List<IBDATA_MsgHandler>();
            }
            int index = 0;
            while (offset < total)
            {
                List<InterBankInterestSettleInfo> filter = collection.Skip(offset).Take(CommonDataHelper.INTER_BANK_INTEREST_CAPABILITY).ToList();
                offset += filter.Count;
                
                InterBankInterestIBData ibdata = new InterBankInterestIBData(filter.Count);
                
                foreach (var item in filter)
                {
                    InterBankInterestIBDataRecord data = new InterBankInterestIBDataRecord(item.AICollection.Count);
                    ibdata.RecordLength = data.TOTAL_WIDTH;
                    data.AccountNO = item.AccountNO;
                    data.Interest = new decimal(item.Interest).ToString();
                    data.InterestAccount = item.InterestAccount;
                    data.PackageID = ++index;
                    data.RecordDate = item.RecordDate.ToString("yyyyMMdd");
                    data.TermFlag = ((int)item.TermFlag).ToString();
                    data.ValueDate = item.ValueDate.ToString("yyyyMMdd");
                    data.Extend = "";
                    foreach (var aiitem in item.AICollection)
                    {
                        InterBankInterestReqAIData aidata = new InterBankInterestReqAIData();
                        aidata.BeginData = aiitem.BeginDate.ToString("yyyyMMdd");
                        aidata.EndData = aiitem.EndDate.ToString("yyyyMMdd");
                        aidata.Aggregate = new decimal(aiitem.Aggregate).ToString();
                        aidata.InterestRate = new decimal(aiitem.Rate).ToString();
                        aidata.Interest = new decimal(aiitem.Interest).ToString();
                        data.AICollection.Add(aidata);
                    }
                    ibdata.RecordCollection.Add(data);
                }
                
                IBDATACollection.Add(ibdata);
            }
        }

        public override bool OnArgumentsValidation()
        {
            StringBuilder msg = new StringBuilder();
            foreach (var acount in IBDATACollection)
            {
                foreach (var record in acount.RecordCollection)
                {
                    InterBankInterestIBDataRecord ibrecord = record as InterBankInterestIBDataRecord;
                    if (ibrecord != null)
                    {
                        if (ibrecord.AICollection.Count > 20)
                        {
                            msg.AppendFormat("当前同业活期账户（{0}）结息明细笔数（{1}）已经超过20笔！", ibrecord.AccountNO, ibrecord.AICollection.Count);
                        }
                    }

                }
            }
            if (msg.Length > 0)
            {
                throw new BizArgumentsException(msg.ToString());
            }
            return true;
        }

    }

    /// <summary>
    /// RQDTL结构
    /// </summary>
    public class InterBankInterestRQDTL : IMessageReqHandler
    {
        public int TOTAL_WIDTH
        {
            get
            {
                return 164;
            }
        }

        #region Property
        /// <summary>
        /// 批号,15
        /// </summary>
        public string KPSN
        {
            get;
            set;
        }
        /// <summary>
        /// 总金额,17
        /// </summary>
        public string TotalAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 总笔数,8
        /// </summary>
        public int TotalCount
        {
            get;
            set;
        }
        /// <summary>
        /// 摘要代码,4
        /// </summary>
        public string SummaryCode
        {
            get
            {
                return CommonDataHelper.INTER_BANK_INTEREST_SUMMARY_CODE;
            }
           
        }
        /// <summary>
        /// 批量名称,80
        /// </summary>
        public string BatchName
        {
            get;
            set;
        }
        /// <summary>
        /// 备用,40
        /// </summary>
        public string Reserve
        {
            get;
            set;
        }
        #endregion

        #region IMessageReqHandler Members

        public byte[] ToBytes()
        {
            int totalLen = 0;
            byte[] bytes = new byte[TOTAL_WIDTH];
            StringBuilder sb = new StringBuilder();
        
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(KPSN, 15));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(TotalAmount, 17));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(TotalCount.ToString(), 8));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(SummaryCode, 4));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(BatchName, 80));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(Reserve, 40));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);

            
            return bytes;
        }

        #endregion
    }

    public class InterBankInterestIBData : IBDATA_MsgHandler
    {
        public InterBankInterestIBData(int count)
        {
            RecordID = CommonDataHelper.INTER_BANK_INTEREST_RECORD_ID;
            if (count > CommonDataHelper.INTER_BANK_INTEREST_CAPABILITY || count < 0)
            {
                count = CommonDataHelper.INTER_BANK_INTEREST_CAPABILITY;
            }
            //RecordLength = InterBankInterestReqSettleData
            RecordCollection = new List<IBDATA_Record>();
        }
    }

    /// <summary>
    /// 结息结构
    /// </summary>
    public class InterBankInterestIBDataRecord : IBDATA_Record
    {
        private List<InterBankInterestReqAIData> _AICollection;
        public List<InterBankInterestReqAIData> AICollection
        {
            get
            {
                return _AICollection;
            }
            set
            {
                _AICollection = value;
            }
        }
        
        public InterBankInterestIBDataRecord(int count)
        {
            if (count > CommonDataHelper.INTER_BANK_INTEREST_MAX_RECORD_COUNT || count < 0)
            {
                count = CommonDataHelper.INTER_BANK_INTEREST_MAX_RECORD_COUNT;
            }
            _AICollection = new List<InterBankInterestReqAIData>(count);
            TOTAL_WIDTH = 112 + CommonDataHelper.INTER_BANK_INTEREST_MAX_RECORD_COUNT * InterBankInterestReqAIData.TOTAL_WIDTH;
        }

        #region Property
        /// <summary>
        /// 包的顺序号,8
        /// </summary>
        public int PackageID
        {
            get;
            set;
        }
        /// <summary>
        /// 活期定期标志;1活 2定
        /// </summary>
        public string TermFlag
        {
            get;
            set;
        }
        /// <summary>
        /// 交易起息日,8
        /// </summary>
        public string ValueDate
        {
            get;
            set;
        }
        /// <summary>
        /// 交易记账日期,8
        /// </summary>
        public string RecordDate
        {
            get;
            set;
        }
        /// <summary>
        /// 入账账号,22
        /// </summary>
        public string AccountNO
        {
            get;
            set;
        }
        /// <summary>
        /// 产生利息账号,22
        /// </summary>
        public string InterestAccount
        {
            get;
            set;
        }
        /// <summary>
        /// 币种,3
        /// </summary>
        public string Currency
        {
            get
            {
                return "CNY";
            }
        }
        /// <summary>
        /// 钞汇,1
        /// </summary>
        public string ParperFlag
        {
            get
            {
                return "1";
            }
        }
        /// <summary>
        /// 利息,17
        /// </summary>
        public string Interest
        {
            get;
            set;
        }
        /// <summary>
        /// 备用,20
        /// </summary>
        public string Extend
        {
            get;
            set;
        }
        /// <summary>
        /// 计息个数,2
        /// </summary>
        public int InterestCount
        {
            get
            {
                return _AICollection.Count;
            }

        }
        #endregion

        public override byte[] ToBytes()
        {
            int totalLen = 0;
            byte[] bytes = new byte[TOTAL_WIDTH];

            StringBuilder sb = new StringBuilder();
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(PackageID.ToString(), 8));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(TermFlag, 1));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(ValueDate, 8));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(RecordDate, 8));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(AccountNO, 22));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(InterestAccount, 22));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(Currency, 3));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(ParperFlag, 1));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(Interest, 17));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(Extend, 20));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(InterestCount.ToString(), 2));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);

            foreach (var item in AICollection)
            {
                Array.Copy(item.ToBytes(), 0, bytes, totalLen, InterBankInterestReqAIData.TOTAL_WIDTH);
                totalLen += InterBankInterestReqAIData.TOTAL_WIDTH;
            }
            int remain = CommonDataHelper.INTER_BANK_INTEREST_MAX_RECORD_COUNT - AICollection.Count;
            if (remain > 0)
            {
                sb = new StringBuilder(" ", remain * InterBankInterestReqAIData.TOTAL_WIDTH);
                CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
                totalLen += remain * InterBankInterestReqAIData.TOTAL_WIDTH;
            }

            return bytes;
        }
    }
    
    /// <summary>
    /// 计息结构
    /// </summary>
    public class InterBankInterestReqAIData : IMessageReqHandler
    {
        public static readonly int TOTAL_WIDTH = 63;
        #region Property
        /// <summary>
        /// 开始日期
        /// </summary>
        public string BeginData
        {
            get;
            set;
        }
        /// <summary>
        /// 结束日期
        /// </summary>
        public string EndData
        {
            get;
            set;
        }
        
        /// <summary>
        /// 利率,11
        /// </summary>
        public string InterestRate
        {
            get;
            set;
        }
        /// <summary>
        /// 积数,19
        /// </summary>
        public string Aggregate
        {
            get;
            set;
        }
        /// <summary>
        /// 利息 17
        /// </summary>
        public string Interest
        {
            get;
            set;
        }
        #endregion
        #region IMessageReqHandler Members

        public byte[] ToBytes()
        {
            int totalLen = 0;
            byte[] bytes = new byte[TOTAL_WIDTH];

            StringBuilder sb = new StringBuilder();
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(BeginData, 8));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(EndData, 8));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(InterestRate, 11));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(Aggregate, 19));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(Interest, 17));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            
            return bytes;
        }

        #endregion
    }

}
