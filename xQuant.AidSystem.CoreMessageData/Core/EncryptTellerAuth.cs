using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class EncryptTellerAuth : CoreBizMsgDataBase
    {
        public TellerAuthData TellerData
        {
            get;
            set;
        }

        public EncryptPin TellerEncrypt
        {
            get;
            set;
        }

        public bool AfterEncrypt
        {
            get;
            set;
        }

        public override UInt32 RQ_TOTAL_WIDTH
        {
            get
            {
                return TellerData.RQ_TOTAL_WIDTH;
            }
            set
            { }

        }

        public override UInt32 RP_TOTAL_WIDTH
        {
            get
            {
                return TellerData.RP_TOTAL_WIDTH;
            }
            set
            { }
        }

        public EncryptTellerAuth() : base()
        {
            TellerData = new TellerAuthData();
            TellerEncrypt = new EncryptPin();
            base.OmsgHandler = TellerData.OmsgHandler;
            base.RPhdrHandler = TellerData.RPhdrHandler;
            base.RQhdrHandler = TellerData.RQhdrHandler;
            base.SyserrHandler = TellerData.SyserrHandler;
        }

        public EncryptTellerAuth(String tellerNO, String orgNO, DateTime TellerLogin, String password) : this()
        {
            TellerData.RQhdrHandler.TEL_ID = tellerNO;
            TellerData.RQhdrHandler.TX_OUNO = orgNO;
            TellerData.RQhdrHandler.TX_DTE = TellerLogin.ToString("yyyy-MM-dd");
            TellerData.RQDTL.STF_NO = tellerNO;
            TellerData.RQDTL.ORG_NO = orgNO;            
            TellerEncrypt.ReqPin = password;
        }

        public void EncryptPassword()
        {
            TellerEncrypt.RespPin = TellerEncrypt.ToBytes();
        }

        public byte[] LoginAuth()
        {
            TellerData.RQDTL.PIN_BLK = TellerEncrypt.RespPin;
            return TellerData.ToBytes();
        }

        protected override byte[] RQDTL_ToBytes(byte[] dest)
        {
            Array.Copy(TellerData.RQDTL.ToBytes(), 0, dest, CoreDataBlockHeader.TOTAL_WIDTH * 2 + RQHDR_MsgHandler.TOTAL_WIDTH, TellerAuthRQDTL.TOTAL_WIDTH);
            return dest;
        
        }

        protected override void ODATA_FromBytes(byte[] buffer)
        {
            TellerData.OData = (TellerAuthODATA)TellerData.OData.FromBytes(buffer);
            
        }

        protected override ushort GetRQDTLLen()
        {
            return TellerAuthRQDTL.TOTAL_WIDTH;
        }

        protected override ushort GetODATALen()
        {
            return TellerAuthODATA.TOTAL_WIDTH;
        }
    }
}
