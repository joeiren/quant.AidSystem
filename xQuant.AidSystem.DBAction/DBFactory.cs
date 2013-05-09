using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace xQuant.AidSystem.DBAction
{
    public class DBFactory
    {
        private const string ConnectionStringAPP = "ConnectionString.xeq_app";
        private const string ConnectionStringMD = "ConnectionString.xeq_md";
        private const string ConnectionStringTRD = "ConnectionString.xeq_trd";

        private static Database _APP;
        private static Database _MD;
        private static Database _TRD;

        static DBFactory()
        {
            try
            {
                //_APP = DatabaseFactory.CreateDatabase(ConnectionStringAPP);
                //_MD = DatabaseFactory.CreateDatabase(ConnectionStringMD);
                _TRD = DatabaseFactory.CreateDatabase(ConnectionStringTRD);
            }
            catch (Exception e)
            {
                xQuant.Log4.LogHelper.Write(xQuant.Log4.LogLevel.Error, String.Format("DBFactory 静态构造函数异常：{0}", e.ToString()));
            }
        }
        protected DBFactory()
        {

        }
        public static Database APP
        {
            get { return _TRD; }
        }

        public static Database MD
        {
            get { return _TRD; }
        }

        public static Database TRD
        {
            get { return _TRD; }
        }

        public static void Reset()
        {
            try
            {
                //_APP = DatabaseFactory.CreateDatabase(ConnectionStringAPP);
                //_MD = DatabaseFactory.CreateDatabase(ConnectionStringMD);
                _TRD = DatabaseFactory.CreateDatabase(ConnectionStringTRD);
            }
            catch
            {

            }
        }

        #region 获取表的查询sql语句中的字段列表
        /// <summary>
        /// 获取指定表名的架构信息
        /// </summary>
        /// <param name="db"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable GetSchema(Database db, string tableName)
        {
            DbDataAdapter ap = db.GetDataAdapter();
            DbCommand cmd = db.GetSqlStringCommand(string.Format("SELECT * FROM {0}", tableName));
            cmd.Connection = db.CreateConnection();
            ap.SelectCommand = cmd;
            DataTable t = new DataTable();
            ap.FillSchema(t, SchemaType.Mapped);
            return t;
        }

        /// <summary>
        /// 将传入的数据表的列名组织成"prefix.ColumnN as prefix_ColumnN,..."格式
        /// </summary>
        /// <param name="table"></param>
        /// <param name="prefixColumnName"></param>
        /// <returns></returns>
        public static string GetSelectFieldNameList(DataTable table, string prefixTableName)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataColumn col in table.Columns)
            {
                if (sb.Length > 0)
                    sb.Append(", ");
                sb.AppendFormat("{0}.{1} AS {0}_{1}", prefixTableName, col.ColumnName);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 将传入的数据表的列名组织成"prefix.ColumnN as prefix_ColumnN,..."格式
        /// </summary>
        /// <param name="db"></param>
        /// <param name="tableName"></param>
        /// <param name="prefixColumnName"></param>
        /// <returns></returns>
        public static string GetSelectFieldNameList(Database db, string tableName, string prefixTableName)
        {
            return GetSelectFieldNameList(GetSchema(db, tableName), prefixTableName);
        }

        /// <summary>
        /// 取数据库用户名
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static string GetUserId(Database db)
        {
            return GetConnectionElement(db.ConnectionString, "User ID");
        }

        private static string GetConnectionElement(string rawString, string name)
        {
            if (!rawString.Contains(name))
            {
                return string.Empty;
            }

            string value = "";
            int startIndex = rawString.IndexOf(name) + name.Length;
            int endIndex1 = rawString.IndexOf(")", startIndex);
            int endIndex2 = rawString.IndexOf(";", startIndex);

            if (endIndex1 > 0)
            {
                value = rawString.Substring(startIndex, endIndex1 - startIndex);
            }
            else if (endIndex2 > 0)
            {
                value = rawString.Substring(startIndex, endIndex2 - startIndex);
            }

            value = value.Replace("=", "").Trim();

            return value;
        }

        #endregion
    }
}
