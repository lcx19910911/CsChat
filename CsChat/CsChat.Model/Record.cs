
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
        /// ����id
        /// </summary>
        [Required]
        public int RelationID { get; set; }

        public int SendUserID { get; set; }

        /// <summary>
        /// ��Ϣ����
        /// </summary>
        public RecordCode RecordCode { get; set; }

        public bool IsRead { get; set; } = false;

        /// <summary>
        /// �û�id
        /// </summary>
        [Required]
        public string Content { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public Nullable<System.DateTime> SendTime { get; set; }

        /// <summary>
        /// ����״̬
        /// </summary>
        public RecordSendCode SendCode { get; set; }
    }
}
