using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupFuncCore
{
    /// <summary>
    /// value of group calculation
    /// </summary>
    public class GroupValue
    {
        /// <summary>
        /// formula
        /// </summary>
        public string fm { get; set; }
        /// <summary>
        /// group type
        /// </summary>
        public string grouptype { get; set; }
        /// <summary>
        /// callback function
        /// </summary>
        public string callback { get; set; }
        /// <summary>
        /// point name
        /// </summary>
        public string pn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        private bool? _result;
        /// <summary>
        /// result of calculation
        /// </summary>
        public bool? result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
                //operation after value set
                if (_result == true)
                {
                    if (callback.ToLower() != "null")
                    {
                        string[] funcs = callback.Split('&');
                        foreach (string f in funcs)
                        {
                            try
                            {
                                string[] fn = f.Substring(1, f.Length - 2).Split(',');
                                if (fn[0].ToLower() == "add")
                                {
                                    (new SQL.GroupRules("dbconn")).AddGroupRuleRd(this.pn, this.rn, this.ts, new DateTime(1900, 1, 1, 0, 0, 0), this.grouptype, this.groupsubtype, "", fn[1]);
                                }
                                else if (fn[0].ToLower() == "add3")
                                {
                                    (new SQL.GroupRules("dbconn")).AddGroupRuleRd(this.pn, this.rn, ((DateTime)this.ts).AddMinutes(double.Parse(fn[2])), new DateTime(1900, 1, 1, 0, 0, 0), this.grouptype, this.groupsubtype, "", fn[1]);
                                }
                                else if (fn[0].ToLower() == "update")
                                {
                                    (new SQL.GroupRules("dbconn")).UpdateGroupRuleRd_CalibFinalize(this.rn, ((DateTime)this.ts).AddMinutes(int.Parse(fn[1])), (DateTime)this.ts, (DateTime)this.ts);
                                }
                                else if (fn[0].ToLower() == "add2")
                                {
                                    (new SQL.GroupRules("dbconn")).AddGroupRuleRd2(this.pn, this.ts, new DateTime(1900, 1, 1, 0, 0, 0), this.grouptype, this.groupsubtype, fn[1]);
                                }
                                else if (fn[0].ToLower() == "add4")
                                {
                                    (new SQL.GroupRules("dbconn")).AddGroupRuleRd2(this.pn, ((DateTime)this.ts).AddMinutes(double.Parse(fn[2])), new DateTime(1900, 1, 1, 0, 0, 0), this.grouptype, this.groupsubtype, fn[1]);
                                }
                                else if (fn[0].ToLower() == "update2")
                                {
                                    (new SQL.GroupRules("dbconn")).UpdateGroupRuleRd2_CalibFinalize(this.pn, ((DateTime)this.ts).AddMinutes(int.Parse(fn[1])), (DateTime)this.ts, (DateTime)this.ts);
                                }
                                else if (fn[0].ToLower() == "addo_h")
                                {
                                    if (fn[2].ToLower() == "h")
                                    {
                                        (new SQL.GroupRules("dbconn")).AddGroupRuleRd(this.pn, this.rn, DateTime.Parse(((DateTime)this.ts).AddHours(int.Parse(fn[1])).ToString("yyyy-MM-dd HH:00:00")), new DateTime(1900, 1, 1, 0, 0, 0), this.grouptype, this.groupsubtype, "", fn[3]);
                                    }
                                }
                                else if (fn[0].ToLower() == "addo_h2")
                                {
                                    if (fn[2].ToLower() == "h")
                                    {
                                        (new SQL.GroupRules("dbconn")).AddGroupRuleRd2(this.pn, DateTime.Parse(((DateTime)this.ts).AddHours(int.Parse(fn[1])).ToString("yyyy-MM-dd HH:00:00")), new DateTime(1900, 1, 1, 0, 0, 0), this.grouptype, this.groupsubtype, fn[3]);
                                    }
                                }
                                else if (fn[0].ToLower() == "setarithvalue")
                                {
                                    ArithmeticFuncCore.Common.SetValue(fn[1], double.Parse(fn[2]));
                                }
                                else if (fn[0].ToLower() == "setpivalue")
                                {
                                    (new PI.PIFunc2(null, null, null)).SetPointHisValue(fn[1], (DateTime)ts, ArithmeticFuncCore.DataArea.cd[fn[2]].pv);
                                }
                                else if (fn[0].ToLower() == "setarithvalue2")
                                {
                                    ArithmeticFuncCore.Common.SetValue(fn[1], ArithmeticFuncCore.DataArea.cd[fn[2]].pv);
                                }
                                else if (fn[0].ToLower() == "setpivalue2")
                                {
                                    (new PI.PIFunc2(null, null, null)).SetPointHisValue(fn[1], ((DateTime)ts).AddMinutes(double.Parse(fn[3])), ArithmeticFuncCore.DataArea.cd[fn[2]].pv);
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }               
            }
        }
        /// <summary>
        /// exception status
        /// </summary>
        public bool? es { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string groupsubtype { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ts { get; set; }
        /// <summary>
        /// rulename
        /// </summary>
        public string rn { get; set; }
    }
}
