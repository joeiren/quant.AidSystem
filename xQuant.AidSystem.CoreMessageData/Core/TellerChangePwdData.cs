using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class TellerChangePwdData : CoreBizMsgDataBase
    {
        #region 构造函数
        public TellerChangePwdData()
            : base()
        {
            RQDTL = new TellerChangePwdRQDTL();
        }
    
        #endregion

        public override UInt32 RQ_TOTAL_WIDTH
        {
            get
            {
                return CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH + TellerChangePwdRQDTL.TOTAL_WIDTH;
            }
        }

        /// <summary>
        /// RQDTL
        /// </summary>
        public TellerChangePwdRQDTL RQDTL
        {
            get;
            set;
        }


        /// <summary>
        /// 把RQDTL转换成二进制数据流
        /// </summary>
        /// <param name="dest"></param>
        /// <returns></returns>
        protected override byte[] RQDTL_ToBytes(byte[] dest)
        {
            Array.Copy(RQDTL.ToBytes(), 0, dest, CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH, TellerChangePwdRQDTL.TOTAL_WIDTH);
            return dest;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        protected override void ODATA_FromBytes(byte[] buffer)
        {
            throw new AidException("没有返回的报文");
            
        }

        /// <summary>
        /// RQDTL报文内容长度
        /// </summary>
        /// <returns></returns>
        protected override ushort GetRQDTLLen()
        {
            return TellerChangePwdRQDTL.TOTAL_WIDTH ;
        }

        /// <summary>
        /// ODATA（返回报文）内容长度
        /// </summary>
        /// <returns></returns>
        protected override ushort GetODATALen()
        {
            return 0;
        }
    }
}
