using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContextFuncCore
{
    /// <summary>
    /// context value body
    /// </summary>
    public class ContextValue
    {
        /// <summary>
        /// function name
        /// </summary>
        public string fn { get; set; }
        /// <summary>
        /// rule name
        /// </summary>
        public string rn { get; set; }
        /// <summary>
        /// end time
        /// </summary>
        public string end { get; set; }
        /// <summary>
        /// off set time ahead
        /// </summary>
        public string offsetahead { get; set; }
        /// <summary>
        /// exception status
        /// </summary>
        public bool? es { get; set; }
        /// <summary>
        /// result of context calculation
        /// </summary>
        public bool? result { get; set; }
        /// <summary>
        /// point name
        /// </summary>
        public string pn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ts { get; set; }
    }
}
