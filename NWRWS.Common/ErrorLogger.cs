using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.Common
{
    public class ErrorLogger
    {

        public static void Error(string message, string stackTrace, string strControllername = "", string strActionName = "", string strActionType = "", bool withDBLog = false)
        {
            using (NLogger log = new NLogger(LogErrorFIle.ErrorLog))
            {
                if (withDBLog)
                {
                    DapperConnection.ErrorLogEntry(" Exception DB Log : " + message + " ### Trace : " + stackTrace, "DapperConnection", "", "DB", "");
                    log.Log(" Exception DB Log : " + message + " ### Trace : " + stackTrace, LogType.Error);
                }
                else
                {
                    DapperConnection.ErrorLogEntry(" Exception Web Log : " + message + " ### Trace : " + stackTrace, "DapperConnection", strControllername, "Web", strActionName + " ActionType=> " + strActionType);
                    log.Log(" Exception Web Log : " + message + " ### Trace : " + stackTrace, LogType.Error);
                }
            }
        }

        public static void Trace(string message, string stackTrace)
        {
            using (NLogger log = new NLogger(LogErrorFIle.traceLog))
            {
                log.Log(" Exception Web Log : " + message + " ### Trace : " + stackTrace, LogType.Info);
            }
        }
    }
}
