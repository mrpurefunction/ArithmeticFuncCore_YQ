using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using PublicLib;

namespace ArithmeticFuncCore
{
    /// <summary>
    /// 主管理器
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// Data Area
        /// </summary>
        public static DataArea da { get; set; }

        public static Mutex arithdicmutex;

        /// <summary>
        /// load input points
        /// </summary>
        /// <param name="path"></param>
        /// <param name="da"></param>
        public void LoadInputs(string path)
        {
            try
            {
                (new ConfigParser()).GetAllInputs(path);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// load output points
        /// </summary>
        /// <param name="path"></param>
        public void LoadOutputs(string path)
        {
            try
            {
                (new ConfigParser()).GetAllOutputs(path);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 同步更新数据
        /// </summary>
        /// <param name="ts"></param>
        public void RefreshDataAreaSync(DateTime ts)
        {
            ExceptionBody eb2 = new ExceptionBody() { et = ExceptionType.Message, info = "清空计量点状态位", ts = DateTime.Now };
            (new PublicLib.Log()).AddExceptionLog(eb2, logtype.console);

            //异常状态清空
            foreach (KeyValuePair<string, PointValue> kvp in DataArea.cd)
            {
                kvp.Value.es = null;
                kvp.Value.ts = ts;
            }

            ExceptionBody eb3 = new ExceptionBody() { et = ExceptionType.Message, info = "开始读取PI数据", ts = DateTime.Now };
            (new PublicLib.Log()).AddExceptionLog(eb3, logtype.console);

            //自动点从ｐｉ取值
            foreach (KeyValuePair<string,PointValue> kvp in DataArea.cd)
            {
                if (kvp.Value.type == 0)
                {
                    PI.RetVal rtv = (new PI.PIFunc2(null,null,null)).GetPointHisValue(kvp.Key, ts);
                    if (rtv == null)
                    {
                        kvp.Value.es = true;
                    }
                    else
                    {
                        kvp.Value.pv = rtv.pvalue;
                        //kvp.Value.ts = rtv.ts;
                        kvp.Value.es = false;
                        ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Message, info = "Arithmetic-RefreshData?" + kvp.Key + " PI数据正常刷新", ts = DateTime.Now };
                        (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
                    }
                }
            }

            ExceptionBody eb4 = new ExceptionBody() { et = ExceptionType.Message, info = "开始运算计量点数据", ts = DateTime.Now };
            (new PublicLib.Log()).AddExceptionLog(eb4, logtype.console);

            //其他点的计算
            foreach (KeyValuePair<string, PointValue> kvp in DataArea.cd)
            {
                if (kvp.Value.type != 0)
                {
                    //kvp.Value.ts = ts;
                    (new ArithmeticFunc(logtype.console)).CalValue(kvp.Value);
                }
            }
        }

        #region for test
        /// <summary>
        /// for test
        /// </summary>
        public void UpdatePv()
        {
            //PointValue pv = new PointValue();
            //pv.pv = 1.1;
            //DataArea.cd.TryAdd("a", pv);
            //PointValue pv2;
            //if (DataArea.cd.TryGetValue("a", out pv2))
            //{
            //    //not used
            //    //DataArea.cd.AddOrUpdate("a", pv2, (k, p) => new PointValue() { pv = 3.0 });
            //    //Do not use the following line's code to update the value;
            //    pv2.pv = 3;

            //    PointValue pv3 = new PointValue();
            //    pv3.pv = 5;
            //    pv3.es = false;

            //    PointValue pv4 = new PointValue();
            //    pv4.pv = 1.1;

            //    //keep the data clean, avoid dirty data
            //    bool b = DataArea.cd.TryUpdate("a", pv3, pv2);
            //}
        }

        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        public Manager()
        {
            if (da == null)
            {
                da = new DataArea(logtype.console);
            }
            if (arithdicmutex == null)
            {
                arithdicmutex = new Mutex();
            }
        }
    }
}
