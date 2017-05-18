using CsChat.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CsChat.Core
{
    public class SocketSession
    {
        /// <summary>
        /// 编号
        /// </summary>
        private string sessionID = null;

        /// <summary>
        /// 设备编号
        /// </summary>
        public string DeviceID
        {
            get
            {
                if (!IsConsole)
                {
                    return sessionID;
                }
                return null;
            }
        }

        /// <summary>
        /// 控制台编号
        /// </summary>
        public string ConsoleID
        {
            get
            {
                if (IsConsole)
                {
                    return sessionID;
                }
                return null;
            }
        }

        /// <summary>
        /// 是否控制台
        /// </summary>
        public bool IsConsole { get; set; } = false;

        /// <summary>
        /// 触发事件
        /// </summary>
        public event EventHandler<string> OnRecevied;

        /// <summary>
        /// 开始接收数据
        /// </summary>
        public void StartAccept()
        {
            Task.Run(() =>
            {
                try
                {
                    while (true)
                    {
                        socketAccepted.Reset();
                        var bs = new byte[Client.ReceiveBufferSize];
                        Client.Client.BeginReceive(bs, 0, bs.Length, SocketFlags.None, new AsyncCallback(AcceptReceiveDataCallback), bs);
                        socketAccepted.WaitOne();
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteException(ex);
                    ConcurrentDictionaryHelper<SocketSession>.Remove(this.sessionID);
                }
            });

        }

        public void Auth(string sessionID, bool isConsole = false)
        {
            this.sessionID = sessionID;
            this.IsConsole = isConsole;
            ConcurrentDictionaryHelper<SocketSession>.Using(x =>
            {
                x.AddOrUpdate(this.sessionID, this, (key, value) =>
                {
                    return this;
                });
            });
        }

        /// <summary>
        /// 客户端IP
        /// </summary>
        public string ClientIP { get; set; }

        /// <summary>
        /// 服务端IP
        /// </summary>
        public string ServerIP { get; set; }

        /// <summary>
        /// Tcp客户端
        /// </summary>
        public TcpClient Client { get; set; }

        /// <summary>
        /// 渠道流
        /// </summary>
        private NetworkStream Stream
        {
            get
            {
                return Client.GetStream();
            }
        }

        /// <summary>
        /// 时间戳
        /// </summary>
        public long Ticks { get; set; }

        private ManualResetEvent socketAccepted = new ManualResetEvent(false);

        /// <summary>
        /// 缓存字节数组
        /// </summary>
        private List<byte> receivedBytes = new List<byte>();

        /// <summary>
        /// 接收到数据回调
        /// </summary>
        /// <param name="ar"></param>
        private void AcceptReceiveDataCallback(IAsyncResult ar)
        {
            try
            {
                var bs = (byte[])ar.AsyncState;
                SocketError errorCode = SocketError.Success;
                var receivedNum = Client.Client.EndReceive(ar, out errorCode);
                ///如果客户端关闭,接受到数据长度为0
                if (errorCode == SocketError.Success)
                {
                    if (receivedNum == 0)
                    {
                        Client.Dispose();
                        ConcurrentDictionaryHelper<SocketSession>.Remove(this.sessionID);
                        return;
                    }

                    receivedBytes.AddRange(bs.Take(receivedNum));
                    var message = Encoding.UTF8.GetString(receivedBytes.ToArray(), 0, receivedBytes.Count);

                    AsyncHelper.Run(() =>
                    {
                        LogHelper.WriteCustom(string.Format("{0}", message), @"SocketMessage\");
                    });

                    // 如果未结束,继续接收,完成后处理
                    if (receivedNum >= Client.ReceiveBufferSize &&
                            !message.EndsWith(Params.Socket_Text_Suffix, StringComparison.OrdinalIgnoreCase))
                    {
                        Thread.Sleep(50);
                        socketAccepted.Set();
                        return;
                    }

                    /// 判断数据是否符合格式要求
                    if (!message.IsNullOrEmpty() && message.EndsWith(Params.Socket_Text_Suffix, StringComparison.OrdinalIgnoreCase))
                    {
                        OnRecevied(this, message.Substring(0, message.Length - 4));
                    }


                    receivedBytes.Clear();

                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteException(ex);
            }
            socketAccepted.Set();

        }



        public void Response(HandleCode actionCode, ErrorCode code)
        {
            Response((int)actionCode, string.Empty, code);
        }

        public void Response<T>(HandleCode actionCode, T model)
        {
            Response((int)actionCode, model, ErrorCode.sys_success);
        }


        public void Response(int actionCode, ErrorCode code)
        {
            Response(actionCode, string.Empty, code);
        }

        public void Response<T>(int actionCode, T model)
        {
            Response(actionCode, model, ErrorCode.sys_success);
        }

        public void Response<T>(int handleCode, T model, ErrorCode code)
        {
            Response(handleCode, model, code, string.Empty);
        }

        public void Response<T>(int handleCode, T model, ErrorCode code, string append)
        {
            var message = new { ActionCode = handleCode, ErrorCode = code, Result = model, Append = append };
            Send(message.ToJson());
        }


        public void Send(string content)
        {
            Send(content, 0, null);
        }

        public void Send(string content, Action removeAction)
        {
            Send(content, 0, removeAction);
        }

        private void Send(string content, int resendNum, Action removeAction)
        {
            try
            {
                if (Stream.CanWrite)
                {
                    var bytes = Encoding.UTF8.GetBytes(content + Params.Socket_Text_Suffix);
                    Stream.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteException(ex);
                ConcurrentDictionaryHelper<SocketSession>.Remove(this.sessionID);
                if (removeAction != null)
                {
                    removeAction();
                }
            }
        }

        /// <summary>
        /// 回收
        /// </summary>
        public void Dispose()
        {
            if (this.Client != null)
            {
                this.Client.Dispose();
            }
        }

        ~SocketSession()
        {
            this.Dispose();
        }
    }
}




