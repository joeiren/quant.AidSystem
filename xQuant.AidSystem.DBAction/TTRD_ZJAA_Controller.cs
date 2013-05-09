using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using xQuant.AidSystem.BizDataModel;
using System.Data;

namespace xQuant.AidSystem.DBAction
{
    /// <summary>
    /// 外围业务流水清单
    /// </summary>
    public class TTRD_ZJAA_Controller
    {

        public static void Clear()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(@"DELETE FROM TTRD_ZJAA");
                Database db = DBFactory.TRD;
                DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString());
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Delete(string orgno, string date)
        {
            try
            {
                if (string.IsNullOrEmpty(orgno) || string.IsNullOrEmpty(date))
                {
                    return;
                }
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(@"DELETE FROM TTRD_ZJAA WHERE ZJAADATE = '{0}' AND ZJAABRNO='{1}'", date, orgno);
                Database db = DBFactory.TRD;
                DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString());
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static int Insert(List<CoreCheckAcctInfo> datalist)
        {
            int retcount = 0;
            while (datalist.Count > 0)
            {
                List<CoreCheckAcctInfo> temp = datalist.Take(500).ToList();
                retcount += InsertPart(temp);
                datalist.RemoveRange(0, temp.Count);
            }
            return retcount;
        }

        private static int InsertPart(List<CoreCheckAcctInfo> datalist)
        {
            Database db = DBFactory.TRD;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("INSERT INTO {0}", "TTRD_ZJAA");
            strSql.Append(@"(ZJAADATE, ZJAANO18, ZJAABRNO, ZJAASTAF, ZJAASN11, ZJAAAC32, ZJABBRNO, ZJAACCYC, ZJAAACID,ZJAACDFG,ZJAARDBL,ZJAAAMT,ZJABFLAG)");
            strSql.Append(" VALUES");
            int i = 0;
            foreach (var pay in datalist)
            {
                strSql.AppendFormat(@"({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}),",
                    db.BuildParameterName("ZJAADATE" + i.ToString()), db.BuildParameterName("ZJAANO18" + i.ToString()),
                    db.BuildParameterName("ZJAABRNO" + i.ToString()),
                    db.BuildParameterName("ZJAASTAF" + i.ToString()), db.BuildParameterName("ZJAASN11" + i.ToString()),        
                    db.BuildParameterName("ZJAAAC32" + i.ToString()), db.BuildParameterName("ZJABBRNO" + i.ToString()),
                    db.BuildParameterName("ZJAACCYC" + i.ToString()), db.BuildParameterName("ZJAAACID" + i.ToString()),
                    db.BuildParameterName("ZJAACDFG" + i.ToString()), db.BuildParameterName("ZJAARDBL" + i.ToString()),
                    db.BuildParameterName("ZJAAAMT" + i.ToString()), db.BuildParameterName("ZJABFLAG" + i.ToString())
                    );
                i++;
            }

            string sql = strSql.ToString().TrimEnd(',');

            DbCommand dbCommand = db.GetSqlStringCommand(sql);
            i = 0;
            foreach (var pay in datalist)
            {
                db.AddInParameter(dbCommand, "ZJAADATE" + i.ToString(), DbType.String, pay.TradeDate);
                db.AddInParameter(dbCommand, "ZJAANO18" + i.ToString(), DbType.String, pay.BizFlowNO);
                db.AddInParameter(dbCommand, "ZJAABRNO" + i.ToString(), DbType.String, pay.OrgNO);
                db.AddInParameter(dbCommand, "ZJAASTAF" + i.ToString(), DbType.String, pay.TellerNO);
                db.AddInParameter(dbCommand, "ZJAASN11" + i.ToString(), DbType.String, pay.TellerFlowNO);
                db.AddInParameter(dbCommand, "ZJAAAC32" + i.ToString(), DbType.String, pay.TradeAcctNO);
                db.AddInParameter(dbCommand, "ZJABBRNO" + i.ToString(), DbType.String, pay.OrgNOWithinAcct);
                db.AddInParameter(dbCommand, "ZJAACCYC" + i.ToString(), DbType.String, pay.Currency);
                db.AddInParameter(dbCommand, "ZJAAACID" + i.ToString(), DbType.String, pay.CheckCode);
                db.AddInParameter(dbCommand, "ZJAACDFG" + i.ToString(), DbType.String, pay.DCFlag);
                db.AddInParameter(dbCommand, "ZJAARDBL" + i.ToString(), DbType.String, pay.RedBlueFlag);
                db.AddInParameter(dbCommand, "ZJAAAMT" + i.ToString(), DbType.String, pay.Amount);
                db.AddInParameter(dbCommand, "ZJABFLAG" + i.ToString(), DbType.String, pay.Status);
                i++;
            }

            return db.ExecuteNonQuery(dbCommand);
        }
    }
}
