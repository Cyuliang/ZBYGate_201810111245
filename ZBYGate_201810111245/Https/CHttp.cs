using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZBYGate_Data_Collection.Https
{
    class CHttp
    {
        private Log.CLog _Log = new Log.CLog();
        public Action<string> SetMessage;

        private string http = Properties.Settings.Default.Http_www;
        private int HttpTimeOut = Properties.Settings.Default.Http_HttpTimeOut;
        private int HttpReadWriteTimeout = Properties.Settings.Default.Http_HttpReadWriteTimeout;

        public CHttp()
        {            
        }

        public string SetJosn(string Id, string Time, string Plate, string Container)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(http);
            request.Method = "POST";
            request.Timeout = HttpTimeOut;
            request.ReadWriteTimeout = HttpReadWriteTimeout;
            request.ContentType = "application/json";/*x-www-form-urlencoded";*/
            string Json = string.Format(@"{{""eqId"":""{0}"",""arrivedTime"":""{1}"",""truckNumber"":""{2}"",""tranNo"":""{3}""}}",
                Id, Time, Plate, Container);
            byte[] Josntobyte = Encoding.UTF8.GetBytes(Json);
            request.ContentLength = Josntobyte.Length;
            Stream writer = null;
            try
            {
                writer = request.GetRequestStream();
            }
            catch (Exception ex)
            {
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
                return postConent;
            }
            return "null";
        }
    }
}
