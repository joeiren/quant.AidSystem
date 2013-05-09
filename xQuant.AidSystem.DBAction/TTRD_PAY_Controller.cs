using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using xQuant.AidSystem.BizDataModel;

namespace xQuant.AidSystem.DBAction
{
    public class TTRD_PAY_Controller
    {
        public static void Clear()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(@"DELETE FROM TTRD_PAY");
                Database db = DBFactory.TRD;
                DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString());
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void Delete(string date)
        {
            try
            {
                if (string.IsNullOrEmpty(date))
                {
                    return;
                }
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(@"DELETE FROM TTRD_PAY WHERE JYRQ = '{0}'", date);
                Database db = DBFactory.TRD;
                DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString());
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static int Insert(List<PayCheckAcct> datalist)
        {
            int retcount = 0;
            if (datalist == null)
            {
                return retcount;
            }
            while (datalist.Count > 0)
            {
                List<PayCheckAcct> temp = datalist.Take(500).ToList();
                retcount += InsertPart(temp);
                datalist.RemoveRange(0, temp.Count);
            }
            return retcount;
        }

        private static int InsertPart(List<PayCheckAcct> datalist)
        {
            Database db = DBFactory.TRD;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("INSERT INTO {0}", "TTRD_PAY");
            strSql.Append(@"(PLAT_DATE, SEQ_NO, QDBZ, ZJLSH, JGH, CMT, FKZH, FKMC, FKHHH, FKHMC, SKZH, SKMC,SKHHH,SKHMC,BZDM,TX_ZMT,CJLL,CJQX,YWZL,BZ,CZY,SSQD,JZBZ,HOST_SEQ,HOST_DATE,SXF,HOST_RET,HOST_RETMSG,JYRQ,JYSJ)");
            strSql.Append(" VALUES");
            int i = 0;
            foreach (var pay in datalist)
            {
                strSql.AppendFormat(@"({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29}),",
                    db.BuildParameterName("PLAT_DATE" + i.ToString()), db.BuildParameterName("SEQ_NO"+ i.ToString()),
                    db.BuildParameterName("QDBZ" + i.ToString()), db.BuildParameterName("ZJLSH" + i.ToString()),
                    db.BuildParameterName("JGH" + i.ToString()), db.BuildParameterName("CMT" + i.ToString()),
                    db.BuildParameterName("FKZH" + i.ToString()), db.BuildParameterName("FKMC" + i.ToString()),
                    db.BuildParameterName("FKHHH" + i.ToString()), db.BuildParameterName("FKHMC" + i.ToString()),
                    db.BuildParameterName("SKZH" + i.ToString()), db.BuildParameterName("SKMC" + i.ToString()),
                    db.BuildParameterName("SKHHH" + i.ToString()), db.BuildParameterName("SKHMC" + i.ToString()),
                    db.BuildParameterName("BZDM" + i.ToString()), db.BuildParameterName("TX_ZMT" + i.ToString()),
                    db.BuildParameterName("CJLL" + i.ToString()), db.BuildParameterName("CJQX" + i.ToString()),
                    db.BuildParameterName("YWZL" + i.ToString()), db.BuildParameterName("BZ" + i.ToString()),
                    db.BuildParameterName("CZY" + i.ToString()), db.BuildParameterName("SSQD" + i.ToString()),
                    db.BuildParameterName("JZBZ" + i.ToString()), db.BuildParameterName("HOST_SEQ" + i.ToString()),
                    db.BuildParameterName("HOST_DATE" + i.ToString()), db.BuildParameterName("SXF" + i.ToString()),
                    db.BuildParameterName("HOST_RET" + i.ToString()), db.BuildParameterName("HOST_RETMSG" + i.ToString()),
                    db.BuildParameterName("JYRQ" + i.ToString()), db.BuildParameterName("JYSJ" + i.ToString())
                    );
                i++;
            }

            string sql = strSql.ToString().TrimEnd(',');

            DbCommand dbCommand = db.GetSqlStringCommand(sql);
            i = 0;
            foreach (var pay in datalist)
            {
                db.AddInParameter(dbCommand, "PLAT_DATE" + i.ToString(), DbType.String, pay.PlatDate);
                db.AddInParameter(dbCommand, "SEQ_NO" + i.ToString(), DbType.String, pay.SeqNO);
                db.AddInParameter(dbCommand, "QDBZ" + i.ToString(), DbType.String, pay.ChanelFlag);
                db.AddInParameter(dbCommand, "ZJLSH" + i.ToString(), DbType.String, pay.SysSeqNO);
                db.AddInParameter(dbCommand, "JGH" + i.ToString(), DbType.String, pay.OrganNO);
                db.AddInParameter(dbCommand, "CMT" + i.ToString(), DbType.String, pay.MsgNO);
                db.AddInParameter(dbCommand, "FKZH" + i.ToString(), DbType.String, pay.PayAccount);
                db.AddInParameter(dbCommand, "FKMC" + i.ToString(), DbType.String, pay.PayName);
                db.AddInParameter(dbCommand, "FKHHH" + i.ToString(), DbType.String, pay.PayBankNO);
                db.AddInParameter(dbCommand, "FKHMC" + i.ToString(), DbType.String, pay.PayBankName);
                db.AddInParameter(dbCommand, "SKZH" + i.ToString(), DbType.String, pay.RecvAccount);
                db.AddInParameter(dbCommand, "SKMC" + i.ToString(), DbType.String, pay.RecvName);
                db.AddInParameter(dbCommand, "SKHHH" + i.ToString(), DbType.String, pay.RecvBankNO);
                db.AddInParameter(dbCommand, "SKHMC" + i.ToString(), DbType.String, pay.RecvBankName);
                db.AddInParameter(dbCommand, "BZDM" + i.ToString(), DbType.String, pay.Currency);
                db.AddInParameter(dbCommand, "TX_ZMT" + i.ToString(), DbType.String, pay.Amount);
                db.AddInParameter(dbCommand, "CJLL" + i.ToString(), DbType.String, pay.IBORate);
                db.AddInParameter(dbCommand, "CJQX" + i.ToString(), DbType.String, pay.IBOLimit);
                db.AddInParameter(dbCommand, "YWZL" + i.ToString(), DbType.String, pay.BizType);
                db.AddInParameter(dbCommand, "BZ" + i.ToString(), DbType.String, pay.Note);
                db.AddInParameter(dbCommand, "CZY" + i.ToString(), DbType.String, pay.Operator);
                db.AddInParameter(dbCommand, "SSQD" + i.ToString(), DbType.String, pay.UploadChannel);
                db.AddInParameter(dbCommand, "JZBZ" + i.ToString(), DbType.String, pay.AccountFlag);
                db.AddInParameter(dbCommand, "HOST_SEQ" + i.ToString(), DbType.String, pay.HostFlowNO);
                db.AddInParameter(dbCommand, "HOST_DATE" + i.ToString(), DbType.String, pay.HostTradeDate);
                db.AddInParameter(dbCommand, "SXF" + i.ToString(), DbType.String, pay.Fee);
                db.AddInParameter(dbCommand, "HOST_RET" + i.ToString(), DbType.String, pay.HostRespCode);
                db.AddInParameter(dbCommand, "HOST_RETMSG" + i.ToString(), DbType.String, pay.HostRespMsg);
                db.AddInParameter(dbCommand, "JYRQ" + i.ToString(), DbType.String, pay.TradeDate);
                db.AddInParameter(dbCommand, "JYSJ" + i.ToString(), DbType.String, pay.TradeTime);
                i++;
            }

            return db.ExecuteNonQuery(dbCommand);  
        }

    }
}
