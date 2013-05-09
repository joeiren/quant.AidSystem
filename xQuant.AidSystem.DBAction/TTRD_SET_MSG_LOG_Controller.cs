using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using xIR.Framework.Transactions;

namespace xQuant.AidSystem.DBAction
{
    public class TTRD_SET_MSG_LOG_Controller
    {
        public static int Insert(TTRD_SET_MSG_LOG table)
        {

            //Database db = DatabaseFactory.CreateDatabase("ConnectionString.xeq_trd");
            Database db = DBFactory.TRD;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("INSERT INTO {0}", "TTRD_SET_MSG_LOG");
            strSql.Append(@"(GUID, FLOW_NO, HOSTFLOW_NO, PLATFORMTYPE, MSGTYPE, SEND_CONTENT, RECV_CONTENT, SEND_TIME, RESP_TIME, INS_ID, USER_CODE, IS_MUL_PKG, STATE, ERRINFO)");
            strSql.AppendFormat(@" VALUES({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13})",
                db.BuildParameterName("GUID"), 
                db.BuildParameterName("FLOW_NO"), 
                db.BuildParameterName("HOSTFLOW_NO"),
                db.BuildParameterName("PLATFORMTYPE"), 
                db.BuildParameterName("MSGTYPE"), 
                db.BuildParameterName("SEND_CONTENT"),
                db.BuildParameterName("RECV_CONTENT"), 
                db.BuildParameterName("SEND_TIME"), 
                db.BuildParameterName("RESP_TIME"), 
                db.BuildParameterName("INS_ID"), 
                db.BuildParameterName("USER_CODE"),
                db.BuildParameterName("IS_MUL_PKG"), 
                db.BuildParameterName("STATE"), 
                db.BuildParameterName("ERRINFO"));

            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());

            db.AddInParameter(dbCommand, "GUID", DbType.String, table.GUID);
            db.AddInParameter(dbCommand, "FLOW_NO", DbType.String, table.FLOW_NO);
            db.AddInParameter(dbCommand, "HOSTFLOW_NO", DbType.String, table.HOSTFLOW_NO);
            db.AddInParameter(dbCommand, "PLATFORMTYPE", DbType.Int32, table.PLATFORMTYPE);
            db.AddInParameter(dbCommand, "MSGTYPE", DbType.Int32, table.MSGTYPE);
            db.AddInParameter(dbCommand, "SEND_CONTENT", DbType.Binary, table.SEND_CONTENT);
            db.AddInParameter(dbCommand, "RECV_CONTENT", DbType.Binary, table.RECV_CONTENT);
            db.AddInParameter(dbCommand, "SEND_TIME", DbType.String, table.SEND_TIME);
            db.AddInParameter(dbCommand, "RESP_TIME", DbType.String, table.RESP_TIME);
            db.AddInParameter(dbCommand, "INS_ID", DbType.String, table.INS_ID);
            db.AddInParameter(dbCommand, "USER_CODE", DbType.String, table.USER_CODE);
            db.AddInParameter(dbCommand, "IS_MUL_PKG", DbType.String, table.IS_MUL_PKG);
            db.AddInParameter(dbCommand, "STATE", DbType.Int32, table.STATE);
            db.AddInParameter(dbCommand, "ERRINFO", DbType.Binary, table.ERRINFO);
            using (TransactionScope trans = new TransactionScope())
            {
                int count = db.ExecuteNonQuery(dbCommand);
                trans.Complete();
                return count;
            }
        }

        public static int Update(TTRD_SET_MSG_LOG log)
        {
            Database db = DBFactory.TRD;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("UPDATE {0} SET ", "TTRD_SET_MSG_LOG");
            if (log.FLOW_NO != null)
            {
                strSql.AppendFormat("FLOW_NO={0},", db.BuildParameterName("FLOW_NO"));
            }
            if (log.HOSTFLOW_NO != null)
            {
                strSql.AppendFormat("HOSTFLOW_NO={0},", db.BuildParameterName("HOSTFLOW_NO"));
            }
            if (log.PLATFORMTYPE > 0)
            {
                strSql.AppendFormat("PLATFORMTYPE={0},", db.BuildParameterName("PLATFORMTYPE"));
            }
            if (log.MSGTYPE > 0)
            {
                strSql.AppendFormat("MSGTYPE={0},", db.BuildParameterName("MSGTYPE"));
            }
            if (log.SEND_CONTENT != null)
            {
                strSql.AppendFormat("SEND_CONTENT={0},", db.BuildParameterName("SEND_CONTENT"));
            }
            if (log.RECV_CONTENT != null)
            {
                strSql.AppendFormat("RECV_CONTENT={0},", db.BuildParameterName("RECV_CONTENT"));
            }
            if (log.SEND_TIME != null)
            {
                strSql.AppendFormat("SEND_TIME={0},", db.BuildParameterName("SEND_TIME"));
            }
            if (log.RESP_TIME != null)
            {
                strSql.AppendFormat("RESP_TIME={0},", db.BuildParameterName("RESP_TIME"));
            }
            if (log.INS_ID != null)
            {
                strSql.AppendFormat("INS_ID={0},", db.BuildParameterName("INS_ID"));
            }
            if (log.USER_CODE != null)
            {
                strSql.AppendFormat("USER_CODE={0},", db.BuildParameterName("USER_CODE"));
            }
            if (log.IS_MUL_PKG != null)
            {
                strSql.AppendFormat("IS_MUL_PKG={0},", db.BuildParameterName("IS_MUL_PKG"));
            }
            if (log.STATE != 0)
            {
                strSql.AppendFormat("STATE={0},", db.BuildParameterName("STATE"));
            }
            if (log.ERRINFO != null)
            {
                strSql.AppendFormat("ERRINFO={0} ", db.BuildParameterName("ERRINFO"));
            }
            strSql = strSql.Replace(",", " ", strSql.Length - 1, 1);
            strSql.Append(" WHERE ");
            strSql.AppendFormat(" GUID={0}", db.BuildParameterName("GUID"));

            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "GUID", DbType.String, log.GUID);
            if (log.FLOW_NO != null)
            {
                db.AddInParameter(dbCommand, "FLOW_NO", DbType.String, log.FLOW_NO);
            }
            if (log.HOSTFLOW_NO != null)
            {
                db.AddInParameter(dbCommand, "HOSTFLOW_NO", DbType.String, log.HOSTFLOW_NO);
            }
            if (log.PLATFORMTYPE > 0)
            {
                db.AddInParameter(dbCommand, "PLATFORMTYPE", DbType.Int32, log.PLATFORMTYPE);
            }
            if (log.MSGTYPE > 0)
            {
                db.AddInParameter(dbCommand, "MSGTYPE", DbType.Int32, log.MSGTYPE);
            }
            if (log.SEND_CONTENT != null)
            {
                db.AddInParameter(dbCommand, "SEND_CONTENT", DbType.Binary, log.SEND_CONTENT);
            }
            if (log.RECV_CONTENT != null)
            {
                db.AddInParameter(dbCommand, "RECV_CONTENT", DbType.Binary, log.RECV_CONTENT);
            }
            if (log.SEND_TIME != null)
            {
                db.AddInParameter(dbCommand, "SEND_TIME", DbType.String, log.SEND_TIME);
            }
            if (log.RESP_TIME != null)
            {
                db.AddInParameter(dbCommand, "RESP_TIME", DbType.String, log.RESP_TIME);
            }
            if (log.INS_ID != null)
            {
                db.AddInParameter(dbCommand, "INS_ID", DbType.String, log.INS_ID);
            }
            if (log.USER_CODE != null)
            {
                db.AddInParameter(dbCommand, "USER_CODE", DbType.String, log.USER_CODE);
            }
            if (log.IS_MUL_PKG != null)
            {
                db.AddInParameter(dbCommand, "IS_MUL_PKG", DbType.String, log.IS_MUL_PKG);
            }
            if (log.STATE != 0)
            {
                db.AddInParameter(dbCommand, "STATE", DbType.Int32, log.STATE);
            }
            if (log.ERRINFO != null)
            {
                db.AddInParameter(dbCommand, "ERRINFO", DbType.String, log.ERRINFO);
            }

            using (TransactionScope trans = new TransactionScope())
            {
                int ret = db.ExecuteNonQuery(dbCommand);
                trans.Complete();
                return ret;
            }
        }

        public static int UpdateByFlowNO(TTRD_SET_MSG_LOG log)
        {
            Database db = DBFactory.TRD;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("UPDATE {0} SET ", "TTRD_SET_MSG_LOG");
            if (log.GUID != null)
            {
                strSql.AppendFormat("GUID={0},", db.BuildParameterName("GUID"));
            }
            if (log.HOSTFLOW_NO != null)
            {
                strSql.AppendFormat("HOSTFLOW_NO={0},", db.BuildParameterName("HOSTFLOW_NO"));
            }
            if (log.PLATFORMTYPE > 0)
            {
                strSql.AppendFormat("PLATFORMTYPE={0},", db.BuildParameterName("PLATFORMTYPE"));
            }
            if (log.MSGTYPE > 0)
            {
                strSql.AppendFormat("MSGTYPE={0},", db.BuildParameterName("MSGTYPE"));
            }
            if (log.SEND_CONTENT != null)
            {
                strSql.AppendFormat("SEND_CONTENT={0},", db.BuildParameterName("SEND_CONTENT"));
            }
            if (log.RECV_CONTENT != null)
            {
                strSql.AppendFormat("RECV_CONTENT={0},", db.BuildParameterName("RECV_CONTENT"));
            }
            if (log.SEND_TIME != null)
            {
                strSql.AppendFormat("SEND_TIME={0},", db.BuildParameterName("SEND_TIME"));
            }
            if (log.RESP_TIME != null)
            {
                strSql.AppendFormat("RESP_TIME={0},", db.BuildParameterName("RESP_TIME"));
            }
            if (log.INS_ID != null)
            {
                strSql.AppendFormat("INS_ID={0},", db.BuildParameterName("INS_ID"));
            }
            if (log.USER_CODE != null)
            {
                strSql.AppendFormat("USER_CODE={0},", db.BuildParameterName("USER_CODE"));
            }
            if (log.IS_MUL_PKG != null)
            {
                strSql.AppendFormat("IS_MUL_PKG={0},", db.BuildParameterName("IS_MUL_PKG"));
            }
            if (log.STATE != 0)
            {
                strSql.AppendFormat("STATE={0},", db.BuildParameterName("STATE"));
            }
            if (log.ERRINFO != null)
            {
                strSql.AppendFormat("ERRINFO={0} ", db.BuildParameterName("ERRINFO"));
            }
            strSql = strSql.Replace(",", " ", strSql.Length - 1, 1);
            strSql.Append(" WHERE ");
            strSql.AppendFormat(" FLOW_NO = {0}", db.BuildParameterName("FLOW_NO"));

            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.AddInParameter(dbCommand, "FLOW_NO", DbType.String, log.FLOW_NO);
            if (log.GUID != null)
            {
                db.AddInParameter(dbCommand, "GUID", DbType.String, log.GUID);
            }
            if (log.HOSTFLOW_NO != null)
            {
                db.AddInParameter(dbCommand, "HOSTFLOW_NO", DbType.String, log.HOSTFLOW_NO);
            }
            if (log.PLATFORMTYPE > 0)
            {
                db.AddInParameter(dbCommand, "PLATFORMTYPE", DbType.Int32, log.PLATFORMTYPE);
            }
            if (log.MSGTYPE > 0)
            {
                db.AddInParameter(dbCommand, "MSGTYPE", DbType.Int32, log.MSGTYPE);
            }
            if (log.SEND_CONTENT != null)
            {
                db.AddInParameter(dbCommand, "SEND_CONTENT", DbType.Binary, log.SEND_CONTENT);
            }
            if (log.RECV_CONTENT != null)
            {
                db.AddInParameter(dbCommand, "RECV_CONTENT", DbType.Binary, log.RECV_CONTENT);
            }
            if (log.SEND_TIME != null)
            {
                db.AddInParameter(dbCommand, "SEND_TIME", DbType.String, log.SEND_TIME);
            }
            if (log.RESP_TIME != null)
            {
                db.AddInParameter(dbCommand, "RESP_TIME", DbType.String, log.RESP_TIME);
            }
            if (log.INS_ID != null)
            {
                db.AddInParameter(dbCommand, "INS_ID", DbType.String, log.INS_ID);
            }
            if (log.USER_CODE != null)
            {
                db.AddInParameter(dbCommand, "USER_CODE", DbType.String, log.USER_CODE);
            }
            if (log.IS_MUL_PKG != null)
            {
                db.AddInParameter(dbCommand, "IS_MUL_PKG", DbType.String, log.IS_MUL_PKG);
            }
            if (log.STATE != 0)
            {
                db.AddInParameter(dbCommand, "STATE", DbType.Int32, log.STATE);
            }
            if (log.ERRINFO != null)
            {
                db.AddInParameter(dbCommand, "ERRINFO", DbType.String, log.ERRINFO);
            }

            using (TransactionScope trans = new TransactionScope())
            {
                int ret = db.ExecuteNonQuery(dbCommand);
                trans.Complete();
                return ret;
            }
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
                    strSql = "SELECT * FROM TTRD_SET_MSG_LOG WHERE 1=1 AND " + condition;
                }
                else
                {
                    strSql = "SELECT * FROM TTRD_SET_MSG_LOG ";
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
