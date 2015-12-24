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
    public class GroupRules
    {
        /// <summary>
        /// connection string
        /// </summary>
        public string connstr { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbstr"></param>
        public GroupRules(string dbstr)
        {
            connstr = dbstr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pn"></param>
        /// <param name="rn"></param>
        /// <param name="ts"></param>
        /// <param name="ts2"></param>
        /// <param name="gt"></param>
        /// <param name="gst"></param>
        /// <param name="description"></param>
        public void AddGroupRuleRd(string pn, string rn, DateTime? ts, DateTime? ts2, string gt, string gst, string description, string cemstype)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("insert into GroupRuleResult(pointname,rulename,timestamps,timestamps2,description,grouptype,groupsubtype,updatetime,cemstype) values('");
                sb.Append(pn);
                sb.Append("','");
                sb.Append(rn);
                sb.Append("','");
                sb.Append(((DateTime)ts).ToString("yyyy-MM-dd HH:mm:ss"));
                sb.Append("','");
                sb.Append(((DateTime)ts2).ToString("yyyy-MM-dd HH:mm:ss"));
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
                if (gt == null)
                {
                    sb.Append("");
                }
                else
                {
                    sb.Append(gt);
                }
                sb.Append("','");
                if (gst == null)
                {
                    sb.Append("");
                }
                else
                {
                    sb.Append(gst);
                }
                sb.Append("','");
                sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sb.Append("','" + cemstype + "')");

                Database db = DatabaseFactory.CreateDatabase(connstr);
                System.Data.Common.DbCommand dbc = db.GetSqlStringCommand(sb.ToString());
                db.ExecuteNonQuery(dbc);
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "SQL-Group-AddRd?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pn"></param>
        /// <param name="rn"></param>
        /// <param name="ts"></param>
        /// <param name="ts2"></param>
        /// <param name="gt"></param>
        /// <param name="gst"></param>
        /// <param name="description"></param>
        /// <param name="cemstype"></param>
        public void AddGroupRuleRd2(string pn, DateTime? ts, DateTime? ts2, string gt, string gst, string cemstype)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("insert into t_RulelogS(rulename,timelog,timelogend,alarmlog,validatedgroup,confirmedgroup,alarmdis,cemstype,color,edituser,edittime) values('");
                sb.Append(pn);
                sb.Append("','");
                sb.Append(((DateTime)ts).ToString("yyyy-MM-dd HH:mm:ss"));
                sb.Append("','");
                sb.Append(((DateTime)ts2).ToString("yyyy-MM-dd HH:mm:ss"));
                sb.Append("','");
                if (gt == null)
                {
                    sb.Append("");
                }
                else
                {
                    sb.Append(gt);
                }
                sb.Append("','");

                //validatedgroup
                if (gt == null)
                {
                    sb.Append("");
                }
                else
                {
                    sb.Append(gt);
                }
                sb.Append("','");

                //confirmedgroup
                if (gt == null)
                {
                    sb.Append("");
                }
                else
                {
                    sb.Append(gt);
                }
                sb.Append("','");

                if (gst == null)
                {
                    sb.Append("");
                }
                else
                {
                    sb.Append(gst);
                }
                sb.Append("','");
                if (cemstype == null)
                {
                    sb.Append("");
                }
                else
                {
                    sb.Append(cemstype);
                }

                sb.Append("','1','ICEMSService','");
                sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sb.Append("')");
                Database db = DatabaseFactory.CreateDatabase(connstr);
                System.Data.Common.DbCommand dbc = db.GetSqlStringCommand(sb.ToString());
                db.ExecuteNonQuery(dbc);
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "SQL-Group-AddRd2?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pn"></param>
        /// <param name="rn"></param>
        /// <param name="ts"></param>
        /// <param name="ts2"></param>
        /// <param name="gt"></param>
        /// <param name="gst"></param>
        /// <param name="description"></param>
        public void UpdateGroupRuleRd(long id, string pn, string rn, DateTime ts, DateTime ts2, string gt, string gst, string description)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rn"></param>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <param name="ts"></param>
        public void UpdateGroupRuleRd2_CalibFinalize(string pn, DateTime st, DateTime et, DateTime ts)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("update t_RulelogS set timelogend='");
                sb.Append(ts.ToString("yyyy/MM/dd HH:mm:ss"));
                sb.Append("' where ");
                sb.Append("timelog <= '" + et.ToString("yyyy-MM-dd HH:mm:ss") + "' and ");
                sb.Append("timelog >= '" + st.ToString("yyyy-MM-dd HH:mm:ss") + "' and ");
                sb.Append("rulename='" + pn + "' and ");
                sb.Append("alarmlog='仪表标定' and ");
                sb.Append("timelog>timelogend");

                Database db = DatabaseFactory.CreateDatabase(connstr);
                System.Data.Common.DbCommand dbc = db.GetSqlStringCommand(sb.ToString());
                db.ExecuteNonQuery(dbc);
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "SQL-Group-UpdateRd2_CalibFinalize?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rn"></param>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <param name="ts"></param>
        public void UpdateGroupRuleRd_CalibFinalize(string rn, DateTime st, DateTime et, DateTime ts)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("update GroupRuleResult set timestamps2='");
                sb.Append(ts.ToString("yyyy/MM/dd HH:mm:ss"));
                sb.Append("' where ");
                sb.Append("timestamps <= '" + et.ToString("yyyy-MM-dd HH:mm:ss") + "' and ");
                sb.Append("timestamps >= '" + st.ToString("yyyy-MM-dd HH:mm:ss") + "' and ");
                sb.Append("rulename='" + rn + "' and ");
                sb.Append("grouptype='仪表标定' and ");
                sb.Append("timestamps>timestamps2");

                Database db = DatabaseFactory.CreateDatabase(connstr);
                System.Data.Common.DbCommand dbc = db.GetSqlStringCommand(sb.ToString());
                db.ExecuteNonQuery(dbc);
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "SQL-Group-UpdateRd_CalibFinalize?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pn"></param>
        /// <param name="rn"></param>
        /// <param name="ts"></param>
        /// <param name="ts2"></param>
        /// <param name="gt"></param>
        /// <param name="gst"></param>
        /// <returns></returns>
        public long[] GetSpecifiedRuleRdId(string pn, string rn, DateTime ts, DateTime ts2, string gt, string gst)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
