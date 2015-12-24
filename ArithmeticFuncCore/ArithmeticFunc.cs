using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using PublicLib;
using Microsoft.JScript.Vsa;

namespace ArithmeticFuncCore
{
    /// <summary>
    ///  数学运算功能
    /// </summary>
    public class ArithmeticFunc
    {
        /// <summary>
        /// 日志模式
        /// </summary>
        public logtype ltype { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private static VsaEngine ve = Microsoft.JScript.Vsa.VsaEngine.CreateEngine();

        /// <summary>
        /// constructor
        /// </summary>
        public ArithmeticFunc(logtype lt)
        {
            ltype = lt;
        }

        /// <summary>
        /// 计算公式的值
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public void CalValue(ArithmeticFuncCore.PointValue pv)
        {
            try
            {
                string tempfm = pv.fm;
                //如果还没有计算
                if (pv.es == null)
                {
                    string[] points = ArithmeticFuncCore.RegFunc.GetPointString(tempfm);
                    foreach (string p in points)
                    {
                        if (p == null)
                        {
                            break;
                        }
                        if (p.Split(':')[0] == "pv")
                        {
                            CalValue(ArithmeticFuncCore.DataArea.cd[p.Split(':')[1]]);
                            if (ArithmeticFuncCore.DataArea.cd[p.Split(':')[1]].es == true)
                            {
                                pv.es = true;
                                ExceptionBody eb3 = new ExceptionBody() { et = ExceptionType.Warning, info = "ArithmeticFunc?" + "未获得相应计量点的值", ts = DateTime.Now };
                                (new PublicLib.Log()).AddExceptionLog(eb3, logtype.console);
                                return;
                            }
                            else
                            {
                                tempfm = tempfm.Replace("[" + p + "]", ArithmeticFuncCore.DataArea.cd[p.Split(':')[1]].pv.ToString());
                            }
                        }
                        //refer to itself
                        else if (p.Split(':')[0] == "ref")
                        {
                            if (ArithmeticFuncCore.DataArea.cd[p.Split(':')[1]].pv == null)
                            {
                                ArithmeticFuncCore.DataArea.cd[p.Split(':')[1]].pv = 0;
                            }
                            tempfm = tempfm.Replace("[" + p + "]", ArithmeticFuncCore.DataArea.cd[p.Split(':')[1]].pv.ToString());
                        }
                    }
                    string[] funcs = ArithmeticFuncCore.RegFunc.GetFuncString(tempfm);
                    foreach (string f in funcs)
                    {
                        if (f == null)
                        {
                            break;
                        }
                        //func parser
                        if (f.Split(':')[1].Split(',')[0].ToLower() == "avg")
                        {
                            double? rtv;
                            //add -5.0 s to make sure there is appropriate value in pi
                            if ((rtv = (new PI.PIFunc2(null, null, null)).GetAverageValue(f.Split(':')[1].Split(',')[1], ((DateTime)pv.ts).AddMinutes(double.Parse(f.Split(':')[1].Split(',')[2])), (DateTime)pv.ts)) == null)
                            {
                                pv.es = true;
                                return;
                            }
                            else
                            {
                                tempfm = tempfm.Replace("[" + f + "]", rtv.ToString());
                            }
                        }
                        else if (f.Split(':')[1].Split(',')[0].ToLower() == "pihis")
                        {
                            double? rtv;
                            if ((rtv = (new PI.PIFunc2(null, null, null)).GetPointHisValue(f.Split(':')[1].Split(',')[1], ((DateTime)pv.ts).AddMinutes(double.Parse(f.Split(':')[1].Split(',')[2]))).pvalue) == null)
                            {
                                pv.es = true;
                                return;
                            }
                            else
                            {
                                tempfm = tempfm.Replace("[" + f + "]", rtv.ToString());
                            }
                        }
                        else if (f.Split(':')[1].Split(',')[0].ToLower() == "diff2abs")
                        {
                            double? rtv;
                            if ((rtv = (new PI.PIFunc2(null, null, null)).GetDiff2Abs(f.Split(':')[1].Split(',')[1], ((DateTime)pv.ts).AddMinutes(double.Parse(f.Split(':')[1].Split(',')[2])))) == null)
                            {
                                pv.es = true;
                                return;
                            }
                            else
                            {
                                tempfm = tempfm.Replace("[" + f + "]", rtv.ToString());
                            }
                        }
                        else if (f.Split(':')[1].Split(',')[0].ToLower() == "diff2")
                        {
                            double? rtv;
                            if ((rtv = (new PI.PIFunc2(null, null, null)).GetDiff2(f.Split(':')[1].Split(',')[1], DateTime.Now.AddMinutes(double.Parse(f.Split(':')[1].Split(',')[2])))) == null)
                            {
                                pv.es = true;
                                return;
                            }
                            else
                            {
                                tempfm = tempfm.Replace("[" + f + "]", rtv.ToString());
                            }
                        }
                        else if (f.Split(':')[1].Split(',')[0].ToLower() == "abs")
                        {
                            double? rtv;
                            if ((rtv = PublicLib.Common.ABS(f.Split(':')[1].Split(',')[1])) == null)
                            {
                                pv.es = true;
                                return;
                            }
                            else
                            {
                                tempfm = tempfm.Replace("[" + f + "]", rtv.ToString());
                            }
                        }
                        else if (f.Split(':')[1].Split(',')[0].ToLower() == "diff")
                        {
                            double? rtv;
                            if ((rtv = (new PI.PIFunc2(null, null, null)).GetDiff(f.Split(':')[1].Split(',')[1], (DateTime)pv.ts, ((DateTime)pv.ts).AddMinutes(double.Parse(f.Split(':')[1].Split(',')[2])))) == null)
                            {
                                pv.es = true;
                                return;
                            }
                            else
                            {
                                tempfm = tempfm.Replace("[" + f + "]", rtv.ToString());
                            }
                        }
                        else if (f.Split(':')[1].Split(',')[0].ToLower() == "diffabs")
                        {
                            double? rtv;
                            if ((rtv = (new PI.PIFunc2(null, null, null)).GetDiffAbs(f.Split(':')[1].Split(',')[1], (DateTime)pv.ts, ((DateTime)pv.ts).AddMinutes(double.Parse(f.Split(':')[1].Split(',')[2])))) == null)
                            {
                                pv.es = true;
                                return;
                            }
                            else
                            {
                                tempfm = tempfm.Replace("[" + f + "]", rtv.ToString());
                            }
                        }
                        //added 20151015
                        else if (f.Split(':')[1].Split(',')[0].ToLower() == "pihouravg")
                        {
                            double? rtv;
                            if ((rtv = (new PI.PIFunc2(null, null, null)).GetAverageValue(f.Split(':')[1].Split(',')[1], DateTime.Parse(((DateTime)pv.ts).ToString("yyyy-MM-dd HH:00:00")).AddHours(double.Parse(f.Split(':')[1].Split(',')[2])).AddSeconds(double.Parse(f.Split(':')[1].Split(',')[3])), DateTime.Parse(((DateTime)pv.ts).ToString("yyyy-MM-dd HH:00:00")).AddHours(double.Parse(f.Split(':')[1].Split(',')[2])).AddHours(1.0).AddSeconds(double.Parse(f.Split(':')[1].Split(',')[3])))) == null)
                            {
                                pv.es = true;
                                return;
                            }
                            else
                            {
                                tempfm = tempfm.Replace("[" + f + "]", rtv.ToString());
                            }
                        }
                        else if (f.Split(':')[1].Split(',')[0].ToLower() == "expr")
                        {
                            double? rtv;
                            if ((rtv = PublicLib.Common.Expr(f.Split(':')[1].Split(',')[1])) == null)
                            {
                                pv.es = true;
                                return;
                            }
                            else
                            {
                                tempfm = tempfm.Replace("[" + f + "]", rtv.ToString());
                            }
                        }
                        else if (f.Split(':')[1].Split(',')[0].ToLower() == "expr2")
                        {
                            double? rtv;
                            if ((rtv = PublicLib.Common.Expr2(f.Split(':')[1].Split(',')[1])) == null)
                            {
                                pv.es = true;
                                return;
                            }
                            else
                            {
                                tempfm = tempfm.Replace("[" + f + "]", rtv.ToString());
                            }
                        }
                        else if (f.Split(':')[1].Split(',')[0].ToLower() == "dashouravg")
                        {
                            double? rtv;
                            if ((rtv = (new SQL.ArithmeticSQL("dbconn")).GetDASHourAvg((DateTime)pv.ts, int.Parse(f.Split(':')[1].Split(',')[3]), int.Parse(f.Split(':')[1].Split(',')[1]), int.Parse(f.Split(':')[1].Split(',')[2]))) == null)
                            {
                                pv.es = true;
                                return;
                            }
                            else
                            {
                                tempfm = tempfm.Replace("[" + f + "]", rtv.ToString());
                            }
                        }
                        else
                        {
                            pv.es = true;
                            ExceptionBody eb2 = new ExceptionBody() { et = ExceptionType.Error, info = "ArithmeticFunc?" + "未找到相应函数实现", ts = DateTime.Now };
                            (new PublicLib.Log()).AddExceptionLog(eb2, logtype.console);
                            return;
                        }
                    }
                }
                //已经计算则返回
                else
                {
                    return;
                }
                //get the value of the formula
                tempfm = tempfm.Replace("--", "+"); //also tempfm = tempfm.Replace("--", "- -");
                object rtnobject = Microsoft.JScript.Eval.JScriptEvaluate(tempfm, "unsafe", ve);
                pv.pv = double.Parse(rtnobject.ToString());
                pv.es = false;

                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Message, info = "ArithmeticFunc?" + "数据正常刷新", ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "ArithmeticFunc?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                pv.es = true;
            }
        }
    }
}
