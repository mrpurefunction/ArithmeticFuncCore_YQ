using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PublicLib;
using Microsoft.JScript.Vsa;

namespace LogicFuncCore
{
    /// <summary>
    /// 
    /// </summary>
    public class LogicFunc
    {
        /// <summary>
        /// 
        /// </summary>
        public logtype ltype { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private static VsaEngine ve = Microsoft.JScript.Vsa.VsaEngine.CreateEngine();

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="lt"></param>
        public LogicFunc(logtype lt)
        {
            ltype = lt;
        }

        /// <summary>
        /// calculate
        /// </summary>
        /// <param name="lp"></param>
        public void CalValue(LogicPoint lp)
        {
            try
            {
                string tempfm = lp.fm;
                //还没有计算
                if (lp.es == null)
                {
                    string[] points = LogicFuncCore.RegFunc.GetPointString(tempfm);
                    foreach (string p in points)
                    {
                        if (p == null)
                        {
                            break;
                        }
                        if (ArithmeticFuncCore.DataArea.cd[p.Split(':')[1]].es == true)
                        {
                            lp.es = true;
                            ExceptionBody eb3 = new ExceptionBody() { et = ExceptionType.Warning, info = "LogicFunc?" + lp.fm + " " + p.Split(':')[1] + "未能获取计量点值", ts = DateTime.Now };
                            (new PublicLib.Log()).AddExceptionLog(eb3, logtype.console);
                            return;
                        }
                        else
                        {
                            tempfm = tempfm.Replace("[" + p + "]", ArithmeticFuncCore.DataArea.cd[p.Split(':')[1]].pv.ToString());
                        }
                    }          
                    string fn;
                    while ((fn = RegFunc.GetLastFuncString(tempfm)) != null)
                    {
                        bool? r = null;
                        if (fn.Contains("GE("))
                        {
                            string[] opers = fn.Substring(3, fn.Length - 3 - 1).Split(',');
                            r = PublicLib.Common.GE(decimal.Parse(opers[0]), decimal.Parse(opers[1]));                         
                        }
                        else if(fn.Contains("LE("))
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
                            lp.es = true;
                            ExceptionBody eb2 = new ExceptionBody() { et = ExceptionType.Error, info = "LogicFunc?" + lp.fm + "未找到相应函数实现", ts = DateTime.Now };
                            (new PublicLib.Log()).AddExceptionLog(eb2, logtype.console);
                            return;
                        }

                        if (r == null)
                        {
                            lp.es = true;
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
                lp.es = false;
                if (rtn == 0)
                {
                    lp.result = false;
                }
                else
                {
                    lp.result = true;
                }
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Message, info = "LogicFunc?" + lp.fm + "数据正常刷新", ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "LogicFunc?" + lp.fm + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                lp.es = true;
            }
        }
    }
}
