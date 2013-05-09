using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public abstract class CoreBizMsgMultiRespDataBase : CoreBizMsgDataBase , IMessageRespHandler
    {
        protected abstract override byte[] RQDTL_ToBytes(byte[] dest);

        protected abstract override void ODATA_FromBytes(byte[] buffer);

        protected abstract override ushort GetRQDTLLen();

        protected abstract override ushort GetODATALen();

        //多包数据解析
        protected virtual void OBDATA_FromBytes(byte[] buffer)
        {
        }

        public new object FromBytes(byte[] messagebytes)
        {
            if (messagebytes != null && messagebytes.Length > 0)
            {
                bool lastFlag = false;
                int len = messagebytes.Length;
                int offset = 0;
                while (messagebytes.Length > 0 && !lastFlag)
                {
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
                    lastFlag = msgHeader.MH_LAST_FLAG;

                    int mbLen = (int)(msgHeader.MH_MESSAGE_LENGTH - CoreMessageHeader.TOTAL_WIDTH);
                    buffer = new byte[mbLen];

                    Array.Copy(messagebytes, CoreMessageHeader.TOTAL_WIDTH, buffer, 0, mbLen);

                    offset = (int)msgHeader.MH_MESSAGE_LENGTH;
                    //len -= offset;

                    DetailFromBytes(buffer);
                    messagebytes = CommonDataHelper.SubBytes(messagebytes, offset, messagebytes.Length - offset);
                }
            }
            
            return this;
        }

        public void DetailFromBytes(byte[] messagebytes)
        {
            if (messagebytes == null || messagebytes.Length < CoreDataBlockHeader.TOTAL_WIDTH)
            {
                return;
            }
            int len = messagebytes.Length;
            int offset = 0;

            while (len > 0)
            {
                byte[] buffer = new byte[CoreDataBlockHeader.TOTAL_WIDTH];
                Array.Copy(messagebytes, offset, buffer, 0, CoreDataBlockHeader.TOTAL_WIDTH);
                CoreDataBlockHeader dbhdr1 = new CoreDataBlockHeader();
                dbhdr1 = (CoreDataBlockHeader)dbhdr1.FromBytes(buffer);
               
                offset += CoreDataBlockHeader.TOTAL_WIDTH;
                len -= CoreDataBlockHeader.TOTAL_WIDTH;
                if (dbhdr1 == null)
                {
                    continue;
                }
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
                            UInt16 odataLen = (UInt16)(dbhdr1.DBH_DB_LENGTH - CoreDataBlockHeader.TOTAL_WIDTH);
                            buffer = new byte[odataLen];
                            Array.Copy(messagebytes, offset, buffer, 0, odataLen);
                            ODATA_FromBytes(buffer);
                            len -= odataLen;
                            offset += odataLen;
                            break;
                        case "@OBDATA":
                            UInt32 obdataLen = dbhdr1.DBH_DB_LENGTH - CoreDataBlockHeader.TOTAL_WIDTH;
                            buffer = new byte[obdataLen];
                            Array.Copy(messagebytes, offset, buffer, 0, obdataLen);
                            OBDATA_FromBytes(buffer);
                            len -= (Int32)obdataLen;
                            offset += (Int32)obdataLen;
                            break;

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
                            break;
                    }

                }
            }
        }

    }
}
