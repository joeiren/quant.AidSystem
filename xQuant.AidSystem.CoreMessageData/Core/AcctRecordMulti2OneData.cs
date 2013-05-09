using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class AcctRecordMulti2OneData :CoreBizMsgDataBase
    {
        public override UInt32 RQ_TOTAL_WIDTH
        {
            get
            {
                // DBH + RQHDR + DBH + RQDTL
                return CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH + RQDTL.TOTAL_WIDTH;
            }
        }

        public AcctRecordMulti2OneODATA OData
        {
            get;
            set;
        }
        public AcctRecordMulti2OneRQDTL RQDTL
        {
            get;
            set;
        }

        public AcctRecordMulti2OneData()
            : base()
        {
            OData = new AcctRecordMulti2OneODATA();
            RQDTL = new AcctRecordMulti2OneRQDTL();
        }

        protected override byte[] RQDTL_ToBytes(byte[] dest)
        {
            Array.Copy(RQDTL.ToBytes(), 0, dest, CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH, RQDTL.TOTAL_WIDTH);
            return dest;
        }

        protected override void ODATA_FromBytes(byte[] buffer)
        {
            OData = (AcctRecordMulti2OneODATA)OData.FromBytes(buffer);
        }

        protected override ushort GetRQDTLLen()
        {
            return (UInt16)RQDTL.TOTAL_WIDTH;
        }

        protected override ushort GetODATALen()
        {
            return AcctRecordMulti2OneODATA.TOTAL_WIDTH;
        }

         public override bool OnArgumentsValidation()
        {
            StringBuilder msg = new StringBuilder();
            if (string.IsNullOrEmpty(RQDTL.BGR33NO181))
            {
                msg.Append("资金流水号不能为空！");
            }
            else if (RQDTL.BGR33NO181.Length != 18)
            {
                msg.Append("资金流水号必须为18位！");
            }

            if (RQDTL.EntryList != null && RQDTL.EntryList.Count > 0)
            {
                foreach (var item in RQDTL.EntryList)
                {
                    if (string.IsNullOrEmpty(item.BGR33SN021))
                    {
                        msg.Append("套内序号不能为空！");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(item.BGR33ACID1))
                        {
                            msg.AppendFormat("套内序号为{0}的分录：核算码不能为空！", item.BGR33SN021);
                        }
                        if (string.IsNullOrEmpty(item.BGR33AMT1))
                        {
                            msg.AppendFormat("套内序号为{0}的分录：发生金额不能为空！", item.BGR33AMT1);
                        }
                        if (string.IsNullOrEmpty(item.BGR33SN041))
                        {
                            msg.AppendFormat("套内序号为{0}的分录：内部账序号不能为空！", item.BGR33SN041);
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
}
