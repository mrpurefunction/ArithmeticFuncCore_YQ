﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PublicLib;

namespace GroupFuncCore
{
    /// <summary>
    /// 
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// 
        /// </summary>
        public DataArea da { get; set; }

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

        /// <summary>
        /// load config
        /// </summary>
        /// <param name="path"></param>
        public void LoadGroupRules(string path)
        {
            try
            {
                (new ConfigParser()).GetAllGroupRules(path);
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
            ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Message, info = "清空分组运算状态位", ts = DateTime.Now };
            (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);

            foreach (KeyValuePair<string, GroupFuncCore.GroupValue> kvp in DataArea.cd)
            {
                kvp.Value.es = null;
                kvp.Value.ts = ts;
            }

            ExceptionBody eb2 = new ExceptionBody() { et = ExceptionType.Message, info = "开始分组运算", ts = DateTime.Now };
            (new PublicLib.Log()).AddExceptionLog(eb2, logtype.console);

            foreach (KeyValuePair<string, GroupFuncCore.GroupValue> kvp in DataArea.cd)
            {
                (new GroupFuncCore.GroupFunc(logtype.console)).CalValue(kvp.Value);
            }
        }
    }
}
