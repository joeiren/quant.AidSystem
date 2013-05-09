using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class RetrieveCstmBaseInfoODATA : IMessageRespHandler
    {
        public const UInt16 TOTAL_WIDTH = CoreDataBlockHeader.TOTAL_WIDTH;

        public List<RetrieveCstmBaseInfoODATA_Item> CstmBaseInfoList
        {
            get;
            set;
        }


        public RetrieveCstmBaseInfoODATA()
        {
            CstmBaseInfoList = new List<RetrieveCstmBaseInfoODATA_Item>();
        }

        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            if (messagebytes.Length >= CoreDataBlockHeader.TOTAL_WIDTH)
            {
                CoreDataBlockHeader dbhdr = new CoreDataBlockHeader();
                int offset = 0;
                while (offset < messagebytes.Length)
                {
                    byte[] dbbytes = CommonDataHelper.SubBytes(messagebytes, offset, messagebytes.Length - offset);
                    dbhdr = (CoreDataBlockHeader)dbhdr.FromBytes(dbbytes);
                    if (dbhdr == null)
                    {
                        offset += CoreDataBlockHeader.TOTAL_WIDTH;
                        continue;
                    }
                    RetrieveCstmBaseInfoODATA_Item item = new RetrieveCstmBaseInfoODATA_Item();
                    item = (RetrieveCstmBaseInfoODATA_Item)item.FromBytes(dbbytes);
                    offset += RetrieveCstmBaseInfoODATA_Item.TOTAL_WIDTH;
                    CstmBaseInfoList.Add(item);
                }
            }
            
            return this;
        }

        #endregion
    }

    public class RetrieveCstmBaseInfoODATA_Item : IMessageRespHandler
    {
        public const UInt16 TOTAL_WIDTH = 195 + CoreDataBlockHeader.TOTAL_WIDTH;
        #region
        /// <summary>
        /// 客户内码,11
        /// </summary>
        public String CUS_CDE
        {
            get;
            set;
        }
        /// <summary>
        /// 客户名称,80
        /// </summary>
        public String CUS_NAM
        {
            get;
            set;
        }
        /// <summary>
        /// 联系地址,80
        /// </summary>
        public String ADDR
        {
            get;
            set;
        }
        /// <summary>
        /// 联系地址序号,2
        /// </summary>
        public String ADSNPTSN
        {
            get;
            set;
        }
        /// <summary>
        /// 电话序号,2
        /// </summary>
        public String TEL_SN
        {
            get;
            set;
        }
        /// <summary>
        /// 电话号码,20
        /// </summary>
        public String TEL_NO
        {
            get;
            set;
        }
        #endregion

        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            if (messagebytes.Length >= TOTAL_WIDTH)
            {
                CoreDataBlockHeader dbhdr = new CoreDataBlockHeader();
                dbhdr = (CoreDataBlockHeader)dbhdr.FromBytes(messagebytes);
                messagebytes = CommonDataHelper.SubBytes(messagebytes, CoreDataBlockHeader.TOTAL_WIDTH, messagebytes.Length - CoreDataBlockHeader.TOTAL_WIDTH);
                CUS_CDE = CommonDataHelper.GetValueFromBytes(ref messagebytes, 11).TrimEnd();
                CUS_NAM = CommonDataHelper.GetValueFromBytes(ref messagebytes, 80).TrimEnd();
                ADDR = CommonDataHelper.GetValueFromBytes(ref messagebytes, 80).TrimEnd();
                ADSNPTSN = CommonDataHelper.GetValueFromBytes(ref messagebytes, 2).TrimEnd();
                TEL_SN = CommonDataHelper.GetValueFromBytes(ref messagebytes, 2).TrimEnd();
                TEL_NO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 20).TrimEnd();
            }

            return this;
        }

        #endregion
    }
}
