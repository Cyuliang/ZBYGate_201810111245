using System;
using ZBYGate_Data_Collection.Log;

namespace ZBYGate_Data_Collection.LED
{
    class LED
    {
        public Action<string> SetMessage;

        private CLog _Log = new CLog();

        #region//控制板参数定义

        private const int CONTROLLER_BX_5E1 = 0x0154;
        private const int CONTROLLER_BX_5E2 = 0x0254;
        private const int CONTROLLER_BX_5E3 = 0x0354;
        private const int CONTROLLER_BX_5Q0P = 0x1056;
        private const int CONTROLLER_BX_5Q1P = 0x1156;
        private const int CONTROLLER_BX_5Q2P = 0x1256;
        private const int CONTROLLER_BX_6Q1 = 0x0166;
        private const int CONTROLLER_BX_6Q2 = 0x0266;
        private const int CONTROLLER_BX_6Q2L = 0x0466;
        private const int CONTROLLER_BX_6Q3 = 0x0366;
        private const int CONTROLLER_BX_6Q3L = 0x0566;

        private const int CONTROLLER_BX_5E1_INDEX = 0;
        private const int CONTROLLER_BX_5E2_INDEX = 1;
        private const int CONTROLLER_BX_5E3_INDEX = 2;
        private const int CONTROLLER_BX_5Q0P_INDEX = 3;
        private const int CONTROLLER_BX_5Q1P_INDEX = 4;
        private const int CONTROLLER_BX_5Q2P_INDEX = 5;
        private const int CONTROLLER_BX_6Q1_INDEX = 6;
        private const int CONTROLLER_BX_6Q2_INDEX = 7;
        private const int CONTROLLER_BX_6Q2L_INDEX = 8;
        private const int CONTROLLER_BX_6Q3_INDEX = 9;
        private const int CONTROLLER_BX_6Q3L_INDEX = 10;

        //==============================================================================
        // 返回状态代码定义
        private const int RETURN_ERROR_NOFIND_DYNAMIC_AREA = 0xE1; //没有找到有效的动态区域。
        private const int RETURN_ERROR_NOFIND_DYNAMIC_AREA_FILE_ORD = 0xE2; //在指定的动态区域没有找到指定的文件序号。
        private const int RETURN_ERROR_NOFIND_DYNAMIC_AREA_PAGE_ORD = 0xE3; //在指定的动态区域没有找到指定的页序号。
        private const int RETURN_ERROR_NOSUPPORT_FILETYPE = 0xE4; //不支持该文件类型。
        private const int RETURN_ERROR_RA_SCREENNO = 0xF8; //已经有该显示屏信息。如要重新设定请先DeleteScreen删除该显示屏再添加；
        private const int RETURN_ERROR_NOFIND_AREA = 0xFA; //没有找到有效的显示区域；可以使用AddScreenProgramBmpTextArea添加区域信息。
        private const int RETURN_ERROR_NOFIND_SCREENNO = 0xFC; //系统内没有查找到该显示屏；可以使用AddScreen函数添加显示屏
        private const int RETURN_ERROR_NOW_SENDING = 0xFD; //系统内正在向该显示屏通讯，请稍后再通讯；
        private const int RETURN_ERROR_OTHER = 0xFF; //其它错误；
        private const int RETURN_NOERROR = 0x00; //没有错误
        private const int RETURN_ERROR_NETWORK = 0xFE;//网络错误
        //==============================================================================

        private bool m_bSendBusy = false;//此变量在数据更新中非常重要，请务必保留。

        //添加屏幕
        private readonly int nControlType = Properties.Settings.Default.LED_nControlType;
        private readonly int nScreenNo = Properties.Settings.Default.LED_nScreenNo;
        private readonly int nWidth = Properties.Settings.Default.LED_nWidth;
        private readonly int nHeight = Properties.Settings.Default.LED_nHeight;
        public readonly string pSocketIP = Properties.Settings.Default.LED_pSocketIP;
        public readonly int nSocketPort = Properties.Settings.Default.LED_nSocketPort;

        //添加区域
        private readonly int nDYAreaID = Properties.Settings.Default.LED_nDYAreaID;
        private readonly int nTimeOut = Properties.Settings.Default.LED_nTimeOut;
        private readonly string nAreaX = Properties.Settings.Default.LED_nAreaX;
        private readonly string nAreaY = Properties.Settings.Default.LED_nAreaY;
        private readonly string nAreaWidth = Properties.Settings.Default.LED_nAreaWidth;
        private readonly string nAreaHeight = Properties.Settings.Default.LED_nAreaHeight;

        private readonly string LED_LogSHow = Properties.Settings.Default.LED_LogSHow;

        //添加文本
        private readonly int nFontSize = Properties.Settings.Default.LED_nFontSize;
        private readonly int nFontColor = Properties.Settings.Default.LED_nFontColor;
        private readonly int ResultFontColor = Properties.Settings.Default.LED_ResultFontColor;
        private readonly int nShowTime = Properties.Settings.Default.LED_nShowTime;

        //发送信息
        private readonly int nDelAllDYArea = Properties.Settings.Default.LED_nDelAllDYArea;
        private readonly string pDYAreaIDList = Properties.Settings.Default.LED_pDYAreaIDList;

        //动态区域
        private readonly string[] nAreaXs;
        private readonly string[] nAreaYs;
        private readonly string[] nAreaWidths;
        private readonly string[] nAreaHeights;

        private readonly string[] nAreaLogShow;//提示区
        private string[] Head =new string[]{ "车牌/柜号：", "是否准时：", "供应商：", "备案/预约：", "停靠区域/货台：", "处理结果：" };//显示头

        public LED()
        {
            nAreaXs = nAreaX.Split(',');
            nAreaYs = nAreaY.Split(',');
            nAreaWidths = nAreaWidth.Split(',');
            nAreaHeights = nAreaHeight.Split(',');

            nAreaLogShow = LED_LogSHow.Split(',');
        }

        #endregion

        /// <summary>
        /// 函数执行返回信息
        /// </summary>
        /// <param name="szfunctionName"></param>
        /// <param name="nResult"></param>
        public void GetErrorMessage(string szfunctionName, int nResult)
        {
            string szResult;
            DateTime dt = DateTime.Now;
            szResult = dt.ToString() + "---执行函数：" + szfunctionName + "---返回结果：";
            switch (nResult)
            {
                case RETURN_ERROR_NOFIND_DYNAMIC_AREA:
                    SetMessage?.Invoke(szResult + "没有找到有效的动态区域。\r\n");
                    _Log.logWarn.Warn(szResult + "没有找到有效的动态区域");
                    break;
                case RETURN_ERROR_NOFIND_DYNAMIC_AREA_FILE_ORD:
                    SetMessage?.Invoke(szResult + "在指定的动态区域没有找到指定的文件序号。\r\n");
                    _Log.logWarn.Warn(szResult + "在指定的动态区域没有找到指定的文件序号");
                    break;
                case RETURN_ERROR_NOFIND_DYNAMIC_AREA_PAGE_ORD:
                    SetMessage?.Invoke(szResult + "在指定的动态区域没有找到指定的页序号。\r\n");
                    _Log.logError.Error(szResult + "在指定的动态区域没有找到指定的页序号");
                    break;
                case RETURN_ERROR_NOSUPPORT_FILETYPE:
                    SetMessage?.Invoke(szResult + "动态库不支持该文件类型。\r\n");
                    _Log.logWarn.Warn(szResult + "动态库不支持该文件类型");
                    break;
                case RETURN_ERROR_RA_SCREENNO:
                    SetMessage?.Invoke(szResult + "系统中已经有该显示屏信息。如要重新设定请先执行DeleteScreen函数删除该显示屏后再添加。\r\n");
                    _Log.logWarn.Warn(szResult + "系统中已经有该显示屏信息。如要重新设定请先执行DeleteScreen函数删除该显示屏后再添加");
                    break;
                case RETURN_ERROR_NOFIND_AREA:
                    SetMessage?.Invoke(szResult + "系统中没有找到有效的动态区域；可以先执行AddScreenDynamicArea函数添加动态区域信息后再添加。\r\n");
                    _Log.logWarn.Warn(szResult + "系统中没有找到有效的动态区域；可以先执行AddScreenDynamicArea函数添加动态区域信息后再添加");
                    break;
                case RETURN_ERROR_NOFIND_SCREENNO:
                    SetMessage?.Invoke(szResult + "系统内没有查找到该显示屏；可以使用AddScreen函数添加该显示屏。\r\n");
                    _Log.logWarn.Warn(szResult + "系统内没有查找到该显示屏；可以使用AddScreen函数添加该显示屏");
                    break;
                case RETURN_ERROR_NOW_SENDING:
                    SetMessage?.Invoke(szResult + "系统内正在向该显示屏通讯，请稍后再通讯。\r\n");
                    _Log.logWarn.Warn(szResult + "系统内正在向该显示屏通讯，请稍后再通讯");
                    break;
                case RETURN_ERROR_OTHER:
                    SetMessage?.Invoke(szResult + "其它错误。\r\n");
                    _Log.logWarn.Warn(szResult + "其它错误");
                    break;
                case RETURN_NOERROR:
                    SetMessage?.Invoke(szResult + "函数执行成功。\r\n");
                    _Log.logInfo.Info(szResult + "函数执行成功");
                    break;
                case RETURN_ERROR_NETWORK:
                    SetMessage?.Invoke(szResult + "网络通讯错误。\r\n");
                    _Log.logWarn.Warn(szResult + "网络通讯错误");
                    break;
            }
        }

        /// <summary>
        /// 初始化动态库
        /// </summary>
        public void Initialize(int i)
        {
            try
            {
                int nResult = SafeNativeMethods.Initialize();
                GetErrorMessage("Initialize", nResult);
            }
            catch (Exception ex)
            {
                _Log.logError.Error("Initialize Error", ex);                
            }
        }

        /// <summary>
        /// 添加显示屏
        /// </summary>
        public void AddScreen_Dynamic(int i)
        {
            try
            {
                int nResult = SafeNativeMethods.AddScreen_Dynamic(nControlType, nScreenNo, 2, nWidth, nHeight,
                            4, 1, "COM1", 57600, pSocketIP, nSocketPort, 0, 0, "", "", "112.65.245.174", 6055, "", "","");
                GetErrorMessage("执行AddScreen函数,", nResult);
            }
            catch (Exception ex)
            {
                _Log.logError.Error("AddScreen Error", ex);
            }
        }

        /// <summary>
        /// 添加动态区
        /// </summary>
        public void AddScreenDynamicArea(int j)
        {
            try
            {
                int nResult = -1;

                //添加一个动态区
                if(j==1)
                {
                    nResult = SafeNativeMethods.AddScreenDynamicArea(nScreenNo, 0, 2, nTimeOut, 0, "", 0,
                                int.Parse(nAreaLogShow[0]), int.Parse(nAreaLogShow[1]), int.Parse(nAreaLogShow[2]), int.Parse(nAreaLogShow[3]),
                                255, 0, 255, 7, 6, 1);
                    GetErrorMessage(string.Format("执行AddScreenDynamicArea函数,添加 {0} X:{1} Y:{2} W:{3} H:{4} 动态区域",
                                 0, nAreaLogShow[0], nAreaLogShow[1], nAreaLogShow[2], nAreaLogShow[3]), nResult);
                }
                else
                {
                    for (int i = 0; i < nDYAreaID; i++)
                    {                        
                        nResult = SafeNativeMethods.AddScreenDynamicArea(nScreenNo, i, 2, nTimeOut, 0, "", 1/*异步节目暂停播放*/,
                                     int.Parse(nAreaXs[i]), int.Parse(nAreaYs[i]), int.Parse(nAreaWidths[i]), int.Parse(nAreaHeights[i]),
                                     255, 0, 255, 7, 6, 1);
                        GetErrorMessage(string.Format("执行AddScreenDynamicArea函数,添加 {0} X:{1} Y:{2} W:{3} H:{4} 动态区域",
                                     i, nAreaXs[i], nAreaYs[i], nAreaWidths[i], nAreaHeights[i]), nResult);
                    }
                }
            }
            catch (Exception ex)
            {
                _Log.logError.Error("AddScreenDynamicArea Error", ex);
            }
        }

        /// <summary>
        /// 添加动态区域文本
        /// </summary>
        public void AddScreenDynamicAreaText(string[] pTexts)
        {
            int nResult = -1;
            int i = 0;
            int nColor = nFontColor;

            foreach (string str in pTexts)
            {
                try
                {
                    if (i==5)
                    {
                        nColor = ResultFontColor;
                    }
                    nResult = SafeNativeMethods.AddScreenDynamicAreaText(nScreenNo, i, Head[i]+str, 0, 0, "宋体", nFontSize, 0, nColor, 2, 8, nShowTime);
                    GetErrorMessage(string.Format("执行AddScreenDynamicAreaText函数,添加 {0} 区域，文本 {1}", i, str), nResult);
                    i++;
                }
                catch (AccessViolationException ex)
                {
                    _Log.logError.Error("AddScreenDynamicAreaText Error", ex);
                }
            }
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        public void SendDynamicAreasInfoCommand(int i)
        {
            try
            {
                if (m_bSendBusy == false)
                {
                    m_bSendBusy = true;
                    if(i==1)
                    {
                        int nResult = SafeNativeMethods.SendDynamicAreasInfoCommand(nScreenNo, 1, "");//如果发送多个动态区域，动态区域编号间用","隔开。
                        GetErrorMessage("执行SendDynamicAreasInfoCommand函数, ", nResult);
                    }
                    else
                    {
                        int nResult = SafeNativeMethods.SendDynamicAreasInfoCommand(nScreenNo, nDelAllDYArea, pDYAreaIDList);//如果发送多个动态区域，动态区域编号间用","隔开。
                        GetErrorMessage("执行SendDynamicAreasInfoCommand函数, ", nResult);
                    }
                    m_bSendBusy = false;
                }
            }
            catch (Exception ex)
            {
                _Log.logError.Error("SendDynamicAreasInfoCommand Error", ex);
            }
        }

        /// <summary>
        /// 删除显示屏
        /// </summary>
        /// <param name="i"></param>
        public void DeleteScreen_Dynamic(int i)
        {
            try
            {
                int nResult = SafeNativeMethods.DeleteScreen_Dynamic(nScreenNo);
                GetErrorMessage("DeleteScreen_Dynamic", nResult);
            }
            catch (Exception ex)
            {
                _Log.logError.Error("DeleteScreen_Dynamic", ex);
            }
        }

        /// <summary>
        /// 释放动态库
        /// </summary>
        public void Uninitialize(int i)
        {
            try
            {
                int nResult = SafeNativeMethods.Uninitialize();
                GetErrorMessage("Uninitialize", nResult);
            }
            catch (Exception ex)
            {
                _Log.logError.Error("Uninitialize Error", ex);
            }
        }
    }
}
