using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PublicLib;

namespace GroupFuncCore
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
        public void GetAllGroupRules(string path)
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
                        GroupValue gv = new GroupValue();
                        gv.pn = tempstr.Trim().Split(new string[] { ":=" }, StringSplitOptions.None)[1].Split(';')[0];
                        gv.fm = tempstr.Trim().Split(new string[] { ":=" }, StringSplitOptions.None)[1].Split(';')[1];
                        gv.callback = tempstr.Trim().Split(new string[] { ":=" }, StringSplitOptions.None)[1].Split(';')[2];
                        gv.grouptype = tempstr.Trim().Split(new string[] { ":=" }, StringSplitOptions.None)[1].Split(';')[3];
                        gv.groupsubtype = tempstr.Trim().Split(new string[] { ":=" }, StringSplitOptions.None)[1].Split(';')[4];
                        gv.rn = tempstr.Trim().Split(new string[] { ":=" }, StringSplitOptions.None)[0];
                        GroupFuncCore.DataArea.cd.Add(tempstr.Trim().Split(new string[] { ":=" }, StringSplitOptions.None)[0], gv);
                    }
                }
                sr.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "Group-ConfigParser?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
            }
        }
    }
}
