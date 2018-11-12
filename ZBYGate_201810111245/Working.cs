using AxVeconclientProj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBYGate_Data_Collection
{
    class Working
    {
        private Log.CLog _Log = new Log.CLog();

        #region//集装箱
        private bool State = false;//车牌和箱号是否都处理完成
        private int AutoState = 0;//开闸自动状态
        private bool IsTrueOcr = false;//是否识别到车牌或箱号
        private string Containernumber = string.Empty;//集装箱号
        private string NewLpn = string.Empty;//空车车牌
        private string UpdateLpn = string.Empty;//重车车牌
        private DateTime Passtime;//过车时间
        #endregion

        #region//道闸
        string In_IP = Properties.Settings.Default.Gate_InDoorIp;
        string In_ControllerSN = Properties.Settings.Default.Gate_InDoorSN;
        string Out_IP = Properties.Settings.Default.Gate_OutDoorIp;        
        string Out_ControllerSN = Properties.Settings.Default.Gate_OutDoorSN;
        int PORT = Properties.Settings.Default.Gate_Port;
        #endregion

        #region//本地数据库
        public Func<string, string, string, string[]> SelectDataBase;//执行数据库查询
        #endregion

        #region//开闸
        public Action<string, int, string> OpenDoorAction;
        #endregion

        #region//LED显示
        public Action<string[]> AddTextAction;//添加文本
        public Action<int> SendAction;//推送消息
        #endregion

        #region//出入闸数据库
        public Action<string , string , DateTime , int > In_InsertDataBaseAction;
        public Action<string , DateTime ,int > Out_InsertDataBaseAction;
        #endregion

        #region
        public Action<string> SetMessage;
        private string NoOCRresult = Properties.Settings.Default.Working_NoOCRresult;
        private string Working_NoDataBaseResult = Properties.Settings.Default.Working_NoDataBaseResult;
        #endregion

        public Working()
        {
            //LED初始化
        }

        /// <summary>
        /// 箱号结果事件
        /// </summary>
        /// <param name="obj"></param>
        internal void ContainerResult(IVECONclientEvents_OnCombinedRecognitionResultISOEvent obj)
        {
            Containernumber = obj.containerNum1;
            Passtime = obj.triggerTime;
            Analysis();
        }

        /// <summary>
        /// 空车结果事件
        /// </summary>
        /// <param name="obj"></param>
        internal void NewLpnResult(IVECONclientEvents_OnNewLPNEventEvent obj)
        {
            NewLpn = obj.lPN;
            Analysis();
        }

        /// <summary>
        /// 重车结果事件
        /// </summary>
        /// <param name="obj"></param>
        internal void UpdateLpnResult(IVECONclientEvents_OnUpdateLPNEventEvent obj)
        {
            UpdateLpn = obj.lPN;
            Analysis();
        }

        /// <summary>
        /// 解析箱号数据
        /// </summary>
        private void Analysis()
        {
            //识别到箱号
            if (Containernumber != string.Empty)
            {
                //识别到空车牌
                if (NewLpn != string.Empty)
                {
                    State = true;
                    Select(NewLpn, Containernumber);
                }
                //识别到重车牌
                else if (UpdateLpn != string.Empty)
                {
                    State = true;
                    Select(UpdateLpn, Containernumber);
                }
            }
        }

        /// <summary>
        /// 出闸车牌识别结果
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        internal void PlateResult(string arg1, string arg2, string arg3, string arg4)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 查询数据库
        /// </summary>
        private void Select(string Lpn, string Container)
        {
            string[] DataBaseResult = new string[] { "NONE", "NONE", "NONE", "NONE", "NONE" };

            //车牌和箱号必须有一个结果
            if (Lpn != null || Container != null)
            {
                //查询数据库
                DataBaseResult = SelectDataBase?.Invoke(Lpn, Container, "");
                if (DataBaseResult.All(string.IsNullOrEmpty))
                {
                    DataBaseResult = new string[] { "NONE", "NONE", "NONE", "NONE", "NONE" };
                    DataBaseResult[4] = Working_NoDataBaseResult;
                    AutoState = 0;
                }
                else
                {
                    //查询到数据开闸
                    OpenDoorAction?.BeginInvoke(In_IP,PORT,In_ControllerSN,OpendoorSuccess,null);
                    AutoState = 1;
                }
                DataBaseResult[0] = string.Format("{0}/{1}", Lpn, Container);//联合显示车牌，箱号

                _Log.logInfo.Info(string.Format("Select {0} {1} from Gate", Lpn, Container));
                SetMessage?.Invoke(string.Format("Select {0} {1} from Gate", Lpn, Container));

                IsTrueOcr = true;
            }
            else
            {
                DataBaseResult[4] = NoOCRresult;
                AutoState = 0;
                IsTrueOcr = false;
            }
            if (State)
            {
                LedShow(DataBaseResult);

                if(IsTrueOcr)
                {
                    In_InsertDataBaseAction?.BeginInvoke(Lpn, Container, Passtime, AutoState,InsertSuccess,null);//插入数据库
                }

                Containernumber = string.Empty;
                NewLpn = string.Empty;
                UpdateLpn = string.Empty;
                State = false;
                AutoState = 0;
                IsTrueOcr = false;
            }
        }

        /// <summary>
        /// 插入数据完成
        /// </summary>
        /// <param name="ar"></param>
        private void InsertSuccess(IAsyncResult ar)
        {
            SetMessage?.Invoke("写入数据库完成");
        }

        /// <summary>
        /// 开闸完成
        /// </summary>
        /// <param name="ar"></param>
        private void OpendoorSuccess(IAsyncResult ar)
        {
            SetMessage?.Invoke("开闸完成");
        }

        /// <summary>
        /// LED推送显示
        /// </summary>
        /// <param name=""></param>
        private void LedShow(string[] dataBaseResult)
        {
            AddTextAction?.Invoke(dataBaseResult);
            SendAction?.BeginInvoke(0, SendCallBack, null);

            string tmp = string.Empty;
            foreach(string str in dataBaseResult)
            {
                tmp += str + ",";
            }
            _Log.logInfo.Info(string.Format("LED Show {0}", tmp));
            SetMessage?.Invoke(string.Format("LED Show {0}",tmp));
        }

        /// <summary>
        /// 推送信息完成
        /// </summary>
        /// <param name="ar"></param>
        private void SendCallBack(IAsyncResult ar)
        {
            SetMessage?.Invoke("LED信息推送完成");
        }
    }
}
