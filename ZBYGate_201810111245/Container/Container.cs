using AxVeconclientProj;
using System;
using ZBYGate_201810111245.Log;

namespace ZBYGate_201810111245.Container
{
    class Container
    {
        private AxVECONclient _AxVECONclient;
        private System.Threading.Timer _TimerConnect2Server;
        private CLog Log = new CLog();

        public Action<string> SetMessage;
        public Action<IVECONclientEvents_OnNewLPNEventEvent> NewLPNEvent;
        public Action<IVECONclientEvents_OnUpdateLPNEventEvent> UpdateLPNEvent;
        public Action<IVECONclientEvents_OnCombinedRecognitionResultISOEvent> CombinResult;
        //public Action<IVECONclientEvents_OnIntermediateRecognitionResultISOEvent> Intermediate;

        public Container()
        {
            #region //箱号初始化
            _AxVECONclient = new AxVECONclient();
            _AxVECONclient.CreateControl();
            _AxVECONclient.ServerIPAddr = Properties.Settings.Default.Container_Ip;
            _AxVECONclient.ServerPort = Properties.Settings.Default.Container_Port;   
            _AxVECONclient.OnServerConnected += _AxVECONclient_OnServerConnected;
            _AxVECONclient.OnServerDisconnected += _AxVECONclient_OnServerDisconnected;
            _AxVECONclient.OnServerError += _AxVECONclient_OnServerError;
            _AxVECONclient.OnNewLPNEvent += _AxVECONclient_OnNewLPNEvent;
            _AxVECONclient.OnUpdateLPNEvent += _AxVECONclient_OnUpdateLPNEvent;
            _AxVECONclient.OnIntermediateRecognitionResultISO += _AxVECONclient_OnIntermediateRecognitionResultISO;
            _AxVECONclient.OnCombinedRecognitionResultISO += _AxVECONclient_OnCombinedRecognitionResultISO;
            #endregion

            #region//对象初始化
            _TimerConnect2Server = new System.Threading.Timer(LinkCallBack, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(0));
            #endregion
        }

        /// <summary>
        /// 定时回调链接
        /// </summary>
        /// <param name="o"></param>
        private void LinkCallBack(object o)
        {
            Log.logInfo.Info("Link Container Init Start");
            SetMessage?.Invoke("Link Container Init Start");
            _AxVECONclient.Connect2Server();
        }

        /// <summary>
        /// 链接箱号系统
        /// </summary>
        public void LinkC(int i)
        {
            Log.logInfo.Info("Link Container Start");
            SetMessage?.Invoke("Link Container Start");
            _AxVECONclient.Connect2Server();
        }

        /// <summary>
        /// 主动断开链接
        /// </summary>
        public void CloseC(int i)
        {
            Log.logWarn.Warn("Container Close");
            SetMessage?.Invoke("Container Close");
            _AxVECONclient.Disconnect();
            _TimerConnect2Server.Change(-1,-1);
        }

        /// <summary>
        /// 获取最后一次结果
        /// </summary>
        /// <param name="num">车道号</param>
        public void LastR(int num)
        {
            _AxVECONclient.SendLastResults(num);
            SetMessage?.Invoke(string.Format("Get {0} Last Result",num));
        }

        /// <summary>
        /// 链接错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _AxVECONclient_OnServerError(object sender, System.EventArgs e)
        {
            Log.logWarn.Warn("Link Container Error");
            SetMessage?.Invoke("Link COntainer Error");
            _TimerConnect2Server.Change(TimeSpan.FromSeconds(5),TimeSpan.FromSeconds(0));
        }

        /// <summary>
        /// 链接断开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _AxVECONclient_OnServerDisconnected(object sender, System.EventArgs e)
        {
            Log.logWarn.Warn("Link Container Disconnect");
            SetMessage?.Invoke("Link Container Disconnect");
            _TimerConnect2Server.Change(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(0));
        }

        /// <summary>
        /// 链接成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _AxVECONclient_OnServerConnected(object sender, System.EventArgs e)
        {
            Log.logInfo.Info("Link Container Connected");
            SetMessage?.Invoke("Link Container Connected");
            _TimerConnect2Server.Change(-1,-1);
        }
        
        /// <summary>
        /// 中间结果事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _AxVECONclient_OnIntermediateRecognitionResultISO(object sender, AxVeconclientProj.IVECONclientEvents_OnIntermediateRecognitionResultISOEvent e)
        {
            //Intermediate?.Invoke(e);
            SetMessage?.Invoke(string.Format("Video：{0} ContainerNum：{1} CheckSum：{2}",e.videoID,e.containerNum,e.checkSum));
        }

        /// <summary>
        /// 箱号事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _AxVECONclient_OnCombinedRecognitionResultISO(object sender, AxVeconclientProj.IVECONclientEvents_OnCombinedRecognitionResultISOEvent e)
        {
            CombinResult?.Invoke(e);
            SetMessage?.Invoke(string.Format("CombinResult1：{0} CombinResult2：{0}", e.containerNum1,e.containerNum2));
            Log.logInfo.Info(string.Format("DateTimt：{0} CombinResult1：{1} CombinResult2：{2}",e.triggerTime.ToString("yyyy-MM-dd HH:mm:ss"), e.containerNum1,e.containerNum2));
        }

        /// <summary>
        /// 重车车牌事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _AxVECONclient_OnUpdateLPNEvent(object sender, AxVeconclientProj.IVECONclientEvents_OnUpdateLPNEventEvent e)
        {
            UpdateLPNEvent?.Invoke(e);
            SetMessage?.Invoke(string.Format("UpdateLPN：{0}", e.lPN));
            Log.logInfo.Info(string.Format("DateTime：{0} UpdateLPN：{1}", e.triggerTime.ToString("yyyy-MM-dd HH:mm:ss"), e.lPN));
        }

        /// <summary>
        /// 空车车牌事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _AxVECONclient_OnNewLPNEvent(object sender, AxVeconclientProj.IVECONclientEvents_OnNewLPNEventEvent e)
        {
            NewLPNEvent?.Invoke(e);
            SetMessage?.Invoke(string.Format("NewLPN：{0}", e.lPN));
            Log.logInfo.Info(string.Format("DateTime：{0} NewLPN：{1}", e.triggerTime.ToString("yyyy-MM-dd HH:mm:ss"), e.lPN));
        }
    }
}
