
namespace CsChat.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// �����ϵ��
    /// </summary>
    [Table("esc_chat_relation")]
    public partial class RelationGroup : BaseEntity
    {

        [Required]
        public int UserID { get; set; }

        [Required]
        public int RelationUserID { get; set; }

        /// <summary>
        /// ���ͨ��ʱ��
        /// </summary>
        public System.DateTime LastTime { get; set; }
    }
}
