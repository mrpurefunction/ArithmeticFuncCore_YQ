using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicLib
{
    #region hide
    /// <summary>
    /// 间隔时间类型
    /// 用于计算的间隔时间类型: 班/日/月/年
    /// </summary>
    public enum IntervalType
    {
        shift = 0,
        day,
        month,
        year
    }

    /// <summary>
    /// 需要计算的数据种类
    /// 包括计量点、产品产量、原材料、指标
    /// </summary>
    public enum CalculateType
    {
        measurepoint = 0,
        productamout,
        rawmaterial,
        indicator
    }
    #endregion
    /// <summary>
    /// 
    /// </summary>
    public enum OffsetType
    {
        Second,
        Minute
    }
    /// <summary>
    /// Time Machine -- control time
    /// </summary>
    public class TimeMachine
    {
        /// <summary>
        /// 基准小时
        /// </summary>
        public int BaseHour
        {
            get;
            private set;
        }
        /// <summary>
        /// 基准分钟
        /// </summary>
        public int BaseMinute
        {
            get;
            private set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int BaseSecond
        {
            get;
            private set;
        }
        /// <summary>
        /// 单位偏移分钟数
        /// </summary>
        public int UnitOffsetMinute
        {
            get;
            private set;
        }
        /// <summary>
        /// 单位偏移秒数
        /// </summary>
        public int UnitOffsetSecond
        {
            get;
            private set;
        }
        /// <summary>
        /// 上次更新时间
        /// </summary>
        public DateTime LastUpdateTime
        {
            get;
            private set;
        }
        #region hide
        /// <summary>
        /// 
        /// </summary>
        //public bool ValidWhenStarted
        //{
        //    get;
        //    private set;
        //}
        #endregion
        /// <summary>
        /// 下次分配执行的时间
        /// </summary>
        public DateTime NextAssignedTime
        {
            get;
            set;
        }
        /// <summary>
        /// 延时计算分钟数
        /// </summary>
        public int Delay
        {
            get;
            private set;
        }
        /// <summary>
        /// 偏移的时间类型
        /// </summary>
        public OffsetType OT
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime LastTimeStamp
        {
            get
            {
                if (OT == OffsetType.Minute)
                {
                    return NextAssignedTime.AddMinutes(-UnitOffsetMinute);
                }
                else if (OT == OffsetType.Second)
                {
                    return NextAssignedTime.AddSeconds(-UnitOffsetSecond);
                }
                return DateTime.Now;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bh"></param>
        /// <param name="bm"></param>
        /// <param name="uom"></param>
        /// <param name="vws">valid when start</param>
        public TimeMachine(int bh, int bm, int bs, int uom/*, bool vws*/, int uos, int delay, OffsetType ot)
        {
            OT = ot;
            //
            if ((bh > 23) || (bh < 0))
            {
                BaseHour = 0;
            }
            else
                BaseHour = bh;

            if ((bm < 0) || (bm > 59))
            {
                BaseMinute = 0;
            }
            else
                BaseMinute = bm;

            if ((bs < 0) || (bs > 59))
            {
                BaseSecond = 0;
            }
            else 
                BaseSecond = bs;

            //minute offset handler
            if (uom < 1)
            {
                UnitOffsetMinute = 1;
            }
            else if ((uom < 1440) && (uom >= 1))
            {
                UnitOffsetMinute = 1440 / (int)Math.Ceiling((double)(1440 / uom));
            }
            else
            {
                UnitOffsetMinute = 1440 * (int)Math.Floor((double)(uom / 1440));
            }
            //second offset handler
            if (uos < 1)
            {
                UnitOffsetSecond = 1;
            }
            else if ((uos < 1440 * 60) && (uos >= 1))
            {
                UnitOffsetSecond = (1440 * 60) / (int)Math.Ceiling((double)((1440 * 60) / uos));
            }
            else
            {
                UnitOffsetSecond = (1440 * 60) * (int)Math.Floor((double)(uos / (1440 * 60)));
            }

            //增加延时
            if (delay < 0)
            {
                Delay = 0;
            }
            else
            {
                Delay = delay;
            }

            LastUpdateTime = new DateTime(1900, 1, 1, bh, bm, bs);
            //如果UnitOffsetMinute或者UnitOffsetSecond的时长>=2天，则以当前时间的下一日为基准日。
            NextAssignedTime = new DateTime(DateTime.Now.AddDays(1.0).Year, DateTime.Now.AddDays(1.0).Month, DateTime.Now.AddDays(1.0).Day, bh, bm, bs);
            for (int i = 0; ; i++)
            {
                if (OT == OffsetType.Minute)
                {
                    //增加延时
                    if (NextAssignedTime.AddMinutes(-i * UnitOffsetMinute).AddMinutes(Delay) < DateTime.Now)
                    {
                        NextAssignedTime = NextAssignedTime.AddMinutes(-i * UnitOffsetMinute);
                        break;
                    }
                }
                else if (OT == OffsetType.Second)
                {
                    if (NextAssignedTime.AddSeconds(-i * UnitOffsetSecond).AddMinutes(Delay) < DateTime.Now)
                    {
                        NextAssignedTime = NextAssignedTime.AddSeconds(-i * UnitOffsetSecond);
                        break;
                    }
                }
            }
        }
        #region hide
        /// <summary>
        /// 新构造函数根据间隔类型来访问数据库
        /// 并确定nextassignedtime。
        /// </summary>
        /// <param name="it"></param>
        /// <param name="delay"></param>
        public TimeMachine(IntervalType it, CalculateType ct, int delay)
        {
            
            //班
            if (it == IntervalType.shift)
            {

            }
            //日
            else if (it == IntervalType.day)
            {

            }
            //月
            else if (it == IntervalType.month)
            {

            }
            //年
            else if (it == IntervalType.year)
            {

            }
            
        }
        #endregion
        /// <summary>
        /// 时间到？
        /// </summary>
        /// <returns></returns>
        public bool IsPermitted()
        {
            //时间被回调
            if (LastUpdateTime > DateTime.Now)
            {
                //写异常日志
                ExceptionBody eb = new ExceptionBody() { et = ExceptionType.Warning, info = "计算机系统时钟被回调", ts = DateTime.Now };
                (new PublicLib.Log()).AddExceptionLog(eb, logtype.console);
            }         
            //时间未到,返回false; 加入Delay延时操作
            if (DateTime.Now < NextAssignedTime.AddMinutes(Delay))
            {
                return false;
            }
            //时间到,允许执行,返回True;加入延时操作
            else
            {               
                for (int i = 1; ; i++)
                {
                    if (OT == OffsetType.Minute)
                    {
                        if (NextAssignedTime.AddMinutes(i * UnitOffsetMinute).AddMinutes(Delay) > DateTime.Now)
                        {
                            NextAssignedTime = NextAssignedTime.AddMinutes(i * UnitOffsetMinute);
                            LastUpdateTime = DateTime.Now;
                            return true;
                        }
                    }
                    else if (OT == OffsetType.Second)
                    {
                        if (NextAssignedTime.AddSeconds(i * UnitOffsetSecond).AddMinutes(Delay) > DateTime.Now)
                        {
                            NextAssignedTime = NextAssignedTime.AddSeconds(i * UnitOffsetSecond);
                            LastUpdateTime = DateTime.Now;
                            return true;
                        }
                    }
                }            
            }       
        }

        #region hide
        /// <summary>
        /// 时间到？
        /// </summary>
        /// <returns></returns>
        public bool IsPermitted2()
        {
            return false;
        }
        #endregion
    }
}
