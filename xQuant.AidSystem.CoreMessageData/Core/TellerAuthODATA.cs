using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class TellerAuthODATA : IMessageRespHandler
    {
        public const UInt16 TOTAL_WIDTH = 8;

        /// <summary>
        /// 密码失效日期,8
        /// </summary>
        public String DUE_DATE
        {
            get;
            set;
        }

        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            if (messagebytes.Length == TOTAL_WIDTH || messagebytes.Length == TOTAL_WIDTH + CoreDataBlockHeader.TOTAL_WIDTH)
            {
                CoreDataBlockHeader dbhdr1 = new CoreDataBlockHeader();
                dbhdr1 = (CoreDataBlockHeader)dbhdr1.FromBytes(messagebytes);

                messagebytes = CommonDataHelper.SubBytes(messagebytes, CoreDataBlockHeader.TOTAL_WIDTH, messagebytes.Length - CoreDataBlockHeader.TOTAL_WIDTH);
                DUE_DATE = CommonDataHelper.GetValueFromBytes(ref messagebytes, 8).TrimEnd();
            }
            else
            {
                DUE_DATE = CommonDataHelper.GetValueFromBytes(ref messagebytes, (UInt16)messagebytes.Length).TrimEnd();
            }

            return this;
        }

        #endregion
    }
}
