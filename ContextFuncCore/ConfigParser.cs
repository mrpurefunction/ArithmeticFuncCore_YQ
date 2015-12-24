using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PublicLib;

namespace ContextFuncCore
{
    public class ConfigParser
    {
        /// <summary>
        /// load all context rules
        /// </summary>
        /// <param name="path"></param>
        public void GetAllCtxRules(string path)
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
                        ContextValue cv = new ContextValue();
                        cv.fn = tempstr.Trim().Split(new string[] { ":=" }, StringSplitOptions.None)[1].Split(';')[0];
                        cv.rn = tempstr.Trim().Split(new string[] { ":=" }, StringSplitOptions.None)[1].Split(';')[1];
                        cv.end = tempstr.Trim().Split(new string[] { ":=" }, StringSplitOptions.None)[1].Split(';')[2];
                        cv.offsetahead = tempstr.Trim().Split(new string[] { ":=" }, StringSplitOptions.None)[1].Split(';')[3];
                        //cv.pn = LogicFuncCore.DataArea.cd[cv.rn].pn;
                        ContextFuncCore.DataArea.cd.Add(tempstr.Trim().Split(new string[] { ":=" }, StringSplitOptions.None)[0], cv);
                    }
                }
                sr.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "Context-ConfigParser?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
            }
        }
    }
}
