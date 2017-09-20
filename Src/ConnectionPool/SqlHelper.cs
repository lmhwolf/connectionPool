using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace ConnectionPool.Core
{
    /// <summary>
    /// ****************************************
    /// * 李明浩  2017/09/18 QQ：61491085      *
    /// * 数据库公共操作类                     *
    /// ****************************************
    /// </summary>
	public class SqlHelper
    {
        #region 返回数据集

        /// <summary>
		/// 返回数据集
		/// </summary>
		/// <param name="strSql"></param>
		/// <param name="conn"></param>
		/// <param name="errorMessage"></param>
		/// <returns></returns>
		public static DataSet executeDataSet(string strSql, SqlConnection conn,out  string errorMessage)
		{
            errorMessage = string.Empty;
            DataSet ds = null;

			try
			{
                using (SqlCommand cmd = new SqlCommand(strSql, conn))
				{
					using (SqlDataAdapter da = new SqlDataAdapter(cmd))
					{
                        if (ds == null) ds = new DataSet();

						da.Fill(ds);
					}
				}
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
			}

			return ds;
        }

        #endregion
    }
}

