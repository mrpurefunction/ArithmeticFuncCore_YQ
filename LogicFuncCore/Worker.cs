using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogicFuncCore
{
    /// <summary>
    /// 
    /// </summary>
    public class Worker
    {
        /// <summary>
        /// 
        /// </summary>
        public void WorkFunc()
        {
            ArithmeticFuncCore.Manager.arithdicmutex.WaitOne();

            ArithmeticFuncCore.Manager.arithdicmutex.ReleaseMutex();
        }
    }
}
