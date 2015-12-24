using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using PublicLib;

namespace ArithmeticFuncCore
{
    /// <summary>
    /// 线程管理
    /// </summary>
    public class Worker
    {
        public logtype ltype { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logmode"></param>
        public Worker(logtype logmode)
        {

        }

        public void WorkFunc()
        {
            Manager.arithdicmutex.WaitOne();

            Manager.arithdicmutex.ReleaseMutex();
        }
    }
}
