using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.CoreMessageData
{
    /// <summary>
    /// 同业存放账户查询输出结构
    /// </summary>
    public class InterBankAcctInfoODATA : IMessageRespHandler
    {
        #region Property

        public const UInt16 TOTAL_WIDTH = 207;

        /// <summary>
        /// 账号 22
        /// </summary>
        public String AccountNO { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public String Currency 
        {
            get
            {
                return "CNY";
            }
            private set
            { }
        }

        /// <summary>
        /// 外汇客户类别 1 (1-甲、2-乙、3-丙)
        /// </summary>
        public String ExchangeType { get; set; }

        /// <summary>
        /// 钞汇属性 1
        /// </summary>
        public String CashFlag
        {
            get
            {
                return "1";
            }
            private set { 
            }
        }

        /// <summary>
        /// 账户状态 1-正常、3-新开户（未启用单位户有效）、4-结清
        /// </summary>
        public String AccountStatus { get; set; }

        /// <summary>
        /// 帐户状态字 16
        /// </summary>
        public String AcctStatusWord { get; set; }

        /// <summary>
        /// 部分冻结止付标志 2 第一位：1-部分冻结、0-正常;  第二位：1-部分止付、0-正常
        /// </summary>
        public String PartyFrozenFlag { get; set; }

        /// <summary>
        /// 当前余额 (必输,17)
        /// </summary>
        public String CurrentBalance { get; set; }

        /// <summary>
        /// 帐户可支用余额 2 
        /// </summary>
        public String AvailableBalance { get; set; }

        /// <summary>
        /// 冻结金额 17
        /// </summary>
        public String FrozenBalance { get; set; }

        /// <summary>
        /// 止付金额 17
        /// </summary>
        public String EndPayBalance { get; set; }

        /// <summary>
        /// 保留金额 17
        /// </summary>
        public String KeepingBalance { get; set; }

        /// <summary>
        /// 授权金额 17
        /// </summary>
        public String AuthBalance { get; set; }

        /// <summary>
        /// 开户日期 8
        /// </summary>
        public String OpenDate { get; set; }

        /// <summary>
        /// 结清日期 8
        /// </summary>
        public String SettleDate { get; set; }

        /// <summary>
        /// 统计机构 6
        /// </summary>
        public String CheckOrg { get; set; }

        /// <summary>
        /// 归属机构 6
        /// </summary>
        public String BelongOrg { get; set; }

        /// <summary>
        /// 存款科目 8
        /// </summary>
        public String ActingSubject { get; set; }

        /// <summary>
        /// 最后存取日期 8
        /// </summary>
        public String LatestDepositDate { get; set; }

        /// <summary>
        /// 计息标志 1 (0-不计息、1-按月、2-按季、3-按年、4-计息不入账、5-利随本清、6-不定期、7-计息入账)
        /// </summary>
        public string CalcInterestFlag { get; set; }
        #endregion

        #region IMessageRespHandler Members

        public object FromBytes(byte[] messagebytes)
        {
            if (messagebytes.Length >= TOTAL_WIDTH)
            {
                int totalLen = messagebytes.Length;
                int offset = 0;
                while (offset < totalLen)
                {
                    byte[] subMessage = CommonDataHelper.SubBytes(messagebytes, offset, totalLen - offset);
                    CoreDataBlockHeader dbhdr1 = new CoreDataBlockHeader();
                    dbhdr1 = (CoreDataBlockHeader)dbhdr1.FromBytes(subMessage);
                    offset += (int)dbhdr1.DBH_DB_LENGTH;
                    if (dbhdr1.DBH_DB_ID.Trim() == "BDO75112")
                    {
                        subMessage = CommonDataHelper.SubBytes(subMessage, CoreDataBlockHeader.TOTAL_WIDTH, subMessage.Length - CoreDataBlockHeader.TOTAL_WIDTH);
                        AccountNO = CommonDataHelper.GetValueFromBytes(ref subMessage, 22).TrimEnd();
                        Currency = CommonDataHelper.GetValueFromBytes(ref subMessage, 3).TrimEnd();
                        ExchangeType = CommonDataHelper.GetValueFromBytes(ref subMessage, 1).TrimEnd();
                        CashFlag = CommonDataHelper.GetValueFromBytes(ref subMessage, 1).TrimEnd();
                        AccountStatus = CommonDataHelper.GetValueFromBytes(ref subMessage, 1).TrimEnd();
                        AcctStatusWord = CommonDataHelper.GetValueFromBytes(ref subMessage, 16).TrimEnd();
                        PartyFrozenFlag = CommonDataHelper.GetValueFromBytes(ref subMessage, 2).TrimEnd();
                        CurrentBalance = CommonDataHelper.GetValueFromBytes(ref subMessage, 17).TrimEnd();
                        AvailableBalance = CommonDataHelper.GetValueFromBytes(ref subMessage, 17).TrimEnd();
                        FrozenBalance = CommonDataHelper.GetValueFromBytes(ref subMessage, 17).TrimEnd();
                        EndPayBalance = CommonDataHelper.GetValueFromBytes(ref subMessage, 17).TrimEnd();
                        KeepingBalance = CommonDataHelper.GetValueFromBytes(ref subMessage, 17).TrimEnd();
                        AuthBalance = CommonDataHelper.GetValueFromBytes(ref subMessage, 17).TrimEnd();
                        OpenDate = CommonDataHelper.GetValueFromBytes(ref subMessage, 8).TrimEnd();
                        SettleDate = CommonDataHelper.GetValueFromBytes(ref subMessage, 8).TrimEnd();
                        CheckOrg = CommonDataHelper.GetValueFromBytes(ref subMessage, 6).TrimEnd();
                        BelongOrg = CommonDataHelper.GetValueFromBytes(ref subMessage, 6).TrimEnd();
                        ActingSubject = CommonDataHelper.GetValueFromBytes(ref subMessage, 8).TrimEnd();
                        LatestDepositDate = CommonDataHelper.GetValueFromBytes(ref subMessage, 8).TrimEnd();
                        CalcInterestFlag = CommonDataHelper.GetValueFromBytes(ref subMessage, 1).TrimEnd();
                    }
                    
                }
            }

            return this;
        }

        #endregion
    }
}
