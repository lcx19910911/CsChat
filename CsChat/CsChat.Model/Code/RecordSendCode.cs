using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsChat.Model
{
    /// <summary>
    /// 聊天记录状态
    /// </summary>
    public enum RecordSendCode
    {
        None = 0,

        /// <summary>
        /// 发送中
        /// </summary>
        [Description("发送中")]
        Sending = 1,

        /// <summary>
        /// 发送成功
        /// </summary>
        [Description("发送成功")]
        SendSuccess = 2,

        /// <summary>
        /// 发送异常
        /// </summary>
        [Description("发送异常")]
        SendError = 3,
    }
}
