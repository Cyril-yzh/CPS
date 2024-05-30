using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS.Entity.Business
{
    public class Video : BaseEntity<Guid>
    {
        /// <summary>
        /// Share的外键  
        /// </summary>
        public Guid? ShareId { get; set; }

        /// <summary>
        /// 导航属性到Share  
        /// </summary>
        [ForeignKey(nameof(ShareId))]
        public virtual Share? Share { get; set; }
        /// <summary>
        /// 视频 src 必须
        /// </summary>
        public required string Url { get; set; }
        /// <summary>
        /// 视频封面图片 Rrl ，可选
        /// </summary>
        public string? Poster { get; set; }
    }
}
