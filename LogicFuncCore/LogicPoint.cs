using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogicFuncCore
{
    /// <summary>
    /// 逻辑点
    /// </summary>
    public class LogicPoint
    {
        /// <summary>
        /// point name
        /// </summary>
        public string pn { get; set; }
        /// <summary>
        /// type
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// subtype
        /// </summary>
        public string subtype { get; set; }
        /// <summary>
        /// formula
        /// </summary>
        public string fm { get; set; }
        /// <summary>
        /// result of logic calculation
        /// </summary>
        public bool? result { get; set; }
        /// <summary>
        /// exception status
        /// </summary>
        public bool? es { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ts { get; set; }
    }
}
