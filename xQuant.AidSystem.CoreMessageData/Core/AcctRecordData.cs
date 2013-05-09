using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class AcctRecordData : CoreBizMsgDataBase
    {
        public override UInt32 RQ_TOTAL_WIDTH
        {
            get
            {
                // DBH + RQHDR + DBH + RQDTL
                return CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH + RQDTL.TOTAL_WIDTH;
            }
        }

        public AcctRecordODATA OData
        {
            get;
            set;
        }
        public AcctRecordRQDTL RQDTL
        {
            get;
            set;
        }

        public AcctRecordData()
            : base()
        {
            OData = new AcctRecordODATA();
            RQDTL = new AcctRecordRQDTL();
        }

        protected override byte[] RQDTL_ToBytes(byte[] dest)
        {
            Array.Copy(RQDTL.ToBytes(), 0, dest, CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH, RQDTL.TOTAL_WIDTH);
            return dest;
        }

        protected override void ODATA_FromBytes(byte[] buffer)
        {
            OData = (AcctRecordODATA)OData.FromBytes(buffer);
            if (OData.OdataRepeatItem != null && !string.IsNullOrEmpty(OData.OdataRepeatItem.CoreSN)) // 重复记账时，置换核心流水号
            {
                RPhdrHandler.SEQ_NO = OData.OdataRepeatItem.CoreSN;
            }
        }

        protected override ushort GetRQDTLLen()
        {
            return (UInt16)RQDTL.TOTAL_WIDTH;
        }

        protected override ushort GetODATALen()
        {
            return AcctRecordODATA.TOTAL_WIDTH;
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
                            msg.AppendFormat("套内序号为{0}的分录：发生金额不能为空！", item.BGR33SN021);
                        }
                        if (string.IsNullOrEmpty(item.BGR33SN041))
                        {
                            msg.AppendFormat("套内序号为{0}的分录：内部账序号不能为空！", item.BGR33SN021);
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
