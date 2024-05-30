using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS.Entity.Business;
/// <summary>
/// 媒体文件类
/// </summary>
public class Share : BaseEntity<Guid>
{
    /// <summary>
    /// 图片集合
    /// </summary>
    public virtual ICollection<Image> Images { get; set; } = [];
    /// <summary>
    /// 视频集合
    /// </summary>
    public virtual ICollection<Video> Videos { get; set; } = [];
    /// <summary>
    /// 文章内容
    /// </summary>
    public required string Content { get; set; }
    /// <summary>
    /// 浏览量
    /// </summary>
    public int BrowseCount { get; set; }
    /// <summary>
    /// 喜欢次数
    /// </summary>
    public int LikeCount { get; set; }
    /// <summary>
    /// 收藏数量
    /// </summary>
    public int CollectCount { get; set; }
    /// <summary>
    /// 分享数量
    /// </summary>
    public int ShareCount { get; set; }
    /// <summary>
    /// 发布状态 0:未发布 1:待审核 2:已发布
    /// </summary>
    public int PublishStatus { get; set; }
}
