using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PublicLib;
namespace PI
{
    /// <summary>
    /// return value
    /// </summary>
    public class RetVal
    {
        public double pvalue { get; set; }
        public DateTime ts { get; set; }
    }

    /// <summary>
    /// 提供PI数据库访问功能
    /// </summary>
    public partial class PIFunc
    {
        /// <summary>
        /// 服务器名称
        /// </summary>
        public string servername { get; set; }
        /// <summary>
        /// Login用户名
        /// </summary>
        public string usrname { get; set; }
        /// <summary>
        /// Login密码
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// global sdk object
        /// </summary>
        public static PISDK.PISDK g_sdk { get; set; }

        /// <summary>
        /// global server object
        /// </summary>
        public static PISDK.Server g_svr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public static readonly int digits = 3;
        /// <summary>
        /// 
        /// </summary>
        private static PISDK.PIPoints s_pps;
        /// <summary>
        /// 
        /// </summary>
        private static PISDK.PIPoint s_pp;
        /// <summary>
        /// 
        /// </summary>
        private static PISDK.PIData s_pd;
        /// <summary>
        /// 
        /// </summary>
        private static PISDK.PIValue s_pv1;
        /// <summary>
        /// 
        /// </summary>
        private static PISDK.PIValue s_pv2;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="usrnm"></param>
        /// <param name="psd"></param>
        public PIFunc(string svrnm, string usrnm, string psd)
        {
            try
            {
                if (g_sdk == null)
                {
                    servername = svrnm;
                    usrname = usrnm;
                    password = psd;
                    g_sdk = new PISDK.PISDK();
                    g_svr = g_sdk.Servers[servername];
                    ConnectPI();
                }
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc-Constructor?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
            }
        }

        /// <summary>
        /// connect the pi server
        /// </summary>
        private void ConnectPI()
        {
            try
            {
                if (!g_svr.Connected)
                {
                    g_svr.Open("UID=" + usrname + ";PWD=" + password);
                }
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc-ConnectPI?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pn"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public RetVal GetPointHisValue(string pn, DateTime ts)
        {
            try
            {
                RetVal rv = new RetVal();
                s_pps = g_svr.PIPoints;
                s_pp = s_pps[pn];
                s_pd = s_pp.Data;
                s_pv1 = s_pd.ArcValue(ts, PISDK.RetrievalTypeConstants.rtAuto);
                if (s_pv1.Value.GetType().IsCOMObject)
                {
                    PISDK.DigitalState DigState;
                    DigState = (PISDK.DigitalState)s_pv1.Value;
                    if ((DigState.Name == "0") || (DigState.Name == "1"))
                    {
                        rv.pvalue = double.Parse(DigState.Name);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(DigState);
                        DigState = null;
                    }
                    else
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(DigState);
                        DigState = null;
                        ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc-HisValue?" + pn + "-未能解析COM值", ts = DateTime.Now };
                        (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);

                        System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pv1);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pd);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pp);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pps);

                        s_pv1 = null;
                        s_pd = null;
                        s_pp = null;
                        s_pps = null;

                        return null;
                    }
                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(pv.Value);
                }
                else
                {
                    rv.pvalue = System.Math.Round(double.Parse(s_pv1.Value.ToString()), digits);
                }
                rv.ts = s_pv1.TimeStamp.LocalDate;
                //             
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pv1);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pd);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pp);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pps);

                s_pv1 = null;
                s_pd = null;
                s_pp = null;
                s_pps = null;

                return rv;
                //
                //g_svr.PIPoints[pn].Data.ArcValue(ts, PISDK.RetrievalTypeConstants.rtInterpolated);
                //g_svr.PIPoints[pn].Data.InterpolatedValues(
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc-HisValue?" + pn + "-" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pv1);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pd);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pp);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pps);

                s_pv1 = null;
                s_pd = null;
                s_pp = null;
                s_pps = null;

                return null;
            }
        }

        /// <summary>
        /// get the snapshot value
        /// </summary>
        /// <param name="pn"></param>
        public RetVal GetPointSnapshot(string pn)
        {
            try
            {
                RetVal rv = new RetVal();
                if (g_svr.PIPoints[pn].Data.Snapshot.Value.GetType().IsCOMObject)
                {
                    PISDK.DigitalState DigState;
                    DigState = (PISDK.DigitalState)g_svr.PIPoints[pn].Data.Snapshot.Value;
                    rv.pvalue = double.Parse(DigState.Name);
                }
                else
                {
                    //non-com object
                    rv.pvalue = System.Math.Round(double.Parse(g_svr.PIPoints[pn].Data.Snapshot.Value.ToString()), digits);
                }
                rv.ts = g_svr.PIPoints[pn].Data.Snapshot.TimeStamp.LocalDate; //.ToString("yyyy-MM-dd HH:mm:ss")
                //rv.pvalue = double.Parse(g_svr.PIPoints[pn].Data.LastValueSent.Value.ToString());
                //rv.ts = g_svr.PIPoints[pn].Data.LastValueSent.TimeStamp.LocalDate; //.ToString("yyyy-MM-dd HH:mm:ss")
                //mov
                //g_svr.PIPoints[pn].Data.Summary(null, null, PISDK.ArchiveSummaryTypeConstants.astAverage);
                return rv;
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc-Snapshot?" + pn + "-" + ex.Message, ts = DateTime.Now };
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
        public bool InsertOrUpdate(string pn, DateTime ts, double pv)
        {
            try
            {
                g_svr.PIPoints[pn].Data.UpdateValue(pv, ts, PISDK.DataMergeConstants.dmInsertDuplicates);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc-InsertUpdate?" + pn + "-" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pn"></param>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <returns></returns>
        public double? GetAverageValue(string pn, DateTime st, DateTime et)
        {
            try
            {
                s_pps = g_svr.PIPoints;
                s_pp = s_pps[pn];
                s_pd = s_pp.Data;
                s_pv1 = s_pd.Summary(st, et, PISDK.ArchiveSummaryTypeConstants.astAverage);
                double temp = double.Parse(s_pv1.Value.ToString());
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(pv.Value);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pv1);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pd);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pp);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pps);

                s_pv1 = null;
                s_pd = null;
                s_pp = null;
                s_pps = null;

                return System.Math.Round(temp, digits);
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc-AVGValue?" + pn + "-" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pv1);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pd);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pp);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pps);

                s_pv1 = null;
                s_pd = null;
                s_pp = null;
                s_pps = null;

                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pn"></param>
        /// <param name="ts1"></param>
        /// <param name="ts2"></param>
        /// <returns></returns>
        public double? GetDiff(string pn, DateTime ts1, DateTime ts2)
        {
            try
            {
                s_pps = g_svr.PIPoints;
                s_pp = s_pps[pn];
                s_pd = s_pp.Data;
                s_pv1 = s_pd.ArcValue(ts1, PISDK.RetrievalTypeConstants.rtAuto);
                s_pv2 = s_pd.ArcValue(ts2, PISDK.RetrievalTypeConstants.rtAuto);
                double temp1, temp2;
                if (s_pv1.Value.GetType().IsCOMObject)
                {
                    PISDK.DigitalState DigState;
                    DigState = (PISDK.DigitalState)s_pv1.Value;
                    if ((DigState.Name == "0") || (DigState.Name == "1"))
                    {
                        temp1 = double.Parse(DigState.Name);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(DigState);
                        DigState = null;
                    }
                    else
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(DigState);
                        DigState = null;
                        ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc-Diff?" + pn + "-未能解析COM值", ts = DateTime.Now };
                        (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);

                        System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pv1);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pv2);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pd);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pp);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pps);

                        s_pv1 = null;
                        s_pv2 = null;
                        s_pd = null;
                        s_pp = null;
                        s_pps = null;

                        return null;
                    }
                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(pv1.Value);
                }
                else
                {
                    temp1 = double.Parse(s_pv1.Value.ToString());
                }

                if (s_pv2.Value.GetType().IsCOMObject)
                {
                    PISDK.DigitalState DigState;
                    DigState = (PISDK.DigitalState)s_pv2.Value;
                    if ((DigState.Name == "0") || (DigState.Name == "1"))
                    {
                        temp2 = double.Parse(DigState.Name);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(DigState);
                        DigState = null;
                    }
                    else
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(DigState);
                        DigState = null;
                        ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc-Diff?" + pn + "-未能解析COM值", ts = DateTime.Now };
                        (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);

                        System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pv1);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pv2);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pd);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pp);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pps);

                        s_pv1 = null;
                        s_pv2 = null;
                        s_pd = null;
                        s_pp = null;
                        s_pps = null;

                        return null;
                    }
                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(pv2.Value);
                }
                else
                {
                    temp2 = double.Parse(s_pv2.Value.ToString());
                }
                            
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pv1);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pv2);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pd);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pp);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pps);

                s_pv1 = null;
                s_pv2 = null;
                s_pd = null;
                s_pp = null;
                s_pps = null;

                return System.Math.Round(temp1 - temp2, digits);
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc-Diff?" + pn + "-" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pv1);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pv2);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pd);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pp);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pps);

                s_pv1 = null;
                s_pv2 = null;
                s_pd = null;
                s_pp = null;
                s_pps = null;

                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pn"></param>
        /// <param name="ts1"></param>
        /// <param name="ts2"></param>
        /// <returns></returns>
        public double? GetDiffAbs(string pn, DateTime ts1, DateTime ts2)
        {
            try
            {
                s_pps = g_svr.PIPoints;
                s_pp = s_pps[pn];
                s_pd = s_pp.Data;
                s_pv1 = s_pd.ArcValue(ts1, PISDK.RetrievalTypeConstants.rtAuto);
                s_pv2 = s_pd.ArcValue(ts2, PISDK.RetrievalTypeConstants.rtAuto);
                double temp1, temp2;
                if (s_pv1.Value.GetType().IsCOMObject)
                {
                    PISDK.DigitalState DigState;
                    DigState = (PISDK.DigitalState)s_pv1.Value;
                    if ((DigState.Name == "0") || (DigState.Name == "1"))
                    {
                        temp1 = double.Parse(DigState.Name);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(DigState);
                        DigState = null;
                    }
                    else
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(DigState);
                        DigState = null;
                        ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc-DiffAbs?" + pn + "-未能解析COM值", ts = DateTime.Now };
                        (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);

                        System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pv1);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pv2);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pd);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pp);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pps);

                        s_pv1 = null;
                        s_pv2 = null;
                        s_pd = null;
                        s_pp = null;
                        s_pps = null;

                        return null;
                    }
                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(pv1.Value);
                }
                else
                {
                    temp1 = double.Parse(s_pv1.Value.ToString());
                }

                if (s_pv2.Value.GetType().IsCOMObject)
                {
                    PISDK.DigitalState DigState;
                    DigState = (PISDK.DigitalState)s_pv2.Value;
                    if ((DigState.Name == "0") || (DigState.Name == "1"))
                    {
                        temp2 = double.Parse(DigState.Name);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(DigState);
                        DigState = null;
                    }
                    else
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(DigState);
                        DigState = null;
                        ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc-DiffAbs?" + pn + "-未能解析COM值", ts = DateTime.Now };
                        (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);

                        System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pv1);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pv2);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pd);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pp);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pps);

                        s_pv1 = null;
                        s_pv2 = null;
                        s_pd = null;
                        s_pp = null;
                        s_pps = null;

                        return null;
                    }
                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(pv2.Value);
                }
                else
                {
                    temp2 = double.Parse(s_pv2.Value.ToString());
                }
                              
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pv1);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pv2);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pd);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pp);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pps);

                s_pv1 = null;
                s_pv2 = null;
                s_pd = null;
                s_pp = null;
                s_pps = null;

                return System.Math.Round(System.Math.Abs(temp1 - temp2), digits);
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc-DiffAbs?" + pn + "-" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pv1);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pv2);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pd);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pp);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_pps);

                s_pv1 = null;
                s_pv2 = null;
                s_pd = null;
                s_pp = null;
                s_pps = null;

                return null;
            }
        }

        /// <summary>
        /// get the difference from value of current time
        /// </summary>
        /// <param name="pn"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public double? GetDiff2(string pn, DateTime ts)
        {
            try
            {
                PISDK.PIValue pv2 = g_svr.PIPoints[pn].Data.ArcValue(ts, PISDK.RetrievalTypeConstants.rtAuto);
                return System.Math.Round(double.Parse(g_svr.PIPoints[pn].Data.Snapshot.Value.ToString()) - double.Parse(pv2.Value.ToString()), digits);
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc-Diff2?" + pn + "-" + ex.Message, ts = DateTime.Now };
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
        public double? GetDiff2Abs(string pn, DateTime ts)
        {
            try
            {
                PISDK.PIValue pv2 = g_svr.PIPoints[pn].Data.ArcValue(ts, PISDK.RetrievalTypeConstants.rtAuto);
                return System.Math.Round(System.Math.Abs(double.Parse(g_svr.PIPoints[pn].Data.Snapshot.Value.ToString()) - double.Parse(pv2.Value.ToString())), digits);
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc-Diff2Abs?" + pn + "-" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                return null;
            }
        }

        /// <summary>
        /// close the connection
        /// </summary>
        public void DisconnectPI()
        {
            try
            {
                g_svr.Close();
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "PIFunc-DisconnectPI?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
            }
        }
    }
}
