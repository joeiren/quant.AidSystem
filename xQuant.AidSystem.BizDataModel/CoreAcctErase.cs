using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.BizDataModel
{
    /// <summary>
    /// 核心抹账交易返回数据
    /// </summary>
    public class CoreAcctErase
    {
        public AcctErase_BY999000 DB_BY999000
        {
            get;
            set;
        }

        public List<AcctErase_BY999001> DB_BY999001_List
        {
            get;
            set;
        }

        public CoreAcctErase()
        {
            DB_BY999000 = new AcctErase_BY999000();
            DB_BY999001_List = new List<AcctErase_BY999001>();
        }
    }

    /// <summary>
    /// BY999000
    /// </summary>
    public class AcctErase_BY999000
    {
        /// <summary>
        /// 反交易时间,8
        /// </summary>
        public String TIME
        {
            get;
            set;
        }
        /// <summary>
        /// 反交易流水号,12
        /// </summary>
        public String JNO
        {
            get;
            set;
        }
        /// <summary>
        /// 交易日期,8
        /// </summary>
        public String Date
        {
            get;
            set;
        }
        /// <summary>
        /// 交易时间,8
        /// </summary>
        public String TX_TIME
        {
            get;
            set;
        }
        /// <summary>
        /// 交易柜员号,7
        /// </summary>
        public String TEL_NO
        {
            get;
            set;
        }
        /// <summary>
        /// 交易码,6
        /// </summary>
        public String TX_ID
        {
            get;
            set;
        }
        /// <summary>
        /// 交易流水号,12
        /// </summary>
        public String TX_JNO
        {
            get;
            set;
        }
    }

    /// <summary>
    /// BY999001
    /// </summary>
    public class AcctErase_BY999001
    {
        /// <summary>
        /// 分录顺序号
        /// </summary>
        public String GL_SEQ
        {
            get;
            set;
        }
        /// <summary>
        /// 账号,20
        /// </summary>
        public String ACC_NO
        {
            get;
            set;
        }
        /// <summary>
        /// 户名,80
        /// </summary>
        public String ACC_NAME
        {
            get;
            set;
        }
        /// <summary>
        /// 借/贷标志,1
        /// </summary>
        public String DRCR_IND
        {
            get;
            set;
        }
        /// <summary>
        /// 交易金额,17
        /// </summary>
        //private Decimal _tradeAmount = new Decimal(0);
        public String TX_AMT
        {
            get;
            //{
            //    return Decimal.Round(_tradeAmount, 2);
            //}
            set;
            //{
            //    _tradeAmount = value;
            //}
        }
        /// <summary>
        /// 交易币种,3
        /// </summary>
        public String TX_CCY
        {
            get;
            set;
        }
    }
}
