using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ConnectionPool.Core;

namespace ConnectionPool.Test
{
    class Program
    {
        static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["db_User"].ConnectionString;

        static void Main(string[] args)
        {
            Console.WriteLine("************************ Start ***********************");

            getConnection();
            resetConnection();

            Console.WriteLine("************************ end *************************");

            Console.ReadLine();
        }

        /// <summary>
        /// 获取连接池
        /// </summary>
        static void getConnection()
        {

            SqlConnection conn = null;
            try
            {
                conn = ConnectionPoolProxy<SqlConnection>.getConnecion();

                string strSql = " select top 10 * from  [dbo].[User] with(nolock) ";
                string errorMessage = string.Empty;

                if (conn != null) conn.ConnectionString = connectionString;

                DataSet ds = SqlHelper.executeDataSet(strSql, conn, out errorMessage);
            }
            finally
            {
                ConnectionPoolProxy<SqlConnection>.closeConnecion(conn);
            }
        }

        /// <summary>
        /// 数据库连接池重置
        /// </summary>
        static void resetConnection()
        {
            ConnectionPoolProxy<SqlConnection>.resetConnecion();
        }
    }
}
