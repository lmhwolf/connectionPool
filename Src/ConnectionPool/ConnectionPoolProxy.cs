using System;
using System.Linq;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ConnectionPool.Core
{
    /// <summary>
    /// ***********************************
    /// * 李明浩  2017/09/18 QQ：61491085 *
    /// * 数据库连接池代理，对外提供方法  *
    /// ***********************************
    /// </summary>
    public class ConnectionPoolProxy<T> where T: class, new()
    {
        #region property

        static ConnectionPool<T> connectionPool = null;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        static ConnectionPoolProxy()
        {
            connectionPool = ConnectionPool<T>.getInstance();
        }

        #endregion

        #region 获取数据库连接池连接

        /// <summary>
        /// 获取数据库连接池连接
        /// </summary>
        /// <returns></returns>
        public static T getConnecion()
        {
            T sqlConnection = null;

            try
            {
                sqlConnection = connectionPool.getConnecion();
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return sqlConnection;
        }

        #endregion

        #region 关闭连接池连接

        /// <summary>
        /// 关闭连接池连接
        /// </summary>
        /// <returns></returns>
        public static void closeConnecion(T conn)
        {
            try
            {
                Action<T> action = connectionPool.closeConnection;

                action.BeginInvoke(conn,(ar)=>
                {
                   action.EndInvoke(ar);
                },
                null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 重置数据库连接池

        /// <summary>
        /// 重置数据库连接池
        /// </summary>
        /// <returns></returns>
        public static void resetConnecion()
        {
            Task.Factory.StartNew(() =>
            {
                connectionPool.clearConnection();
                connectionPool.initConnection();
            });
        }

        #endregion
    }
}
