using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xQuant.AidSystem.BizDataModel
{
    public class PayBanksInfo
    {
        #region Property
        /// <summary>
        /// 行号,12
        /// </summary>
        public String BankNO
        {
            get;
            set;
        }
        /// <summary>
        /// 行名,60
        /// </summary>
        public String BankName
        {
            get;
            set;
        }
        /// <summary>
        ///所属直接参与者,12
        /// </summary>
        public String DirectParticipator
        {
            get;
            set;
        }
        /// <summary>
        /// 节点代码,4
        /// </summary>
        public String NodeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 城市代码,4
        /// </summary>
        public String CityCode
        {
            get;
            set;
        }
        /// <summary>
        /// 电话,30
        /// </summary>
        public String TelephoneNO
        {
            get;
            set;
        }
        /// <summary>
        /// 地址，60
        /// </summary>
        public String Address
        {
            get;
            set;
        }
        #endregion
    }
}
