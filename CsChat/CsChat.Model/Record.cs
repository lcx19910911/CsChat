
namespace CsChat.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("esc_chat_record")]
    public partial class Record:BaseEntity
    {
        [Column("GuID", TypeName = "char"), MaxLength(32)]
        [Required]
        public  string GuID { get; set; }

        /// <summary>
        /// 关联id
        /// </summary>
        [Required]
        public int RelationID { get; set; }

        public int SendUserID { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public RecordCode RecordCode { get; set; }

        public bool IsRead { get; set; } = false;

        /// <summary>
        /// 用户id
        /// </summary>
        [Required]
        public string Content { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public Nullable<System.DateTime> SendTime { get; set; }

        /// <summary>
        /// 发送状态
        /// </summary>
        public RecordSendCode SendCode { get; set; }
    }
}
