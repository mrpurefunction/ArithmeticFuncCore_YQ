using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Microsoft.JScript.Vsa;
using PublicLib;

namespace test
{
    public partial class Form1 : Form
    {
        public DateTime? st { get; set; }
        public DateTime? et { get; set; }

        Thread t1/*log*/, t2/*calculation*/;

        public PI.PIFunc2 pf { get; set; }

        ArithmeticFuncCore.Manager m;

        LogicFuncCore.Manager m2;

        ContextFuncCore.Manager m3;

        GroupFuncCore.Manager m4;

        public Form1()
        {
            InitializeComponent();

            button13.Enabled = false;
            button14.Enabled = false;

            t1 = new Thread(new ThreadStart(LogThread));
            t1.Start();

            t2 = new Thread(new ThreadStart(WorkerThread));

            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);

            ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Message, info = "-----------Start----------", ts = DateTime.Now };
            (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);

            pf = new PI.PIFunc2("10.150.124.193", "pirw", "pirw");

            m = new ArithmeticFuncCore.Manager();
            m.LoadInputs("inputs.ini");
            m.LoadOutputs("outputs.ini");
            
            m2 = new LogicFuncCore.Manager();
            m2.LoadLogicRules("logicrules.ini");

            m3 = new ContextFuncCore.Manager();
            m3.LoadCtxRules("contextrules.ini");

            m4 = new GroupFuncCore.Manager();
            m4.LoadGroupRules("grouprules.ini");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public void Form1_FormClosing(object o, EventArgs e)
        {
            //pf.DisconnectPI();
            //ExceptionBody eb2 = new ExceptionBody() { et = ExceptionType.Message, info = "-----------End----------", ts = DateTime.Now };
            //(new PublicLib.Log()).AddExceptionLog(eb2, logtype.console);

            if (t1.ThreadState != ThreadState.Unstarted)
            {
                t1.Abort();
            }

            if (t2.ThreadState != ThreadState.Unstarted)
            {
                if ((t2.ThreadState != ThreadState.Stopped) || (t2.ThreadState != ThreadState.Aborted))
                {
                    t2.Abort();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void LogThread()
        {
            while (1 == 1)
            {
                (new PublicLib.Log()).ExportConsoleLog();
                Thread.Sleep(500);
            }
        }
        #region hide
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            ArithmeticFuncCore.Manager m = new ArithmeticFuncCore.Manager();
            m.UpdatePv();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string[] temp = ArithmeticFuncCore.RegFunc.GetPointString("[pv:a]+[pv:tst]+[pv2:exe]");
            string[] temp2 = ArithmeticFuncCore.RegFunc.GetFuncString("[fn:(xx,5,0)]+[fn:(yyy,1,2)]");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            ArithmeticFuncCore.Manager m = new ArithmeticFuncCore.Manager();
            m.LoadInputs("inputs.ini");
            m.LoadOutputs("outputs.ini");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            string temp = LogicFuncCore.RegFunc.GetLastFuncString("OR(GE(1,2),1)");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            PI.PIFunc pf = new PI.PIFunc("piadmin", "pirw", "pirw");
            //pf.InsertOrUpdate("10HSA31CQ101AASIM", new DateTime(2000, 1, 1, 0, 0, 0), 5.3);
            //pf.GetPointHisValue("10HSA31CQ101AASIM", new DateTime(2000, 1, 1, 0, 0, 0));
            pf.GetPointHisValue("10HSK31AA101-A", DateTime.Now.AddDays(-180));
            pf.GetPointSnapshot("10HSK31AA101-A");           
            pf.DisconnectPI();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Message, info = "-----------Start----------", ts = DateTime.Now };
            (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);

            ArithmeticFuncCore.Manager m = new ArithmeticFuncCore.Manager();
            m.LoadInputs("inputs.ini");
            m.LoadOutputs("outputs.ini");

            PI.PIFunc pf = new PI.PIFunc("piadmin", "pirw", "pirw");
            st = new DateTime(2014, 8, 1, 0, 0, 0);
            et = new DateTime(2014, 8, 1, 0, 10, 0);

            LogicFuncCore.Manager m2 = new LogicFuncCore.Manager();
            m2.LoadLogicRules("logicrules.ini");

            ContextFuncCore.Manager m3 = new ContextFuncCore.Manager();
            m3.LoadCtxRules("contextrules.ini");

            GroupFuncCore.Manager m4 = new GroupFuncCore.Manager();
            m4.LoadGroupRules("grouprules.ini");
            

            while (st < et)
            {
                m.RefreshDataAreaSync((DateTime)st);
                m2.RefreshDataAreaSync((DateTime)st);
                m3.RefreshDataAreaSync((DateTime)st);
                m4.RefreshDataAreaSync((DateTime)st);
                st = ((DateTime)st).AddMinutes(1.0);
            }

            pf.DisconnectPI();
            ExceptionBody eb2 = new ExceptionBody() { et = ExceptionType.Message, info = "-----------End----------", ts = DateTime.Now };
            (new PublicLib.Log()).AddExceptionLog(eb2, logtype.console);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            ArithmeticFuncCore.Manager m = new ArithmeticFuncCore.Manager();
            m.LoadInputs("inputs.ini");
            m.LoadOutputs("outputs.ini");

            PI.PIFunc pf = new PI.PIFunc("piadmin", "pirw", "pirw");
            st = new DateTime(2014, 6, 1, 0, 0, 0);
            m.RefreshDataAreaSync((DateTime)st);
            pf.DisconnectPI();

            LogicFuncCore.Manager m2 = new LogicFuncCore.Manager();
            m2.LoadLogicRules("logicrules.ini");
            m2.RefreshDataAreaSync((DateTime)st);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            st = new DateTime(2014, 6, 1, 0, 0, 0);
            LogicFuncCore.Manager m2 = new LogicFuncCore.Manager();
            m2.LoadLogicRules("logicrules.ini");
            m2.RefreshDataAreaSync((DateTime)st);

            ContextFuncCore.Manager m = new ContextFuncCore.Manager();
            m.LoadCtxRules("contextrules.ini");
            m.RefreshDataAreaSync((DateTime)st);

            GroupFuncCore.Manager m3 = new GroupFuncCore.Manager();
            m3.LoadGroupRules("grouprules.ini");
            m3.RefreshDataAreaSync((DateTime)st);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                //VsaEngine ve = Microsoft.JScript.Vsa.VsaEngine.CreateEngine();
                //object rtnobject = Microsoft.JScript.Eval.JScriptEvaluate("2+ +1.4", "unsafe", ve);
                //double v = double.Parse(rtnobject.ToString());
                double? t = PublicLib.Common.Expr("2>1?true:,5");
                t = PublicLib.Common.Expr2("2>1?true:,5");
                //t = PublicLib.Common.Expr2("true||0");
                //t = PublicLib.Common.Expr2("3&&2");
                //t = PublicLib.Common.Expr2("2>1?3:5");
                //t = PublicLib.Common.Expr("2>1?3:5");

                bool? b = PublicLib.Common.LExpr("3||0");
                b = PublicLib.Common.LExpr("true||0");
                b = PublicLib.Common.LExpr("3&&2");
                b = PublicLib.Common.LExpr("2>1?3:5");
                b = PublicLib.Common.LExpr("2>1?0:5");
                b = PublicLib.Common.LExpr("2>1?true:,5");
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e)
        {
            button11.Enabled = false;
            button12.Enabled = false;
            button13.Enabled = false;
            button14.Enabled = false;

            st = DateTime.Parse(textBox1.Text);
            et = DateTime.Parse(textBox2.Text);
            #region hide
            //ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Message, info = "-----------Start----------", ts = DateTime.Now };
            //(new PublicLib.Log()).AddExceptionLog(eb, logtype.console);

            //ArithmeticFuncCore.Manager m = new ArithmeticFuncCore.Manager();
            //m.LoadInputs("inputs.ini");
            //m.LoadOutputs("outputs.ini");
            //PI.PIFunc pf = new PI.PIFunc("piadmin", "pirw", "pirw");

            //LogicFuncCore.Manager m2 = new LogicFuncCore.Manager();
            //m2.LoadLogicRules("logicrules.ini");

            //ContextFuncCore.Manager m3 = new ContextFuncCore.Manager();
            //m3.LoadCtxRules("contextrules.ini");

            //GroupFuncCore.Manager m4 = new GroupFuncCore.Manager();
            //m4.LoadGroupRules("grouprules.ini");
            #endregion
            while (st < et)
            {
                m.RefreshDataAreaSync((DateTime)st);
                m2.RefreshDataAreaSync((DateTime)st);
                m3.RefreshDataAreaSync((DateTime)st);
                m4.RefreshDataAreaSync((DateTime)st);
                st = ((DateTime)st).AddMinutes(1.0);
                Thread.Sleep(100);
            }
          
            ExceptionBody eb2 = new ExceptionBody() { et = ExceptionType.Message, info = "-----------End----------", ts = DateTime.Now };
            (new PublicLib.Log()).AddExceptionLog(eb2, logtype.console);
            //pf.DisconnectPI();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_Click(object sender, EventArgs e)
        {
            button10.Enabled = false;
            button12.Enabled = false;
            button13.Enabled = false;
            button14.Enabled = false;

            if ((st == null) || (et == null))
            {
                st = DateTime.Parse(textBox1.Text);
                et = DateTime.Parse(textBox2.Text);
            }
            if (st < et)
            {
                m.RefreshDataAreaSync((DateTime)st);
                m2.RefreshDataAreaSync((DateTime)st);
                m3.RefreshDataAreaSync((DateTime)st);
                m4.RefreshDataAreaSync((DateTime)st);
                
                richTextBox1.Clear();

                richTextBox1.Text += ((DateTime)st).ToString("yyyy/MM/dd HH:mm:ss");
                richTextBox1.Text += "\r\n";

                foreach (KeyValuePair<string, ArithmeticFuncCore.PointValue> kvp in ArithmeticFuncCore.DataArea.cd)
                {
                    richTextBox1.Text += "Arithmetic-" + kvp.Key + ":" + " 异常状态" + (kvp.Value.es == null ? "null" : kvp.Value.es.ToString()) + " 值" + (kvp.Value.pv == null ? "null" : kvp.Value.pv.ToString()) + " 类型" + (kvp.Value.type == null ? "null" : kvp.Value.type.ToString()) + " 公式" + (string.IsNullOrEmpty(kvp.Value.fm) ? "null" : kvp.Value.fm.ToString());
                    richTextBox1.Text += "\r\n";
                }

                foreach (KeyValuePair<string, LogicFuncCore.LogicPoint> kvp in LogicFuncCore.DataArea.cd)
                {
                    richTextBox1.Text += "Logic-" + kvp.Key + ":" + " 异常状态" + (kvp.Value.es == null ? "null" : kvp.Value.es.ToString()) + " 值" + (kvp.Value.result == null ? "null" : kvp.Value.result.ToString()) + " 公式" + (kvp.Value.fm == null ? "null" : kvp.Value.fm.ToString()) + " 点名" + (kvp.Value.pn == null ? "null" : kvp.Value.pn.ToString());
                    richTextBox1.Text += "\r\n";
                }

                foreach (KeyValuePair<string, ContextFuncCore.ContextValue> kvp in ContextFuncCore.DataArea.cd)
                {
                    richTextBox1.Text += "Context-" + kvp.Key + ":" + " 异常状态" + (kvp.Value.es == null ? "null" : kvp.Value.es.ToString()) + " 值" + (kvp.Value.result == null ? "null" : kvp.Value.result.ToString()) + " 公式" + (kvp.Value.fn == null ? "null" : kvp.Value.fn.ToString()) + " 点名" + (kvp.Value.pn == null ? "null" : kvp.Value.pn.ToString());
                    richTextBox1.Text += "\r\n";
                }

                foreach (KeyValuePair<string, GroupFuncCore.GroupValue> kvp in GroupFuncCore.DataArea.cd)
                {
                    richTextBox1.Text += "Group-" + kvp.Key + ":" + " 异常状态" + (kvp.Value.es == null ? "null" : kvp.Value.es.ToString()) + " 值" + (kvp.Value.result == null ? "null" : kvp.Value.result.ToString()) + " 公式" + (kvp.Value.fm == null ? "null" : kvp.Value.fm.ToString()) + " 点名" + (kvp.Value.pn == null ? "null" : kvp.Value.pn.ToString());
                    richTextBox1.Text += "\r\n";
                }
                st = ((DateTime)st).AddMinutes(1.0);
            }
            else
            {
                ExceptionBody eb2 = new ExceptionBody() { et = ExceptionType.Message, info = "-----------End----------", ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb2, logtype.console);
            }
        }

        /// <summary>
        /// realtime---start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button12_Click(object sender, EventArgs e)
        {
            button10.Enabled = false;
            button11.Enabled = false;

            button12.Enabled = false;
            button13.Enabled = true;
            button14.Enabled = true;

            if (t2.ThreadState == ThreadState.Unstarted)
            {
                t2.Start();
            }
            else
            {
                t2.Resume();
            }
        }

        /// <summary>
        /// realtime---pause
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button13_Click(object sender, EventArgs e)
        {
            button12.Enabled = true;
            button13.Enabled = false;
            button14.Enabled = false;

            t2.Suspend();
        }

        /// <summary>
        /// realtime---stop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button14_Click(object sender, EventArgs e)
        {
            button12.Enabled = false;
            button13.Enabled = false;
            button14.Enabled = false;

            t2.Abort();
        }

        /// <summary>
        /// 
        /// </summary>
        public void WorkerThread()
        {
            TimeMachine tm = new TimeMachine(0, 0, 0, 1, 60, 10, OffsetType.Second);
            while (1 == 1)
            {
                if (tm.IsPermitted())
                {
                    DateTime ts = tm.LastTimeStamp;

                    ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Message, info = "   ", ts = DateTime.Now };
                    (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                    ExceptionBody eb2 = new ExceptionBody() { et = ExceptionType.Message, info = "时间标签:" + ts.ToString("yyyy/MM/dd HH:mm:ss") + " 延时" + tm.Delay.ToString() + "分钟", ts = DateTime.Now };
                    (new PublicLib.Log()).AddExceptionLog(eb2, logtype.console);

                    m.RefreshDataAreaSync(ts);
                    m2.RefreshDataAreaSync(ts);
                    m3.RefreshDataAreaSync(ts);
                    m4.RefreshDataAreaSync(ts);

                    GC.Collect();
                }
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button15_Click(object sender, EventArgs e)
        {

            //(new SQL.GroupRules("dbconn")).AddGroupRuleRd("abcd", "efgd", System.DateTime.Now, new DateTime(1900, 1, 1, 0, 0, 0), "type", "subtype", "", "ems");
            (new SQL.GroupRules("dbconn")).UpdateGroupRuleRd_CalibFinalize("efgd", DateTime.Now.AddDays(-1), DateTime.Now, DateTime.Now);
            (new SQL.GroupRules("dbconn")).AddGroupRuleRd2("abcd", DateTime.Now, new DateTime(1900, 1, 1, 0, 0, 0), "type", "subtype", "ems");
            (new SQL.GroupRules("dbconn")).UpdateGroupRuleRd2_CalibFinalize("abcd", DateTime.Now, DateTime.Now, DateTime.Now);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button16_Click(object sender, EventArgs e)
        {
            //PI.PIFunc pf = new PI.PIFunc("piadmin", "pirw", "pirw");"10.150.124.193"
            //int s = PI.PIFunc2.piut_connect(null);
            int i = PI.PIFunc2.piut_isconnected();
            int g = PI.PIFunc2.piut_setservernode("10.150.124.193");
            i = PI.PIFunc2.piut_isconnected();
            int v = 0;
            int s2 = PI.PIFunc2.piut_login("pirw", "pirw", ref v);

            for (int ii = 0; ii < 10000; ii++)
            {
                #region digit point
                ////////////////////////////digit point
                int pn1 = 0;
                int h = PI.PIFunc2.pipt_findpoint("00AAESSP-P", ref pn1);

                int ty = 0;
                int sss = PI.PIFunc2.pipt_pointtypex(pn1, ref ty);

                int dtr = 0;
                
                //PI.PIFunc2.pitm_servertime(ref dtr);

                int[] dt = new int[6];
                dt[0] = 12;
                dt[1] = 22;
                dt[2] = 2013;
                dt[3] = 16;
                dt[4] = 35;
                dt[5] = 0;

                PI.PIFunc2.pitm_intsec(ref dtr, dt);

                float vv = 0;
                Int32 iv = 0;
                
                int ttt = PI.PIFunc2.piar_value(pn1, ref dtr, 3, ref vv, ref iv);

                char ds = ' ';
                
                //ttt = PI.PIFunc2.pipt_digstate(iv, ref ds, 1);

                PI.PIFunc2.pisn_getsnapshot(pn1, ref vv, ref iv, ref dtr);

                //ttt = PI.PIFunc2.pipt_digstate(iv, ref ds, 1);

                #endregion

                #region real point
                /////////////////////real point ---- it is fine now.
                h = PI.PIFunc2.pipt_findpoint("10HSA32CQ101AA", ref pn1);

                sss = PI.PIFunc2.pipt_pointtypex(pn1, ref ty);
                
                dt[0] = 1;
                dt[1] = 25;
                dt[2] = 2014;
                dt[3] = 0;
                dt[4] = 0;
                dt[5] = 0;
                PI.PIFunc2.pitm_intsec(ref dtr, dt);

                ttt = PI.PIFunc2.piar_value(pn1, ref dtr, 1, ref vv, ref iv);

                PI.PIFunc2.pisn_getsnapshot(pn1, ref vv, ref iv, ref dtr);

                int iiv = (int)iv;
                #endregion 

                Thread.Sleep(10);
            }
            int t = PI.PIFunc2.piut_disconnect();
            i = PI.PIFunc2.piut_isconnected();
        }
    }
}
