using System;
using System.Collections.Generic;
using WG3000_COMM.Core;

namespace ZBYGate_Data_Collection.Gate
{
    class Gate:IDisposable
    {
        private Log.CLog _Log = new Log.CLog();
        private System.Threading.Timer GetTimer;

        public Action<string> SetMessageAction;

        /// <summary>
        /// 控制器监听类
        /// </summary>
        wgWatchingService wgWatching;

        /// <summary>
        /// 控制器类
        /// </summary>
        private wgMjController wgMjController1, wgMjController2;

        /// <summary>
        /// 控制器状态事件
        /// </summary>
        public event EventHandler<DoorStateEventArgs> NewState;
         
        /// <summary>
        /// 监控的控制表
        /// </summary>
        private readonly Dictionary<Int32, wgMjController> selectedControllers;

        public Gate()
        {
            GetTimer = new System.Threading.Timer(GetStatus, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10));

            wgMjController1 = new wgMjController
            {
                IP = Properties.Settings.Default.Gate_InDoorIp,
                PORT = Properties.Settings.Default.Gate_Port,
                ControllerSN = Int32.Parse(Properties.Settings.Default.Gate_InDoorSN)
            };

            wgMjController2 = new wgMjController
            {
                IP = Properties.Settings.Default.Gate_OutDoorIp,
                PORT = Properties.Settings.Default.Gate_Port,
                ControllerSN = Int32.Parse(Properties.Settings.Default.Gate_OutDoorSN)
            };

            selectedControllers = new Dictionary<int, wgMjController>
            {
                { wgMjController1.ControllerSN, wgMjController1 },
                { wgMjController2.ControllerSN, wgMjController2 }
            };

            wgWatching = new wgWatchingService();
            wgWatching.EventHandler += WgWatching_EventHandler;
            wgWatching.WatchingController = selectedControllers;
        }

        /// <summary>
        /// 监听类事件
        /// </summary>
        /// <param name="message"></param>
        private void WgWatching_EventHandler(string message)
        {
            _Log.logWarn.Warn(message);
        }

        /// <summary>
        /// 定时查询
        /// </summary>
        /// <param name="state"></param>
        private void GetStatus(object state)
        {
            DoorSatus(Properties.Settings.Default.Gate_InDoorSN, Properties.Settings.Default.Gate_OutDoorSN);
        }

        /// <summary>
        /// 停止监控
        /// </summary>
        public void StopDoorState()
        {
            try
            {
                if (wgWatching != null)
                {
                    wgWatching.WatchingController = null;
                    wgWatching.StopWatch();
                }
            }
            catch (Exception ex)
            {
                _Log.logError.Error("Stop Door Watching", ex);
            }
            GetTimer.Change(-1, -1);
        }

        /// <summary>
        /// 开闸动作
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        public void OpenDoor(string Ip, int Port, string SN)
        {
            using (wgMjController wgMjController1 = new wgMjController())
            {
                wgMjController1.IP = Ip;
                wgMjController1.PORT = Port;
                wgMjController1.ControllerSN = Int32.Parse(SN);
                if(wgMjController1.RemoteOpenDoorIP(1)>0)
                {
                    _Log.logInfo.Info(string.Format("Open {0} Door Success",SN));
                    SetMessageAction?.Invoke(string.Format("Open {0} Door Success", SN));
                }
                else
                {
                    _Log.logInfo.Info(string.Format("Open {0} Door Failed",SN));
                    SetMessageAction?.Invoke(string.Format("Open {0} Door Failed", SN));
                }
            }
        }

        /// <summary>
        /// 查询状态
        /// </summary>
        /// <param name="Ip"></param>
        /// <param name="Port"></param>
        /// <param name="SN"></param>
        /// <returns></returns>
        private void DoorSatus(string InSN, string OutSN)
        {
            EventHandler<DoorStateEventArgs> newState = NewState;
            if (newState != null)
            {
                try
                {
                    wgMjControllerRunInformation conRunInfo = null;
                    int InStatus = wgWatching.CheckControllerCommStatus(Int32.Parse(InSN), ref conRunInfo);
                    newState(this, new DoorStateEventArgs(InStatus, Int32.Parse(InSN)));
                    int OutStatus = wgWatching.CheckControllerCommStatus(Int32.Parse(OutSN), ref conRunInfo);
                    newState(this, new DoorStateEventArgs(OutStatus, Int32.Parse(OutSN)));
                }
                catch (Exception ex)
                {
                    _Log.logError.Error("Query  Door State Error", ex);
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    StopDoorState();
                    wgWatching.Dispose();
                    GetTimer.Dispose();
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。     
                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~Gate() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
