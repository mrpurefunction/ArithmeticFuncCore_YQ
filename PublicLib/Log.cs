using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace PublicLib
{
    /// <summary>
    /// 异常类型
    /// </summary>
    public enum ExceptionType
    {
        Error = 0,
        Warning,
        Notification,
        Message
    }

    /// <summary>
    /// 异常信息
    /// </summary>
    public class ExceptionBody
    {
        public DateTime ts;
        public ExceptionType et;
        public string info;
    }
    /// <summary>
    /// log type
    /// </summary>
    public enum logtype
    {
        console,//控制台
        evtlog,//事件
        db,//数据库
        console_evtlog,
        console_db,
        evtlog_db,
        console_evtlog_db
    }

    /// <summary>
    /// log handler
    /// </summary>
    public class Log
    {
        /// <summary>
        /// 缓冲区中的最大日志长度
        /// </summary>
        private readonly int MaxLogBufferLength = 2048;
        //log repository in cache
        private static System.Collections.Concurrent.ConcurrentQueue<ExceptionBody> consolelog { get; set; }
        private static System.Collections.Concurrent.ConcurrentQueue<ExceptionBody> evtlog { get; set; }
        private static System.Collections.Concurrent.ConcurrentQueue<ExceptionBody> dblog { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Log()
        {
            if (consolelog == null)
            {
                consolelog = new ConcurrentQueue<ExceptionBody>();
            }
            if (evtlog == null)
            {
                evtlog = new ConcurrentQueue<ExceptionBody>();
            }
            if (dblog == null)
            {
                dblog = new ConcurrentQueue<ExceptionBody>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eb"></param>
        /// <param name="lt"></param>
        public void AddExceptionLog(ExceptionBody eb, logtype lt)
        {
            //console
            if ((lt == logtype.console) || (lt == logtype.console_db) || (lt == logtype.console_evtlog) || (lt == logtype.console_evtlog_db))
            {
                while (consolelog.Count >= MaxLogBufferLength)
                {
                    ExceptionBody tempeb;
                    consolelog.TryDequeue(out tempeb);
                }
                consolelog.Enqueue(eb);
            }
            //db
            if ((lt == logtype.db) || (lt == logtype.console_db) || (lt == logtype.evtlog_db) || (lt == logtype.console_evtlog_db))
            {
                while (dblog.Count >= MaxLogBufferLength)
                {
                    ExceptionBody tempeb;
                    dblog.TryDequeue(out tempeb);
                }
                dblog.Enqueue(eb);
            }
            //evtviewer
            if ((lt == logtype.evtlog) || (lt == logtype.console_evtlog) || (lt == logtype.evtlog_db) || (lt == logtype.console_evtlog_db))
            {
                while (evtlog.Count >= MaxLogBufferLength)
                {
                    ExceptionBody tempeb;
                    evtlog.TryDequeue(out tempeb);
                }
                evtlog.Enqueue(eb);
            }
        }

        /// <summary>
        /// 输出console log
        /// </summary>
        public void ExportConsoleLog()
        {
            try
            {
                Console.BufferHeight = 2048;

                ExceptionBody eb;
                while (consolelog.TryDequeue(out eb))
                {
                    if (eb != null)
                    {
                        if (eb.et == ExceptionType.Error)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        else if (eb.et == ExceptionType.Warning)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        Console.WriteLine(eb.ts.ToString("yyyy/MM/dd HH:mm:ss.fff") + "---" + System.Enum.GetName(typeof(ExceptionType), eb.et) + ": " + eb.info);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 输出event log
        /// </summary>
        public void ExportEvtLog()
        {
        }

        /// <summary>
        /// 输出db log
        /// </summary>
        public void ExportDbLog()
        {
        }
    }
}
