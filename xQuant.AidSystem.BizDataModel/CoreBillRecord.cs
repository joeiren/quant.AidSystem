using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.BizDataModel
{
    /// <summary>
    /// 核心记账数据
    /// </summary>
    public class CoreBillRecord
    {
        /// <summary>
        /// 交易账号
        /// </summary>
        public string TradeAcctNO
        {
            get;
            set;
        }
        /// <summary>
        /// 套内序号
        /// </summary>
        public String InnerSN
        { get; set; }
        /// <summary>
        ///机构号 
        /// </summary>
        public String OrgNO
        { get; set; }
        /// <summary>
        /// 币种
        /// </summary>
        public String Currency
        { get; set; }
        /// <summary>
        /// 核算码
        /// </summary>
        public String Subject
        { get; set; }        
        /// <summary>
        /// 内部账顺序号
        /// </summary>
        public String InterAcctSN
        { get; set; }
        /// <summary>
        /// 钞汇属性
        /// </summary>
        public String IsNote
        {
            get;
            set;
        }
        /// <summary>
        /// 借贷标志
        /// </summary>
        public String Opt
        { get; set; }
        /// <summary>
        /// 发生额
        /// </summary>
        public String TradeMoney
        { get; set; }
        /// <summary>
        /// 对方账号
        /// </summary>
        public String OptAcct
        { get; set; }
        /// <summary>
        /// 挂销账标志
        /// </summary>
        public String IsEliminateFlag
        {
            get;
            set;
        }
        /// <summary>
        /// 挂账序号
        /// </summary>
        public String PendingAcctSN
        {
            get;
            set;
        }
        /// <summary>
        /// 红蓝字标记, '1' -正常，'2' -红字，'3' -蓝字
        /// </summary>
        public String RedBlueFlag
        {
            get;
            set;
        }

        /// <summary>
        /// 摘要       
        /// </summary>
        public string Summary // add by hxc at 20110919
        {
            get;
            set;
        }

    }
}
