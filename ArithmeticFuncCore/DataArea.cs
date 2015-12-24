using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace ArithmeticFuncCore
{
    /// <summary>
    /// 数据存储区
    /// </summary>
    public class DataArea
    {
        /// <summary>
        /// 
        /// </summary>
        public PublicLib.logtype ltype { get; set; }

        /// <summary>
        /// 核心存储区
        /// </summary>
        public static Dictionary<string, PointValue> cd { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public DataArea(PublicLib.logtype lm)
        {
            ltype = lm;
            if (cd == null)
            {
                cd = new Dictionary<string, PointValue>();
            }
        }
    }
}
