using CsChat.Core;
using CsChat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsChat.IService
{
    /// <summary>
    /// 聊天记录
    /// </summary>
    public interface IRecordService: IBaseService<Record>
    {
        /// <summary>
        /// 图片消息
        /// </summary>
        /// <param name="id">消息id</param>
        /// <param name="imgUrl"></param>
        /// <param name="relationId"></param>
        /// <returns></returns>
        WebResult<bool> AddImage(string id, string imgUrl, int relationId,int sendUserId);

        /// <summary>
        /// 文本消息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <param name="relationId"></param>
        /// <returns></returns>
        WebResult<bool> AddText(string id, string content, int relationId, int sendUserId);


        /// <summary>
        /// 加载搜索时间前记录
        /// </summary>
        /// <param name="relationId">联系关系</param>
        /// <param name="searchTime"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<Record> GetPrevList(int relationId, DateTime searchTime,int count,int searchId);

        /// <summary>
        /// 获取未读消息
        /// </summary>
        /// <param name="relationId">联系关系</param>
        /// <returns></returns>
        List<Record> GetNoReadList(int relationId, int toUserId);



        /// <summary>
        /// 设置消息已读
        /// </summary>
        /// <param name="relationId">联系关系</param>
        /// <returns></returns>
        void SetReadList(int relationId);

        /// <summary>
        /// 搜索聊天记录
        /// </summary>
        /// <param name="relationId">联系关系</param>
        /// <param name="keyWord">关键字</param>
        /// <returns></returns>
        List<Record> GetSearchList(int relationId,string keyWord);
    }
}
