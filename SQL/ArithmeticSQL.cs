using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;
using PublicLib;

namespace SQL
{
    /// <summary>
    /// SQL interface in Arithmetic Func
    /// </summary>
    public class ArithmeticSQL
    {
        /// <summary>
        /// 
        /// </summary>
        public string connstr { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbstr"></param>
        public ArithmeticSQL(string dbstr)
        {
            connstr = dbstr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="offset"></param>
        /// <param name="machineid"></param>
        /// <param name="indicatorid"></param>
        /// <returns></returns>
        public double? GetDASHourAvg(DateTime dt, int offset, int machineid, int indicatorid)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from EnvirIndicatorValue t where t.timestamps='");
                sb.Append(dt.AddHours(offset).ToString("yyyy-MM-dd HH:00:00"));
                sb.Append("' and ");
                sb.Append("t.indicatorid = " + indicatorid.ToString()+ " and ");
                sb.Append("t.pointname like '" + machineid.ToString() + "%'");
                Database db = DatabaseFactory.CreateDatabase(connstr);
                System.Data.Common.DbCommand dbc = db.GetSqlStringCommand(sb.ToString());
                DataSet ds = db.ExecuteDataSet(dbc);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Warning, info = "SQL-Arithmetic-GetDASHourAvg?未找到该DAS小时均值", ts = DateTime.Now };
                    (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                    return null;
                }
                else
                {
                    return double.Parse(ds.Tables[0].Rows[0]["indicatorvalue"].ToString());
                }            
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "SQL-Arithmetic-GetDASHourAvg?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }

    }
}
