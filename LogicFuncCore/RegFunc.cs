using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using PublicLib;

namespace LogicFuncCore
{
    /// <summary>
    /// 正则解析
    /// </summary>
    public class RegFunc
    {
        /// <summary>
        /// 获得point string
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public static string[] GetPointString(string formula)
        {
            try
            {
                string[] pointstr = new string[256];
                for (int i = 0; i < pointstr.Length; i++)
                {
                    pointstr[i] = null;
                }
                Regex regfunc = new Regex(@"(\[pv:.*?\])");
                MatchCollection mc = regfunc.Matches(formula, 0);
                int t = 0;
                foreach (Match m in mc)
                {
                    pointstr[t] = m.Value.Substring(1, m.Value.Length - 2);
                    t++;
                }
                return pointstr;
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "Logic-RegFunc-PointString?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public static string GetLastFuncString(string formula)
        {
            try
            {
                Regex regfunc = new Regex(@"(GE\(.*?|LE\(.*?|GT\(.*?|LT\(.*?|EQ\(.*?|OR\(.*?|AND\(.*?|EXPR\(.*?|IIF\(.*?|ABS\(.*?|RAT\(.*?|MOV\(.*?)");
                MatchCollection mc = regfunc.Matches(formula, 0);
                if (mc.Count == 0)
                {
                    return null;
                }
                else
                {
                    int startpos = mc[mc.Count - 1].Index;
                    int endpos = 0;
                    int leftparentcount = 0;
                    for (int i = startpos; i < formula.Length; i++)
                    {
                        if (formula[i] == '(')
                        {
                            leftparentcount++;
                        }
                        if (formula[i] == ')')
                        {
                            leftparentcount--;
                            if (leftparentcount == 0)
                            {
                                endpos = i;
                                if (endpos <= startpos)
                                {
                                    return null;
                                }
                                else
                                {
                                    return formula.Substring(startpos, endpos - startpos + 1);
                                }
                            }
                        }
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "Logic-RegFunc-FuncString?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }
    }
}
