using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ZBYGate_Data_Collection.Https
{
    class CHttp
    {

        private Log.CLog _Log = new Log.CLog();
        public Action<string> SetMessageAction;

        private readonly string http = Properties.Settings.Default.Http_www;
        private readonly string eqid = Properties.Settings.Default.Http_eqId;
        private readonly int HttpTimeOut = Properties.Settings.Default.Http_HttpTimeOut;
        private readonly int HttpReadWriteTimeout = Properties.Settings.Default.Http_HttpReadWriteTimeout;
        private readonly string Http_NoStatus = Properties.Settings.Default.Http_NoStatus;
        private readonly string Http_Status = Properties.Settings.Default.Http_Status;

        public CHttp()
        {
            ServicePointManager.DefaultConnectionLimit = 50;
        }

        private  Dictionary<string, string> ArriveParkJsonDict = new Dictionary<string, string>();
        /// <summary>
        /// 华为后端入闸数据交换(入园)
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Time"></param>
        /// <param name="Plate"></param>
        /// <param name="Container"></param>
        /// <returns></returns>
        public string SetInJosn( string Time, string Plate, string Container)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(http);
            request.Proxy = null;
            request.Method = "POST";
            request.Timeout = HttpTimeOut;
            request.ReadWriteTimeout = HttpReadWriteTimeout;
            request.ContentType = "application/json";/*x-www-form-urlencoded";*/            

            ////////////////////////////////////////201903301515
            ArriveParkJsonDict["eqId"] = eqid;

            string[] TestType = new string[] { };
            if (!string.IsNullOrEmpty(Container))//不为null才分割数据
            {
                TestType = Container.Split('|');
            }
            else
            {
                SetMessageAction?.Invoke(string.Format("DEBUG TestType 索引超出,测试使用"));
            }

            if (TestType.Length==2)
            {
                switch (int.Parse(TestType[1]))
                {
                    case 3:
                        ArriveParkJsonDict["ArriveParkTime"] = Time;
                        break;
                    case 1:
                        ArriveParkJsonDict["ArrivePortalTime"] = Time;
                        break;
                    case 4:
                        ArriveParkJsonDict["LeaveParkTime"] = Time;
                        break;
                    case 2:
                        ArriveParkJsonDict["LeavePortalTime"] = Time;
                        break;
                }
                ArriveParkJsonDict["Type"] = TestType[1];//测试状态
                ArriveParkJsonDict["tranNo"] = TestType[0];//测试箱号
            }
            else
            {
                ArriveParkJsonDict["Type"] = "3";//默认入园
                ArriveParkJsonDict["tranNo"] = Container;//默认箱号
                ArriveParkJsonDict["ArriveParkTime"] = Time;
                ArriveParkJsonDict["ArrivePortalTime"] = "";
                ArriveParkJsonDict["LeaveParkTime"] = "";
                ArriveParkJsonDict["LeavePortalTime"] = "";
            }
            ////////////////////////////////////////////////////////////

   
            ArriveParkJsonDict["truckNumber"] = Plate;
            ArriveParkJsonDict["OverTimeFlag"] = "";
            

            //string Json = string.Format(@"{{""eqId"":""{0}"",""arrivedTime"":""{1}"",""truckNumber"":""{2}"",""tranNo"":""{3}""}}",
            //    eqid, Time, Plate, Container);
            //string Json = string.Format(@"{{""eqId"":""{0}"",""ArriveParkTime"":""{1}"",""ArrivePortalTime"":"""",""LeaveParkTime"":"""",""LeavePortalTime"":"""",""truckNumber"":""{2}"",""OverTimeFlag"":"""",""Type"":""{3}"",""tranNo"":""{4}""}}",
            //                    eqid, Time, Plate,"3", Container);

            string Json= JsonConvert.SerializeObject(ArriveParkJsonDict);

            SetMessageAction?.Invoke(string.Format("Post ArriveParkTime Data：{0}", Json));
            _Log.logInfo.Info(string.Format("Post ArriveParkTime Data：{0}", Json));

            byte[] Josntobyte = Encoding.UTF8.GetBytes(Json);
            request.ContentLength = Josntobyte.Length;
            Stream writer = null;
            try
            {
                System.Net.ServicePointManager.DefaultConnectionLimit = 200;

                writer = request.GetRequestStream();
                if (writer != null)
                {
                    writer.Write(Josntobyte, 0, Josntobyte.Length);
                    writer.Close();

                    HttpWebResponse respone;
                    try
                    {
                        respone = (HttpWebResponse)request.GetResponse();
                    }
                    catch (WebException ex)
                    {
                        respone = ex.Response as HttpWebResponse;
                        SetMessageAction?.Invoke(string.Format("Result ArriveParkTime Data Error:{0}", ex.ToString()));
                        _Log.logError.Error("Result ArriveParkTime Data Error", ex);
                    }
                    //HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                    //StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                    //string ret = sr.ReadToEnd();
                    Stream s = respone.GetResponseStream();
                    StreamReader sreader = new StreamReader(s);
                    string postConent = sreader.ReadToEnd();
                    sreader.Close();
                    SetMessageAction(string.Format("Return ArriveParkTime Data：{0}", postConent));
                    _Log.logInfo.Info(string.Format("Return ArriveParkTime Data：{0}", postConent));

                    request.Abort();
                    request = null;

                    return postConent;
                }
            }
            catch (Exception ex)
            {
                request.Abort();
                request = null;


                SetMessageAction?.Invoke(string.Format("Send ArriveParkTime Data Error:{0}", ex.ToString()));
                _Log.logError.Error("Send ArriveParkTime Data Error", ex);
            }      
            return null;
        }

        private Dictionary<string, string> LeaveParkJsonDict = new Dictionary<string, string>();
        /// <summary>
        /// 华为后端数据交换(离园时间)
        /// </summary>
        /// <param name="Time"></param>
        /// <param name="Plate"></param>
        public void SetOutJosn(string Time, string Plate)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(http);
            request.Proxy = null;
            request.Method = "POST";
            request.Timeout = HttpTimeOut;
            request.ReadWriteTimeout = HttpReadWriteTimeout;
            request.ContentType = "application/json";/*x-www-form-urlencoded";*/

            //string Json = string.Format(@"{{""eqId"":""{0}"",""ArriveParkTime"":"""",""ArrivePortalTime"":"""",""LeaveParkTime"":""{1}"",""LeavePortalTime"":"""",""truckNumber"":""{2}"",""OverTimeFlag"":"""",""Type"":""{3}"",""tranNo"":""""}}",
            //                    eqid, Time, Plate, "4");

            LeaveParkJsonDict["eqId"] = eqid;
            LeaveParkJsonDict["ArriveParkTime"] = "";
            LeaveParkJsonDict["ArrivePortalTime"] = "";
            LeaveParkJsonDict["LeaveParkTime"] = Time;
            LeaveParkJsonDict["LeavePortalTime"] = "";
            LeaveParkJsonDict["truckNumber"] = Plate;
            LeaveParkJsonDict["OverTimeFlag"] = "";
            LeaveParkJsonDict["Type"] = "4";
            LeaveParkJsonDict["tranNo"] = "";

            string Json = JsonConvert.SerializeObject(LeaveParkJsonDict);

            SetMessageAction?.Invoke(string.Format("Post LeaveParkTime Data：{0}", Json));
            _Log.logInfo.Info(string.Format("Post LeaveParkTime Data：{0}", Json));

            byte[] Josntobyte = Encoding.UTF8.GetBytes(Json);
            request.ContentLength = Josntobyte.Length;
            Stream writer = null;
            try
            {
                System.Net.ServicePointManager.DefaultConnectionLimit = 200;

                writer = request.GetRequestStream();
                if (writer != null)
                {
                    writer.Write(Josntobyte, 0, Josntobyte.Length);
                    writer.Close();
                }

                request.Abort();
                request = null;
            }
            catch (Exception ex)
            {
                request.Abort();
                request = null;

                SetMessageAction?.Invoke(string.Format("Send LeaveParkTime Data Error:{0}", ex.ToString()));
                _Log.logError.Error("Send LeaveParkTime Data Error", ex);
            }
        }

        /// <summary>
        /// 解析Json数据
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public string[] JsonSplit(string result)
        {
            string[] ReturnData = new string[] {"*", "*" , "*" , "*" , "*" ,"*"};

            JsonPaner jp = JsonConvert.DeserializeObject<JsonPaner>(result);
            ReturnData[0] = string.Format("{0} {1}", jp.result.truckNumber, jp.result.tranNo);//车牌，箱号
            if (jp.result.resultList=="Y")
            {
                if(!string.IsNullOrEmpty(jp.result.resultList))
                {
                    ReturnData[1] = jp.result.resultList;//是否准时
                }
                if (!string.IsNullOrEmpty(jp.result.visitor))
                {
                    ReturnData[2] = jp.result.visitor;//供应商
                }
                if (!string.IsNullOrEmpty(jp.result.status))
                {
                    ReturnData[3] = jp.result.status;//车辆状体
                }
                if (!string.IsNullOrEmpty(jp.result.platform))
                {
                    ReturnData[4] = jp.result.platform;//停靠位
                }
                ReturnData[5] = Http_Status;//请进闸
               
            }
            else if(jp.result.resultList=="N")
            {
                ReturnData[5] = Http_NoStatus;//请登记
            }
            return ReturnData;
        }
    }

    public class JsonPaner
    {
        public string error_code;       //返回状态码
        public string error_desc;       //返回状态码描述
        public Result result;           //返回相关信息
    }

    public class Result
    {
        public string resultList;       //是否准点
        public string status;           //车辆状态
        public string visitor;          //来访者
        public string ledgename;        //平台
        public string platform;         //货位
        public string truckNumber;      //车牌
        public string tranNo;           //集装箱
        public string arrivedTime;      //当前时间
        //public string status1;
        //public string sysdate;
    }  
}
