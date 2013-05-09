using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.BizDataModel
{
    public class AidTypeDefine
    {
        /// <summary>
        /// 同业存放操作类型
        /// </summary>
        public enum INTER_BANK_OPERATION_TYPE
        {
            /// <summary>
            /// 新增
            /// </summary>
            CreateNew = 1,
            /// <summary>
            /// 撤销
            /// </summary>
            Cancel =2,
        }

        /// <summary>
        /// 同业存放通知单类型
        /// </summary>
        public enum INTER_BANK_NOTICE_TYPE
        { 
            /// <summary>
            /// 开户
            /// </summary>
            OpenAccount = 1,
            /// <summary>
            /// 销户
            /// </summary>
            DeleteAccount =2,
            /// <summary>
            /// 部提
            /// </summary>
            PartWithdraw = 3,
        }

        /// <summary>
        /// 同业存放业务期限类型
        /// </summary>
        public enum INTER_BANK_BIZ_TERM_TYPE
        {
            /// <summary>
            /// 活期
            /// </summary>
            Current = 1,
            /// <summary>
            /// 定期
            /// </summary>
            Fixed = 2,
        }

        /// <summary>
        /// 同业开户产品类别
        /// </summary>
        public enum INTER_BANK_PROCUDT_CATEGORY_ID
        {
            /// <summary>
            /// 活期
            /// </summary>
            Current = 21,
            /// <summary>
            /// 定期 
            /// </summary>
            Fixed = 23,
        }

        /// <summary>
        /// 同业开户产品代码
        /// </summary>
        public enum INTER_BANK_PRODUCT_CODE
        {
            Current_Fixed = 106,
        }

        /// <summary>
        /// 同业存放开户存款种类
        /// </summary>
        public enum INTER_BANK_DEPOSITING_CATEGORY
        {
            /// <summary>
            /// 农业银行
            /// </summary>
            ABC = 21,
            /// <summary>
            /// 工商银行
            /// </summary>
            ICBC = 22,
            /// <summary>
            /// 中国银行
            /// </summary>
            Bank_China = 23,
            /// <summary>
            /// 建设银行
            /// </summary>
            CBC = 24,
            /// <summary>
            /// 交通银行
            /// </summary>
            Bank_Communications = 25,
            /// <summary>
            /// 股份商业银行
            /// </summary>
            Joint_Stock_Commercial = 26,
            /// <summary>
            /// 政策性银行
            /// </summary>
            Policy_Related = 27,
            /// <summary>
            /// 其他银行
            /// </summary>
            Other_Banks = 28,
            /// <summary>
            /// 29-境内非银行
            /// </summary>
            Domestic_Non_Banking = 29,
            /// <summary>
            /// 30-境外银行
            /// </summary>
            China_Overseas_Banks = 30,
            /// <summary>
            /// 31-境外非银行
            /// </summary>
            China_Overseas_Non_Banking = 31,

        }

        /// <summary>
        /// 计息方式
        /// </summary>
        public enum INTER_BANK_COUPON_TYPE
        { 
            /// <summary>
            /// 0-不计息
            /// </summary>
            Nothing = 0, 
            /// <summary>
            /// 按月
            /// </summary>
            Month = 1,
            /// <summary>
            /// 按季
            /// </summary>
            Season = 2,
            /// <summary>
            /// 按年
            /// </summary>
            Year = 3,
            /// <summary>
            /// 计息不入账
            /// </summary>
            InterestWithoutRecord = 4,
            /// <summary>
            /// 利随本清
            /// </summary>
            InterestWithPrincipal = 5,
            /// <summary>
            /// 不定期
            /// </summary>
            Unterm = 6,
            /// <summary>
            /// 计息入账
            /// </summary>
            InterestWithRecord = 7,
        }

    }

    
}
