using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace xQuant.AidSystem.CoreMessageData
{
    public class CoreDataBlockHeader : IMessageReqHandler, IMessageRespHandler
    {
        #region Field & Property
        public const UInt16 DB_LENGTH_WIDTH = 6;
        public const UInt16 DB_ID_WIDTH = 8;
        public const UInt16 TOTAL_WIDTH = DB_LENGTH_WIDTH + DB_ID_WIDTH;

        //数据块长度;6位长度
        private UInt32 _dbLength;
        public UInt32 DBH_DB_LENGTH
        {
            get
            {
                return _dbLength;
            }
            set
            {
                _dbLength = value;
            }
        }
        //数据块 ID;8位长度
        public String DBH_DB_ID
        {
            get;
            set;
        }
        #endregion


        #region IMessageReqHandler Members

        public byte[] ToBytes()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(DBH_DB_LENGTH.ToString().PadLeft(DB_LENGTH_WIDTH));
            sb.Append(CommonDataHelper.FillSpecifyWidthString(DBH_DB_ID, DB_ID_WIDTH));
            byte[] bytes = new byte[TOTAL_WIDTH * 2];
            int len = EBCDICEncoder.WideCharToEBCDIC(EBCDICEncoder.CCSID_IBM_1388, sb.ToString(), sb.Length, bytes, bytes.Length);
            if (len != bytes.Length)
            {
                return CommonDataHelper.SubBytes(bytes, 0, len);
            }
            return bytes;
        }

        #endregion

        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            if (messagebytes.Length >= TOTAL_WIDTH)
            {
                
                byte[] buffer = new byte[CoreDataBlockHeader.TOTAL_WIDTH];
                Array.Copy(messagebytes, buffer, CoreDataBlockHeader.TOTAL_WIDTH);
                String result = CommonDataHelper.GetValueFromBytes(ref buffer, CoreDataBlockHeader.TOTAL_WIDTH);

                if (result.Length >= TOTAL_WIDTH)
                {
                    UInt32.TryParse(result.Substring(0, 6), out _dbLength);
                    DBH_DB_ID = result.Substring(6, 8);
                }
                
            }
            return this;
        }

        #endregion
    }
}
