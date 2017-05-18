
using CsChat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsChat.Core
{
    /// <summary>
    /// 登录用户
    /// </summary>
    public class LoginUser
    {
        public LoginUser(int id,string account,string name, string img)
        {
            this.ID = id;
            this.Account = account;
            this.Name = name;
            this.Img = img;
        }


        public LoginUser()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string Img { get; set; }

    }
}
