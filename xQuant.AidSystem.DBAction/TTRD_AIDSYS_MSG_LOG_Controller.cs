using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace xQuant.AidSystem.DBAction
{
    public class TTRD_AIDSYS_MSG_LOG_Controller
    {

        public static int Insert(TTRD_AIDSYS_MSG_LOG table)
        {
            
            //Database db = DatabaseFactory.CreateDatabase("ConnectionString.xeq_trd");
            Database db = DBFactory.TRD;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("INSERT INTO {0}", "TTRD_AIDSYS_MSG_LOG");
            strSql.Append(@"(M_ID, U_ID, I_ID, M_PLATTYPE, M_SUBID, M_ISSINGLE, M_SERIALNO, M_ERROR, M_SENDDATE, M_STATE, M_S_CONTENT, M_R_CONTENT)");
            strSql.AppendFormat(@" VALUES({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})",
                db.BuildParameterName("M_ID"), db.BuildParameterName("U_ID"),
                db.BuildParameterName("I_ID"), db.BuildParameterName("M_PLATTYPE"),
                db.BuildParameterName("M_SUBID"), db.BuildParameterName("M_ISSINGLE"),
                db.BuildParameterName("M_SERIALNO"), db.BuildParameterName("M_ERROR"),
                db.BuildParameterName("M_SENDDATE"), db.BuildParameterName("M_STATE"),
                db.BuildParameterName("M_S_CONTENT"), db.BuildParameterName("M_R_CONTENT"));

            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());

            db.AddInParameter(dbCommand, "M_ID", DbType.String, table.M_ID);                
            db.AddInParameter(dbCommand, "U_ID", DbType.String, table.U_ID);
            db.AddInParameter(dbCommand, "I_ID", DbType.String, table.I_ID);
            db.AddInParameter(dbCommand, "M_PLATTYPE", DbType.Int32, table.M_PLATTYPE);
            db.AddInParameter(dbCommand, "M_SUBID", DbType.Int32, table.M_SUBID);
            db.AddInParameter(dbCommand, "M_ISSINGLE", DbType.String, table.M_ISSINGLE);
            db.AddInParameter(dbCommand, "M_SERIALNO", DbType.String, table.M_SERIALNO);
            db.AddInParameter(dbCommand, "M_ERROR", DbType.String, table.M_ERROR);
            db.AddInParameter(dbCommand, "M_SENDDATE", DbType.String, table.M_SENDDATE);
            db.AddInParameter(dbCommand, "M_STATE", DbType.String, table.M_STATE);
            db.AddInParameter(dbCommand, "M_S_CONTENT", DbType.Binary, table.M_S_CONTENT);
            db.AddInParameter(dbCommand, "M_R_CONTENT", DbType.Binary, table.M_R_CONTENT);
            return db.ExecuteNonQuery(dbCommand);            
        }

        public static int Update(TTRD_AIDSYS_MSG_LOG log)
        {
            Database db = DBFactory.TRD;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("UPDATE {0} SET ", "TTRD_AIDSYS_MSG_LOG");     
            strSql.AppendFormat("M_STATE={0},", db.BuildParameterName("M_STATE"));
            strSql.Append(" WHERE ");
            strSql.AppendFormat(" M_ID={0}", db.BuildParameterName("M_ID"));

            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "M_STATE", DbType.Int32, log.M_STATE);
            db.AddInParameter(dbCommand, "M_ID", DbType.String, log.M_ID);
            return db.ExecuteNonQuery(dbCommand);
        }

        public static DataTable Query(string condition)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ConnectionString.xeq_trd");
                DataTable dt = new DataTable();
                //Database db = DBFactory.TRD;
                string strSql = "";
                if (condition != null)
                {
                    strSql = "SELECT * FROM TTRD_AIDSYS_MSG_LOG WHERE 1=1 AND " + condition;
                }
                else
                {
                    strSql = "SELECT * FROM TTRD_AIDSYS_MSG_LOG ";
                }

                DbCommand cmd = db.GetSqlStringCommand(strSql);
                db.LoadDataTable(cmd, dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
