using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CsChat.Model
{

    [Table("esc_user")]
    public class User:BaseEntity
    {
        [Column("user_id")]
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new  int ID { get; set; }


        /// <summary>
        /// 昵称
        /// </summary>
        [Column("user_name")]
        [MaxLength(60)]
        public string Name { get; set; }


        /// <summary>
        /// 密码
        /// </summary>
        [Column("password")]
        [MaxLength(60)]
        public string Password { get; set; }



        /// <summary>
        /// 头像
        /// </summary>
        [Column("user_img")]
        [MaxLength(255)]
        public string Img { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Display(Name = "修改时间")]
        [Required]
        [Column("reg_time")]
        public new DateTime CreatedTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Display(Name = "修改时间")]
        [Required]
        [Column("last_time")]
        public new DateTime UpdatedTime { get; set; }

        [Column("flag")]
        public new bool IsDelete { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        public bool IsOnLine { get; set; } = false;

        [NotMapped]
        public bool IsHadNoReadMsg { get; set; }

        [NotMapped]
        public DateTime? SendTime { get; set; }

        [NotMapped]
        public List<Record> RecordList { get; set; }
           
        [NotMapped]
        public int RelationID { get; set; }
        
    }
}

