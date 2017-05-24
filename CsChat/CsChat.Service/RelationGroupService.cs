using CsChat.Code;
using CsChat.Core;
using CsChat.IService;
using CsChat.Model;
using CsChat.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CsChat.Service
{
    public class RelationGroupService : BaseService<RelationGroup>, IRelationGroupService
    {
        /// <summary>
        /// 增加聊天关系
        /// </summary>
        /// <param name="id"></param>
        /// <param name="toUserId"></param>
        /// <returns></returns>
        public WebResult<bool> Add(int userId, int toUserId)
        {
            if (userId <= 0 || toUserId <= 0||userId==toUserId)
            {
                return Result(false, ErrorCode.sys_param_format_error);
            }
            else
            {
                if (!IsExits(x => (x.UserID == userId || x.RelationUserID == userId) && (x.UserID == toUserId || x.RelationUserID == toUserId)))
                {
                    var result = Add(new RelationGroup()
                    {
                        UserID = userId,
                        RelationUserID = toUserId,
                        LastTime = DateTime.Now,
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
                else
                { 
                    return Result(false, ErrorCode.sys_param_format_error);
                }
            }
        }

        public PageList<RelationGroup> GetPageList(int pageIndex, int pageSize, string sendUserName, string toUserName, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            using (DbRepository db = new DbRepository())
            {
                var query = db.RelationGroup.Where(x => !x.IsDelete);
                if (sendUserName.IsNotNullOrEmpty())
                {
                    var userIdList = db.User.Where(x => !x.IsDelete & x.Name.Contains(sendUserName)).Select(x => x.ID).ToList();
                    if (userIdList != null && userIdList.Count > 0)
                    {
                        query = query.Where(x => userIdList.Contains(x.UserID) || userIdList.Contains(x.RelationUserID));
                    }
                }
                if (toUserName.IsNotNullOrEmpty())
                {
                    var userIdList = db.User.Where(x => !x.IsDelete & x.Name.Contains(toUserName)).Select(x => x.ID).ToList();
                    if (userIdList != null && userIdList.Count > 0)
                    {
                        query = query.Where(x => userIdList.Contains(x.UserID) || userIdList.Contains(x.RelationUserID));
                    }
                }
                if (createdTimeStart != null)
                {
                    query = query.Where(x => x.LastTime >= createdTimeStart);
                }
                if (createdTimeEnd != null)
                {
                    createdTimeEnd = createdTimeEnd.Value.AddDays(1);
                    query = query.Where(x => x.LastTime < createdTimeEnd);
                }
                query = query.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize);

                var pageList = CreatePageList(query, pageIndex, pageSize);

                return pageList;

            }
        }

        /// <summary>
        /// 删除用户聊天关系
        /// </summary>
        /// <param name="id"></param>
        /// <param name="toUserId"></param>
        /// <returns></returns>
        public WebResult<bool> Delete(int userId, int toUserId)
        {
            if (userId <= 0 || toUserId <= 0)
            {
                return Result(false, ErrorCode.sys_param_format_error);
            }
            else
            {
                if (IsExits(x => (x.UserID == userId || x.RelationUserID == userId) && (x.UserID == toUserId || x.RelationUserID == toUserId)))
                {
                    var result = Delete(x=> (x.UserID == userId || x.RelationUserID == userId) && (x.UserID == toUserId || x.RelationUserID == toUserId));
                    if (result > 0)
                    {
                        return Result(true);
                    }
                    else
                    {
                        return Result(false, ErrorCode.sys_fail);
                    }
                }
                else
                {
                    return Result(false, ErrorCode.sys_param_format_error);
                }
            }
        }

        /// <summary>
        /// 删除用户聊天关系
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WebResult<bool> Delete(int id)
        {
            if (id <= 0)
            {
                return Result(false, ErrorCode.sys_param_format_error);
            }
            else
            {
                if (IsExits(x => x.ID== id))
                {
                    var result = Delete(x => x.ID == id);
                    if (result > 0)
                    {
                        return Result(true);
                    }
                    else
                    {
                        return Result(false, ErrorCode.sys_fail);
                    }
                }
                else
                {
                    return Result(false, ErrorCode.sys_param_format_error);
                }
            }
        }

        /// <summary>
        /// 获取用户的聊天关系
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<RelationGroup> GetListByUserID(int userId)
        {
            if (userId <= 0)
            {
                return null;
            }
            else
            {
                return GetList(x => x.UserID == userId||x.RelationUserID==userId);
            }
        }
    }
}
