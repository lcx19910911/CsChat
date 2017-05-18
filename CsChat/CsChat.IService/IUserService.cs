using CsChat.Core;
using CsChat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsChat.IService
{
    public interface IUserService : IBaseService<User>
    {
        WebResult<User> Login(string account, string password);
        

        /// <summary>
        /// 获取在线用户id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<int> GetOnLineIdsByUserIDs(List<int> userIds);
    }
}
