using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQL
{
    /// <summary>
    /// 
    /// </summary>
    public class Log
    {
        /// <summary>
        /// connection string
        /// </summary>
        public string connstr { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="dbstr"></param>
        public Log(string dbstr)
        {
            connstr = dbstr;
        }

    }
}
