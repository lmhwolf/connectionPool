using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

using ConnectionPool.Interface;

namespace ConnectionPool.Core
{
    /// <summary>
    /// ****************************************
    /// * 李明浩  2017/09/18 QQ：61491085      *
    /// * 数据库连接池，包括初始化、销毁和获取 *
    /// ****************************************
    /// </summary>
    internal class ConnectionPool<T> : ConnectionPoolInterface<T> where T:class, new()
    {
        #region property

        private static  ConcurrentDictionary<T, int> currentDic = null;

        private int _minSize = 50;

        /// <summary>
        /// 连接池最小数量
        /// </summary>
        public int minSize {
            get
            {
                return _minSize;
            }
            set
            {
                _minSize = value;
            }
        }

        private int _maxSize = 200;

        /// <summary>
        /// 连接池最大数量
        /// </summary>
        public int maxSize {
            get
            {
                return _maxSize;
            }
            set
            {
                _maxSize = value;
            }
        }

        /// <summary>
        /// 连接池初始化步长
        /// </summary>
        private  const int stepSize = 10;

        /// <summary>
        /// 数据连接池状态
        /// </summary>
        private  const int connectionStatus = (int)ConnectionState.Closed;

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        private T sqlConnection = default(T);

        /// <summary>
        /// ConnectionPool类对象
        /// </summary>
        private static ConnectionPool<T> connectionPool = null;

        /// <summary>
        /// 锁对象
        /// </summary>
        private static object lockObj = new object();

        #endregion

        #region private method

        #region 私有构造函数

        /// <summary>
        /// 私有构造函数
        /// </summary>
        private ConnectionPool()
        {
        }

        #endregion

        #region 搜索字典中可用的连接池对象

        /// <summary>
        /// 搜索字典中可用的连接池对象
        /// </summary>
        /// <param name="currentDic"></param>
        /// <returns></returns>
        private T searchConnection()
        {
            T conn = default(T);

            if (currentDic == null || currentDic.Count == 0) return conn;

            foreach (var k in currentDic.Keys)
            {
                if (currentDic[k] == (int)ConnectionState.Closed)
                {
                    conn = k;
                    currentDic[k] = (int)ConnectionState.Open;

                    break;
                }
            }

            return conn;
        }

        #endregion

        #endregion

        #region public method

        #region 获取连接池单例对象

        /// <summary>
        /// 获取连接池单例对象
        /// </summary>
        /// <returns></returns>
        public static ConnectionPool<T> getInstance()
        {
            if (connectionPool == null)
            {
                lock (lockObj)
                {
                    if (connectionPool == null)
                    {
                        connectionPool = new ConnectionPool<T>();
                    }
                }
            }

            return connectionPool;
        }

        #endregion

        #region 初始化连接池

        /// <summary>
        /// 初始化连接池
        /// </summary>
        public void initConnection()
        {
            if (minSize <= 0) throw new ArgumentException("最小连接数不能为0");
            if (minSize > maxSize) throw new ArgumentException("最小连接数不能大于最大连接数");
            if (currentDic == null) currentDic = new ConcurrentDictionary<T, int>();
            if (currentDic.Count > 0) clearConnection();

            Parallel.For(0, minSize, (k) =>
            {
                sqlConnection = new T();

                currentDic.TryAdd(sqlConnection, connectionStatus);
            });
        }

        #endregion

        #region 如果无可用连接,按照指定步长生成连接

        /// <summary>
        /// 如果无可用连接,按照指定步长生成连接
        /// </summary>
        public void stepInitConnection()
        {
            if (stepSize <= 0) throw new ArgumentException("创建连接步长必须大于0");
            if (currentDic == null) currentDic = new ConcurrentDictionary<T, int>();

            int currentDicCount = currentDic.Count;
            int initCount = stepSize;

            if (currentDicCount + stepSize > maxSize) initCount = maxSize - currentDicCount;

            Parallel.For(1, initCount, (k) =>
            {
                sqlConnection = new T();

                currentDic.TryAdd(sqlConnection, connectionStatus);
            });
        }

        #endregion

        #region 获取可用连接池对象

        /// <summary>
        /// 获取可用的连接池对象
        /// </summary>
        /// <returns></returns>
        public T getConnecion()
        {
            if (currentDic == null) initConnection();

            int currentDicCount = currentDic.Count;

            sqlConnection = searchConnection();

            if (sqlConnection == null && currentDicCount < maxSize)
            {
                stepInitConnection();

                sqlConnection = searchConnection();
            }

            return sqlConnection;
        }

        #endregion

        #region 搜索字典中可用的连接池对象

        /// <summary>
        /// 搜索字典中可用的连接池对象
        /// </summary>
        /// <param name="currentDic"></param>
        /// <returns></returns>
        public void closeConnection(T conn)
        {
            foreach (var k in currentDic.Keys)
            {
                if (k.Equals(conn))
                {
                    if (currentDic[k] == (int)ConnectionState.Open) currentDic[k] = (int)ConnectionState.Closed;

                    break;
                }
            }
        }

        #endregion

        #region 销毁线程池中的连接

        /// <summary>
        /// 销毁线程池中的连接
        /// </summary>
        public void clearConnection()
        {
            if (currentDic == null || currentDic.Count == 0) return;

            currentDic.Clear();
            currentDic = null;
        }

        #endregion

        #endregion
    }
}
