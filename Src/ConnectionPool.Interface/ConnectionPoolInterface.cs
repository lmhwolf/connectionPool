using System;
using System.Data.SqlClient;

namespace ConnectionPool.Interface
{
    /// <summary>
    /// ****************************************
    /// * 李明浩  2017/09/18 QQ：61491085      *
    /// * 数据库连接池接口                     *
    /// ****************************************
    /// </summary>
    public interface ConnectionPoolInterface<T>
    {
        /// <summary>
        /// 获取可用的数据库连接
        /// </summary>
        /// <returns></returns>
        T getConnecion();

        /// <summary>
        /// 初始化连接
        /// </summary>
        void initConnection();

        /// <summary>
        /// 按照指定步长生成连接
        /// </summary>
        void stepInitConnection();

        /// <summary>
        /// 销毁线程池的连接
        /// </summary>
        void clearConnection();

        /// <summary>
        /// 关闭线程池连接
        /// </summary>
        /// <param name="conn"></param>
        void closeConnection(T conn);
    }
}
