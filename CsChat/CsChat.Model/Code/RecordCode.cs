using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsChat.Model
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum RecordCode
    {
        None = 0,

        /// <summary>
        /// 文本消息
        /// </summary>
        [Description("文本")]
        Text = 1,

        /// <summary>
        /// 图片
        /// </summary>
        [Description("图片")]
        Image = 3,

        /// <summary>
        /// 音频
        /// </summary>
        [Description("音频")]
        Audio = 34,

        /// <summary>
        /// 名片
        /// </summary>
        [Description("名片")]
        BusinessCard = 42,

        /// <summary>
        /// 视频
        /// </summary>
        [Description("视频")]
        Video = 43,

        /// <summary>
        /// 链接
        /// </summary>
        [Description("分享链接")]
        Share = 49,

        
    }
}
