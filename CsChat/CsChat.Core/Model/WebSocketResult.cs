using CsChat.Code;
using CsChat.Core;

namespace CsChat.Model
{
    /// <summary>
    /// 返回结果集
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WebSocketResult
    {
        /// <summary>
        /// 操作码
        /// </summary>
        public HandleCode ActionCode;
        /// <summary>
        /// 错误码
        /// </summary>
        public ErrorCode ErrorCode;

        /// <summary>
        /// 微信ID
        /// </summary>
        public string WeChatID { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public string DeviceID { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        public object Result { get; set; }

        /// <summary>
        /// 附加消息
        /// </summary>
        public string Append { get; set; }

        /// <summary>
        /// 消息ID
        /// </summary>
        public string MessageID { get; set; }

        /// <summary>
        /// 是否是APP请求
        /// </summary>
        public bool IsApp { get; set; } = false;

        /// <summary>
        /// 异常消息
        /// </summary>
        public string ErrorDesc
        {
            get
            {
                if (this.ErrorCode != ErrorCode.sys_success)
                {
                    return this.ErrorCode.GetDescription();
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
