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
    public class UserService : BaseService<User>, IUserService
    {
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public WebResult<User> Login(string account, string password)
        {
                string md5Password = CryptoHelper.MD5_Encrypt(password);
            var user = Find(x => x.Name == account && x.Password == md5Password && !x.IsDelete);
            if (user == null)
                return Result(user, ErrorCode.user_login_error);
            else
                return Result(user);
        }
        /// <summary>
        /// 获取在线用户id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<int> GetOnLineIdsByUserIDs(List<int> userIds)
        {
            if (userIds == null || userIds.Count == 0)
            {
                return null;
            }
            else
            {
                return GetList(x => userIds.Contains(x.ID) && x.IsOnLine).Select(x => x.ID).ToList();
            }
        }
    }
}
