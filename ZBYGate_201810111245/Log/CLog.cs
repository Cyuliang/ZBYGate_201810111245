using log4net;

namespace ZBYGate_Data_Collection.Log
{
    public class CLog
    {
        public ILog logError = LogManager.GetLogger("ErrorLog");
        public ILog logDebug = LogManager.GetLogger("DebugLog");
        public ILog logInfo = LogManager.GetLogger("InfoLog");
        public ILog logWarn = LogManager.GetLogger("WarnLog");
    }
}
