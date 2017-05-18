using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using CsChat.Core;
using CsChat.Code;
using CsChat.Model;
using System.Web.WebSockets;
using CsChat.Domain;

namespace CsChat.Service
{
    public partial class WebService
    {
        private WebSocketSession webSocketSession = null;

        public void Process()
        {
            /// 处理WebSocket请求
            var context = this.Client.Request.RequestContext.HttpContext;
            if (context.IsWebSocketRequest || context.IsWebSocketRequestUpgrading)
            {
                context.AcceptWebSocketRequest(Process);
            }
        }

        private async Task Process(AspNetWebSocketContext context)
        {
            webSocketSession = new WebSocketSession(Client.LoginUser.ID, context.WebSocket);
            webSocketSession.OnRecevied += WebSocketSession_OnRecevied;
            webSocketSession.OnClose += WebSocketSession_OnClose;
            await webSocketSession.StartAccept();
        }

        /// <summary>
        /// todo:关闭连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebSocketSession_OnClose(object sender, EventArgs eventArgs)
        {

        }

        /// <summary>
        /// 接受数据
        /// </summary>
        /// <param name="obj"></param>
        private void WebSocketSession_OnRecevied(object sender, WebSocketRequest e)
        {
            var session = (WebSocketSession)sender;
            try
            {
                Func<string, WebSocketResult> handle = null;
                if (e.ActionCode != HandleCode.None && Handles.TryGetValue(e.ActionCode, out handle))
                {
                        var result = handle(e.Data);
                        if (result != null)
                        {
                            result.ActionCode = e.ActionCode;
                            result.DeviceID = e.DeviceID;
                            result.WeChatID = e.WeChatID;
                            result.MessageID = e.MessageID;
                            session.Send(result);
                        }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteException(ex);
                session.Send(new WebSocketResult() { ActionCode = e.ActionCode, ErrorCode = ErrorCode.sys_fail });
            }
        }

        private Dictionary<HandleCode, Func<string, WebSocketResult>> handles = null;

        protected Dictionary<HandleCode, Func<string, WebSocketResult>> Handles
        {
            get
            {
                if (handles == null)
                {
                    handles = new Dictionary<HandleCode, Func<string, WebSocketResult>>();

                    handles[HandleCode.Web_SendText] = Web_SendText;
                }
                return handles;
            }
        }

        public WebSocketResult SocketResult()
        {
            return new WebSocketResult { ErrorCode = ErrorCode.sys_success };
        }
        public WebSocketResult SocketResult(ErrorCode code)
        {
            return new WebSocketResult { ErrorCode = code };
        }
        public WebSocketResult SocketResult(ErrorCode code, string append)
        {
            return new WebSocketResult { ErrorCode = code, Append = append };
        }
        public WebSocketResult SocketResult<T>(T model)
        {
            return SocketResult(model, ErrorCode.sys_success);
        }

        public WebSocketResult SocketResult<T>(T model, ErrorCode code)
        {
            return new WebSocketResult { ErrorCode = code, Result = model };
        }

        public WebSocketResult SocketResult<T>(T model, ErrorCode code, string append)
        {
            return new WebSocketResult { ErrorCode = code, Result = model, Append = append };
        }
    }

}
