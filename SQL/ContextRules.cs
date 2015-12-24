using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;

using PublicLib;

namespace SQL
{
    /// <summary>
    /// 
    /// </summary>
    public class ContextRules
    {
        /// <summary>
        /// connection string
        /// </summary>
        public string connstr { get; set; }
        /// <summary>
        /// 是否存在?
        /// </summary>
        /// <param name="rn"></param>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <returns></returns>
        public bool? IsExist(string rn,DateTime st,DateTime et)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select count(*) from LogicRuleAlarmResult t where rulename='");
                sb.Append(rn);
                sb.Append("' and ");
                sb.Append("t.timestamps <= '" + et.ToString("yyyy-MM-dd HH:mm:ss") + "' and ");
                sb.Append("t.timestamps >= '" + st.ToString("yyyy-MM-dd HH:mm:ss") + "'");
                Database db = DatabaseFactory.CreateDatabase(connstr);
                System.Data.Common.DbCommand dbc = db.GetSqlStringCommand(sb.ToString());
                int count = (int)db.ExecuteScalar(dbc);
                if (count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }            
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "SQL-Context-IsExist?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }

        /// <summary>
        /// 是否不存在?
        /// </summary>
        /// <param name="rn"></param>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <returns></returns>
        public bool? NotExist(string rn, DateTime st, DateTime et)
        {
            try
            {
                bool? rtn = IsExist(rn,st,et);
                if (rtn == null)
                {
                    return null;
                }
                else
                {
                    return !rtn;
                }
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "SQL-Context-NotExist?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }

        /// <summary>
        /// 分组规则是否存在?
        /// </summary>
        /// <param name="rn"></param>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <returns></returns>
        public bool? IsExistG(string rn, DateTime st, DateTime et)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select count(*) from GroupRuleResult t where rulename='");
                sb.Append(rn);
                sb.Append("' and ");
                sb.Append("t.timestamps <= '" + et.ToString("yyyy-MM-dd HH:mm:ss") + "' and ");
                sb.Append("t.timestamps >= '" + st.ToString("yyyy-MM-dd HH:mm:ss") + "'");
                Database db = DatabaseFactory.CreateDatabase(connstr);
                System.Data.Common.DbCommand dbc = db.GetSqlStringCommand(sb.ToString());
                int count = (int)db.ExecuteScalar(dbc);
                if (count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "SQL-Context-IsExistG?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }

        /// <summary>
        /// IsExistG2
        /// </summary>
        /// <param name="rn"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public bool? IsExistG(string rn, DateTime ts)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select count(*) from GroupRuleResult t where rulename='");
                sb.Append(rn);
                sb.Append("' and ");
                sb.Append("t.timestamps = '" + ts.ToString("yyyy-MM-dd HH:00:00") + "'");
                Database db = DatabaseFactory.CreateDatabase(connstr);
                System.Data.Common.DbCommand dbc = db.GetSqlStringCommand(sb.ToString());
                int count = (int)db.ExecuteScalar(dbc);
                if (count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "SQL-Context-IsExistG2?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }

        /// <summary>
        /// 分组规则是否不存在
        /// </summary>
        /// <param name="rn"></param>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <returns></returns>
        public bool? NotExistG(string rn, DateTime st, DateTime et)
        {
            try
            {
                bool? rtn = IsExistG(rn, st, et);
                if (rtn == null)
                {
                    return null;
                }
                else
                {
                    return !rtn;
                }
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "SQL-Context-NotExistG?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }

        /// <summary>
        /// NotExistG2
        /// </summary>
        /// <param name="rn"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public bool? NotExistG(string rn, DateTime ts)
        {
            try
            {
                bool? rtn = IsExistG(rn, ts);
                if (rtn == null)
                {
                    return null;
                }
                else
                {
                    return !rtn;
                }
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "SQL-Context-NotExistG2?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="rn"></param>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <returns></returns>
        public bool? IsExistG_Calib_UF(string rn, DateTime st, DateTime et)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select count(*) from GroupRuleResult t where rulename='");
                sb.Append(rn);
                sb.Append("' and ");
                sb.Append("t.timestamps <= '" + et.ToString("yyyy-MM-dd HH:mm:ss") + "' and ");
                sb.Append("t.timestamps >= '" + st.ToString("yyyy-MM-dd HH:mm:ss") + "' and ");
                sb.Append("t.grouptype = '仪表标定' and t.timestamps>t.timestamp2");
                Database db = DatabaseFactory.CreateDatabase(connstr);
                System.Data.Common.DbCommand dbc = db.GetSqlStringCommand(sb.ToString());
                int count = (int)db.ExecuteScalar(dbc);
                if (count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "SQL-Context-IsExistG_Calib_UF?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rn"></param>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <returns></returns>
        public bool? NotExistG_Calib_UF(string rn, DateTime st, DateTime et)
        {
            try
            {
                bool? rtn = IsExistG_Calib_UF(rn, st, et);
                if (rtn == null)
                {
                    return null;
                }
                else
                {
                    return !rtn;
                }
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "SQL-Context-NotExistG_Calib_UF?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="dbstr"></param>
        public ContextRules(string dbstr)
        {
            if (dbstr != null)
            {
                connstr = dbstr;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rulename"></param>
        /// <param name="ts"></param>
        /// <param name="pointname"></param>
        public void AddCtxRuleRd(string rulename, DateTime ts, string pointname)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateCtxRuleRd()
        {
        }
    }
}
