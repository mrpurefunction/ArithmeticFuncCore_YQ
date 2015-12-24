using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PublicLib;
using Microsoft.JScript.Vsa;

namespace ContextFuncCore
{
    /// <summary>
    /// 
    /// </summary>
    public class ContextFunc
    {
        /// <summary>
        /// 
        /// </summary>
        public logtype ltype { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public static VsaEngine ve = Microsoft.JScript.Vsa.VsaEngine.CreateEngine();

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="lt"></param>
        public ContextFunc(logtype lt)
        {
            ltype = lt;
        }

        /// <summary>
        /// calculate the value of the formula expression
        /// </summary>
        /// <param name="cv"></param>
        public void CalValue(ContextValue cv)
        {
            try
            {
                bool? rtn = null;
                DateTime st;
                DateTime et;

                //time control--1th symbol
                if (cv.end.Contains("Now") || cv.end.ToLower().Contains("now"))
                {
                    st = (DateTime)cv.ts;
                    if ((cv.end.Contains('m')) || (cv.end.Contains('M')))
                    {
                        int offset = int.Parse(cv.end.Substring(4, cv.end.Length - 6));
                        st = st.AddMinutes(offset);
                    }
                    else if ((cv.end.Contains('h')) || (cv.end.Contains('H')))
                    {
                        int offset = int.Parse(cv.end.Substring(4, cv.end.Length - 6));
                        st = st.AddHours(offset);
                    }
                    else
                    {
                        cv.es = true;
                        ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "ContextFunc?" + "未找到相应offset时间标志", ts = DateTime.Now };
                        (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                        return;
                    }
                }
                else
                {
                    cv.es = true;
                    ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "ContextFunc?" + "出现不能解析的时间标志", ts = DateTime.Now };
                    (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                    return;
                }

                //time control--2th symbol
                if (cv.offsetahead.Contains("Now") || cv.offsetahead.ToLower().Contains("now"))
                {
                    et = (DateTime)cv.ts;
                    if ((cv.offsetahead.Contains('m')) || (cv.offsetahead.Contains('M')))
                    {
                        int offset = int.Parse(cv.offsetahead.Substring(4, cv.offsetahead.Length - 6));
                        et = et.AddMinutes(offset);
                    }
                    else if ((cv.offsetahead.Contains('h')) || (cv.offsetahead.Contains('H')))
                    {
                        int offset = int.Parse(cv.offsetahead.Substring(4, cv.offsetahead.Length - 6));
                        et = et.AddHours(offset);
                    }
                    else
                    {
                        cv.es = true;
                        ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "ContextFunc?" + "未找到相应offset时间标志", ts = DateTime.Now };
                        (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                        return;
                    }
                }
                else
                {
                    cv.es = true;
                    ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "ContextFunc?" + "出现不能解析的时间标志", ts = DateTime.Now };
                    (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                    return;
                }
                
                if (st > et)
                {
                    DateTime temp = st;
                    st = et;
                    et = temp;
                }

                //get the value
                if ((cv.fn == "IsExist") || (cv.fn.ToLower() == "isexist"))
                {
                    rtn = (new SQL.ContextRules("dbconn")).IsExist(cv.rn, st, et);                 
                    if (rtn == null)
                    {
                        cv.es = true;
                        //ExceptionBody eb3 = new ExceptionBody() { et = ExceptionType.Warning, info = "LogicFunc?" + cv.fn + " " + cv.rn + "未能获取上下文函数值", ts = DateTime.Now };
                        //(new PublicLib.Log()).AddExceptionLog(eb3, logtype.console);
                        return;
                    }
                    else
                    {
                        cv.result = rtn;
                        cv.es = false;
                    }
                }
                else if ((cv.fn == "NotExist") || (cv.fn.ToLower() == "notexist"))
                {
                    rtn = (new SQL.ContextRules("dbconn")).NotExist(cv.rn, st, et);
                    if (rtn == null)
                    {
                        cv.es = true;
                        //ExceptionBody eb3 = new ExceptionBody() { et = ExceptionType.Warning, info = "LogicFunc?" + cv.fn + " " + cv.rn + "未能获取上下文函数值", ts = DateTime.Now };
                        //(new PublicLib.Log()).AddExceptionLog(eb3, logtype.console);
                        return;
                    }
                    else
                    {
                        cv.result = rtn;
                        cv.es = false;
                    }
                }
                else if ((cv.fn == "IsExistG") || (cv.fn.ToLower() == "isexistg"))
                {
                    rtn = (new SQL.ContextRules("dbconn")).IsExistG(cv.rn, st, et);
                    if (rtn == null)
                    {
                        cv.es = true;
                        //ExceptionBody eb3 = new ExceptionBody() { et = ExceptionType.Warning, info = "LogicFunc?" + cv.fn + " " + cv.rn + "未能获取上下文函数值", ts = DateTime.Now };
                        //(new PublicLib.Log()).AddExceptionLog(eb3, logtype.console);
                        return;
                    }
                    else
                    {
                        cv.result = rtn;
                        cv.es = false;
                    }
                }
                else if ((cv.fn == "NotExistG") || (cv.fn.ToLower() == "notexistg"))
                {
                    rtn = (new SQL.ContextRules("dbconn")).NotExistG(cv.rn, st, et);
                    if (rtn == null)
                    {
                        cv.es = true;
                        //ExceptionBody eb3 = new ExceptionBody() { et = ExceptionType.Warning, info = "LogicFunc?" + cv.fn + " " + cv.rn + "未能获取上下文函数值", ts = DateTime.Now };
                        //(new PublicLib.Log()).AddExceptionLog(eb3, logtype.console);
                        return;
                    }
                    else
                    {
                        cv.result = rtn;
                        cv.es = false;
                    }
                }
                else if ((cv.fn == "IsExistG2") || (cv.fn.ToLower() == "isexistg2"))
                {
                    rtn = (new SQL.ContextRules("dbconn")).IsExistG(cv.rn, st);
                    if (rtn == null)
                    {
                        cv.es = true;
                        //ExceptionBody eb3 = new ExceptionBody() { et = ExceptionType.Warning, info = "LogicFunc?" + cv.fn + " " + cv.rn + "未能获取上下文函数值", ts = DateTime.Now };
                        //(new PublicLib.Log()).AddExceptionLog(eb3, logtype.console);
                        return;
                    }
                    else
                    {
                        cv.result = rtn;
                        cv.es = false;
                    }
                }
                else if ((cv.fn == "NotExistG2") || (cv.fn.ToLower() == "notexistg2"))
                {
                    rtn = (new SQL.ContextRules("dbconn")).NotExistG(cv.rn, st);
                    if (rtn == null)
                    {
                        cv.es = true;
                        //ExceptionBody eb3 = new ExceptionBody() { et = ExceptionType.Warning, info = "LogicFunc?" + cv.fn + " " + cv.rn + "未能获取上下文函数值", ts = DateTime.Now };
                        //(new PublicLib.Log()).AddExceptionLog(eb3, logtype.console);
                        return;
                    }
                    else
                    {
                        cv.result = rtn;
                        cv.es = false;
                    }
                }
                else if ((cv.fn == "IsExistG_Calib_UF") || (cv.fn.ToLower() == "isexistg_calib_uf"))
                {
                    rtn = (new SQL.ContextRules("dbconn")).IsExistG_Calib_UF(cv.rn, st, et);
                    if (rtn == null)
                    {
                        cv.es = true;
                        //ExceptionBody eb3 = new ExceptionBody() { et = ExceptionType.Warning, info = "LogicFunc?" + cv.fn + " " + cv.rn + "未能获取上下文函数值", ts = DateTime.Now };
                        //(new PublicLib.Log()).AddExceptionLog(eb3, logtype.console);
                        return;
                    }
                    else
                    {
                        cv.result = rtn;
                        cv.es = false;
                    }
                }
                else if ((cv.fn == "NotExistG_Calib_UF") || (cv.fn.ToLower() == "notexistg_calib_uf"))
                {
                    rtn = (new SQL.ContextRules("dbconn")).NotExistG_Calib_UF(cv.rn, st, et);
                    if (rtn == null)
                    {
                        cv.es = true;
                        //ExceptionBody eb3 = new ExceptionBody() { et = ExceptionType.Warning, info = "LogicFunc?" + cv.fn + " " + cv.rn + "未能获取上下文函数值", ts = DateTime.Now };
                        //(new PublicLib.Log()).AddExceptionLog(eb3, logtype.console);
                        return;
                    }
                    else
                    {
                        cv.result = rtn;
                        cv.es = false;
                    }
                }
                else
                {
                    cv.es = true;
                    ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "ContextFunc?" + "未找到相应函数实现", ts = DateTime.Now };
                    (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                    return;
                }
                ExceptionBody eb2 = new ExceptionBody() { et = ExceptionType.Message, info = "ContextFunc?" + "数据正常刷新", ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb2, logtype.console);
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "ContextFunc?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                cv.es = true;
            }
        }
    }
}
