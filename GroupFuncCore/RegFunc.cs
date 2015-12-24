using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using PublicLib;

namespace GroupFuncCore
{
    /// <summary>
    /// 正则表达式
    /// </summary>
    public class RegFunc
    {
        /// <summary>
        /// 获取分组规则引用,递归使用
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public static string[] GetRuleString(string formula)
        {
            try
            {
                string[] rulestr = new string[256];
                for (int i = 0; i < rulestr.Length; i++)
                {
                    rulestr[i] = null;
                }
                Regex regfunc = new Regex(@"(\[rv:.*?\])");
                MatchCollection mc = regfunc.Matches(formula, 0);
                int t = 0;
                foreach (Match m in mc)
                {
                    rulestr[t] = m.Value.Substring(1, m.Value.Length - 2);
                    t++;
                }
                return rulestr;
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "Group-RegFunc-RuleString?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }

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
                Regex regfunc = new Regex(@"(\[ctxrule:.*?\]|\[lrule:.*?\])");
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
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "Group-RegFunc-PointString?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }

        /// <summary>
        /// get the last func string
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
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "Group-RegFunc-FuncString?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }
    }
}
