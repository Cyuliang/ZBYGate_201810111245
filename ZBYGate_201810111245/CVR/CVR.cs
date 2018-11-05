﻿using System;
using ZBYGate_201810111245.Log;

namespace ZBYGate_201810111245.CVR
{
    class CVR
    {
        private CLog _Log = new CLog();
        private volatile bool STATE;

        public Action<byte[], byte[], byte[], byte[], byte[], byte[], byte[], byte[], byte[]> FillDataActive;
        public Action<byte[], int> FillDataBmpActive;
        public Action<string> SetMessageAction;

        public CVR()
        {
            STATE = false;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitComm(int i)
        {
            try
            {
                int ret = SafeNativeMethods.CVR_InitComm(Properties.Settings.Default.CVR_Com);
                if (ret == 1)
                {
                    _Log.logInfo.Info("CVR InitComm Success");
                    SetMessageAction?.Invoke("CVR InitComm Success");
                }
                else
                {
                    _Log.logWarn.Warn("CVR InitComm Fail");
                    SetMessageAction?.Invoke("CVR InitComm Fail");
                }
            }
            catch (Exception ex)
            {
                _Log.logError.Error("CVR InitComm error", ex);
                SetMessageAction?.Invoke("CVR InitComm error");
            }
        }

        /// <summary>
        /// 读卡
        /// </summary>
        public void Authenticate(int i)
        {
            try
            {
                //寻卡
                int authenticate = SafeNativeMethods.CVR_Authenticate();
                if (authenticate == 1)
                {
                    //读卡
                    int readContent = SafeNativeMethods.CVR_Read_FPContent();
                    if (readContent == 1)
                    {
                        _Log.logInfo.Info("CVR Read Cards Success");
                        SetMessageAction?.Invoke("CVR Read Cards Success");
                        FillData();
                    }
                    else
                    {
                        _Log.logWarn.Warn("CVR Read Cards Faile");
                        SetMessageAction?.Invoke("CVR Read Cards Faile");
                    }
                }
                else
                {
                    _Log.logWarn.Warn("CVR Authenticate error");
                    SetMessageAction?.Invoke("CVR Authenticate error");
                }
            }
            catch (Exception ex)
            {
                _Log.logError.Error("CVR Read Data Error", ex);
                SetMessageAction?.Invoke("CVR Read Data Error");
            }
        }

        /// <summary>
        /// 循环读卡状态
        /// </summary>
        /// <param name="state"></param>
        public void GetStarted(bool state)
        {
            STATE = state;
        }

        /// <summary>
        /// 无限循环读卡
        /// </summary>
        /// <param name="state"></param>
        public void AuthenticateWhile(int i)
        {
            int j = 0;
            while (STATE)
            {
                int authenticate = SafeNativeMethods.CVR_Authenticate();
                if (authenticate == 1)
                {
                    int readContent = SafeNativeMethods.CVR_Read_FPContent();
                    if (readContent == 1)
                    {
                        _Log.logInfo.Info("CVR While Read Cards Success");
                        SetMessageAction?.Invoke("CVR While Read Cards Success");
                        FillData();
                    }
                }
                j++;
                System.Threading.Thread.Sleep(2000);
                SetMessageAction?.Invoke(string.Format("CVR While Read Cards {0:d}次",j));
            }
            SetMessageAction?.Invoke("CVR While Read Success");
        }

        /// <summary>
        /// 定时循环读取
        /// </summary>
        public void AuthenticateFor(int i)
        {
            STATE = false;//停止无限循环读取
            int j = 0;
            while (j < Properties.Settings.Default.CVR_Read_While && (!STATE))
            {
                int authenticate = SafeNativeMethods.CVR_Authenticate();
                if (authenticate == 1)
                {
                    int readContent = SafeNativeMethods.CVR_Read_FPContent();
                    if (readContent == 1)
                    {
                        _Log.logInfo.Info("CVR Read For Cards Success");
                        SetMessageAction?.Invoke("CVR Read For Cards Success");
                        FillData();
                        break;
                    }
                }
                j++;
                System.Threading.Thread.Sleep(2000);
                SetMessageAction?.Invoke(string.Format("Wait CVR For Read Cards {0:d}次",j));
            }
            SetMessageAction?.Invoke("While CVR For Read Success");
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void CloseComm(int i)
        {
            if (SafeNativeMethods.CVR_CloseComm() == 1)
            {
                _Log.logInfo.Info("CVR Close Success");
                SetMessageAction?.Invoke("CVR Close Success");
            }
            else
            {
                SetMessageAction?.Invoke("CVR Close Error");
            }
        }

        /// <summary>
        /// 数据
        /// </summary>
        public void FillData()
        {
            try
            {
                byte[] imgData = new byte[40960];
                int Imglength = 40960;
                SafeNativeMethods.GetBMPData(ref imgData[0], ref Imglength);

                byte[] name = new byte[128];
                int length = 128;
                SafeNativeMethods.GetPeopleName(ref name[0], ref length);

                byte[] cnName = new byte[128];
                length = 128;
                SafeNativeMethods.GetPeopleChineseName(ref cnName[0], ref length);

                byte[] number = new byte[128];
                length = 128;
                SafeNativeMethods.GetPeopleIDCode(ref number[0], ref length);

                byte[] peopleNation = new byte[128];
                length = 128;
                SafeNativeMethods.GetPeopleNation(ref peopleNation[0], ref length);

                byte[] peopleNationCode = new byte[128];
                length = 128;
                SafeNativeMethods.GetNationCode(ref peopleNationCode[0], ref length);

                byte[] validtermOfStart = new byte[128];
                length = 128;
                SafeNativeMethods.GetStartDate(ref validtermOfStart[0], ref length);

                byte[] birthday = new byte[128];
                length = 128;
                SafeNativeMethods.GetPeopleBirthday(ref birthday[0], ref length);

                byte[] address = new byte[128];
                length = 128;
                SafeNativeMethods.GetPeopleAddress(ref address[0], ref length);

                byte[] validtermOfEnd = new byte[128];
                length = 128;
                SafeNativeMethods.GetEndDate(ref validtermOfEnd[0], ref length);

                byte[] signdate = new byte[128];
                length = 128;
                SafeNativeMethods.GetDepartment(ref signdate[0], ref length);

                byte[] sex = new byte[128];
                length = 128;
                SafeNativeMethods.GetPeopleSex(ref sex[0], ref length);

                byte[] samid = new byte[128];
                SafeNativeMethods.CVR_GetSAMID(ref samid[0]);

                bool bCivic = true;
                byte[] certType = new byte[32];
                length = 32;
                SafeNativeMethods.GetCertType(ref certType[0], ref length);

                string strType = System.Text.Encoding.ASCII.GetString(certType);
                int nStart = strType.IndexOf("I");
                if (nStart != -1) bCivic = false;

                _Log.logInfo.Info(string.Format("CVR Data Name：{0}   Number：{1}", System.Text.Encoding.GetEncoding("GB2312").GetString(name), System.Text.Encoding.GetEncoding("GB2312").GetString(number).Replace("\0", "").Trim()));
                SetMessageAction?.Invoke(string.Format("CVR Data Name：{0}   Number：{1}", System.Text.Encoding.GetEncoding("GB2312").GetString(name), System.Text.Encoding.GetEncoding("GB2312").GetString(number).Replace("\0", "").Trim()));

                if (bCivic)
                {
                    FillDataBmpActive?.Invoke(imgData, Imglength);
                    FillDataActive?.Invoke(name, sex, peopleNation, birthday, number, address, signdate, validtermOfStart, validtermOfEnd);
                }
                STATE = false;
            }
            catch (Exception ex)
            {
                _Log.logError.Error("CVR Read Data error", ex);
                SetMessageAction?.Invoke("CVR Read Data error");
            }
        }
    }
}
