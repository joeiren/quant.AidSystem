using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace xQuant.AidSystem.DBAction
{
    public class TTRD_SET_MSG_LOG_Manager
    {
        public int LogInsert(TTRD_SET_MSG_LOG log)
        {
            return TTRD_SET_MSG_LOG_Controller.Insert(log);
        }

        public int LogUpdate(TTRD_SET_MSG_LOG log)
        {
            return TTRD_SET_MSG_LOG_Controller.Update(log);
        }

        public int LogUpdateByFlowNO(TTRD_SET_MSG_LOG log)
        {
            return TTRD_SET_MSG_LOG_Controller.UpdateByFlowNO(log);
        }

        public DataTable LogQuery(string condition)
        {
            return TTRD_SET_MSG_LOG_Controller.Query(condition);
        }
    }
}
