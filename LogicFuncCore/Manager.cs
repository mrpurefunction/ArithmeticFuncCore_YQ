using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PublicLib;

namespace LogicFuncCore
{
    /// <summary>
    /// 主管理器
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// data repository
        /// </summary>
        public static DataArea da { get; set; }

        /// <summary>
        /// load config
        /// </summary>
        /// <param name="path"></param>
        public void LoadLogicRules(string path)
        {
            try
            {
                (new ConfigParser()).GetAllLogicRules(path);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// refresh data
        /// </summary>
        /// <param name="ts"></param>
        public void RefreshDataAreaSync(DateTime ts)
        {
            ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Message, info = "清空逻辑运算状态位", ts = DateTime.Now };
            (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);

            foreach (KeyValuePair<string, LogicPoint> kvp in DataArea.cd)
            {
                kvp.Value.es = null;
                kvp.Value.ts = ts;
            }

            ExceptionBody eb2 = new ExceptionBody() { et = ExceptionType.Message, info = "开始逻辑运算", ts = DateTime.Now };
            (new PublicLib.Log()).AddExceptionLog(eb2, logtype.console);

            foreach (KeyValuePair<string, LogicPoint> kvp in DataArea.cd)
            {
                (new LogicFuncCore.LogicFunc(logtype.console)).CalValue(kvp.Value);
            }

            //update to database
            foreach (KeyValuePair<string, LogicPoint> kvp in DataArea.cd)
            {
                if (kvp.Value.es == false)
                {
                    if (kvp.Value.result == true)
                    {
                        (new SQL.LogicRules("dbconn")).AddLogicRuleRd(kvp.Value.pn, kvp.Key, (DateTime)kvp.Value.ts, "", kvp.Value.type, kvp.Value.subtype);
                    }
                }
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        public Manager()
        {
            if (da == null)
            {
                da = new DataArea();
            }
        }
    }
}
