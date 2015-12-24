using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.SqlClient;
using PublicLib;

namespace SQL
{
    /// <summary>
    /// 
    /// </summary>
    public class LogicRules
    {
        /// <summary>
        /// 
        /// </summary>
        public string connstr { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbstr"></param>
        public LogicRules(string dbstr)
        {
            connstr = dbstr;
        }

        /// <summary>
        /// add result into db
        /// </summary>
        /// <param name="pn"></param>
        /// <param name="rn"></param>
        /// <param name="ts"></param>
        /// <param name="discription"></param>
        /// <param name="type"></param>
        /// <param name="subtype"></param>
        public void AddLogicRuleRd(string pn, string rn, DateTime ts, string description, string type, string subtype)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("insert into LogicRuleAlarmResult(pointname,rulename,timestamps,description,type,subtype,updatetime) values('");
                sb.Append(pn);
                sb.Append("','");
                sb.Append(rn);
                sb.Append("','");
                sb.Append(ts.ToString("yyyy-MM-dd HH:mm:ss"));
                sb.Append("','");
                if (description == null)
                {
                    sb.Append("");
                }
                else
                {
                    sb.Append(description);
                }
                sb.Append("','");
                if (type == null)
                {
                    sb.Append("");
                }
                else
                {
                    sb.Append(type);
                }
                sb.Append("','");
                if (subtype == null)
                {
                    sb.Append("");
                }
                else
                {
                    sb.Append(subtype);
                }
                sb.Append("','");
                sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sb.Append("')");

                Database db = DatabaseFactory.CreateDatabase(connstr);
                System.Data.Common.DbCommand dbc = db.GetSqlStringCommand(sb.ToString());
                db.ExecuteNonQuery(dbc);
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "SQL-Logic-AddRd?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DeleteLogicRuleRd()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateLogicRuleRd()
        {

        }
    }
}
