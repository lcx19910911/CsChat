using CsChat.Code;
using CsChat.Core;
using CsChat.IService;
using CsChat.Model;
using System;
using System.Collections.Generic;

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
            if (userId <= 0 || toUserId <= 0)
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
