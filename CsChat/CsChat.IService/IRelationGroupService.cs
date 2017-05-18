using CsChat.Core;
using CsChat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsChat.IService
{
    public interface IRelationGroupService : IBaseService<RelationGroup>
    {
        /// <summary>
        /// 获取用户的交谈用户集合
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<RelationGroup> GetListByUserID(int userId);

        /// <summary>
        /// 增加聊天关系
        /// </summary>
        /// <param name="id"></param>
        /// <param name="toUserId"></param>
        /// <returns></returns>
        WebResult<bool> Add(int userId, int toUserId);

        /// <summary>
        /// 删除用户聊天关系
        /// </summary>
        /// <param name="id"></param>
        /// <param name="toUserId"></param>
        /// <returns></returns>
        WebResult<bool> Delete(int userId, int toUserId);


        /// <summary>
        /// 删除用户聊天关系
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        WebResult<bool> Delete(int id);
    }
}
