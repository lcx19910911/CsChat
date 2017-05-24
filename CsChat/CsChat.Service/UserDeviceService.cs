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
    public class UserDeviceService : BaseService<UserDevice>, IUserDeviceService
    {

        /// <summary>
        /// 增加用户设备
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public WebResult<bool> Add(int userId, string ip,out int id)
        {
            id = 0;
            if (userId <= 0 || ip.IsNullOrEmpty())
            {
                return Result(false, ErrorCode.sys_param_format_error);
            }
            else
            {
                var model = Find(x => x.UserID == userId && x.IP == ip);
                if (model == null)
                {
                    model = (new UserDevice()
                    {
                        UserID = userId,
                        IP = ip,
                    });
                    id = Add(model);
                    if (id > 0)
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
                    if (!model.IsDelete)
                        return Result(true);
                    else
                        model.IsDelete = false;
                    id = model.ID;
                    if (Update(model, model.ID) > 0)
                    {
                        return Result(true);
                    }
                    else
                    {
                        return Result(false, ErrorCode.sys_fail);
                    }
                }
            }
        }

        /// <summary>
        /// 删除用户设备
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public WebResult<bool> DeleteDevice(int id)
        {
            if (id <= 0)
            {
                return Result(false, ErrorCode.sys_param_format_error);
            }
            else
            {

                var result = Delete(id + "");
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
        /// 删除用户设备
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public WebResult<bool> DeleteDeviceByUserID(int userId,string ip)
        {
            if (userId <= 0)
            {
                return Result(false, ErrorCode.sys_param_format_error);
            }
            else
            {
                var result = Delete(x => x.UserID == userId&&ip==x.IP);
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
        /// 获取用户的聊天关系
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserDevice> GetListByUserIDs(List<int> userIds)
        {
            if (userIds==null&& userIds.Count <= 0)
            {
                return null;
            }
            else
            {
                return GetList(x => userIds.Contains(x.UserID)&&!x.IsDelete);
            }
        }

        /// <summary>
        /// 获取用户的聊天关系
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserDevice> GetListByUserID(int userId)
        {
            if (userId <= 0)
            {
                return null;
            }
            else
            {
                return GetList(x => x.UserID == userId&&!x.IsDelete);
            }
        }
    }
}
