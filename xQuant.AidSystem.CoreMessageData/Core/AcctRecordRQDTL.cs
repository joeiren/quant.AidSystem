using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    public class AcctRecordRQDTL : IMessageReqHandler
    {
        public UInt32 TOTAL_WIDTH
        {
            get 
            {
                return (UInt32)(22 + BGR33CHPG1 * AccountingEntry.TOTAL_WIDTH + BGR33CHPG2 * AccountingEliminate.TOTAL_WIDTH);
            }
        }
        /// <summary>
        /// 资金业务系统流水号,18
        /// </summary>
        public String BGR33NO181
        {
            get;
            set;
        }
        /// <summary>
        /// 套内分录笔数
        /// </summary>
        public UInt16 BGR33CHPG1
        {
            get;
            set;
        }

        /// <summary>
        /// 套内挂销账笔数
        /// </summary>
        public UInt16 BGR33CHPG2
        {
            get;
            set;
        }
        public List<AccountingEntry> EntryList
        {
            get;
            set;
        }
        public List<AccountingEliminate> EliminateList
        {
            get;
            set;
        }

        public AcctRecordRQDTL()
        {
            EntryList = new List<AccountingEntry>();
            EliminateList = new List<AccountingEliminate>();
        }

        #region IMessageReqHandler Members

        public byte[] ToBytes()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(CommonDataHelper.FillSpecifyWidthString(BGR33NO181, 18));
            sb.Append(BGR33CHPG1.ToString().PadLeft(2));
            sb.Append(BGR33CHPG2.ToString().PadLeft(2));

            byte[] byteBuffer = new byte[TOTAL_WIDTH];
            int offset = 0;
            byte[] temp = new byte[22];
            int len = EBCDICEncoder.WideCharToEBCDIC(EBCDICEncoder.CCSID_IBM_1388, sb.ToString(), sb.Length, temp, temp.Length);

            Array.Copy(temp, 0, byteBuffer, offset, 22);
            offset += 22;

            
            if (BGR33CHPG1 > 0 && BGR33CHPG1 == EntryList.Count)
            {
                
                foreach (AccountingEntry entry in EntryList)
                {
                    byte[] buffer = entry.ToBytes();
                    Array.Copy(buffer, 0, byteBuffer, offset, AccountingEntry.TOTAL_WIDTH);
                    offset += AccountingEntry.TOTAL_WIDTH;
                }
            }

            //销账，本系统没有
            if (BGR33CHPG2 > 0 && BGR33CHPG2 == EliminateList.Count)
            {
                foreach (AccountingEliminate item in EliminateList)
                {
                    byte[] buffer = item.ToBytes();
                    Array.Copy(buffer, 0, byteBuffer, offset, AccountingEliminate.TOTAL_WIDTH);
                    offset += AccountingEliminate.TOTAL_WIDTH;
                }
            }
            return byteBuffer;
        }

        #endregion
    }

    /// <summary>
    /// 会计分录
    /// </summary>
    public class AccountingEntry : IMessageReqHandler
    {
        public const UInt16 TOTAL_WIDTH = 299;
        #region Property
        /// <summary>
        /// 套内序号,2位
        /// </summary>
        public String BGR33SN021
        {
            get;
            set;
        }
        /// <summary>
        /// 交易账号,20位
        /// </summary>
        public String BGR33AC201
        {
            get;
            set;
        }
        /// <summary>
        /// 机构号,6位
        /// </summary>
        public String BGR33BRNO1
        {
            get;
            set;
        }
        /// <summary>
        /// 币种,3位
        /// </summary>
        public String BGR33CCYC1
        {
            get;
            set;
        }
        /// <summary>
        /// 科目,8位
        /// </summary>
        public String BGR33ACID1
        {
            get;
            set;
        }
        /// <summary>
        /// 内部账顺序号,4位
        /// </summary>
        public String BGR33SN041
        {
            get;
            set;
        }
        /// <summary>
        /// 红蓝字标志,1位
        /// </summary>
        public String BGR33RDBL1
        {
            get;

            set;

        }
        /// <summary>
        /// 借贷标志,1位
        /// </summary>
        public String BGR33CDFG1
        {
            get;
            set;
        }
        /// <summary>
        /// 发生额,17位
        /// </summary>
        //private Decimal _tradeMoney = new Decimal(0);
        public String BGR33AMT1
        {
            get;
            set;
            //get
            //{
            //    return Decimal.Round(_tradeMoney, 2);
            //}
            //set
            //{
            //    _tradeMoney = value;
            //}
        }
        /// <summary>
        /// 起息日期,8位
        /// </summary>
        public String BGR33DATE1
        {
            get
            {
                return CommonDataHelper.SpaceString(8);
            }
            set
            { }
        }
        /// <summary>
        /// 生效日期,8位
        /// </summary>
        public String BGR33DATE2
        {
            get
            {
                return CommonDataHelper.SpaceString(8);
            }
            set
            { }
        }
        /// <summary>
        /// 对方账号,32位
        /// </summary>
        public String BGR33AC321
        {
            get;
            set;
        }
        /// <summary>
        /// 凭证种类,4位
        /// </summary>
        public String BGR33CRDT1
        {
            get
            {
                return CommonDataHelper.SpaceString(4);
            }
            set
            { }
        }
        /// <summary>
        /// 凭证号码,20位
        /// </summary>
        public String BGR33PBNO1
        {
            get
            {
                return CommonDataHelper.SpaceString(20);
            }
            set
            { }
        }
        /// <summary>
        /// 摘要代码,4位
        /// </summary>
        public String BGR33NTCD1
        {
            get
            {
                return CommonDataHelper.SpaceString(4);
            }
            set
            { }
        }
        /// <summary>
        /// 摘要,80位
        /// </summary>
        public String BGR33FLNM1
        {
            get;
            set;
            //get
            //{
            //    return CommonDataHelper.SpaceString(80);
            //}
            //set
            //{ }
        }
        /// <summary>
        /// 挂销账标志,1位
        /// </summary>
        public String BGR33WRFG1
        {
            get;
            set;
        }
        /// <summary>
        /// 备用,80位
        /// </summary>
        public String BGR33FLNM2
        {
            get
            {
                return CommonDataHelper.SpaceString(80);
            }
            set
            { }
        }
        #endregion

        #region IMessageReqHandler Members

        public byte[] ToBytes()
        {
            int totalLen = 0;
            byte[] bytes = new byte[TOTAL_WIDTH];

            StringBuilder sb = new StringBuilder();
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(BGR33SN021, 2));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(BGR33AC201, 20));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(BGR33BRNO1, 6));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(BGR33CCYC1, 3));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(BGR33ACID1, 8));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(BGR33SN041, 4));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(BGR33RDBL1, 1));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(BGR33CDFG1, 1));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthFigure(BGR33AMT1, 17));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(BGR33DATE1, 8));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(BGR33DATE2, 8));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(BGR33AC321, 32));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(BGR33CRDT1, 4));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(BGR33PBNO1, 20));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(BGR33NTCD1, 4));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(BGR33FLNM1, 80));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(BGR33WRFG1, 1));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(BGR33FLNM2, 80));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            return bytes;
        }

        #endregion
    }

    /// <summary>
    /// 挂销账
    /// </summary>
    public class AccountingEliminate : IMessageReqHandler
    {
        public const UInt16 TOTAL_WIDTH = 145;

        #region
        /// <summary>
        /// 套内序号,2位
        /// </summary>
        public String BGR33SN022
        {
            get;
            set;
        }
        /// <summary>
        /// 挂账序号,14位
        /// </summary>
        public String BGR33SQNO1
        {
            get;
            set;
        }
        /// <summary>
        /// 挂账对方账号,32位
        /// </summary>
        public String BGR33AC322
        {
            get;
            set;
        }
        /// <summary>
        /// 挂账对方名称,80位
        /// </summary>
        public String BGR33FLNM3
        {
            get;
            set;
        }
        /// <summary>
        /// 挂销账金额,17位
        /// </summary>
        public String BGR33AMT2
        {
            get;
            set;
        }
        #endregion

        #region IMessageReqHandler Members

        public byte[] ToBytes()
        {
            int totalLen = 0;
            byte[] bytes = new byte[TOTAL_WIDTH];

            StringBuilder sb = new StringBuilder();
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(BGR33SN022, 2));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(BGR33SQNO1, 14));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(BGR33AC322, 32));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthString(BGR33FLNM3, 80));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            sb = sb.Append(CommonDataHelper.FillSpecifyWidthFigure(BGR33AMT2, 17));
            CommonDataHelper.ResetByteBuffer(sb, ref bytes, ref totalLen, true);
            sb.Remove(0, sb.Length);
            return bytes;
        }

        #endregion
    }
}
