using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.BizDataModel
{
    public class CoreCustomerBrief
    {
        /// <summary>
        /// 客户号
        /// </summary>
        public String CstmNO
        {
            get;
            set; 
        }
        
        public String CName
        { get; set; }
        public String OName
        { get; set; }
        public String EName
        { get; set; }
        public String Address
        { get; set; }
        public String TeleNO
        { get; set; }
        public String MobileNO
        { get; set; }
        public String ZIP
        { get; set; }
        
    }
}
