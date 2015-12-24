using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.JScript.Vsa;

namespace PublicLib
{
    /// <summary>
    /// 
    /// </summary>
    public class Common
    {
        /// <summary>
        /// 
        /// </summary>
        private static VsaEngine ve = Microsoft.JScript.Vsa.VsaEngine.CreateEngine();
        /// <summary>
        /// >=
        /// </summary>
        /// <param name="oper1"></param>
        /// <param name="oper2"></param>
        /// <returns></returns>
        public static bool? GE(decimal oper1, decimal oper2)
        {
            try
            {
                if (oper1 >= oper2)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PublicCommon-GE?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }

        /// <summary>
        /// <=
        /// </summary>
        /// <param name="oper1"></param>
        /// <param name="oper2"></param>
        /// <returns></returns>
        public static bool? LE(decimal oper1, decimal oper2)
        {
            try
            {
                if (oper1 <= oper2)
                {
                    return true;
                }
                else
                    return false;
            }
            catch(Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PublicCommon-LE?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }

        /// <summary>
        /// >
        /// </summary>
        /// <param name="oper1"></param>
        /// <param name="oper2"></param>
        /// <returns></returns>
        public static bool? GT(decimal oper1, decimal oper2)
        {
            try
            {
                if (oper1 > oper2)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PublicCommon-?GT" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }

        /// <summary>
        /// <
        /// </summary>
        /// <param name="oper1"></param>
        /// <param name="oper2"></param>
        /// <returns></returns>
        public static bool? LT(decimal oper1, decimal oper2)
        {
            try
            {
                if (oper1 < oper2)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PublicCommon-LT?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }

        /// <summary>
        /// =
        /// </summary>
        /// <param name="oper1"></param>
        /// <param name="oper2"></param>
        /// <returns></returns>
        public static bool? EQ(decimal oper1, decimal oper2)
        {
            try
            {
                if (oper1 == oper2)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PublicCommon-EQ?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }

        /// <summary>
        /// or ||
        /// </summary>
        /// <param name="oper1"></param>
        /// <param name="oper2"></param>
        /// <returns></returns>
        public static bool? OR(decimal oper1, decimal oper2)
        {
            try
            {
                if ((oper1 > 0) || (oper2 > 0))
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PublicCommon-OR?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }

        /// <summary>
        /// and &&
        /// </summary>
        /// <param name="oper1"></param>
        /// <param name="oper2"></param>
        /// <returns></returns>
        public static bool? AND(decimal oper1, decimal oper2)
        {
            try
            {
                if ((oper1 > 0) && (oper2 > 0))
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PublicCommon-AND?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }

        /// <summary>
        /// abstract
        /// </summary>
        /// <param name="oper"></param>
        /// <returns></returns>
        public static double? ABS(string oper)
        {
            try
            {
                oper = oper.Replace("--", "+"); //also tempfm = tempfm.Replace("--", "- -");
                object rtnobject = Microsoft.JScript.Eval.JScriptEvaluate(oper, "unsafe", ve);
                double r = double.Parse(rtnobject.ToString());
                return System.Math.Abs(r);
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PublicCommon-ABS?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pn"></param>
        /// <returns></returns>
        public static decimal? MOV(string pn)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pn"></param>
        /// <returns></returns>
        public static decimal? RAT(string pn)
        {
            return null;
        }

        /// <summary>
        /// expression for arithmetic calculation
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static double? Expr(string expr)
        {
            try
            {
                expr= expr.Replace("--", "+"); //also can be tempfm = tempfm.Replace("--", "- -");
                object rtnobject = Microsoft.JScript.Eval.JScriptEvaluate(expr, "unsafe", ve);
                if (rtnobject.ToString().ToLower() == "true")
                {
                    return 1.0;
                }
                if (rtnobject.ToString().ToLower() == "false")
                {
                    return 0.0;
                }
                double r = double.Parse(rtnobject.ToString());
                if (r == 0)
                {
                    return 0.0;
                }
                else
                {
                    return 1.0;
                }
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PublicCommon-Expr-Arithmetic?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }

        /// <summary>
        /// expression for arithmetic calculation 2
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static double? Expr2(string expr)
        {
            try
            {
                expr = expr.Replace("--", "+"); //also can be tempfm = tempfm.Replace("--", "- -");
                object rtnobject = Microsoft.JScript.Eval.JScriptEvaluate(expr, "unsafe", ve);
                if (rtnobject.ToString().ToLower() == "true")
                {
                    return 1.0;
                }
                if (rtnobject.ToString().ToLower() == "false")
                {
                    return 0.0;
                }
                return double.Parse(rtnobject.ToString());
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PublicCommon-Expr2-Arithmetic?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }

        /// <summary>
        /// expression for logic calculation---logic rule and group rule
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static bool? LExpr(string expr)
        {
            try
            {
                expr = expr.Replace("--", "+"); //also tempfm = tempfm.Replace("--", "- -");
                object rtnobject = Microsoft.JScript.Eval.JScriptEvaluate(expr, "unsafe", ve);
                if (rtnobject.ToString().ToLower() == "true")
                {
                    return true;
                }
                if (rtnobject.ToString().ToLower() == "false")
                {
                    return false;
                }
                double r = double.Parse(rtnobject.ToString());
                if (r == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PublicCommon-LExpr?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }

    }
}
