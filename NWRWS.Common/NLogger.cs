using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;


namespace NWRWS.Common
{
    public enum LogErrorFIle
    {
        ErrorLog,
        traceLog,

    }

    public enum LogType
    {
        Trace = 1,
        Debug = 2,
        Info = 3,
        Warn = 4,
        Error = 5,
        Fatal = 6
    }

    public class NLogger : IDisposable
    {
        Logger logger;

        public NLogger()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        public NLogger(LogErrorFIle fileName)
        {
            logger = LogManager.GetLogger(fileName.ToString());
        }

        public void Log(string message, LogType type)
        {
            switch (type)
            {
                case LogType.Trace:
                    logger.Trace(message);
                    break;
                case LogType.Debug:
                    logger.Debug(message);
                    break;
                case LogType.Info:
                    logger.Info(message);
                    break;
                case LogType.Warn:
                    logger.Warn(message);
                    break;
                case LogType.Error:
                    logger.Error(message);
                    break;
                case LogType.Fatal:
                    logger.Fatal(message);
                    break;
                default:
                    logger.Debug(message);
                    break;
            }
        }

        public void Dispose()
        {
            logger = null;
        }
    }
}
