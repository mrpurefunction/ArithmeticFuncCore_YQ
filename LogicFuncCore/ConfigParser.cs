using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PublicLib;

namespace LogicFuncCore
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigParser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public void GetAllLogicRules(string path)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                string tempstr;
                while ((tempstr = sr.ReadLine()) != null)
                {
                    if ((tempstr.Trim() != "") && (tempstr.Trim().Substring(0, 2) != "//"))
                    {
                        LogicPoint lp = new LogicPoint();
                        lp.pn = tempstr.Trim().Split(new string[] { ":=" }, StringSplitOptions.None)[1].Split(';')[0];
                        lp.type = tempstr.Trim().Split(new string[] { ":=" }, StringSplitOptions.None)[1].Split(';')[1];
                        lp.subtype = tempstr.Trim().Split(new string[] { ":=" }, StringSplitOptions.None)[1].Split(';')[2];
                        lp.fm = tempstr.Trim().Split(new string[] { ":=" }, StringSplitOptions.None)[1].Split(';')[3];
                        LogicFuncCore.DataArea.cd.Add(tempstr.Trim().Split(new string[] { ":=" }, StringSplitOptions.None)[0], lp);
                    }
                }
                sr.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "Logic-ConfigParser?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
            }
        }
    }
}
