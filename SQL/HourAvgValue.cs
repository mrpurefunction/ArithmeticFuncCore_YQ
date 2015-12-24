using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;

namespace SQL
{
    /// <summary>
    /// 
    /// </summary>
    public class HourAvgValue
    {
        /// <summary>
        /// connection string
        /// </summary>
        public string connstr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="indicatorname"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public bool? IsExist(int indicatorid, DateTime ts)
        {
            try
            {
                return false;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indicatorid"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public double? GetIndicatorValue(int indicatorid, DateTime ts)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbstr"></param>
        public HourAvgValue(string dbstr)
        {
            if (dbstr != null)
            {
                connstr = dbstr;
            }
        }
    }
}
