using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public abstract class CoreBizMsgDataBase : BizMsgDataBase, IMessageRespHandler, IMessageReqHandler 
    {
        public RPHDR_MsgHandler RPhdrHandler
        {
            get;
            set;
        }
        public OMSG_MsgHandler OmsgHandler
        {
            get;
            set;
        }
        public SYSERR_MsgHandler SyserrHandler
        {
            get;
            set;
        }
        public RQHDR_MsgHandler RQhdrHandler
        {
            get;
            set;
        }

        public override UInt32 RQ_TOTAL_WIDTH
        {
            get;
            set;
        }

        public override UInt32 RP_TOTAL_WIDTH
        {
            get;
            set;
        }

        private bool _lastFlag = true;
        public bool MessageHeaderLastFlag
        {
            get
            {
                return _lastFlag;
            }
            set
            {
                _lastFlag = value;
            }
        }

        protected CoreBizMsgDataBase()
        {
            RPhdrHandler = new RPHDR_MsgHandler();
            OmsgHandler = new OMSG_MsgHandler();
            SyserrHandler = new SYSERR_MsgHandler();
            RQhdrHandler = new RQHDR_MsgHandler();
        }

        protected abstract byte[] RQDTL_ToBytes(byte[] dest);

        protected abstract void ODATA_FromBytes(byte[] buffer);

        protected abstract UInt16 GetRQDTLLen();

        protected abstract UInt16 GetODATALen();
        

        #region IMessageReqHandler Members

        public byte[] ToBytes()
        {
            //统一编码消息头 MH

            CoreMessageHeader msgHeader = new CoreMessageHeader();
            msgHeader.MH_MESSAGE_LENGTH = CoreMessageHeader.TOTAL_WIDTH + RQ_TOTAL_WIDTH;
            msgHeader.MH_LAST_FLAG = MessageHeaderLastFlag;
            byte[] buffer = new byte[msgHeader.MH_MESSAGE_LENGTH];
            Array.Copy(msgHeader.ToBytes(), buffer, CoreMessageHeader.TOTAL_WIDTH);

            byte[] rqtotal = new byte[RQ_TOTAL_WIDTH];
            CoreDataBlockHeader dbhdr1 = new CoreDataBlockHeader();
            dbhdr1.DBH_DB_LENGTH = CoreDataBlockHeader.TOTAL_WIDTH + RQHDR_MsgHandler.TOTAL_WIDTH;
            dbhdr1.DBH_DB_ID = "@RQHDR";
            Array.Copy(dbhdr1.ToBytes(), rqtotal, CoreDataBlockHeader.TOTAL_WIDTH);
            Array.Copy(RQhdrHandler.ToBytes(), 0, rqtotal, CoreDataBlockHeader.TOTAL_WIDTH, RQHDR_MsgHandler.TOTAL_WIDTH);

            if (GetRQDTLLen() > 0)
            {
                CoreDataBlockHeader dbhdr2 = new CoreDataBlockHeader();
                dbhdr2.DBH_DB_LENGTH = (UInt32)(CoreDataBlockHeader.TOTAL_WIDTH + GetRQDTLLen());
                dbhdr2.DBH_DB_ID = "@RQDTL";
                Array.Copy(dbhdr2.ToBytes(), 0, rqtotal, CoreDataBlockHeader.TOTAL_WIDTH + RQHDR_MsgHandler.TOTAL_WIDTH, CoreDataBlockHeader.TOTAL_WIDTH);
                rqtotal = RQDTL_ToBytes(rqtotal);
            }
            Array.Copy(rqtotal, 0, buffer, CoreMessageHeader.TOTAL_WIDTH, RQ_TOTAL_WIDTH);
            return buffer;
        }

        #endregion

        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            // MessageHeader
            byte[] buffer = new byte[CoreMessageHeader.TOTAL_WIDTH];
            if (messagebytes.Length >= buffer.Length)
            {
                Array.Copy(messagebytes, buffer, buffer.Length);
            }
            else
            {
                return null;
            }
            CoreMessageHeader msgHeader = new CoreMessageHeader();
            msgHeader.FromBytes(buffer);

            UInt32 mbLen = msgHeader.MH_MESSAGE_LENGTH - CoreMessageHeader.TOTAL_WIDTH;
            messagebytes = CommonDataHelper.SubBytes(messagebytes, (int)CoreMessageHeader.TOTAL_WIDTH, (int)mbLen);

            // MessageBody
            int len = messagebytes.Length;
            int offset = 0;

            while (len > 0)
            {
                buffer = new byte[CoreDataBlockHeader.TOTAL_WIDTH];
                Array.Copy(messagebytes, offset, buffer, 0, CoreDataBlockHeader.TOTAL_WIDTH);
                CoreDataBlockHeader dbhdr1 = new CoreDataBlockHeader();
                dbhdr1 = (CoreDataBlockHeader)dbhdr1.FromBytes(buffer);
                len -= CoreDataBlockHeader.TOTAL_WIDTH;
                offset += CoreDataBlockHeader.TOTAL_WIDTH;
                if (len > 0)
                {
                    switch (dbhdr1.DBH_DB_ID.Trim())
                    {
                        case "@RPHDR":
                            buffer = new byte[RPHDR_MsgHandler.TOTAL_WIDTH];
                            Array.Copy(messagebytes, offset, buffer, 0, RPHDR_MsgHandler.TOTAL_WIDTH);
                            RPhdrHandler = (RPHDR_MsgHandler)RPhdrHandler.FromBytes(buffer);
                            len -= RPHDR_MsgHandler.TOTAL_WIDTH;
                            offset += RPHDR_MsgHandler.TOTAL_WIDTH;
                            break;

                        case "@ODATA":
                            //buffer = new byte[GetODATALen()];
                            UInt16 odataLen = (UInt16)(dbhdr1.DBH_DB_LENGTH - CoreDataBlockHeader.TOTAL_WIDTH);
                            buffer = new byte[odataLen];
                            Array.Copy(messagebytes, offset, buffer, 0, odataLen);
                            //Array.Copy(messagebytes, offset, buffer, 0, GetODATALen());
                            //OData = (RetrieveCstmODATA)OData.FromBytes(buffer);
                            ODATA_FromBytes(buffer);
                            len -= odataLen;
                            offset += odataLen;
                            //len -= GetODATALen();
                            //offset += GetODATALen();
                            break;
                        //case "@OBDATA":
                        //    UInt32 obdataLen = dbhdr1.DBH_DB_LENGTH - CoreDataBlockHeader.TOTAL_WIDTH;
                        //    buffer = new byte[obdataLen];
                        //    Array.Copy(messagebytes, offset, buffer, 0, obdataLen);
                        //    OBDATA_FromBytes(buffer);
                        //    len -= (Int32)obdataLen;
                        //    offset += (Int32)obdataLen;
                        //    break;

                        case "@OMSG":
                            UInt32 omsgLen = dbhdr1.DBH_DB_LENGTH - CoreDataBlockHeader.TOTAL_WIDTH;
                            buffer = new byte[omsgLen];
                            Array.Copy(messagebytes, offset, buffer, 0, omsgLen);
                            OmsgHandler = (OMSG_MsgHandler)OmsgHandler.FromBytes(buffer);
                            len -= (int)omsgLen;
                            offset += (int)omsgLen;
                            break;

                        case "@SYSERR":
                            int syserrLen = (int)dbhdr1.DBH_DB_LENGTH - CoreDataBlockHeader.TOTAL_WIDTH;
                            buffer = new byte[syserrLen];
                            //ms.Read(buffer, offset, (int)syserrLen);
                            Array.Copy(messagebytes, offset, buffer, 0, syserrLen);
                            SyserrHandler = (SYSERR_MsgHandler)SyserrHandler.FromBytes(buffer);
                            len -= (int)syserrLen;
                            offset += (int)syserrLen;
                            break;
                        default:
                            len--;
                            break;

                    }
                }
            }

            return this;
        }

        #endregion
    }
}
