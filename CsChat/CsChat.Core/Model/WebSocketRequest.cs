using System.Web;
using System.Collections.Specialized;
using CsChat.Code;
using CsChat.Core;

namespace CsChat.Model
{
    /// <summary>
    /// Socket请求解析类
    /// </summary>
    public class WebSocketRequest
    {
        private NameValueCollection request = null;

        public WebSocketRequest(string args)
        {
            request = HttpUtility.ParseQueryString(args);
            this.ActionCode = request["ActionCode"].ToEnum(HandleCode.None);
            this.WeChatID = request["WeChatID"];
            this.DeviceID = request["DeviceID"];
            this.MessageID = request["MessageID"];
            this.Data = request["Data"];
        }

        public string this[string key]
        {
            get
            {
                return request[key];
            }
        }


        public HandleCode ActionCode { get; set; }

        public string WeChatID { get; set; }

        public string DeviceID { get; set; }

        public string Data { get; set; }

        public string MessageID { get; set; }


    }
}
