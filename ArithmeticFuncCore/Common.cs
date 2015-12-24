using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArithmeticFuncCore
{
    public class Common
    {
        /// <summary>
        /// 设置中间变量值
        /// </summary>
        /// <param name="pointkey"></param>
        /// <param name="pointvalue"></param>
        public static void SetValue(string pointkey, double? pointvalue)
        {
            try
            {
                DataArea.cd[pointkey].pv = pointvalue;
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 获得中间变量值
        /// </summary>
        /// <param name="pointkey"></param>
        /// <returns></returns>
        public static double? GetValue(string pointkey)
        {
            try
            {
                return DataArea.cd[pointkey].pv;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
