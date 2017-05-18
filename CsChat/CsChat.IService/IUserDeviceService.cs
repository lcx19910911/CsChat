using CsChat.Core;
using CsChat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsChat.IService
{
    public interface IUserDeviceService : IBaseService<UserDevice>
    {
        /// <summary>
        /// 增加用户设备
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        WebResult<bool> Add(int userId, string ip,out int id);

        /// <summary>
        /// 删除用户设备
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        WebResult<bool> DeleteDevice(int id);


        /// <summary>
        /// 删除用户设备
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        WebResult<bool> DeleteDeviceByUserID(int userId);

        /// <summary>
        /// 获取用户的聊天关系
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<UserDevice> GetListByUserID(int userId);

        List<UserDevice> GetListByUserIDs(List<int> userIds);
    }
}
