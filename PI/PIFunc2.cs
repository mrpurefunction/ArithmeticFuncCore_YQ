using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PublicLib;

using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace PI
{
    /// <summary>
    /// 
    /// </summary>
    public class PIFunc2
    {
        [DllImport("piapi32.dll")]
        public extern static Int32 piut_connect(string sn);
        [DllImport("piapi32.dll")]
        public extern static Int32 piut_login(string un, string pd, ref Int32 v);
        [DllImport("piapi32.dll")]
        public extern static Int32 piar_value(Int32 pt, ref Int32 dt, Int32 m, ref float v, ref Int32 s);
        [DllImport("piapi32.dll")]
        public extern static Int32 pipt_findpoint(string t, ref Int32 pn);
        [DllImport("piapi32.dll")]
        public extern static Int32 piar_summary(Int32 pt, ref Int32 st, ref Int32 et, ref float v, ref float g, Int32 c);
        [DllImport("piapi32.dll")]
        public extern static Int32 piut_disconnect();
        [DllImport("piapi32.dll")]
        public extern static Int32 piut_isconnected();   
        [DllImport("piapi32.dll")]
        public extern static Int32 piut_setservernode(string sn);
        [DllImport("piapi32.dll")]
        public extern static void pitm_intsec(ref Int32 tdr, Int32[] td);
        [DllImport("piapi32.dll")]
        public extern static Int32 pipt_pointtypex(Int32 p, ref Int32 t);
        [DllImport("piapi32.dll")]
        public extern static Int32 pitm_servertime(ref Int32 t);
        [DllImport("piapi32.dll")]
        public extern static Int32 pisn_getsnapshot(Int32 pt, ref float v, ref Int32 s, ref Int32 t);
        [DllImport("piapi32.dll")]
        public extern static Int32 pipt_digstate(Int32 dc, ref char ds, Int32 l);
        //[DllImport("piapi32.dll")]
        //public extern static Int32 pipt_digstate(Int32 dc, char* ds, Int32 l);
        [DllImport("piapi32.dll")]
        public extern static Int32 piar_putvalue(Int32 pt, float v, Int32 v2, Int32 dt, Int32 w);

        /// <summary>
        /// server ip
        /// </summary>
        public static string serverip { get; set; }
        /// <summary>
        /// login name
        /// </summary>
        public static string usrname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public static string password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public static readonly int digits = 3;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="sip"></param>
        /// <param name="un"></param>
        /// <param name="psd"></param>
        public PIFunc2(string sip, string un, string psd)
        {
            try
            {
                int c = -1;
                //if it is connected
                if ((c = piut_isconnected()) != 1)
                {
                    serverip = sip;
                    usrname = un;
                    password = psd;
                    int s = -1;
                    if ((s = piut_setservernode(sip)) != 0)
                    {
                        ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc2-Constructor?" + "未能成功连接服务器" + serverip, ts = DateTime.Now };
                        (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                        return;
                    }
                    int l = -1;
                    int p = -1;
                    if ((l = piut_login(usrname, password, ref p)) != 0)
                    {
                        ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc2-Constructor?" + "未能成功登陆服务器", ts = DateTime.Now };
                        (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                        return;
                    }
                    if (p != 2)
                    {
                        ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Warning, info = "PIFunc2-Constructor?" + "未获得PI读写权限", ts = DateTime.Now };
                        (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc2-Constructor?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
            }
        }

        /// <summary>
        /// get point his value
        /// </summary>
        /// <param name="pn"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public RetVal GetPointHisValue(string pn, DateTime ts)
        {
            try
            {
                RetVal rv = new RetVal();
                //获取点号
                int pno = -1;
                int p = -1;
                if ((p = pipt_findpoint(pn, ref pno)) != 0)
                {
                    ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc2-HisValue?" + "未能获得点号 " + pn, ts = DateTime.Now };
                    (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                    return null;
                }
                //获取点类型
                int pt = 0;
                int t = -1;
                if ((t = pipt_pointtypex(pno, ref pt)) != 0)
                {
                    ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc2-HisValue?" + "未能获得点数据类型 " + pn + " " + pno.ToString(), ts = DateTime.Now };
                    (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                    return null;
                }
                //digital
                if (pt == 101)
                {
                    //time transfer
                    int[] dt = new int[6];
                    dt[0] = ts.Month;
                    dt[1] = ts.Day;
                    dt[2] = ts.Year;
                    dt[3] = ts.Hour;
                    dt[4] = ts.Minute;
                    dt[5] = ts.Second;
                    int pdt = -1;
                    PI.PIFunc2.pitm_intsec(ref pdt, dt);

                    //get value
                    float fv = 0;
                    Int32 iv = 0;
                    int vs = -1;
                    if ((vs = piar_value(pno, ref pdt, 3, ref fv, ref iv)) != 0)
                    {
                        ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc2-HisValue?" + "未能读取点历史值 " + pn + " " + pno.ToString(), ts = DateTime.Now };
                        (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                        return null;
                    }
                    else
                    {
                        char r = ' ';
                        int dis = -1;
                        if ((dis = pipt_digstate(iv, ref r, 1)) != 0)
                        {
                            ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc2-HisValue?" + "未能转化点数字值 " + pn + " " + pno.ToString(), ts = DateTime.Now };
                            (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                            return null;
                        }
                        if ((r != '0') && (r != '1'))
                        {
                            ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc2-HisValue?" + "点数字值不为0或1 " + pn + " " + pno.ToString(), ts = DateTime.Now };
                            (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                            return null;
                        }
                        if (r == '0')
                        {
                            rv.pvalue = 0;
                            return rv;
                        }
                        else
                        {
                            rv.pvalue = 1;
                            return rv;
                        }
                    }
                }
                //float32
                else if (pt == 12)
                {
                    //time transfer
                    int[] dt = new int[6];
                    dt[0] = ts.Month;
                    dt[1] = ts.Day;
                    dt[2] = ts.Year;
                    dt[3] = ts.Hour;
                    dt[4] = ts.Minute;
                    dt[5] = ts.Second;
                    int pdt = -1;
                    PI.PIFunc2.pitm_intsec(ref pdt, dt);

                    //get value
                    float fv = 0;
                    Int32 iv = 0;
                    int vs = -1;
                    if ((vs = piar_value(pno, ref pdt, 3, ref fv, ref iv)) != 0)
                    {
                        ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc2-HisValue?" + "未能读取点历史值 " + pn + " " + pno.ToString(), ts = DateTime.Now };
                        (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                        return null;
                    }
                    else
                    {
                        if (iv != 0)
                        {
                            ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc2-HisValue?" + "点模拟值状态异常 " + pn + " " + pno.ToString(), ts = DateTime.Now };
                            (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                            return null;
                        }
                        else
                        {
                            rv.pvalue = System.Math.Round(fv, digits);
                            return rv;
                        }
                    }
                }
                //not supported
                else
                {
                    ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc2-HisValue?" + "不支持该点数据类型 " + pn, ts = DateTime.Now };
                    (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc2-HisValue?" + pn + "-" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }

        /// <summary>
        /// get avg value
        /// </summary>
        /// <param name="pn"></param>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <returns></returns>
        public double? GetAverageValue(string pn, DateTime st, DateTime et)
        {
            try
            {
                //获取点号
                int pno = -1;
                int p = -1;
                if ((p = pipt_findpoint(pn, ref pno)) != 0)
                {
                    ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc2-AVGValue?" + "未能获得点号 " + pn, ts = DateTime.Now };
                    (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                    return null;
                }
                //获取点类型
                int pt = 0;
                int t = -1;
                if ((t = pipt_pointtypex(pno, ref pt)) != 0)
                {
                    ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc2-AVGValue?" + "未能获得点数据类型 " + pn + " " + pno.ToString(), ts = DateTime.Now };
                    (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                    return null;
                }
                if (pt != 12)
                {
                    ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc2-AVGValue?" + "点数据类型不是浮点32位 " + pn + " " + pno.ToString(), ts = DateTime.Now };
                    (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                    return null;
                }
                else
                {
                    //time transfer
                    int[] dt = new int[6];
                    dt[0] = st.Month;
                    dt[1] = st.Day;
                    dt[2] = st.Year;
                    dt[3] = st.Hour;
                    dt[4] = st.Minute;
                    dt[5] = st.Second;
                    int pdt = -1;
                    PI.PIFunc2.pitm_intsec(ref pdt, dt);

                    int[] dt2 = new int[6];
                    dt2[0] = et.Month;
                    dt2[1] = et.Day;
                    dt2[2] = et.Year;
                    dt2[3] = et.Hour;
                    dt2[4] = et.Minute;
                    dt2[5] = et.Second;
                    int pdt2 = -1;
                    PI.PIFunc2.pitm_intsec(ref pdt2, dt2);

                    //get value
                    float fv = 0;
                    float gv = 0;
                    int vs = -1;
                    if ((vs = piar_summary(pno, ref pdt, ref pdt2, ref fv, ref gv, 5)) != 0)
                    {
                        ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc2-AVGValue?" + "未能读取点平均值 " + pn + " " + pno.ToString(), ts = DateTime.Now };
                        (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                        return null;
                    }
                    else
                    {
                        if (gv < 50)
                        {
                            ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Warning, info = "PIFunc2-AVGValue?" + "点模拟值正常采样比例低于50% " + pn + " " + pno.ToString(), ts = DateTime.Now };
                            (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                            return System.Math.Round(fv, digits);
                        }
                        else
                        {
                            return System.Math.Round(fv, digits);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc2-AVGValue?" + pn + "-" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }

        /// <summary>
        /// 获得差值
        /// </summary>
        /// <param name="pn"></param>
        /// <param name="ts1"></param>
        /// <param name="ts2"></param>
        /// <returns></returns>
        public double? GetDiff(string pn, DateTime ts1, DateTime ts2)
        {
            try
            {
                RetVal rv1 = GetPointHisValue(pn, ts1);
                RetVal rv2 = GetPointHisValue(pn, ts2);

                if ((rv1 != null) && (rv2 != null))
                {
                    return System.Math.Round(rv1.pvalue - rv2.pvalue, digits);
                }
                else
                {
                    ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc2-Diff?" + pn + "-" + "未能获得差值", ts = DateTime.Now };
                    (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc2-Diff?" + pn + "-" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }

        /// <summary>
        /// 获得差值的绝对值
        /// </summary>
        /// <param name="pn"></param>
        /// <param name="ts1"></param>
        /// <param name="ts2"></param>
        /// <returns></returns>
        public double? GetDiffAbs(string pn, DateTime ts1, DateTime ts2)
        {
            try
            {
                double? rv = GetDiff(pn, ts1, ts2);
                if (rv == null)
                {
                    ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc2-DiffAbs?" + pn + "-" + "未能获得差值绝对值", ts = DateTime.Now };
                    (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                    return null;
                }
                else
                {
                    return System.Math.Abs((double)rv);
                }              
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc2-DiffAbs?" + pn + "-" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pn"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public double? GetDiff2(string pn, DateTime ts)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pn"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public double? GetDiff2Abs(string pn, DateTime ts)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 写入历史值
        /// </summary>
        /// <param name="pn"></param>
        /// <param name="ts"></param>
        /// <param name="pv"></param>
        public void SetPointHisValue(string pn, DateTime ts, double? pv)
        {
            try
            {
                //获取点号
                int pno = -1;
                int p = -1;
                if ((p = pipt_findpoint(pn, ref pno)) != 0)
                {
                    ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc2-SetValue?" + "未能获得点号 " + pn, ts = DateTime.Now };
                    (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                    return;
                }
                //time transfer
                int[] dt = new int[6];
                dt[0] = ts.Month;
                dt[1] = ts.Day;
                dt[2] = ts.Year;
                dt[3] = ts.Hour;
                dt[4] = ts.Minute;
                dt[5] = ts.Second;
                int pdt = -1;
                PI.PIFunc2.pitm_intsec(ref pdt, dt);

                //set point value
                if (pv != null)
                {
                    int vs = -1;
                    if ((vs = piar_putvalue(pno, (float)pv, 0, pdt, 1)) != 0)
                    {
                        ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc2-SetValue?" + "未能写入点历史值 " + pn + " " + pno.ToString(), ts = DateTime.Now };
                        (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                        return;
                    }
                    else
                    {
                        ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Message, info = "PIFunc2-SetValue?" + "成功写入点历史值 " + pn + " " + pno.ToString(), ts = DateTime.Now };
                        (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                        return;
                    }
                }
                else
                {
                    ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc2-SetValue?" + "计量点值为空 " + pn + " " + pno.ToString(), ts = DateTime.Now };
                    (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                    return;
                }
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc2-SetValue?" + pn + "-" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return;
            }
        }

        /// <summary>
        /// 断开PI连接
        /// </summary>
        public void DisconnectPI()
        {
            try
            {
                 int c = -1;
                //if it is connected
                 if ((c = piut_isconnected()) == 1)
                 {
                     int d = piut_disconnect();
                     if (d != 0)
                     {
                         ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc2-DisconnectPI?" + "未能成功断开PI", ts = DateTime.Now };
                         (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                     }
                 }
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc2-DisconnectPI?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
            }
        }
    }
}
