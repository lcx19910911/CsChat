using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MulitChat.Domain.WebSocketRequest
{
    public class Message_Web_SendText: Message_Web
    {

        /// <summary>
        /// 消息记录ID
        /// </summary>
        public string RecordID { get; set; }


        /// <summary>
        /// 送达好友ID
        /// </summary>
        public string ToUserID { get; set; }

        /// <summary>
        /// 发送内容
        /// </summary>
        public string Content { get; set; }
    }
}
