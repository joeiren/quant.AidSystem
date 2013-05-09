using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class PayBankInfoRP : IMessageRespHandler
    {
        public uint TOTAL_WIDTH
        {
            get
            {
                return (uint)(90 + (PayBankInfoItemRP.TOTAL_WIDTH + 1) * BankCount);
            }
            
        }
        #region Property
        /// <summary>
        /// 交易结果,10;00—成功,其他—失败,X2
        /// </summary>
        public String RetCode
        {
            get;
            set;
        }
        /// <summary>
        /// 交易结果描述,X80
        /// </summary>
        public String RetMsg
        {
            get;
            set;
        }
        /// <summary>
        /// 记录条数
        /// </summary>
        public int BankCount
        {
            get;
            set;
        }

        public List<PayBankInfoItemRP> BankList
        {
            get;
            set;
        }
        #endregion
        public PayBankInfoRP()
        {
            BankList = new List<PayBankInfoItemRP>();
        }

        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            if (messagebytes.Length >= TOTAL_WIDTH - PaymentBizMsgDataBase.HEADER_WIDTH)
            {
                RetCode = CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 10);
                RetMsg = CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 80);
                string number = CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 8);
                int count = 0;
                if (int.TryParse(number, out count))
                {
                    BankCount = count;
                }

                int offset = 0;
                while (offset < messagebytes.Length && messagebytes.Length - offset > PayBankInfoItemRP.TOTAL_WIDTH && BankList.Count < BankCount)
                {
                    byte[] dbbytes = CommonDataHelper.SubBytes(messagebytes, offset, PayBankInfoItemRP.TOTAL_WIDTH + 1);
                    PayBankInfoItemRP item = new PayBankInfoItemRP();
                    item.FromBytes(dbbytes);
                    BankList.Add(item);
                    offset += PayBankInfoItemRP.TOTAL_WIDTH + 1;
                }
            }
            return this;
        }

        #endregion
    }

    public class PayBankInfoItemRP:IMessageRespHandler
    {
        public const int TOTAL_WIDTH = 182;

         #region Property
        /// <summary>
        /// 行号,12
        /// </summary>
        public String BankNO
        {
            get;
            set;
        }
        /// <summary>
        /// 行名,60
        /// </summary>
        public String BankName
        {
            get;
            set;
        }
        /// <summary>
        ///所属直接参与者,12
        /// </summary>
        public String DirectParticipator
        {
            get;
            set;
        }
        /// <summary>
        /// 节点代码,4
        /// </summary>
        public String NodeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 城市代码,4
        /// </summary>
        public String CityCode
        {
            get;
            set;
        }
        /// <summary>
        /// 电话,30
        /// </summary>
        public String TelephoneNO
        {
            get;
            set;
        }
        /// <summary>
        /// 地址，60
        /// </summary>
        public String Address
        {
            get;
            set;
        }
        #endregion

    
        #region IMessageRespHandler Members

        public object  FromBytes(byte[] messagebytes)
        {
 	        if (messagebytes.Length > TOTAL_WIDTH)
            {
                BankNO = CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 12);
                BankName = CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 60);
                DirectParticipator = CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 12);
                NodeCode = CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 4);
                CityCode = CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 4);
                TelephoneNO = CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 30);
                Address = CommonDataHelper.GetValueFromGBKBytes(ref messagebytes, 60);
            }
            return this;
        }

        #endregion
    }
}
