using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupFuncCore
{
    /// <summary>
    /// 
    /// </summary>
    public class DataArea
    {
        /// <summary>
        /// core data
        /// </summary>
        public static Dictionary<string, GroupValue> cd { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        public DataArea()
        {
            if (cd == null)
            {
                cd = new Dictionary<string, GroupValue>();
            }
        }
    }
}
