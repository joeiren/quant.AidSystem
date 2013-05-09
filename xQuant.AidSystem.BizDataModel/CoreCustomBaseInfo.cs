using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.BizDataModel
{
    public class CoreCustomBaseInfo
    {
        #region
        /// <summary>
        /// 客户内码,11
        /// </summary>
        public String CustomerInnerCode
        {
            get;
            set;
        }
        /// <summary>
        /// 客户名称,80
        /// </summary>
        public String CustomerName
        {
            get;
            set;
        }
        /// <summary>
        /// 联系地址,80
        /// </summary>
        public String Address
        {
            get;
            set;
        }
        /// <summary>
        /// 联系地址序号,2
        /// </summary>
        public String AddressSN
        {
            get;
            set;
        }
        /// <summary>
        /// 电话序号,2
        /// </summary>
        public String TelephoneSN
        {
            get;
            set;
        }
        /// <summary>
        /// 电话号码,20
        /// </summary>
        public String TelephoneNumber
        {
            get;
            set;
        }
        #endregion
    }
}
