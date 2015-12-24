using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArithmeticFuncCore
{
    /// <summary>
    /// 点相关信息
    /// </summary>
    public class PointValue
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        public DateTime? ts { get; set; }
        /// <summary>
        /// Pointvalue
        /// </summary>
        public double? pv { get; set; }
        /// <summary>
        /// formula
        /// </summary>
        public string fm { get; set; }
        /// <summary>
        /// point type
        /// </summary>
        public int? type { get; set; }
        /// <summary>
        /// exception Status
        /// </summary>
        public bool? es { get; set; }
        /// <summary>
        /// active status
        /// </summary>
        public bool? activated { get; set; }
    }
}
