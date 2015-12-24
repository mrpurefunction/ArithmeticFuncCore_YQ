using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using PublicLib;

namespace ArithmeticFuncCore
{
    /// <summary>
    /// 正则解析
    /// </summary>
    public class RegFunc
    {
        /// <summary>
        /// get point expression string
        /// </summary>
        /// <param name="fomular"></param>
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
                Regex regfunc = new Regex(@"(\[pv:.*?\]|\[ref:.*?\])");
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
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "Arithmetic-RegFunc-PointString?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }

        /// <summary>
        /// get function expression string
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public static string[] GetFuncString(string formula)
        {
            try
            {
                string[] funcstr = new string[256];
                for (int i = 0; i < funcstr.Length; i++)
                {
                    funcstr[i] = null;
                }
                Regex regfunc = new Regex(@"(\[fn:.*?\])");
                MatchCollection mc = regfunc.Matches(formula, 0);
                int t = 0;
                foreach (Match m in mc)
                {
                    funcstr[t] = m.Value.Substring(1, m.Value.Length - 2);
                    t++;
                }
                return funcstr;
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "Arithmetic-RegFunc-FuncString?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }
    }
}
