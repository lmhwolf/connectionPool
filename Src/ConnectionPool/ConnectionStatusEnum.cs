using System;
using System.Threading.Tasks;

namespace ConnectionPool.Core
{
    /// <summary>
    /// 连接状态枚举
    /// </summary>
    internal enum ConnectionStatusEnum
    {
        /// <summary>
        /// 关闭
        /// </summary>
        isClose = 0,
        /// <summary>
        /// 打开
        /// </summary>
        isOpen = 1
    }
}
