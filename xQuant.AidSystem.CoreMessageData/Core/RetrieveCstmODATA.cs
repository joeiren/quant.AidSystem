using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class RetrieveCstmODATA : IMessageRespHandler
    {
        public const UInt16 TOTAL_WIDTH = 409 + CoreDataBlockHeader.TOTAL_WIDTH;
        #region Property
        /// <summary>
        /// 客户内码,11
        /// </summary>
        public String CUS_CDE
        {
            get;
            set;
        }
        /// <summary>
        /// 客户号,23
        /// </summary>
        public String CUS_NO
        {
            get;
            set;
        }
        /// <summary>
        /// 客户类型,1
        /// </summary>
        public String CUS_TYP
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
        /// 其他名称,30
        /// </summary>
        public String CUS_ONAM
        {
            get;
            set;
        }
        /// <summary>
        /// 英文名称,80
        /// </summary>
        public String CUS_ENAM
        {
            get;
            set;
        }
        /// <summary>
        /// 国籍,3
        /// </summary>
        public String NATION
        {
            get;
            set;
        }
        /// <summary>
        /// 贵宾类型,1
        /// </summary>
        public String VIP_TYP
        {
            get;
            set;
        }
        /// <summary>
        /// 客户状态,1
        /// </summary>
        public String CUS_STS
        {
            get;
            set;
        }
        /// <summary>
        /// 地址序号,2
        /// </summary>
        public String ADD_SN
        {
            get;
            set;
        }
        /// <summary>
        ///  地址,80
        /// </summary>
        public String ADDR
        {
            get;
            set;
        }
        /// <summary>
        ///电话序号,2
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
        /// <summary>
        /// 手机号码,20
        /// </summary>
        public String MBL_NO
        {
            get;
            set;
        }
        /// <summary>
        /// 邮编,10
        /// </summary>
        public String ZIP
        {
            get;
            set;
        }
        /// <summary>
        /// 区域类型,3
        /// </summary>
        public String CMB_QYLX
        {
            get;
            set;
        }
        /// <summary>
        /// 建立机构,6
        /// </summary>
        public String CRT_PDT
        {
            get;
            set;
        }
        /// <summary>
        ///建立柜员,7
        /// </summary>
        public String CRT_OPR
        {
            get;
            set;
        }
        /// <summary>
        /// 建立日期,8
        /// </summary>
        public String CRT_DTE
        {
            get;
            set;
        }
        /// <summary>
        /// 更新机构,6
        /// </summary>
        public String UP_PDT
        {
            get;
            set;
        }
        /// <summary>
        /// 更新柜员,7
        /// </summary>
        public String UP_OPR
        {
            get;
            set;
        }
        /// <summary>
        /// 更新日期,8 
        /// </summary>
        public String UP_DTE
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
                CoreDataBlockHeader dbhdr1 = new CoreDataBlockHeader();
                dbhdr1 = (CoreDataBlockHeader)dbhdr1.FromBytes(messagebytes);
                messagebytes = CommonDataHelper.SubBytes(messagebytes, CoreDataBlockHeader.TOTAL_WIDTH, messagebytes.Length - CoreDataBlockHeader.TOTAL_WIDTH);
                CUS_CDE = CommonDataHelper.GetValueFromBytes(ref messagebytes, 11).TrimEnd();
                CUS_NO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 23).TrimEnd();
                CUS_TYP = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                CUS_NAM = CommonDataHelper.GetValueFromBytes(ref messagebytes, 80).TrimEnd();
                CUS_ONAM = CommonDataHelper.GetValueFromBytes(ref messagebytes, 30).TrimEnd();
                CUS_ENAM = CommonDataHelper.GetValueFromBytes(ref messagebytes, 80).TrimEnd();
                NATION = CommonDataHelper.GetValueFromBytes(ref messagebytes, 3).TrimEnd();
                VIP_TYP = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                CUS_STS = CommonDataHelper.GetValueFromBytes(ref messagebytes, 1).TrimEnd();
                ADD_SN = CommonDataHelper.GetValueFromBytes(ref messagebytes, 2).TrimEnd();
                ADDR = CommonDataHelper.GetValueFromBytes(ref messagebytes, 80).TrimEnd();
                TEL_SN = CommonDataHelper.GetValueFromBytes(ref messagebytes, 2).TrimEnd();
                TEL_NO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 20).TrimEnd();
                MBL_NO = CommonDataHelper.GetValueFromBytes(ref messagebytes, 20).TrimEnd();
                ZIP = CommonDataHelper.GetValueFromBytes(ref messagebytes, 10).TrimEnd();
                CMB_QYLX = CommonDataHelper.GetValueFromBytes(ref messagebytes, 3).TrimEnd();
                CRT_PDT = CommonDataHelper.GetValueFromBytes(ref messagebytes, 6).TrimEnd();
                CRT_OPR = CommonDataHelper.GetValueFromBytes(ref messagebytes, 7).TrimEnd();
                CRT_DTE = CommonDataHelper.GetValueFromBytes(ref messagebytes, 8).TrimEnd();
                UP_PDT = CommonDataHelper.GetValueFromBytes(ref messagebytes, 6).TrimEnd();
                UP_OPR = CommonDataHelper.GetValueFromBytes(ref messagebytes, 7).TrimEnd();
                UP_DTE = CommonDataHelper.GetValueFromBytes(ref messagebytes, 8).TrimEnd();
            }

            return this;
        }

        #endregion
    }
}
