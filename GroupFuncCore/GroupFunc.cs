using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PublicLib;
using Microsoft.JScript.Vsa;

namespace GroupFuncCore
{
    /// <summary>
    /// 
    /// </summary>
    public class GroupFunc
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
        /// 
        /// </summary>
        public static System.Configuration.AppSettingsReader asr = new System.Configuration.AppSettingsReader(); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lt"></param>
        public GroupFunc(logtype lt)
        {
            ltype = lt;
        }

        /// <summary>
        /// calculate the value of expression
        /// </summary>
        /// <param name="gv"></param>
        public void CalValue(GroupValue gv)
        {
            try
            {
                string tempfm = gv.fm;
                //如果还没有计算
                if (gv.es == null)
                {
                    //recursive
                    string[] rules = GroupFuncCore.RegFunc.GetRuleString(tempfm);
                    foreach (string r in rules)
                    {
                        if (r == null)
                        {
                            break;
                        }
                        CalValue(GroupFuncCore.DataArea.cd[r.Split(':')[1]]);
                        if (GroupFuncCore.DataArea.cd[r.Split(':')[1]].es == true)
                        {
                            gv.es = true;
                            ExceptionBody eb3 = new ExceptionBody() { et = ExceptionType.Warning, info = "GroupFunc?" + "未获得相应规则点的值", ts = DateTime.Now };
                            (new PublicLib.Log()).AddExceptionLog(eb3, logtype.console);
                            return;
                        }
                        else
                        {
                            tempfm = tempfm.Replace("[" + r + "]", (GroupFuncCore.DataArea.cd[r.Split(':')[1]].result) == true ? "1" : "0");
                        }
                    }
                    //logic rule or ctx rule
                    string[] points = GroupFuncCore.RegFunc.GetPointString(tempfm);
                    foreach (string p in points)
                    {
                        if (p == null)
                        {
                            break;
                        }
                        if (p.Split(':')[0].ToLower() == "ctxrule")
                        {
                            if (ContextFuncCore.DataArea.cd[p.Split(':')[1]] != null)
                            {
                                if (ContextFuncCore.DataArea.cd[p.Split(':')[1]].es == true)
                                {
                                    gv.es = true;
                                    ExceptionBody eb3 = new ExceptionBody() { et = ExceptionType.Warning, info = "GroupFunc?" + "未获得上下文规则点的值", ts = DateTime.Now };
                                    (new PublicLib.Log()).AddExceptionLog(eb3, logtype.console);
                                    return;
                                }
                                else
                                {
                                    if (ContextFuncCore.DataArea.cd[p.Split(':')[1]].result == true)
                                    {
                                        tempfm = tempfm.Replace("[" + p + "]", "1");
                                    }
                                    else
                                    {
                                        tempfm = tempfm.Replace("[" + p + "]", "0");
                                    }
                                }
                            }
                            else
                            {
                                gv.es = true;
                                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Warning, info = "GroupFunc?" + "未找到相应上下文规则", ts = DateTime.Now };
                                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                                return;
                            }
                        }
                        else if (p.Split(':')[0].ToLower() == "lrule")
                        {
                            if (LogicFuncCore.DataArea.cd[p.Split(':')[1]] != null)
                            {
                                if (LogicFuncCore.DataArea.cd[p.Split(':')[1]].es == true)
                                {
                                    gv.es = true;
                                    ExceptionBody eb3 = new ExceptionBody() { et = ExceptionType.Warning, info = "GroupFunc?" + "未获得逻辑规则点的值", ts = DateTime.Now };
                                    (new PublicLib.Log()).AddExceptionLog(eb3, logtype.console);
                                    return;
                                }
                                else
                                {
                                    if (LogicFuncCore.DataArea.cd[p.Split(':')[1]].result == true)
                                    {
                                        tempfm = tempfm.Replace("[" + p + "]", "1");
                                    }
                                    else
                                    {
                                        tempfm = tempfm.Replace("[" + p + "]", "0");
                                    }
                                }
                            }
                            else
                            {
                                if ((string)asr.GetValue("LogicRuleNullValueAsFalseInGroupRule", typeof(string)) != "true")
                                {
                                    gv.es = true;
                                    ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Warning, info = "GroupFunc?" + "未找到相应逻辑规则", ts = DateTime.Now };
                                    (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                                    return;
                                }
                                else
                                {
                                    //if "LogicRuleNullValueAsFalseInGroupRule" is true, set the value to false(0)  
                                    tempfm = tempfm.Replace("[" + p + "]", "0");
                                    ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Warning, info = "GroupFunc?" + "将逻辑规则值null作为false使用", ts = DateTime.Now };
                                    (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);

                                }
                            }
                        }
                        else
                        {
                            gv.es = true;
                            ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "GroupFunc?" + "未找到相应规则实现", ts = DateTime.Now };
                            (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                            return;
                        }
                    }
                    //func
                    string fn;
                    while ((fn = RegFunc.GetLastFuncString(tempfm)) != null)
                    {
                        bool? r = null;
                        if (fn.Contains("GE("))
                        {
                            string[] opers = fn.Substring(3, fn.Length - 3 - 1).Split(',');
                            r = PublicLib.Common.GE(decimal.Parse(opers[0]), decimal.Parse(opers[1]));
                        }
                        else if (fn.Contains("LE("))
                        {
                            string[] opers = fn.Substring(3, fn.Length - 3 - 1).Split(',');
                            r = PublicLib.Common.LE(decimal.Parse(opers[0]), decimal.Parse(opers[1]));
                        }
                        else if (fn.Contains("GT("))
                        {
                            string[] opers = fn.Substring(3, fn.Length - 3 - 1).Split(',');
                            r = PublicLib.Common.GT(decimal.Parse(opers[0]), decimal.Parse(opers[1]));
                        }
                        else if (fn.Contains("LT("))
                        {
                            string[] opers = fn.Substring(3, fn.Length - 3 - 1).Split(',');
                            r = PublicLib.Common.LT(decimal.Parse(opers[0]), decimal.Parse(opers[1]));
                        }
                        else if (fn.Contains("EQ("))
                        {
                            string[] opers = fn.Substring(3, fn.Length - 3 - 1).Split(',');
                            r = PublicLib.Common.EQ(decimal.Parse(opers[0]), decimal.Parse(opers[1]));
                        }
                        else if (fn.Contains("OR("))
                        {
                            string[] opers = fn.Substring(3, fn.Length - 3 - 1).Split(',');
                            r = PublicLib.Common.OR(decimal.Parse(opers[0]), decimal.Parse(opers[1]));
                        }
                        else if (fn.Contains("AND("))
                        {
                            string[] opers = fn.Substring(4, fn.Length - 4 - 1).Split(',');
                            r = PublicLib.Common.AND(decimal.Parse(opers[0]), decimal.Parse(opers[1]));
                        }
                        else if (fn.Contains("EXPR("))
                        {
                            r = PublicLib.Common.LExpr(fn.Substring(5, fn.Length - 5 - 1));
                        }
                        else
                        {
                            gv.es = true;
                            ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "GroupFunc?" + "未找到相应函数实现", ts = DateTime.Now };
                            (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                            return;
                        }

                        if (r == null)
                        {
                            gv.es = true;
                            return;
                        }
                        else
                        {
                            if (r == true)
                            {
                                tempfm = tempfm.Replace(fn, "1");
                            }
                            else
                            {
                                tempfm = tempfm.Replace(fn, "0");
                            }
                        }
                    }
                }
                else
                {
                    return;
                }
                //get the value of the formula
                object rtnobject = Microsoft.JScript.Eval.JScriptEvaluate(tempfm, "unsafe", ve);
                double rtn = double.Parse(rtnobject.ToString());
                gv.es = false;
                if (rtn == 0)
                {
                    gv.result = false;
                }
                else
                {
                    gv.result = true;
                }
                ExceptionBody eb2 = new ExceptionBody() { et = ExceptionType.Message, info = "GroupFunc?" + "数据正常刷新", ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb2, logtype.console);
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "GroupFunc?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                gv.es = true;
            }
        }
    }
}
