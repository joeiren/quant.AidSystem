using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.BizDataModel
{
    public class CoreBillRetrieve
    {
        public CoreBillRetrieve_BGO30800 BGO30800
        {
            get;
            set;
        }
        public List<CoreBillRetrieve_BGO30801> BGO30801_List
        {
            get;
            set;
        }
        public CoreBillRetrieve_BGO30802 BGO30802
        {
            get;
            set;
        }
        public List<CoreBillRetrieve_BGO30803> BGO30803_List
        {
            get;
            set;
        }

        public CoreBillRetrieve()
        {
            BGO30800 = new CoreBillRetrieve_BGO30800();
            BGO30801_List = new List<CoreBillRetrieve_BGO30801>();
            BGO30802 = new CoreBillRetrieve_BGO30802();
            BGO30803_List = new List<CoreBillRetrieve_BGO30803>();
        }
    }

    public class CoreBillRetrieve_BGO30800
    {
        //核心交易流水号,12
        public String CoreTradeSN
        {
            get;
            set;
        }
        //发起方系统代码,2
        public String SrcSystemCode
        {
            get;
            set;
        }
        //有会计流水标志,1
        public String AcctSNFlag
        {
            get;
            set;
        }
        //外系统跟踪号,12
        public String OutsideSN
        {
            get;
            set;
        }
        //被抹账标志,1
        public String ErasedFlag
        {
            get;
            set;
        }
    }

    public class CoreBillRetrieve_BGO30801
    {
        //核心交易流水号,12
        public String CoreTradeSN
        {
            get;
            set;
        }

        //交易账号,20
        public String TradeAccount
        {
            get;
            set;
        }
        //户名,80
        public String AccountName
        {
            get;
            set;
        }
        //现转标志,1
        public String CashFalg
        {
            get;
            set;
        }
        //借贷标志,1
        public String LoanFlag
        {
            get;
            set;
        }
        //金额,17
        private Decimal _amount = new Decimal(0);
        public Decimal Amount
        {
            get
            {
                return Decimal.Round(_amount, 2);
            }
            set
            {
                _amount = value;
            }
        }

        //凭证号码,20
        public String VoucherNO
        {
            get;
            set;
        }
        //发起方系统代码,2
        public String SrcSystemCode
        {
            get;
            set;
        }
        //入账配钞标志,1
        public String PostingCashFlag
        {
            get;
            set;
        }
    }

    public class CoreBillRetrieve_BGO30802
    {
        //是否配钞 ,1
        public String IsQuotaMoney
        {
            get;
            set;
        }
        //配钞笔数,2
        public UInt16 QuotaMoneyCount
        {
            get;
            set;
        }
    }

    public class CoreBillRetrieve_BGO30803
    {
        //币种,3
        public String Currency
        {
            get;
            set;
        }
        //配钞金额,17
        private Decimal _quotaAmount = new Decimal(0);
        public Decimal QuotaAmount
        {
            get
            {
                return Decimal.Round(_quotaAmount, 2);
            }
            set
            {
                _quotaAmount = value;
            }
        }
    }
}
