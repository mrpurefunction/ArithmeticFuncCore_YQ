using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using PublicLib;

namespace ArithmeticFuncCore
{
    /// <summary>
    /// 配置文件解析器
    /// </summary>
    public class ConfigParser
    {
        /// <summary>
        /// get all of the input points
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public void GetAllInputs(string path)
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
                        PointValue pv = new PointValue();
                        pv.type = 0;
                        ArithmeticFuncCore.DataArea.cd.Add(tempstr.Trim(), pv);
                    }
                }
                sr.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "Arithmetic-ConfigParser?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
            }
        }

        /// <summary>
        /// get all of the output point and inside point
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public void GetAllOutputs(string path)
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
                        PointValue pv = new PointValue();
                        pv.type = int.Parse(tempstr.Trim().Split(';')[1]);
                        pv.fm = tempstr.Trim().Split(';')[0].Split(new string[] { ":=" }, StringSplitOptions.None)[1];
                        ArithmeticFuncCore.DataArea.cd.Add(tempstr.Trim().Split(';')[0].Split(new string[] { ":=" }, StringSplitOptions.None)[0], pv);
                    }
                }
                sr.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Error, info = "Arithmetic-ConfigParser?" + ex.Message, ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
            }
        }
    }
}
