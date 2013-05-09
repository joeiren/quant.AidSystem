using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace xQuant.AidSystem.DBAction
{
    /// <summary>
    /// 同业存放活期定期结息批号序列
    /// </summary>
    public class S_AUTO_CORE_KP
    {
        /// <summary>
        /// 活期批号序列
        /// </summary>
        /// <returns></returns>
        public static int GetNextID()
        {
            Database db = DBFactory.TRD;
            string sql = "SELECT NEXTVAL FOR S_AUTO_CORE_KP AS KPNO FROM SYSIBM.SYSDUMMY1;";
            DbCommand dbCommand = db.GetSqlStringCommand(sql);
            using (IDataReader reader = db.ExecuteReader(dbCommand))
            {
                if (reader.Read())
                {
                    int index = reader.GetOrdinal("KPNO");
                    if (!reader.IsDBNull(index))
                    {
                        return reader.GetInt32(index);
                    }
                    else
                    {
                        throw new Exception("不能获取S_AUTO_CORE_KP的序列！");
                    }
                }
            }
            return 0;
        }
    }
}
