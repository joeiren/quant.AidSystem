using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xQuant.AidSystem.BizDataModel;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 同业存放计提数据
    /// </summary>
    public class InterBankPreparedData : CoreBizMsgMultiReqDataBase
    {
        public InterBankPreparedData()
            : base()
        {
            RQDTL = new InterBankPreparedRQDTL();
        }

        public InterBankPreparedRQDTL RQDTL
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
                return (uint)(CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH + GetRQDTLLen());
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

        public void SetRQDTL(int totalCount)
        {
            RQDTL.TOTAL_COUNT = totalCount;
        }

        public void SetIBDATA(List<InterBankPreparedInfo> infolist)
        {
            if (infolist != null)
            {
                int offset = 0;
                int total = infolist.Count;
                if (IBDATACollection == null)
                {
                    IBDATACollection = new List<IBDATA_MsgHandler>();
                }
                while (offset < total)
                {
                    List<InterBankPreparedInfo> filter = infolist.Skip(offset).Take(CommonDataHelper.INTER_BANK_PREPARE_CAPABILITY).ToList();
                    offset += filter.Count;

                    InterBankPreparedIBData ibdata = new InterBankPreparedIBData(filter.Count);
                    foreach (var item in filter)
                    {
                        InterBankPreparedIBDataRecord data = new InterBankPreparedIBDataRecord();
                        ibdata.RecordLength = data.TOTAL_WIDTH;
                        data.AccountNO = item.AccountNO;
                        data.CurrentBalance = new decimal(item.CurrentBalance).ToString();
                        data.PreparedDate = item.PreparedDate.ToString("yyyyMMdd");
                        data.PreparedInterest =new decimal(item.PreparedInterest).ToString();
                        data.TermFlag = ((int)item.TermFlag).ToString();
                        ibdata.RecordCollection.Add(data);
                    }

                    IBDATACollection.Add(ibdata);
                }
            }
        }
    }

    public class InterBankPreparedRQDTL : IMessageReqHandler
    {
        public int TOTAL_WIDTH
        {
            get
            {
                return 8;
            }
        }
        /// <summary>
        /// 总比数，8
        /// </summary>
        public int TOTAL_COUNT
        {
            get;
            set;
        }
        #region IMessageReqHandler Members
        public byte[] ToBytes()
        {
            int totalLen = 0;
            byte[] bytes = new byte[TOTAL_WIDTH];
            StringBuilder sb = new StringBuilder();
        
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthFigure(TOTAL_COUNT, 8));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            return bytes;
        }
        #endregion
    }

    /// <summary>
    /// 多包结构数据
    /// </summary>
    public class InterBankPreparedIBData : IBDATA_MsgHandler
    {

        public InterBankPreparedIBData(int count)
        {
            RecordID = CommonDataHelper.INTER_BANK_PREPARE_RECORD_ID;
            if (count > CommonDataHelper.INTER_BANK_PREPARE_CAPABILITY || count < 0)
            {
                count = CommonDataHelper.INTER_BANK_PREPARE_CAPABILITY;
            }
            RecordCollection = new List<IBDATA_Record>(count);
        }
    }


    public class InterBankPreparedIBDataRecord : IBDATA_Record
    {
        public InterBankPreparedIBDataRecord()
        {
            TOTAL_WIDTH = 102;
        }

        #region Property
        /// <summary>
        /// 定活标志;1活 2定
        /// </summary>
        public string TermFlag
        {
            get;
            set;
        }
        /// <summary>
        /// 账号,15
        /// </summary>
        public string AccountNO
        {
            get;
            set;
        }
        /// <summary>
        /// 币种
        /// </summary>
        public string Currency
        {
            get
            {
                return "CNY";
            }
        }
        /// <summary>
        /// 钞汇,1-钞
        /// </summary>
        public string ParperFlag
        {
            get
            {
                return "1";
            }
        }
        /// <summary>
        /// 计提日期,8
        /// </summary>
        public string PreparedDate
        {
            get;
            set;
        }
        /// <summary>
        /// 计提利息,17
        /// </summary>
        public string PreparedInterest
        {
            get;
            set;
        }
        /// <summary>
        /// 当前余额,17
        /// </summary>
        public string CurrentBalance
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

        public override byte[] ToBytes()
        {
            int totalLen = 0;
            byte[] bytes = new byte[TOTAL_WIDTH];

            StringBuilder sb = new StringBuilder();
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(TermFlag, 1));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(AccountNO, 15));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(Currency, 3));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(ParperFlag, 1));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(PreparedDate, 8));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(PreparedInterest, 17));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(CurrentBalance, 17));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(Reserve, 40));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);

            return bytes;
        }

        #endregion
    }
}
