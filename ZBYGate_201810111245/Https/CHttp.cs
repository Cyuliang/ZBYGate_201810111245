using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace ZBYGate_Data_Collection.Https
{
    class CHttp
    {
        private Log.CLog _Log = new Log.CLog();
        public Action<string> SetMessage;

        private readonly string http = Properties.Settings.Default.Http_www;
        private readonly string eqid = Properties.Settings.Default.Http_eqId;
        private readonly int HttpTimeOut = Properties.Settings.Default.Http_HttpTimeOut;
        private readonly int HttpReadWriteTimeout = Properties.Settings.Default.Http_HttpReadWriteTimeout;
        private readonly string Http_NoStatus = Properties.Settings.Default.Http_NoStatus;

        /// <summary>
        /// 远程通讯处理
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Time"></param>
        /// <param name="Plate"></param>
        /// <param name="Container"></param>
        /// <returns></returns>
        public string SetJosn( string Time, string Plate, string Container)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(http);
            request.Method = "POST";
            request.Timeout = HttpTimeOut;
            request.ReadWriteTimeout = HttpReadWriteTimeout;
            request.ContentType = "application/json";/*x-www-form-urlencoded";*/
            string Json = string.Format(@"{{""eqId"":""{0}"",""arrivedTime"":""{1}"",""truckNumber"":""{2}"",""tranNo"":""{3}""}}",
                eqid, Time, Plate, Container);
            byte[] Josntobyte = Encoding.UTF8.GetBytes(Json);
            request.ContentLength = Josntobyte.Length;
            Stream writer = null;
            try
            {
                writer = request.GetRequestStream();
            }
            catch (Exception ex)
            {
                SetMessage?.Invoke(string.Format("Send {0} Error",Json));
                _Log.logError.Error("Send Data Error", ex);
            }
            if (writer != null)
            {
                writer.Write(Josntobyte, 0, Josntobyte.Length);
                writer.Close();
                SetMessage?.Invoke(string.Format("Post Data：{0}", Json));
                _Log.logInfo.Info(string.Format("Post Data：{0}", Json));

                HttpWebResponse respone;
                try
                {
                    respone = (HttpWebResponse)request.GetResponse();
                }
                catch (WebException ex)
                {
                    respone = ex.Response as HttpWebResponse;
                    SetMessage?.Invoke("Post Data Error");
                    _Log.logError.Error("Result Data Error", ex);
                }
                //HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                //StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                //string ret = sr.ReadToEnd();
                Stream s = respone.GetResponseStream();
                StreamReader sreader = new StreamReader(s);
                string postConent = sreader.ReadToEnd();
                sreader.Close();
                SetMessage(string.Format("Return Data：{0}", postConent));
                _Log.logInfo.Info(string.Format("Return Data：{0}", postConent));
                //return postConent;
                return postConent;
            }
            return null;
        }

        /// <summary>
        /// 解析Json数据
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public string[] JsonSplit(string result)
        {
            string[] ReturnData = new string[] {"NONE", "NONE" , "NONE" , "NONE" , "NONE" ,"NONE"};

            JsonPaner jp = JsonConvert.DeserializeObject<JsonPaner>(result);
            ReturnData[0] = string.Format("{0}/{1}", jp.result.truckNumber, jp.result.tranNo);
            ReturnData[5] = jp.result.resultList;
            if (jp.result.resultList=="Y")
            {
                if(!string.IsNullOrEmpty(jp.result.arrivedTime))
                {
                    ReturnData[1] = jp.result.arrivedTime;
                }
                if (!string.IsNullOrEmpty(jp.result.visitor))
                {
                    ReturnData[2] = jp.result.visitor;
                }
                if (!string.IsNullOrEmpty(jp.result.platform))
                {
                    ReturnData[3] = jp.result.platform;
                }
                if (!string.IsNullOrEmpty(jp.result.status))
                {
                    ReturnData[4] = jp.result.status;
                }                                               
            }
            else if(jp.result.resultList=="N")
            {
                ReturnData[4] = Http_NoStatus;
            }
            return ReturnData;
        }
    }

    public class JsonPaner
    {
        public string error_code;
        public string error_desc;
        public Result result;
    }

    public class Result
    {
        public string resultList;
        public string status;
        public string visitor;
        public string ledgename;
        public string platform;
        public string truckNumber;
        public string tranNo;
        public string arrivedTime;
    }  
}
