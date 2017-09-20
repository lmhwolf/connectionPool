using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ConnectionPool.Core;

namespace ConnectionPool.UnitTest
{

    [TestClass]
    class ConnectionPoolTest
    {
        string connectionString = "ata Source=(local)\\SQLSERVER;Initial Catalog=User;User ID=sa;Password=3287960;";

        [TestMethod]
        public void initConnectionPool()
        {
           var conn = ConnectionPoolProxy.getSqlConnecion(connectionString);
        }
    }
}
