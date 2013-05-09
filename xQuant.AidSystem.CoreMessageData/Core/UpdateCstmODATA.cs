using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class UpdateCstmODATA : IMessageRespHandler
    {
        public const UInt16 TOTAL_WIDTH = 11 + CoreDataBlockHeader.TOTAL_WIDTH;
        /// <summary>
        /// 客户内码,11
        /// </summary>
        public String CUS_CDE
        {
            get;
            set;
        }


        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            if (messagebytes.Length == TOTAL_WIDTH)
            {
                CoreDataBlockHeader dbhdr1 = new CoreDataBlockHeader();
                dbhdr1 = (CoreDataBlockHeader)dbhdr1.FromBytes(messagebytes);

                messagebytes = CommonDataHelper.SubBytes(messagebytes, CoreDataBlockHeader.TOTAL_WIDTH, messagebytes.Length - CoreDataBlockHeader.TOTAL_WIDTH);
                CUS_CDE = CommonDataHelper.GetValueFromBytes(ref messagebytes, 11).TrimEnd();
            }
            else
            {
                CUS_CDE = CommonDataHelper.GetValueFromBytes(ref messagebytes, (UInt16)messagebytes.Length).TrimEnd(); 
            }

            return this;
        }

        #endregion
    }
}
