using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.Concurrent;

namespace LogicFuncCore
{
    /// <summary>
    /// 
    /// </summary>
    public class DataArea
    {
        /// <summary>
        /// 数据存储区
        /// </summary>
        public static Dictionary<string, LogicPoint> cd { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        public DataArea()
        {
            if (cd == null)
            {
                cd = new Dictionary<string, LogicPoint>();
            }
        }

    }
}
