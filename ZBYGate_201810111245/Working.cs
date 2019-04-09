using AxVeconclientProj;
using System;
using System.Linq;

namespace ZBYGate_Data_Collection
{
    class Working : IDisposable//远程服务器和本地数据库同步查询
    {
        #region//统计数据值初始化
        /// <summary>
        /// 进闸统计
        /// </summary>
        public static int IN = 0;
        /// <summary>
        /// 出闸统计
        /// </summary>
        public static int OUT = 0;
        /// <summary>
        /// 结余统计
        /// </summary>
        public static int BALANCE = 0;
        #endregion

        #region //对象初始
        private Log.CLog _Log = new Log.CLog();
        /// <summary>
        /// 运行消息委托
        /// </summary>
        public Action<string> SetMessage_Action;
        /// <summary>
        /// 统计信息回写到主界面委托
        /// </summary>
        public Action<string, string, string> SetStatisticsLable_Action;
        #endregion

        #region //定时器
        /// <summary>
        /// 开机回写统计数据到界面
        /// </summary>
        private System.Threading.Timer _Timer_Start = null;
        /// <summary>
        /// 定时更新LED显示
        /// </summary>
        private System.Threading.Timer _Timer_LEDSHow = null;
        /// <summary>
        /// 定时检测时间点，创建数据库
        /// </summary>
        //private System.Threading.Timer _Timer_InsertTraffic = null;
        #endregion

        #region//集装箱
        private string Containernumber = string.Empty;//集装箱号
        private string NewLpn = string.Empty;//空车车牌
        private string UpdateLpn = string.Empty;//重车车牌
        private string Plate = string.Empty;//身份证使用车牌数据
        private string Con = string.Empty;//身份证使用箱号数据
        private DateTime Passtime=DateTime.Now;//过车时间
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

        #region //统计数据库
        public Func<int, int, int, DateTime,int> StatisticsDataBaseUpdate;//更新统计数据库
        public Action<DateTime,bool> StatisticsDataBaseInsert;//插入统计数据
        public Func<DateTime, string[]> StatisticsDateBaseSelect;//查询统计数据库，回写到参数和界面；
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
        public Func<string, string, string, string> HttpPostInAction;//远程查询数据
        public Action<string, string> HttpPostOutAction;//出闸远端交换数据
        public Func<string, string[]> HttpJsonSplitAction;//解析Json
        #endregion

        #region //配置文件变量
        private readonly string Working_NoOCRresult = Properties.Settings.Default.Working_NoOCRresult;
        private readonly string Working_NoDataBaseResult = Properties.Settings.Default.Working_NoDataBaseResult;
        private readonly string Http_Status = Properties.Settings.Default.Http_Status;
        private readonly bool HttpSwitch = Properties.Settings.Default.Http_switch;
        private readonly string Plate_Local_End_Message = Properties.Settings.Default.Plate_Local_End_Message;
        private readonly string Plate_local_Message = Properties.Settings.Default.Plate_Local_Message;
        private readonly string Led_Log = Properties.Settings.Default.Led_Log;
        private readonly bool WorkIng_ReadID = Properties.Settings.Default.WorkIng_ReadCards;
        private readonly string Http_HttpNull = Properties.Settings.Default.Http_HttpNull;

        private DateTime EndTime = Convert.ToDateTime("23:30:00");//创建新纪录时间点
        #endregion

        public Working()
        {
            _Timer_Start = new System.Threading.Timer(InsertTraffic_timer_CallBack,null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(0));
            _Timer_LEDSHow = new System.Threading.Timer(OutLedDefaultShowCallBack, null, TimeSpan.FromSeconds(6), TimeSpan.FromSeconds(0));
            //_Timer_InsertTraffic = new System.Threading.Timer(TimerInsertTrafficCallBack, _Timer_InsertTraffic, TimeSpan.FromSeconds(60), TimeSpan.FromSeconds(60));
        }

        #region //traffic数据库

        /// <summary>
        /// 启动创建traffic数据库一次今天记录
        /// </summary>
        /// <param name="state"></param>
        public void InsertTraffic_timer_CallBack(object state)
        {
            StatisticsDateBaseSelect?.BeginInvoke(DateTime.Now, StatisticsDateBase_Select_CallBack, null);
        }

        /// <summary>
        /// 读取统计数据库数据回写到参数回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void StatisticsDateBase_Select_CallBack(IAsyncResult ar)
        {
            string[] Retusl = new string[3] {"0","0","0"};
            try
            {
                //可能发生错误，未处理
                Retusl = StatisticsDateBaseSelect?.EndInvoke(ar);
                if(string.IsNullOrEmpty(Retusl[0]))
                {
                    //查询不到数据，创建数据
                    StatisticsDataBaseInsert?.BeginInvoke(DateTime.Now, true, StatisticsDataBase_Insert_CallBack, null);
                }
            }
            catch
            {
                ;
            }

            Working.IN =int.Parse( Retusl[0]);
            Working.OUT =int.Parse( Retusl[1]);
            Working.BALANCE =int.Parse( Retusl[2]);

            SetStatisticsLable_Action?.BeginInvoke(Working.IN.ToString(), Working.OUT.ToString(), Working.BALANCE.ToString(), null, null);

            SetMessage_Action?.Invoke("StatisticsDateBaseSelect[回调|查询|读取统计数据库数据回写到参数回调函数]");
        }


        //private DateTime ZeroPastTime;
        ///// <summary>
        ///// 循环定时检测
        ///// 超过23:00插入traffic数据库新记录
        ///// </summary>
        ///// <param name="state"></param>
        //private void TimerInsertTrafficCallBack(object state)
        //{
        //    if (DateTime.Compare(DateTime.Now, EndTime) > 0)
        //    {
        //        //判断日期有没有变化，有变化代表隔天，清零数据。
        //        if (DateTime.Now.Day != ZeroPastTime.Day)
        //        {
        //            Working.IN = 0;
        //            Working.OUT = 0;
        //            Working.BALANCE = 0;
        //        }
        //        else
        //        {
        //            ZeroPastTime = DateTime.Now;
        //        }

        //        StatisticsDataBaseInsert?.BeginInvoke(DateTime.Now, false, StatisticsDataBase_Insert_CallBack, null);
        //    }
        //}

        /// <summary>
        /// 插入statistics数据库新记录
        /// </summary>
        /// <param name="ar"></param>
        private void StatisticsDataBase_Insert_CallBack(IAsyncResult ar)
        {
            SetMessage_Action?.Invoke("StatisticsDataBaseInsert[回调|插入|插入statistics数据库新记录]");
        }

        #endregion

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
        }

        /// <summary>
        /// 空车结果事件
        /// </summary>
        /// <param name="obj"></param>
        internal void NewLpnResult(IVECONclientEvents_OnNewLPNEventEvent obj)
        {
            NewLpn = obj.lPN;
            Plate = obj.lPN;
            Passtime = obj.triggerTime;
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
            Passtime = obj.triggerTime;
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
            SetMessage_Action?.Invoke("SelectLpnCon[函数|Log|识别完成，数据开始处理]");

            //优先置空车牌和箱号
            NewLpn = string.Empty;
            UpdateLpn = string.Empty;
            Containernumber = string.Empty;

            Calculation_In();//统计入闸

            if (Lpn != null || Container != null)//字段其中一个不为空就查询服务器
            {
                SetMessage_Action?.Invoke("SelectDataBase[函数|Log|开始查询本地数据库]");

                string[] Head = { Lpn, Container };                                                //组合显示车牌和箱号
                SelectDataBase?.BeginInvoke(Lpn, Container, "", One_SelectDataBaseCallBack, Head);//查询数据库  
            }
            else//没有识别到数据
            {
                bool IsOpenDoor = false;                             //是否开闸
                var LedShowDataResult = new string[] { "*", "*", "*", "*", "*", Working_NoOCRresult };
                LedShow(LedShowDataResult, IsOpenDoor);
            } 
        }
        #endregion

        #region //分析数据
        /// <summary>
        /// 查询本地数据库
        /// </summary>
        /// <param name="ar"></param>
        private void One_SelectDataBaseCallBack(IAsyncResult ar)
        {            
            bool IsOpenDoorD = false;                                             //是否开闸
            var Head = (string[])ar.AsyncState;                                   //车牌和箱号   
            var LedShowDataResult = SelectDataBase.EndInvoke(ar);                 //查询本地数据库回调返回数据

            if (LedShowDataResult.All(string.IsNullOrEmpty))                      //数据库记录为空
            {
                SetMessage_Action?.Invoke("One_SelectDataBaseCallBack[函数|Log|本地数据库没有记录]");

                SetMessage_Action?.Invoke("HttpPostInAction[函数|Log|开始查询后台服务器]");
                //查询远端数据库
                HttpPostInAction?.BeginInvoke(Passtime.ToString("yyyyMMddHHmmss"), Head[0], Head[1], Tow_SelectHttpCallBack, Head);
            }
            else                                                                  //查询到数据库记录
            {
                LedShowDataResult[0] = string.Format("{0}/{1}", Head[0], Head[1]);//设置显示识别到的箱号和车牌
                LedShowDataResult[1] = "准时";                                    //准时字段
                LedShowDataResult[5] = Http_Status;                               //提示进闸字段
                IsOpenDoorD = true;
                LedShow(LedShowDataResult, IsOpenDoorD);
            }
        }

        /// <summary>
        /// 查询后台数据库回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void Tow_SelectHttpCallBack(IAsyncResult ar)
        {
            SetMessage_Action?.Invoke("Tow_SelectHttpCallBack[函数|Log|查询后台服务器完成]");

            bool IsOpenDoorH = false;                                               //是否开闸
            var Head = (string[])ar.AsyncState;                                     //车牌和箱号
            var HttpResult = HttpPostInAction.EndInvoke(ar);                        //http请求数据回调函数返回数据  

            if (HttpResult != null)
            {
                var LedShowDataResult = HttpJsonSplitAction?.Invoke(HttpResult);    //分析Json数据
                //如果后台服务器返回正常，可以不用替换
                LedShowDataResult[0] = string.Format("{0}/{1}", Head[0], Head[1]);  //设置显示识别到的箱号和车牌
                                                                                    //防止JOSN返回数据错误，退换回车牌和箱号
                if (LedShowDataResult[1] == "Y")                                    //是否开闸
                {
                    IsOpenDoorH = true;
                    LedShowDataResult[1] = "准时";                                  //替换字段
                }
                
                LedShow(LedShowDataResult,IsOpenDoorH);                             //暂时不处理身份证，显示数据

                # region 没有数据，读取身份证                
                if (LedShowDataResult[1]=="N")                                       //没有数据，读取身份证
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
                #endregion

            }
            else
            {
                var LedShowDataResult = new string[] { string.Format("{0}/{1}", Head[0], Head[1]), "*", "*", "*", "*", Http_HttpNull };
                LedShow(LedShowDataResult, IsOpenDoorH);

                SetMessage_Action?.Invoke("Tow_SelectHttpCallBack[函数|Log|后台服务器没有返回数据]");
            }
        }
        #endregion

        #region//读取分析身份证
        /// <summary>
        /// 身份证读取回调
        /// </summary>
        /// <param name="ar"></param>
        private void ForDoneCallBack(IAsyncResult ar)
        {
            SetMessage_Action?.Invoke("CVRForReadAction[回调|查询|查询本地身份证数据库回调函数]");

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
                LedShowDataResult = new string[] { string.Format("{0}/{1}", Head[0], Head[1]), "*", "*", "*", "*", Working_NoNumberResult };
            }
            else//查询到数据库记录
            {
                In_UpdateDataBaseAction?.BeginInvoke(LedShowDataResult[0],Passtime,0, In_UpdateDataBaseActionCallBack, null);
                LedShowDataResult[0] = string.Format("{0}/{1}", Head[0], Head[1]);       //设置显示识别到的箱号和车牌
                LedShowDataResult[1] = "准时";     //准时字段
                LedShowDataResult[5] = Http_Status;//提示进闸
                IsOpenDoorD = true;
            }

            SetMessage_Action?.Invoke("SelectDataBase[回调|查询|查询本地身份证数据库回调函数]");

            LedShow(LedShowDataResult, IsOpenDoorD);            //推送LED
        }

        /// <summary>
        /// 更新入闸RunData数据库身份证信息回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void In_UpdateDataBaseActionCallBack(IAsyncResult ar)
        {
            SetMessage_Action?.Invoke("In_UpdateDataBaseAction[回调|查询|更新入闸RunData数据库身份证信息回调函数]");
        }

        #endregion

        #region //LED Show
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

            SendAction?.BeginInvoke(0, SendCallBack, null);

            try//防止非法数据，导致索引超出
            {
                SetMessage_Action?.Invoke("LedShow[函数|Log|开始插入入闸数据到Run数据库]");
                string[] tmpIn = dataBaseResult[0].Split('/');
                Rundata_InsertAction?.BeginInvoke(tmpIn[0], tmpIn[1], isOpenDoor ? 1 : 0, Passtime, Rundata_InsertCallBack, null);//插入数据库
            }
            catch (Exception)
            {; }

            string tmp = string.Empty;
            foreach (string v in dataBaseResult)
            {
                tmp += v + ",";
            }
            SetMessage_Action?.Invoke(string.Format("LedShow[函数|Message|LED Show {0}]",tmp));
            _Log.logInfo.Info(string.Format("LedShow[函数|Log|LED Show {0}]", tmp));
        }

        /// <summary>
        /// 插入入闸数据到RunData数据库回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void Rundata_InsertCallBack(IAsyncResult ar)
        {
            SetMessage_Action?.Invoke("Rundata_InsertAction[回调|插入|插入入闸数据到RunData数据库回调函数]");
        }

        /// <summary>
        /// 入闸LED消息推送回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void SendCallBack(IAsyncResult ar)
        {
            SetMessage_Action?.Invoke("SendAction[回调|推送|入闸LED消息推送回调函数]");
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
            Calculation_Out();//统计出闸

            if (!string.IsNullOrEmpty(ChLicesen))//车牌不为空
            {
                SetMessage_Action?.Invoke("PlateResult[函数|Log|开始插入出闸数据到Run数据库]");

                OpenDoorAction?.BeginInvoke(Out_IP, PORT, Out_ControllerSN, OpendoorCallBack, null);//出闸开门
                SetOutLedMessageAction?.BeginInvoke(string.Format("{0} {1}", ChLicesen, Plate_Local_End_Message),SetOutLedCallBack,null);//LED推送显示
                Out_InsertDataBaseAction?.BeginInvoke(ChLicesen, ChTime,  1, InsertCallBack, null);//插入OutData数据库
                Rundata_updateAction?.BeginInvoke(ChLicesen, ChTime, Rundata_updateActionCallBack, null);//插入RunData数据库
                HttpPostOutAction?.BeginInvoke(ChTime.ToString("yyyyMMddHHmmss"), ChLicesen, HttpPostOutActionCallBack,null);//发送出闸数据到远端服务器
            }
            else//车牌为空直接显示倒车信息。
            {
                SetOutLedMessageAction?.BeginInvoke(Working_NoOCRresult, SetOutLedCallBack, null);//LED显示
            }
        }

        /// <summary>
        /// 出闸开闸回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void OpendoorCallBack(IAsyncResult ar)
        {
            SetMessage_Action?.Invoke("OpenDoorAction[回调|动作|出闸开闸回调函数]");

        }

        /// <summary>
        /// 更新RunData数据库回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void Rundata_updateActionCallBack(IAsyncResult ar)
        {
            SetMessage_Action?.Invoke("Rundata_updateAction[回调|更新|更新出闸数据到RunData数据库回调函数]");
        }

        /// <summary>
        /// 插入OutData数据库回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void InsertCallBack(IAsyncResult ar)
        {
            SetMessage_Action?.Invoke("Out_InsertDataBaseAction[回调|插入|插入出闸数据到OutData数据库回调函数]");
        }

        /// <summary>
        /// 推送出闸数据到后台服务器回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void HttpPostOutActionCallBack(IAsyncResult ar)
        {
            SetMessage_Action?.Invoke("HttpPostOutAction[回调|查询|推送出闸数据到后台服务器回调函数]");
        }

        /// <summary>
        /// 出闸LED显示回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void SetOutLedCallBack(IAsyncResult ar)
        {
            _Timer_LEDSHow.Change(TimeSpan.FromSeconds(6), TimeSpan.FromSeconds(0));
        }

        /// <summary>
        /// 恢复出闸默认显示
        /// </summary>
        /// <param name="state"></param>
        private void OutLedDefaultShowCallBack(object state)
        {
            SetOutLedMessageAction?.BeginInvoke(Plate_local_Message,null,null);
            SetMessage_Action?.Invoke("SetOutLedCallBack[定时器|推送|恢复出闸LED默认显示]");
        }

        #endregion

        #region//处理统计数据库
        /// <summary>
        /// 更新统计值进闸
        /// </summary>
        private void Calculation_In()
        {
            Working.IN = Working.IN + 1;
            Working.BALANCE = Working.IN - Working.OUT;

            StatisticsDataBaseUpdate?.BeginInvoke(Working.BALANCE, Working.IN, 0, DateTime.Now, Calculation_InCallBack,null);

            //if (StatisticsDataBaseUpdate?.Invoke(Working.BALANCE, Working.IN, 0, DateTime.Now)!=1)
            //{
            //    //查询不到数据，创建数据
            //    StatisticsDataBaseInsert?.BeginInvoke(DateTime.Now, true, StatisticsDataBase_Insert_CallBack, null);
            //    Working.IN = 1;
            //    Working.OUT = 0;
            //    Working.BALANCE = 1;
            //}

            ////写统计数据到主界面
            //SetStatisticsLable_Action?.BeginInvoke(Working.IN.ToString(), Working.OUT.ToString(), Working.BALANCE.ToString(), null, null);

        }

        /// <summary>
        /// 更新统计值出闸
        /// </summary>
        private void Calculation_Out()
        {
            Working.OUT = Working.OUT + 1;
            Working.BALANCE = Working.IN - Working.OUT;

            StatisticsDataBaseUpdate?.BeginInvoke(Working.BALANCE, 0,Working.OUT ,DateTime.Now, Calculation_OutCallBack, null);

            //if (StatisticsDataBaseUpdate?.Invoke(Working.BALANCE, 0, Working.OUT, DateTime.Now) != 1)
            //{
            //    //查询不到数据，创建数据
            //    StatisticsDataBaseInsert?.BeginInvoke(DateTime.Now, true, StatisticsDataBase_Insert_CallBack, null);
            //    Working.IN = 0;
            //    Working.OUT = 1;
            //    Working.BALANCE = -1;
            //}

            ////写统计数据到主界面
            //SetStatisticsLable_Action?.BeginInvoke(Working.IN.ToString(), Working.OUT.ToString(), Working.BALANCE.ToString(),null,null);
        }

        /// <summary>
        /// 统计出闸回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void Calculation_OutCallBack(IAsyncResult ar)
        {

            if(1!= StatisticsDataBaseUpdate.EndInvoke(ar))
            {
                //查询不到数据，创建数据
                StatisticsDataBaseInsert?.BeginInvoke(DateTime.Now, true, StatisticsDataBase_Insert_CallBack, null);
                Working.IN = 0;
                Working.OUT = 1;
                Working.BALANCE = -1;
            }

            //写统计数据到主界面
            SetStatisticsLable_Action?.BeginInvoke(Working.IN.ToString(), Working.OUT.ToString(), Working.BALANCE.ToString(), null, null);

            SetMessage_Action?.Invoke("StatisticsDataBaseUpdate[回调|更新|统计出闸数据库回调函数完成]");
        }

        /// <summary>
        /// 统计进闸回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void Calculation_InCallBack(IAsyncResult ar)
        {           
            if(1 != StatisticsDataBaseUpdate.EndInvoke(ar))
            {
                //查询不到数据，创建数据
                StatisticsDataBaseInsert?.BeginInvoke(DateTime.Now, true, StatisticsDataBase_Insert_CallBack, null);
                Working.IN = 1;
                Working.OUT = 0;
                Working.BALANCE = 1;          
            }
            //写统计数据到主界面
            SetStatisticsLable_Action?.BeginInvoke(Working.IN.ToString(), Working.OUT.ToString(), Working.BALANCE.ToString(), null, null);

            SetMessage_Action?.Invoke("StatisticsDataBaseUpdate[回调|更新|统计入闸数据库回调函数完成]");
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
                    _Timer_LEDSHow.Dispose();
                    //_Timer_InsertTraffic.Dispose();
                    _Timer_Start.Dispose();
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
