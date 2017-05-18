using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MulitChat.Domain.WebSocketRequest
{
    /// <summary>
    /// 发送图片
    /// </summary>
    public class Message_Web_SendImage: Message_Web
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
        /// 图片地址
        /// </summary>
        public string ImgUrl { get; set; }
    }
}
