using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;

using System.IO;

namespace tt2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
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
                int h = PI.PIFunc2.pipt_findpoint("00AAESSP-J", ref pn1);//00AAESSP-J  00AAESSP-P IH.E_HZ.00ETN70AA0221.F_CV  CDEP158

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

                #region
                //char[] dss = new char[32];
                //unsafe
                //{
                //    fixed (char* pdss = dss)
                //    {
                //        ttt = PI.PIFunc2.pipt_digstate(iv, pdss, 31);
                //    }
                //}
                #endregion

                ttt = PI.PIFunc2.pipt_digstate(iv, ref ds, 1);

                PI.PIFunc2.pisn_getsnapshot(pn1, ref vv, ref iv, ref dtr);

                ttt = PI.PIFunc2.pipt_digstate(iv, ref ds, 1);

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

                //Thread.Sleep(10);
            }
            //int t = PI.PIFunc2.piut_disconnect();
            //i = PI.PIFunc2.piut_isconnected();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
