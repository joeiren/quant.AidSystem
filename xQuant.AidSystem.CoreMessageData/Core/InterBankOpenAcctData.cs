/*
* 版权所有：杭州衡泰软件
* 作    者：林尚都(linshangdou@yahoo.com.cn)
* 创建时间：2011-5-12 10:44:54
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

namespace xQuant.AidSystem.CoreMessageData
{
    //活期定期开户
    public class InterBankOpenAcctData : CoreBizMsgDataBase
    {
        public override UInt32 RQ_TOTAL_WIDTH
        {
            get
            {
                // DBH + RQHDR + DBH + RQDTL
                return CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH + InterBankOpenAcctRQDTL.TOTAL_WIDTH;
            }
        }

        public InterBankOpenAcctRQDTL RQDTL { get; set; }

        public InterBankOpenAcctData() :base()
        {
            RQDTL=new InterBankOpenAcctRQDTL();
        }

        #region Implemented Methods from Base class

        protected override byte[] RQDTL_ToBytes(byte[] dest)
        {
            Array.Copy(RQDTL.ToBytes(), 0, dest, CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH, InterBankOpenAcctRQDTL.TOTAL_WIDTH);
            return dest;
        }

        protected override void ODATA_FromBytes(byte[] buffer)
        {
            throw new NotImplementedException();
        }

        protected override ushort GetRQDTLLen()
        {
            return InterBankOpenAcctRQDTL.TOTAL_WIDTH;
        }

        protected override ushort GetODATALen()
        {
            throw new NotImplementedException();
        }

        public override bool OnArgumentsValidation()
        {
            StringBuilder msg = new StringBuilder();
            if (string.IsNullOrEmpty(RQDTL.OPERATE_TYPE))
            {
                msg.Append("维护类型不能为空！");
            }
            if (string.IsNullOrEmpty(RQDTL.ACCOUNT_DATE))
            {
                msg.Append("会计日期不能为空！");
            }
            if (string.IsNullOrEmpty(RQDTL.NOTICE_NO))
            {
                msg.Append("通知单编号不能为空！");
            }
            if (string.IsNullOrEmpty(RQDTL.NOTICE_TYPE))
            {
                msg.Append("通知单类型不能为空！");
            }
            if (string.IsNullOrEmpty(RQDTL.BUSINESS_TYPE))
            {
                msg.Append("业务类型不能为空！");
            }
            if (RQDTL.OPERATE_TYPE == "1")
            {
                if (string.IsNullOrEmpty(RQDTL.CURRENCY))
                {
                    msg.Append("币种不能为空！");
                }
                if (string.IsNullOrEmpty(RQDTL.CASH_PROPERTY))
                {
                    msg.Append("钞汇属性不能为空！");
                }
                if (string.IsNullOrEmpty(RQDTL.PRODUCT_CODE))
                {
                    msg.Append("产品代码不能为空！");
                }
                if (string.IsNullOrEmpty(RQDTL.PRODUCT_TYPE))
                {
                    msg.Append("产品类别不能为空！");
                }
                
                if (string.IsNullOrEmpty(RQDTL.CUSTOMER_CODE))
                {
                    msg.Append("客户内码不能为空！");
                }
                if (RQDTL.BUSINESS_TYPE == "2" && RQDTL.AMOUNT <= 0)
                {
                    msg.Append("交易金额不能为0！");
                }
                if (RQDTL.BUSINESS_TYPE == "2" && (string.IsNullOrEmpty(RQDTL.CURRENT_ACCOUNT) || string.IsNullOrEmpty(RQDTL.INTEREST_ACCOUNT)))
                {
                    msg.Append("定期开户时资金来源活期账号或收息账号不能为空！");
                }               
                if (RQDTL.BUSINESS_TYPE == "2" && (string.IsNullOrEmpty(RQDTL.VALUE_DATE) || string.IsNullOrEmpty(RQDTL.MATURITY_DATE)))
                {
                    msg.Append("定期开户时起息日期或到期日期不能为空！");
                }
                if (RQDTL.BUSINESS_TYPE == "2" && !(string.IsNullOrEmpty(RQDTL.VALUE_DATE) || string.IsNullOrEmpty(RQDTL.MATURITY_DATE)))
                {
                    if (string.Compare(RQDTL.VALUE_DATE,RQDTL.MATURITY_DATE) >=0 )
                    {
                        msg.Append("定期开户时起息日期要小于到期日期！");
                    }
                }               
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
