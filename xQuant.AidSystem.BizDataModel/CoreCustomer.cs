using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.BizDataModel
{
    /// <summary>
    /// 核心客户数据， 提供给客户端调用和返回使用
    /// </summary>
    public class CoreCustomer
    {
        #region Property
        /// <summary>
        /// 客户内码,11
        /// </summary>
        public String InternalCode
        {
            get;
            set;
        }
        /// <summary>
        /// 客户号,23
        /// </summary>
        public String CustomerNO
        {
            get;
            set;
        }
        /// <summary>
        /// 客户类型,1
        /// </summary>
        public String CustomerType
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
        /// 其他名称,30
        /// </summary>
        public String CustomerOName
        {
            get;
            set;
        }
        /// <summary>
        /// 英文名称,80
        /// </summary>
        public String CustomerEName
        {
            get;
            set;
        }
        /// <summary>
        /// 国籍,3
        /// </summary>
        public String Nation
        {
            get;
            set;
        }
        /// <summary>
        /// 贵宾类型,1
        /// </summary>
        public String VIPType
        {
            get;
            set;
        }
        /// <summary>
        /// 客户状态,1
        /// </summary>
        public String CustomerStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 地址序号,2
        /// </summary>
        public String AddressSN
        {
            get;
            set;
        }
        /// <summary>
        ///  地址,80
        /// </summary>
        public String Address
        {
            get;
            set;
        }
        /// <summary>
        ///电话序号,2
        /// </summary>
        public String TeleSN
        {
            get;
            set;
        }
        /// <summary>
        /// 电话号码,20
        /// </summary>
        public String TeleNO
        {
            get;
            set;
        }
        /// <summary>
        /// 手机号码,20
        /// </summary>
        public String MobileNO
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
        public String DistrictType
        {
            get;
            set;
        }
        /// <summary>
        /// 建立机构,6
        /// </summary>
        public String CreatedDept
        {
            get;
            set;
        }
        /// <summary>
        ///建立柜员,7
        /// </summary>
        public String CreatedOperater
        {
            get;
            set;
        }

        /// <summary>
        /// 建立日期,8
        /// </summary>
        public String CreatedDate
        {
            get;
            set;
        }
        /// <summary>
        /// 更新机构,6
        /// </summary>
        public String UpdatedDept
        {
            get;
            set;
        }
        /// <summary>
        /// 更新柜员,7
        /// </summary>
        public String UpdatedOperator
        {
            get;
            set;
        }
        /// <summary>
        /// 更新日期,8 
        /// </summary>
        public String UpdatedDate
        {
            get;
            set;
        }
        #endregion
    }
}
