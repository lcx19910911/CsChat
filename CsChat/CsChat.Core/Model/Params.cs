using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web.Security;

namespace CsChat.Core
{
    public class Params
    {
        /// <summary>
        /// 缓存前缀
        /// </summary>
        public static readonly string Cache_Prefix_Key = "WOL";

        public static readonly string Mobile = ConfigurationManager.AppSettings["Mobile"];

        /// <summary>
        /// 公司名称
        /// </summary>
        public static readonly string CompanyName = ConfigurationManager.AppSettings["CompanyName"];               

        /// <summary>
        /// 密钥
        /// </summary>
        public static readonly string SecretKey = ConfigurationManager.AppSettings["SecretKey"];

        /// <summary>
        /// 教练员默认权限
        /// </summary>
        public static readonly string CoachRoleId = ConfigurationManager.AppSettings["CoachRoleId"];

        /// <summary>
        /// 登陆cookie
        /// </summary>
        public static readonly string UserCookieName = "CsChat_user";

        /// <summary>
        /// cookie 过期时间
        /// </summary>
        public static readonly int CookieExpires = 12000;

        /// <summary>
        /// WebSocket的最大发送次数
        /// </summary>
        public readonly static int WebSocket_MaxSendNum = 5;
        /// <summary>
        /// WebSocket的缓冲池大小
        /// </summary>
        public readonly static int WebSocket_Buffer = 65535;

        /// <summary>
        /// Wen通讯文本协议结束后缀
        /// </summary>
        public readonly static string Socket_Text_Suffix = "#END";


        /// <summary>
        /// WebSocket重发次数
        /// </summary>
        public readonly static int WebSocket_Max_Resend = 5;
    }
}
