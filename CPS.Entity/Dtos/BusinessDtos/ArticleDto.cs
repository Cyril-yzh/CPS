using System.ComponentModel.DataAnnotations;

namespace CPS.Entity.Dtos.BusinessDtos;
public class ArticleDto : BaseEntityDto<long>
{
    /// <summary>
    /// 文章标题
    /// </summary>
    [StringLength(100)]
    public required string Title { get; set; }

    /// <summary>
    /// 文章分类
    /// </summary>
    public int ClassifyId { get; set; }

    /// <summary>
    /// 浏览量
    /// </summary>
    public int BrowseCount { get; set; }

    /// <summary>
    /// 点赞数量
    /// </summary>
    public int SupportCount { get; set; }

    /// <summary>
    /// 收藏数量
    /// </summary>
    public int CollectCount { get; set; }

    /// <summary>
    /// 分享数量
    /// </summary>
    public int ShareCount { get; set; }

    /// <summary>
    /// 文章内容
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// 封面图，前端传入base64码
    /// </summary>
    public string? CoverImage { get; set; }

    /// <summary>
    /// 发布状态 0:未发布 1:待审核 2:已发布
    /// </summary>
    public int PublishStatus { get; set; }
    public long? CreateUserId { get; set; }
    public string? CreateUserName { get; set; }
}
