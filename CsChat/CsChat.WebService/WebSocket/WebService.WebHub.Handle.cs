using CsChat.Code;
using CsChat.Core;
using CsChat.Model;
using CsChat.Repository;
using MulitChat.Domain.WebSocketRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.WebSockets;

namespace CsChat.Service
{
    public partial class WebService
    {
        /// <summary>
        /// 消息图片
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="e">通道</param>
        /// <returns></returns>
        private WebSocketResult Web_SendImage(string data)
        {
            Message_Web_SendImage model = JsonExtensions.DeserializeJson<Message_Web_SendImage>(data);
            Record record = null;
            if (model == null || model.FormUserID.IsNullOrEmpty() || model.ToUserID.IsNullOrEmpty() || model.ImgUrl.IsNullOrEmpty() || model.RecordID.IsNullOrEmpty() || model.RecordID.Length != 32)
            {
                return SocketResult(ErrorCode.sys_param_format_error);
            }
            using (var db = new DbRepository())
            {
                record = new Record();
                record.ID = model.RecordID;
                record.FormUserID = model.FormUserID;
                record.RecordCode =RecordCode.Image;
                //去掉图片网址前缀
                record.Content = model.ImgUrl;
                record.SendCode = RecordSendCode.Sending;
                record.SendTime = null;
                record.CreatedTime = DateTime.Now;
                record.UpdatedTime = DateTime.Now;
                db.Record.Add(record);
                db.SaveChanges();
                return SocketResult(record.ID);
            }
        }
        /// <summary>
        /// 消息发送
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="e">通道</param>
        /// <returns></returns>
        private WebSocketResult Web_SendText(string data)
        {
            Message_Web_SendText model = JsonExtensions.DeserializeJson<Message_Web_SendText>(data);
            if (model == null || model.FormUserID.IsNullOrEmpty() || model.ToUserID.IsNullOrEmpty() || model.Content.IsNullOrEmpty() || model.RecordID.IsNullOrEmpty() || model.RecordID.Length != 32)
            {
                return SocketResult(ErrorCode.sys_param_format_error);
            }
            else
            {
                using (var db = new DbRepository())
                {

                    Record record = new Record();
                    record.ID = model.RecordID;
                    record.ID = model.RecordID;
                    record.FormUserID = model.FormUserID;
                    record.RecordCode = RecordCode.Text;
                    record.Content = model.Content;
                    record.SendCode = RecordSendCode.Sending;
                    record.SendTime = null;
                    record.CreatedTime = DateTime.Now;
                    record.UpdatedTime = DateTime.Now;
                    db.Record.Add(record);
                    db.SaveChanges();
                    return SocketResult(record.ID);
                }
            }
        }
    }
}
