using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsChat.Model
{
    /// <summary>
    /// 用户设备端口 1用户可多个登陆端
    /// </summary>
    [Table("esc_chat_userDevice")]
    public class UserDevice : BaseEntity
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [Required]
        public int UserID { get; set; }

        /// <summary>
        /// ip
        /// </summary>
        [Required]
        [MaxLength(32)]
        public string IP { get; set; }

        ///// <summary>
        ///// IP
        ///// </summary>
        //public string Ip { get; set; }
    }
}
