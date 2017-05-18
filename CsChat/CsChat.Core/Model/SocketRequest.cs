namespace CsChat.Core
{
    public class SocketRequest
    {
        /// <summary>
        /// 事件码
        /// </summary>
        public int ActionCode { get; set; }

        /// <summary>
        /// 异常编码
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// 消息编号
        /// </summary>
        public string MessageID { get; set; }

    }
}
