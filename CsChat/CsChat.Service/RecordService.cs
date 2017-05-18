using CsChat.Code;
using CsChat.Core;
using CsChat.IService;
using CsChat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsChat.Service
{
    public class RecordService : BaseService<Record>,  IRecordService
    {

        /// <summary>
        /// 消息图片
        /// </summary>
        /// <param name="model">数据</param>
        /// <returns></returns>
        public WebResult<bool> AddImage(string id, string imgUrl, int relationId,int sendUserId)
        {
            if (relationId<=0|| sendUserId <= 0 || imgUrl.IsNullOrEmpty() || id.IsNullOrEmpty() || id.Length != 32)
            {
                return Result(false, ErrorCode.sys_param_format_error);
            }
            var result = Add(new Record()
            {
                GuID = id,
                RelationID = relationId,
                RecordCode = RecordCode.Image,
                SendCode = RecordSendCode.Sending,
                Content = imgUrl,
                SendUserID= sendUserId,
                SendTime = DateTime.Now,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now
            });
            if (result > 0)
            {
                return Result(true);
            }
            else
            {
                return Result(false, ErrorCode.sys_fail);
            }
        }
        /// <summary>
        /// 消息发送
        /// </summary>
        /// <param name="model">数据</param>
        /// <returns></returns>
        public WebResult<bool> AddText(string id, string content, int relationId, int sendUserId)
        {
            if (relationId <= 0 || sendUserId <= 0 || content.IsNullOrEmpty() || id.IsNullOrEmpty() || id.Length != 32)
            {
                return Result(false, ErrorCode.sys_param_format_error);
            }
            else
            {
                var result = Add(new Record()
                {
                    GuID = id,
                    RelationID = relationId,
                    RecordCode = RecordCode.Text,
                    SendCode = RecordSendCode.Sending,
                    Content = content,
                    SendUserID = sendUserId,
                    SendTime = DateTime.Now,
                    CreatedTime = DateTime.Now,
                    UpdatedTime = DateTime.Now
                });
                if (result > 0)
                {
                    return Result(true);
                }
                else
                {
                    return Result(false, ErrorCode.sys_fail);
                }
            }
        }


        /// <summary>
        /// 加载搜索时间前记录
        /// </summary>
        /// <param name="relationId">联系关系</param>
        /// <param name="searchTime"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Record> GetPrevList(int relationId, DateTime searchTime, int count,int searchId)
        {
            if (relationId <= 0 || count<=0)
            {
                return null;
            }
            else
            {
                if(searchId==0)
                    return GetList(x => x.RelationID == relationId && x.SendTime < searchTime, count);
                else
                    return GetList(x => x.RelationID == relationId && x.ID> searchId, count);
            }
        }

        /// <summary>
        /// 获取未读消息
        /// </summary>
        /// <param name="relationId">联系关系</param>
        /// <returns></returns>
        public List<Record> GetNoReadList(int relationId,int toUserId)
        {
            if (relationId <= 0|| toUserId <= 0)
            {
                return null;
            }
            else
            {
               return GetList(x => x.RelationID == relationId && !x.IsRead&&x.SendUserID!= toUserId);

            }
        }
        /// <summary>
        /// 获取未读消息
        /// </summary>
        /// <param name="relationId">联系关系</param>
        /// <returns></returns>
        public void SetReadList(int relationId)
        {
            if (relationId > 0 )
            {
                var list = GetList(x => x.RelationID == relationId && !x.IsRead);
                if (list != null && list.Count > 0)
                {
                    list.ForEach(x =>
                    {
                        x.IsRead = true;
                        Update(x);
                    });
                }
            }
        }

        /// <summary>
        /// 搜索聊天记录
        /// </summary>
        /// <param name="relationId">联系关系</param>
        /// <param name="keyWord">关键字</param>
        /// <returns></returns>
        public List<Record> GetSearchList(int relationId, string keyWord)
        {
            if (relationId <= 0||keyWord.IsNullOrEmpty())
            {
                return null;
            }
            else
            {
                return GetList(x => x.RelationID == relationId && x.Content.Contains(keyWord));
            }
        }
    }
}
