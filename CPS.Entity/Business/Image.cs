using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS.Entity.Business
{
    public class Image : BaseEntity<Guid>
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
        /// 图片src 必须
        /// </summary>
        public required string Url { get; set; }
        /// <summary>
        /// 图片描述文字，非必须
        /// </summary>
        public string? Alt { get; set; }
        /// <summary>
        /// 图片的链接，非必须
        /// </summary>
        public string? Href { get; set; }
    }
}
