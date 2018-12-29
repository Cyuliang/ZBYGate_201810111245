﻿using AxVeconclientProj;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ZBYGate_Data_Collection
{
    class Working : IDisposable//远程服务器和本地数据库同步查询
    {
        private Log.CLog _Log = new Log.CLog();

        private int Select_Logic = Properties.Settings.Default.Working_Select_Logic;//处理数据逻辑

        #region//集装箱
        private string Containernumber = string.Empty;//集装箱号
        private string NewLpn = string.Empty;//空车车牌
        private string UpdateLpn = string.Empty;//重车车牌
        private string Plate = string.Empty;//身份证使用车牌数据
        private string Con = string.Empty;//身份证使用箱号数据
        private DateTime Passtime;//过车时间
        #endregion

        #region//道闸
        private readonly string In_IP = Properties.Settings.Default.Gate_InDoorIp;
        private readonly string In_ControllerSN = Properties.Settings.Default.Gate_InDoorSN;
        private readonly string Out_IP = Properties.Settings.Default.Gate_OutDoorIp;
        private readonly string Out_ControllerSN = Properties.Settings.Default.Gate_OutDoorSN;
        private readonly int PORT = Properties.Settings.Default.Gate_Port;
        #endregion

        #region//身份证读卡器
        public Action<int> CVRForReadAction;//定时循环读取身份证
        private readonly string Working_NoNumberResult = Properties.Settings.Default.Working_NoNumberResult;//没有身份证数据
        private bool ReadForBooen = true;//可以读取身份证
        #endregion

        #region//本地数据库
        public Func<string, string, string, string[]> SelectDataBase;//执行数据库查询
        #endregion

        #region//开闸
        public Action<string, int, string> OpenDoorAction;//开闸
        #endregion

        #region//LED显示
        public Action<int> DeleteScreen_DynamicAction;//删除显示屏
        public Action<int> AddScreen_DynamicAction;//添加显示屏
        public Action<int> AddScreenDynamicAreaAction;//添加动区
        public Action<string[]> AddTextAction;//添加文本
        public Action<int> SendAction;//推送消息
        #endregion

        #region//出入闸数据库
        public Action<string, string, DateTime, int> In_InsertDataBaseAction;
        public Action<string, DateTime, int> Out_InsertDataBaseAction;
        public Action<string, DateTime, int> In_UpdateDataBaseAction;
        public Action<string, string, int, DateTime> Rundata_InsertAction;
        public Action<string, DateTime> Rundata_updateAction;
        public Action<string> SetOutLedMessageAction;
        #endregion

        #region//HTTP
        public Func<string, string, string, string> HttpPostAction;//远程查询数据
        public Func<string, string[]> HttpJsonSplitAction;//解析Json
        #endregion

        #region
        public Action<string> SetMessage;
        private readonly string Working_NoOCRresult = Properties.Settings.Default.Working_NoOCRresult;
        private readonly string Working_NoDataBaseResult = Properties.Settings.Default.Working_NoDataBaseResult;
        private readonly string Http_Status = Properties.Settings.Default.Http_Status;
        private readonly bool HttpSwitch = Properties.Settings.Default.Http_switch;
        private readonly string Plate_Local_End_Message = Properties.Settings.Default.Plate_Local_End_Message;
        private readonly string Plate_local_Message = Properties.Settings.Default.Plate_Local_Message;
        private readonly string Led_Log = Properties.Settings.Default.Led_Log;
        private readonly bool WorkIng_ReadID = Properties.Settings.Default.WorkIng_ReadCards;
        private readonly string Http_HttpNull = Properties.Settings.Default.Http_HttpNull;
        private System.Threading.Timer _Timer = null;

        private Dictionary<IAsyncResult, bool> IsOpenDoorDictionary=new Dictionary<IAsyncResult, bool>();//开闸字典
        private Dictionary<IAsyncResult, string[]> ResultDictionary=new Dictionary<IAsyncResult, string[]>();//处理结果字典
        private IAsyncResult IAsyncResultHttp = null;
        private IAsyncResult IAsyncResultData = null;
        private IAsyncResult IAsyncResultJson = null;
        #endregion

        public Working()
        {
            //LED初始化
            _Timer = new System.Threading.Timer(OutLedDefaultShowCallBack, null, TimeSpan.FromSeconds(6), TimeSpan.FromSeconds(0));
        }

        #region//入闸

        /// <summary>
        /// 箱号结果事件
        /// </summary>
        /// <param name="obj"></param>
        internal void ContainerResult(IVECONclientEvents_OnCombinedRecognitionResultISOEvent obj)
        {
            Containernumber = obj.containerNum1;
            Con = obj.containerNum1;
            Passtime = obj.triggerTime;
            Analysis();
            //LedShow(new string[] { Led_Log });
        }

        /// <summary>
        /// 空车结果事件
        /// </summary>
        /// <param name="obj"></param>
        internal void NewLpnResult(IVECONclientEvents_OnNewLPNEventEvent obj)
        {
            NewLpn = obj.lPN;
            Plate = obj.lPN;
            Analysis();
        }

        /// <summary>
        /// 重车结果事件
        /// </summary>
        /// <param name="obj"></param>
        internal void UpdateLpnResult(IVECONclientEvents_OnUpdateLPNEventEvent obj)
        {
            UpdateLpn = obj.lPN;
            Plate = obj.lPN;
            Analysis();
        }

        /// <summary>
        /// 判断箱号和车牌是否都处理完成
        /// </summary>
        private void Analysis()
        {
            //识别到箱号
            if (Containernumber != string.Empty)
            {
                //识别到空车牌
                if (NewLpn != string.Empty)
                {
                    SelectLpnCon(NewLpn, Containernumber);
                }
                //识别到重车牌
                else if (UpdateLpn != string.Empty)
                {
                    SelectLpnCon(UpdateLpn, Containernumber);
                }
            }
        }

        /// <summary>
        /// 校验http和本地数据库数据
        /// </summary>
        /// <param name="Lpn"></param>
        /// <param name="Container"></param>
        private void SelectLpnCon(string Lpn, string Container)
        {
            if (Lpn != null || Container != null)//字段其中一个不为空就查询服务器
            {
                string[] Head = { Lpn, Container };                                                //组合显示车牌和箱号

                switch (Select_Logic)
                {
                    case 0://同步查询
                        IAsyncResultData = SelectDataBase?.BeginInvoke(Lpn, Container, "", SelectDataBaseCallBack, Head);//查询数据库    
                        if (HttpSwitch)                                                                                  //是否查询远端服务器
                        {
                            IAsyncResultHttp = HttpPostAction?.BeginInvoke(Passtime.ToString("yyyyMMddhhmmss"), Lpn, Container, SelectHttpCallBack, Head);
                        }
                        break;
                    case 1://顺序查询
                        IAsyncResultData = SelectDataBase?.BeginInvoke(Lpn, Container, "", One_SelectDataBaseCallBack, Head);//查询数据库    
                        break;
                    default://判断查询逻辑
                        if(Select_Logic!=0||Select_Logic!=1)
                        {
                            SetMessage?.Invoke("没有设置处理逻辑 Working_Select_Logic is 0 or 1");
                        }
                        break;
                }

            }
            else//没有识别到数据
            {
                bool IsOpenDoor = false;                             //是否开闸
                var LedShowDataResult = new string[] { "*", "*", "*", "*", "*", Working_NoOCRresult };
                LedShow(LedShowDataResult, IsOpenDoor);
            }
            NewLpn = string.Empty;
            UpdateLpn = string.Empty;
            Containernumber = string.Empty;
        }

        /// <summary>
        /// 1优先查询数据库
        /// </summary>
        /// <param name="ar"></param>
        private void One_SelectDataBaseCallBack(IAsyncResult ar)
        {
            bool IsOpenDoorD = false;                                             //是否开闸
            var Head = (string[])ar.AsyncState;                                   //车牌和箱号   
            var LedShowDataResult = SelectDataBase.EndInvoke(ar);                 //查询本地数据库回调返回数据
            SetMessage?.Invoke("查询数据库回调函数完成");

            if (LedShowDataResult.All(string.IsNullOrEmpty))                      //数据库记录为空
            {                                                                     //查询远端数据库
                IAsyncResultHttp = HttpPostAction?.BeginInvoke(Passtime.ToString("yyyyMMddhhmmss"), Head[0], Head[1], Tow_SelectHttpCallBack, Head);
            }
            else                                                                  //查询到数据库记录
            {
                LedShowDataResult[0] = string.Format("{0} {1}", Head[0], Head[1]);//设置显示识别到的箱号和车牌
                LedShowDataResult[1] = "准时";                                    //准时字段
                LedShowDataResult[5] = Http_Status;                               //提示进闸字段
                IsOpenDoorD = true;
                LedShow(LedShowDataResult, IsOpenDoorD);
            }

        }

        /// <summary>
        /// 2随后查询远端数据库
        /// </summary>
        /// <param name="ar"></param>
        private void Tow_SelectHttpCallBack(IAsyncResult ar)
        {
            bool IsOpenDoorH = false;                                               //是否开闸
            var Head = (string[])ar.AsyncState;                                     //车牌和箱号
            var HttpResult = HttpPostAction.EndInvoke(ar);                          ////http请求数据回调函数返回数据  
            SetMessage?.Invoke("分析远端服务器返回数据完成");

            //HttpResult = @"{""error_code"":""AE0000"",""error_desc"":""The request handled successful."",""result"":{""resultList"":""N"",""status"":""已预约"",""visito"":""TMT"",""ledgename"":""松山湖"",""platform"":""活态"",""truckNumber"":""粤B050CS"",""tranNo"":""12345"",""arrivedTime"":""2018 - 11 - 12 17:10:30""}}";
            if (HttpResult != null)
            {
                var LedShowDataResult = HttpJsonSplitAction?.Invoke(HttpResult);    //分析Json数据
                
                if (LedShowDataResult[1] == "Y")                                    //是否开闸
                {
                    IsOpenDoorH = true;
                    LedShowDataResult[1] = "准时";                                  //替换字段
                }
                LedShow(LedShowDataResult,IsOpenDoorH);                             //最后处理数据

                if(LedShowDataResult[1]=="N")                                       //没有数据刷身份证
                {
                    if (WorkIng_ReadID)
                    {
                        if (ReadForBooen)//读取身份证
                        {
                            CVRForReadAction?.BeginInvoke(0, this.ForDoneCallBack, null);
                            ReadForBooen = false;
                        }
                    }
                }

            }
            else
            {
                var LedShowDataResult = new string[] { string.Format("{0} {1}", Head[0], Head[1]), "*", "*", "*", "*", Http_HttpNull };
                LedShow(LedShowDataResult, IsOpenDoorH);
            }
        }

        /// <summary>
        /// 查询远端服务器回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void SelectHttpCallBack(IAsyncResult ar)
        {
            bool IsOpenDoorH = false;                                               //是否开闸
            var Head = (string[])ar.AsyncState;                                     //车牌和箱号

            var HttpResult = HttpPostAction.EndInvoke(ar);                          ////http请求数据回调函数返回数据  
            if (IAsyncResultData != null)
            {
                IAsyncResultData.AsyncWaitHandle.WaitOne();                         //等待数据库查询完成
            }

            //HttpResult = @"{""error_code"":""AE0000"",""error_desc"":""The request handled successful."",""result"":{""resultList"":""N"",""status"":""已预约"",""visito"":""TMT"",""ledgename"":""松山湖"",""platform"":""活态"",""truckNumber"":""粤B050CS"",""tranNo"":""12345"",""arrivedTime"":""2018 - 11 - 12 17:10:30""}}";
            if (HttpResult != null)                                                 
            {
                var LedShowDataResult = HttpJsonSplitAction?.Invoke(HttpResult);    //分析Json数据
                SetMessage?.Invoke("分析远端服务器返回数据完成");
                if (LedShowDataResult[1] == "Y")                                    //是否开闸
                {
                    IsOpenDoorH = true;
                    LedShowDataResult[1] = "准时";                                  //替换字段
                }
                if (IAsyncResultHttp != null)
                {
                    if (!ResultDictionary.ContainsKey(IAsyncResultHttp))
                    {
                        ResultDictionary.Add(IAsyncResultHttp, LedShowDataResult);  //添加显示字典
                    }
                    if (!IsOpenDoorDictionary.ContainsKey(IAsyncResultHttp))
                    {
                        IsOpenDoorDictionary.Add(IAsyncResultHttp, IsOpenDoorH);   //添加开闸字典
                    }
                }
                LedShow();                                                         //最后处理数据
            }
            else
            {
                IAsyncResultHttp = null;                                           //没有返回结果置空异步状态
            }
        }

        /// <summary>
        /// 查询数据据回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void SelectDataBaseCallBack(IAsyncResult ar)
        {
            bool IsOpenDoorD = false;                                             //是否开闸
            var Head = (string[])ar.AsyncState;                                   //车牌和箱号   

            var LedShowDataResult = SelectDataBase.EndInvoke(ar);                 //查询本地数据库回调返回数据

            if (IAsyncResultHttp != null)
            {
                IAsyncResultHttp.AsyncWaitHandle.WaitOne();                       //等待远端服务器查询完成
            }

            SetMessage?.Invoke("查询数据库回调函数完成");

            if (LedShowDataResult.All(string.IsNullOrEmpty))                      //数据库记录为空
            {
                LedShowDataResult = new string[] { string.Format("{0} {1}", Head[0], Head[1]), "*", "*", "*", "*", Working_NoDataBaseResult };
            }
            else                                                                  //查询到数据库记录
            {
                LedShowDataResult[0] = string.Format("{0} {1}", Head[0], Head[1]);//设置显示识别到的箱号和车牌
                LedShowDataResult[1] = "准时";                                    //准时字段
                LedShowDataResult[5] = Http_Status;                               //提示进闸字段
                IsOpenDoorD = true;
            }
            if (IAsyncResultData != null)
            {
                if (!ResultDictionary.ContainsKey(IAsyncResultData))
                {
                    ResultDictionary.Add(IAsyncResultData, LedShowDataResult);
                }
                if (!IsOpenDoorDictionary.ContainsKey(IAsyncResultData))
                {
                    IsOpenDoorDictionary.Add(IAsyncResultData, IsOpenDoorD);
                }
            }
            LedShow();
        }

        //查询本地数据库和远端服务器同步执行处理LED显示
        private void LedShow()
        {
            if (IsOpenDoorDictionary != null || ResultDictionary != null)
            {
                if ((IsOpenDoorDictionary.Count == 2 || IAsyncResultHttp == null)&&IsOpenDoorDictionary.Count!=0)                      //本地和远端处理完成或远端返回空
                {
                    bool status = false;
                    string[] dataBaseResult = new string[] { "*", "*", "*", "*", "*", "*" };
                    if (IsOpenDoorDictionary.ContainsValue(true))                                         //是否有开闸数据在字典中
                    {
                        OpenDoorAction?.BeginInvoke(In_IP, PORT, In_ControllerSN, OpendoorCallBack, null);//查询到数据开闸
                        var firstKey = IsOpenDoorDictionary.FirstOrDefault(q => q.Value == true).Key;     //获取开闸字段对应LED显示的信息的键
                        dataBaseResult = ResultDictionary[firstKey];                                      //对应VALUES 
                        status = true;
                    }
                    else
                    {
                        try
                        {
                            dataBaseResult = ResultDictionary.Values.First();
                            //没有开闸数据显示字典第一条
                        }
                        catch (Exception ex)
                        {
                            SetMessage?.Invoke("No Dictionary Data");
                            _Log.logError.Error("No Dictionary Data", ex);
                            return;
                        }

                        if(WorkIng_ReadID)
                        {
                            if (ReadForBooen)//读取身份证
                            {
                                CVRForReadAction?.BeginInvoke(0, this.ForDoneCallBack, null);
                                ReadForBooen = false;
                            }
                        }
                    }


                    ClearVar();//清除字典和回调状态
                    DeleteScreen_DynamicAction?.Invoke(0);//删除显示屏
                    AddScreen_DynamicAction?.Invoke(0);           //添加显示屏
                    AddScreenDynamicAreaAction?.Invoke(0);        //添加动态区
                    AddTextAction?.Invoke(dataBaseResult);//添加文本
                    SendAction?.BeginInvoke(0, SendCallBack, "");

                    string[] tmpIn = dataBaseResult[0].Split(' ');

                    Rundata_InsertAction?.BeginInvoke(tmpIn[0], tmpIn[1], status ? 1 : 0, Passtime, null, null);//插入数据库


                    string tmp = string.Empty;
                    foreach (string v in dataBaseResult)
                    {
                        tmp += v + ",";
                    }
                    SetMessage?.Invoke(string.Format("LED Show {0}", tmp));
                    _Log.logInfo.Info(string.Format("LED Show {0}", tmp));
                }
            }
        }

        /// <summary>
        /// 清除变量
        /// </summary>
        private void ClearVar()
        {
            IsOpenDoorDictionary.Clear();//开闸字典
            ResultDictionary.Clear();//处理结果字典

            IAsyncResultJson = null;
            IAsyncResultHttp = null;
            IAsyncResultData = null;
        }

        /// <summary>
        /// 身份证读取回调
        /// </summary>
        /// <param name="ar"></param>
        private void ForDoneCallBack(IAsyncResult ar)
        {
            SetMessage?.Invoke("身份证读取完成");
            ReadForBooen = true;
        }

        /// <summary>
        /// 身份证回调信息
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        /// <param name="arg5"></param>
        /// <param name="arg6"></param>
        /// <param name="arg7"></param>
        /// <param name="arg8"></param>
        /// <param name="arg9"></param>
        internal void FillDataAction(byte[] arg1, byte[] arg2, byte[] arg3, byte[] arg4, byte[] arg5, byte[] arg6, byte[] arg7, byte[] arg8, byte[] arg9)
        {
            string number = System.Text.Encoding.GetEncoding("GB2312").GetString(arg5).Replace("\0", "").Trim();//身份证号码
            string[] Head = { Plate, Con };
            SelectDataBase?.BeginInvoke("", "", number, this.SelectIDCallBack, Head);//查询身份证数据库    
        }

        /// <summary>
        /// 查询身份证回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void SelectIDCallBack(IAsyncResult ar)
        {
            bool IsOpenDoorD = false;                             //是否开闸
            var Head = (string[])ar.AsyncState;                    //入闸LED回调传递参数    
            var LedShowDataResult = SelectDataBase.EndInvoke(ar);//回调返回数据
            if (LedShowDataResult.All(string.IsNullOrEmpty))      //数据库记录为空
            {
                LedShowDataResult = new string[] { string.Format("{0} {1}", Head[0], Head[1]), "*", "*", "*", "*", Working_NoNumberResult };
                //LedShow(LedShowDataResult, IsOpenDoorD);            //推送LED
            }
            else//查询到数据库记录
            {
                In_UpdateDataBaseAction?.BeginInvoke(LedShowDataResult[0],Passtime,0,null,null);
                LedShowDataResult[0] = string.Format("{0} {1}", Head[0], Head[1]);       //设置显示识别到的箱号和车牌
                LedShowDataResult[1] = "准时";     //准时字段
                LedShowDataResult[5] = Http_Status;//提示进闸
                IsOpenDoorD = true;

                //if (IsOpenDoorD)                             //是否开闸
                //{
                //    OpenDoorAction?.BeginInvoke(In_IP, PORT, In_ControllerSN, OpendoorCallBack, null);//查询到数据开闸
                //}

                //LedShow(LedShowDataResult, IsOpenDoorD);            //推送LED
            }

            LedShow(LedShowDataResult, IsOpenDoorD);            //推送LED
            SetMessage?.Invoke("查询身份证回调函数完成");
        }


        /// <summary>
        /// LED推送显示
        /// </summary>
        /// <param name=""></param>
        private void LedShow(string[] dataBaseResult,bool isOpenDoor)
        {
            if (isOpenDoor)                             //是否开闸
            {
                OpenDoorAction?.BeginInvoke(In_IP, PORT, In_ControllerSN, OpendoorCallBack, null);//查询到数据开闸
            }
            DeleteScreen_DynamicAction?.Invoke(0);//删除显示屏
            AddScreen_DynamicAction?.Invoke(0);           //添加显示屏
            AddScreenDynamicAreaAction?.Invoke(0);        //添加动态区
            AddTextAction?.Invoke(dataBaseResult);//添加文本
            SendAction?.BeginInvoke(0, SendCallBack, isOpenDoor);

            string[] tmpIn = dataBaseResult[0].Split(' ');
            Rundata_InsertAction?.BeginInvoke(tmpIn[0], tmpIn[1], isOpenDoor ? 1 : 0, Passtime, null, null);//插入数据库

            string tmp = string.Empty;
            foreach (string v in dataBaseResult)
            {
                tmp += v + ",";
            }
            SetMessage?.Invoke(string.Format("LED Show {0}", tmp));
            _Log.logInfo.Info(string.Format("LED Show {0}", tmp));
        }

        ///// <summary>
        ///// 车辆进闸提示显示
        ///// </summary>
        ///// <param name=""></param>
        //private void LedShow(string[] Result)
        //{
        //    bool IsOpenDoor = false;
        //    DeleteScreen_DynamicAction?.Invoke(0);//删除显示屏
        //    AddScreen_DynamicAction(0);           //添加显示屏
        //    AddScreenDynamicAreaAction(1);        //添加动态区
        //    AddTextAction?.Invoke(Result);        //添加文本
        //    SendAction?.BeginInvoke(0, SendCallBack, IsOpenDoor);

        //    SetMessage?.Invoke(string.Format("LED Show {0}", Result[0]));
        //}

        /// <summary>
        /// 推送信息完成
        /// </summary>
        /// <param name="ar"></param>
        private void SendCallBack(IAsyncResult ar)
        {
            //var IsOpenDoor = (bool)ar.AsyncState;       //开闸回调传递参数
            SetMessage?.Invoke("LED信息推送回调完成");
            //if (IsOpenDoor)                             //是否开闸
            //{
            //    OpenDoorAction?.BeginInvoke(In_IP, PORT, In_ControllerSN, OpendoorCallBack, null);//查询到数据开闸
            //}
        }

        /// <summary>
        /// 开闸完成
        /// </summary>
        /// <param name="ar"></param>
        private void OpendoorCallBack(IAsyncResult ar)
        {
            SetMessage?.Invoke("开闸函数回调完成");
        }

        #endregion

        #region/公共

        /// <summary>
        /// 插入数据完成
        /// </summary>
        /// <param name="ar"></param>
        private void InsertCallBack(IAsyncResult ar)
        {            
            SetMessage?.Invoke("写入数据库完成");
        }

        #endregion

        #region//出闸
        /// <summary>
        /// 出闸车牌识别结果
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        internal void PlateResult(string ChIp, string ChLicesen, string ChColor, DateTime ChTime)
        {
            if(!string.IsNullOrEmpty(ChLicesen))
            {
                OpenDoorAction?.BeginInvoke(Out_IP, PORT, Out_ControllerSN, OpendoorCallBack, null);//出闸开闸
                SetOutLedMessageAction?.BeginInvoke(string.Format("{0} {1}", ChLicesen, Plate_Local_End_Message),SetOutLedCallBack,null);//LED显示
                Out_InsertDataBaseAction?.BeginInvoke(ChLicesen, ChTime,  1, InsertCallBack, null);//插入数据库
                Rundata_updateAction?.BeginInvoke(ChLicesen, ChTime,null,null);
            }
            else
            {
                SetOutLedMessageAction?.BeginInvoke(Working_NoOCRresult, SetOutLedCallBack, null);//LED显示
            }
        }

        /// <summary>
        /// 出闸LED显示完成
        /// </summary>
        /// <param name="ar"></param>
        private void SetOutLedCallBack(IAsyncResult ar)
        {
            _Timer.Change(TimeSpan.FromSeconds(6), TimeSpan.FromSeconds(0));
        }

        /// <summary>
        /// 恢复默认显示
        /// </summary>
        /// <param name="state"></param>
        private void OutLedDefaultShowCallBack(object state)
        {
            SetOutLedMessageAction?.Invoke(Plate_local_Message);
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    _Timer.Dispose();
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~Working() {
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
