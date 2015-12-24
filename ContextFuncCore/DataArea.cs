using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContextFuncCore
{
    /// <summary>
    /// 
    /// </summary>
    public class DataArea
    {
        /// <summary>
        /// core data
        /// </summary>
        public static Dictionary<string, ContextValue> cd { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        public DataArea()
        {
            if (cd == null)
            {
                cd = new Dictionary<string, ContextValue>();
            }
        }
    }
}
