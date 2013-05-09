using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace xQuant.AidSystem.DBAction
{
    public class TTRD_AIDSYS_MSG_LOG_Manager 
    {

        public int LogInsert(TTRD_AIDSYS_MSG_LOG log)
        {
            return TTRD_AIDSYS_MSG_LOG_Controller.Insert(log);
        }

        public int LogUpdate(TTRD_AIDSYS_MSG_LOG log)
        {
            return TTRD_AIDSYS_MSG_LOG_Controller.Update(log);
        }

        public DataTable LogQuery(string condition)
        {
            return TTRD_AIDSYS_MSG_LOG_Controller.Query(condition);
        }
        //public int OrderUpdate(TTRD_AIDSYS_MSG_LOG o)
        //{
        //    return xQuant.Dal.Common.TRD.OTC.TTRD_OTC_ORDER.Update(o);
        //}


        //public System.Data.DataTable Query(string strWhere)
        //{
        //    return xQuant.Dal.Common.TRD.OTC.TTRD_OTC_ORDER.Query(strWhere);
        //}

        //public int OrderDelete(TTRD_AIDSYS_MSG_LOG xp)
        //{
        //    return xQuant.Dal.Common.TRD.OTC.TTRD_OTC_ORDER.Delete(xp);
        //}
    }
}
