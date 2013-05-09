/*
* 版权所有：杭州衡泰软件
* 作    者：林尚都(linshangdou@yahoo.com.cn)
* 创建时间：2011-5-12 17:53:36
* 版 本 号：1.0.0
* 模块说明：
* ----------------------------------
* 修改记录：
* 日    期：
* 版 本 号：
* 修 改 人：
* 修改内容：
* 
*/


using System;
using System.Text;
using System.Linq;

namespace xQuant.AidSystem.CoreMessageData
{
    public class InterBankDeleteAcctData : CoreBizMsgDataBase
    {
        public override UInt32 RQ_TOTAL_WIDTH
        {
            get
            {
                // DBH + RQHDR + DBH + RQDTL
                return CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH + InterBankDeleteAcctRQDTL.TOTAL_WIDTH;
            }
        }

        public InterBankDeleteAcctRQDTL RQDTL { get; set; }

        public InterBankDeleteAcctData()
            : base()
        {
            RQDTL = new InterBankDeleteAcctRQDTL();
        }

        #region Implemented Methods from Base class

        protected override byte[] RQDTL_ToBytes(byte[] dest)
        {
            Array.Copy(RQDTL.ToBytes(), 0, dest, CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH, InterBankDeleteAcctRQDTL.TOTAL_WIDTH);
            return dest;
        }

        protected override void ODATA_FromBytes(byte[] buffer)
        {
            throw new NotImplementedException();
        }

        protected override ushort GetRQDTLLen()
        {
            return InterBankDeleteAcctRQDTL.TOTAL_WIDTH;
        }

        protected override ushort GetODATALen()
        {
            throw new NotImplementedException();
        }

        public override bool OnArgumentsValidation()
        {
            StringBuilder msg = new StringBuilder();
            if (string.IsNullOrEmpty(RQDTL.ACCOUNT_DATE))
            {
                msg.Append("会计日期不能为空！");
            }
            if (string.IsNullOrEmpty(RQDTL.NOTICE_NO))
            {
                msg.Append("通知单编号不能为空！");
            }
            if (RQDTL.OPERATE_TYPE == "1" && string.IsNullOrEmpty(RQDTL.ACCOUNT))
            {
                msg.Append("账号不能为空！");
            }
            if (RQDTL.OPERATE_TYPE == "1" && RQDTL.BUSINESS_TYPE == "2" && string.IsNullOrEmpty(RQDTL.INTEREST_ACCOUNT))
            {
                msg.Append("收息账号不能为空！");
            }
            if (RQDTL.OPERATE_TYPE == "1" && RQDTL.BUSINESS_TYPE == "2" && RQDTL.AMOUNT<=0)
            {
                msg.Append("交易金额要大于0！");
            }
            if (RQDTL.OPERATE_TYPE == "1" && RQDTL.BUSINESS_TYPE == "2" && RQDTL.DETAIL_NUMBER > 0)
            {
                double interestSum = (from ai in RQDTL.DETAILS
                                      select ai.INTEREST).Sum();
                if (Math.Abs(interestSum - RQDTL.INTEREST) > 0.0001)
                {
                    msg.Append("利息金额与计息结构的利息之和不相等！");
                }
            }
            if (RQDTL.OPERATE_TYPE == "1" && (RQDTL.DETAILS != null &&RQDTL.DETAILS.Count > 20))
            {
                msg.Append("计息明细最多不能超过20个！");
            }
            
            if (msg.Length > 0)
            {
                throw new BizArgumentsException(msg.ToString());
            }

            return true;
        }

        #endregion
    }
}
