using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System;
using InterDesignCad.Util;
//using Autodesk.AutoCAD.DatabaseServices;
using CadObjId = Autodesk.AutoCAD.DatabaseServices.ObjectId;
using CadObjIdCollection = Autodesk.AutoCAD.DatabaseServices.ObjectIdCollection;

namespace InterDesignCad.Db
{
    public class SqliteHelper
    {
        //private readonly static string connStr = ConfigurationManager.ConnectionStrings["intercad"].ConnectionString;
        private readonly static string connStr = "Data Source =" + SysUtil.getCfgPath() + "db\\cad.db;password=intercad"; 
        //获取 appsetting 设置的值
        //private readonly static string appStr = ConfigurationManager.AppSettings["TestKey"];

        //获取 connection 对象
        public static IDbConnection CreateConnection()
        {
            IDbConnection conn = new SQLiteConnection(connStr);//MySqlConnection //SqlConnection

            conn.Open();
            return conn;
        }

        //执行非查询语句
        public static int ExecuteNonQuery(IDbConnection conn, string sql, Dictionary<string, object> parameters)
        {
            using (IDbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                foreach (KeyValuePair<string, object> keyValuePair in parameters)
                {
                    IDbDataParameter parameter = cmd.CreateParameter();
                    parameter.ParameterName = keyValuePair.Key;
                    parameter.Value = keyValuePair.Value;
                    cmd.Parameters.Add(parameter);
                }
                return cmd.ExecuteNonQuery();
            }
        }

        //执行非查询语句-独立连接
        public static int ExecuteNonQuery(string sql, Dictionary<string, object> parameters)
        {
            using (IDbConnection conn = CreateConnection())
            {
                return ExecuteNonQuery(conn, sql, parameters);
            }
        }

        //查询首行首列
        public static object ExecuteScalar(IDbConnection conn, string sql, Dictionary<string, object> parameters)
        {
            using (IDbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                foreach (KeyValuePair<string, object> keyValuePair in parameters)
                {
                    IDbDataParameter parameter = cmd.CreateParameter();
                    parameter.ParameterName = keyValuePair.Key;
                    parameter.Value = keyValuePair.Value;
                    cmd.Parameters.Add(parameter);
                }
                return cmd.ExecuteScalar();
            }
        }

        //查询首行首列-独立连接
        public static object ExecuteScalar(string sql, Dictionary<string, object> parameters)
        {
            using (IDbConnection conn = CreateConnection())
            {
                return ExecuteScalar(conn, sql, parameters);
            }
        }

        //查询表
        public static DataTable ExecuteQuery(IDbConnection conn, string sql, Dictionary<string, object> parameters)
        {
            DataTable dt = new DataTable();
            using (IDbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                foreach (KeyValuePair<string, object> keyValuePair in parameters)
                {
                    IDbDataParameter parameter = cmd.CreateParameter();
                    parameter.ParameterName = keyValuePair.Key;
                    parameter.Value = keyValuePair.Value;
                    cmd.Parameters.Add(parameter);
                }
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }

            return dt;
        }

        //查询表--独立连接
        public static DataTable ExecuteQuery(string sql, Dictionary<string, object> parameters)
        {
            using (IDbConnection conn = CreateConnection())
            {
                return ExecuteQuery(conn, sql, parameters);
            }
        }


        /// <summary>
        /// 检查数据库是否存在不存在创建
        /// </summary>
        /// <returns></returns>
        public static bool CheckDataBase()
        {
            try
            {
                //判断数据文件是否存在
                CreateConnection();
                //bool dbExist = File.Exists("mesclient.sqlite");
                //if (!dbExist)
                //{
                //    SQLiteConnection.CreateFile("mesclient.sqlite");
                //}

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        /// <summary>
        /// 检查数据表是否存在，不存在创建
        /// </summary>
        /// <returns></returns>
        public static bool CheckDataTable(string connStr)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connStr))
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = 'serverinfo'";
                    object ob = cmd.ExecuteScalar();
                    long tableCount = Convert.ToInt64(ob);
                    if (tableCount == 0)
                    {
                        //创建表
                        cmd.CommandText = @"
            BEGIN;
                create table serverinfo 
                (Id INTEGER PRIMARY KEY AUTOINCREMENT,Name TEXT,
                Url text,DelayTime integer,UsageCounter INTEGER,
                 Status integer,CreateTime DATETIME);
                CREATE UNIQUE INDEX idx_serverInfo ON serverinfo (Name);
            COMMIT;
            ";
                        //此语句返回结果为0
                        int rowCount = cmd.ExecuteNonQuery();
                        return true;
                    }
                    else if (tableCount > 1)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static CadObjId[] GetViewportObjects(int vportnumber)
        {
            string sql = "SELECT * FROM pentity WHERE vportnumber =@vportnumber;";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("vportnumber", vportnumber);

            DataTable dt = ExecuteQuery(sql, parameters);
            CadObjId cadobjid;
            CadObjIdCollection cadobjidls;
            long pobjid,pid1;
            if (dt.Rows.Count > 0)
            {

              pobjid= dt.Rows[0].Field<long>("mobjectid");
              pid1 = dt.Rows[1].Field<long>("mobjectid");
            }
            return null;
        }
        //public static void sql()
        //{
        //    string sql = "SELECT * FROM serverinfo WHERE Name =@ServerName AND Url = @Url and date(CreateTime)=date(@Date);";
        //    Dictionary<string, object> parameters = new Dictionary<string, object>();
        //    parameters.Add("ServerName", endpointElement.Name);
        //    parameters.Add("Url", endpointElement.Address);
        //    parameters.Add("Date", DateTime.Now.ToString("yyyy-MM-dd"));
        //    DataTable dt = SqliteHelper.ExecuteQuery(connStr, sql, parameters);
        //    if (dt.Rows.Count > 0)
        //    {
        //        UsageCounter = dt.Rows[0].Field<long>("UsageCounter");
        //        GetTime = dt.Rows[0].Field<DateTime>("CreateTime");
        //    }


        //}
        //public static void update()
        //{
        //    //存在更新，不存在插入
        //    string updateSql = "REPLACE INTO serverinfo(Name,Url,DelayTime,UsageCounter, Status,CreateTime) VALUES(@Name,@Url,@DelayTime,@UsageCounter,@Status, @CreateTime)";
        //    Dictionary<string, object> ups = new Dictionary<string, object>();
        //    ups.Add("Name", name);
        //    ups.Add("Url", url);
        //    ups.Add("DelayTime", delayTime);
        //    ups.Add("UsageCounter", usageCounter);
        //    ups.Add("Status", status);
        //    ups.Add("CreateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //    int count = SqliteHelper.ExecuteNonQuery(connStr, updateSql, ups);
        //    if (count > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }


        //}

        //public static void delete()
        //{


        //    //删除记录
        //    string updateSql =
        //        "DELETE FROM serverinfo where content=@Content and flag=@Flag;";
        //    Dictionary<string, object> updateParameters = new Dictionary<string, object>();
        //    updateParameters.Add("Content", Content);
        //    updateParameters.Add("Flag", Flag);
        //    int count = SqliteHelper.ExecuteNonQuery(connStr, updateSql, updateParameters);
        //    if (count > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }


        //}


    }
}
