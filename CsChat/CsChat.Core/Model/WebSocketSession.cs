using CsChat.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CsChat.Model
{
    /// <summary>
    /// WebSocket的session类
    /// </summary>
    public class WebSocketSession
    {
        private WebSocket socket = null;

        public WebSocketSession(string sessionID, WebSocket socket)
        {
            this.socket = socket;
            this.SessionID = sessionID;
        }

        /// <summary>
        /// 编号
        /// </summary>
        public string SessionID { get; set; }

        /// <summary>
        /// 触发事件
        /// </summary>
        public event EventHandler<WebSocketRequest> OnRecevied;

        /// <summary>
        /// 连接关闭事件
        /// </summary>
        public event EventHandler OnClose;

        private List<byte> receivedBytes = new List<byte>();

        /// <summary>
        /// 开始接收数据
        /// </summary>
        public async Task StartAccept()
        {
            try
            {
                while (socket.State == WebSocketState.Open)
                {
                    var buffer = new ArraySegment<byte>(new byte[Params.WebSocket_Buffer]);
                    var task = socket.ReceiveAsync(buffer, CancellationToken.None);
                    var exTask = task.ContinueWith((t) =>
                    {
                        if (t.Exception != null)
                        {
                            LogHelper.WriteException(t.Exception);
                        }
                    }, TaskContinuationOptions.OnlyOnFaulted);
                    var result = await task;
                    if (result != null)
                    {
                        receivedBytes.AddRange(buffer.Take(result.Count));
                        var message = Encoding.UTF8.GetString(receivedBytes.ToArray(), 0, receivedBytes.Count);
                        // 判断数据是否接收完成
                        if (result.Count >= Params.WebSocket_Buffer &&
                            !message.EndsWith(Params.Socket_Text_Suffix, StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }
                        /// 判断数据是否符合格式要求
                        if (!message.IsNullOrEmpty() && message.EndsWith(Params.Socket_Text_Suffix, StringComparison.OrdinalIgnoreCase))
                        {
                            // 异步启动接受数据事件
                            OnRecevied(this, new WebSocketRequest(message.Substring(0, message.Length - Params.Socket_Text_Suffix.Length)));
                        }
                        receivedBytes.Clear();
                    }
                }
                OnClose(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                LogHelper.WriteException(ex);
            }
        }

        public void Send(WebSocketResult model)
        {
            SendJson(model.ToJson(), 0);
        }


        public void SendJson(string json, int resend)
        {
            try
            {
                if (resend <= Params.WebSocket_Max_Resend)
                {
                    if (socket.State != WebSocketState.Closed && socket.State != WebSocketState.CloseReceived && socket.State != WebSocketState.CloseSent)
                    {
                        if (socket.State == WebSocketState.Open)
                        {
                            var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(json));
                            var task = socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                        else
                        {
                            Thread.Sleep(300);
                            SendJson(json, ++resend);
                        }
                    }
                }
            }
            catch (ObjectDisposedException ex)
            {
                LogHelper.WriteException(ex);
            }
            // 如果是无效操作,则重新发送.
            catch (InvalidOperationException ex)
            {
                LogHelper.WriteException(ex);
                Thread.Sleep(300);
                SendJson(json, ++resend);
            }
            catch (Exception ex)
            {
                LogHelper.WriteException(ex);
            }
        }


    }
}
