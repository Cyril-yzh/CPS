using Microsoft.AspNetCore.Mvc;
using CPS.Entity;
using CPS.Entity.Business;
using CPS.Service.IServices;
using CPS.Entity.Dtos.BusinessDtos;
using CPS.Service;
using Microsoft.AspNetCore.Authorization;

namespace CPS.API.Controllers.WebApiControllers;
/// <summary>
/// 文章API接口
/// </summary>
[Route("api/[controller]")]
[Authorize]
[ApiController]
public class ArticleController(IArticleService articleService, IWebHostEnvironment webHostEnvironment) : ControllerBase
{
    private readonly IArticleService _articleService = articleService;
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;


    /// <summary>
    /// 获取文章列表列表数据分页
    /// </summary>
    /// <param name="title">标题</param>
    /// <param name="content">内容</param>
    /// <param name="classifyId">分类</param>
    /// <param name="pageIndex">当前页</param>
    /// <param name="pageSize">每页显示行</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetArticleListPage(string? title, string? content, int? classifyId, int? publishStatus, int pageIndex = 1, int pageSize = 10)
    {
        ApiResult<PageData<Article>> apiResult = await _articleService.GetArticleListPageAsync(title, content
            , classifyId, publishStatus, pageIndex, pageSize);

        return Ok(apiResult);
    }

    /// <summary>
    /// 发布文章
    /// </summary>
    /// <param name="articleDto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> PostArticle(ArticleDto articleDto)
    {
        Article article = new()
        {
            Title = articleDto.Title,
            Content = articleDto.Content ?? "文章详情信息不存在！",
            BrowseCount = articleDto.BrowseCount,
            SupportCount = articleDto.SupportCount,
            ShareCount = articleDto.ShareCount,
            PublishStatus = articleDto.PublishStatus,
            CollectCount = articleDto.CollectCount,
            ClassifyId = articleDto.ClassifyId,
            CreateUserId = articleDto.CreateUserId,
            CreateUserName = articleDto.CreateUserName,
        };
        if (!string.IsNullOrEmpty(articleDto.CoverImage))
        {
            string[] base64Array = articleDto.CoverImage.Split(',');
            string dataType = base64Array[0];
            string base64 = base64Array[1];

            //后缀名
            string suffix = "." + dataType.Replace("data:", "").Replace(";base64", "").Split('/')[1];
            //base64转字节数组
            byte[] base64Bytes = Convert.FromBase64String(base64);
            //生成文件名
            string fileName = $"article/{Guid.NewGuid()}{suffix}";  //Guid.NewGuid() + suffix;
            //要保存的文件路径
            string saveFilePath = Path.Combine(_webHostEnvironment.WebRootPath, fileName);
            //写入字节流到指定路径
            await System.IO.File.WriteAllBytesAsync(saveFilePath, base64Bytes);
            article.CoverImage = fileName;
        }

        bool res = await _articleService.AddAsync(article);
        ApiResult<bool> apiResult = new()
        {
            Data = res,
            Message = res ? "文章发布成功！" : "文章发布出现异常！"
        };
        return Ok(apiResult);
    }

    /// <summary>
    /// 通过文章编号，获取文章详情信息
    /// </summary>
    /// <param name="id">文章编号</param>
    /// <returns></returns>
    [HttpGet("{id}")] //api/article/12
    public async Task<IActionResult> GetArticle(long id)
    {
        Article? article = await _articleService.FindAsync(id);

        ApiResult<Article> apiResult = new()
        {
            Data = article,
            Code = article == null ? 404 : 200,
            Message = article == null ? "文章详情信息不存在！" : "获取文章详情信息"
        };

        return Ok(apiResult);
    }

    /// <summary>
    /// 更新文章信息
    /// </summary>
    /// <param name="articleDto"></param>
    [HttpPut]
    public async Task<IActionResult> PutArticle(ArticleDto articleDto)
    {
        ApiResult<bool> apiResult = new();

        //查询文章是否存在
        Article? articleRes = await _articleService.FindAsync(articleDto.Id);
        if (articleRes == null)
        {
            apiResult.Code = 404;
            apiResult.Message = "文章详情信息不存在！";
            return Ok(articleRes);
        }
        articleRes.UpdateTime=DateTime.Now;
        //更新文章的信息
        articleRes.Title = articleDto.Title;
        articleRes.Content = articleDto.Content ?? "文章详情信息不存在！";
        articleRes.BrowseCount = articleDto.BrowseCount;
        articleRes.SupportCount = articleDto.SupportCount;
        articleRes.ShareCount = articleDto.ShareCount;
        articleRes.PublishStatus = articleDto.PublishStatus;
        articleRes.CollectCount = articleDto.CollectCount;
        articleRes.ClassifyId = articleDto.ClassifyId;
        if (!string.IsNullOrEmpty(articleDto.CoverImage) && articleDto.CoverImage.IndexOf("base64") > 0)
        {
            string[] base64Array = articleDto.CoverImage.Split(',');
            string dataType = base64Array[0];
            string base64 = base64Array[1];
            //后缀名
            string suffix = "." + dataType.Replace("data:", "").Replace(";base64", "").Split('/')[1];
            //base64转字节数组
            byte[] base64Bytes = Convert.FromBase64String(base64);
            //生成文件名
            string fileName = $"article/{Guid.NewGuid()}{suffix}";  //Guid.NewGuid() + suffix;
            //要保存的文件路径
            string saveFilePath = Path.Combine(_webHostEnvironment.WebRootPath, fileName);
            //写入字节流到指定路径
            await System.IO.File.WriteAllBytesAsync(saveFilePath, base64Bytes);
            articleRes.CoverImage = fileName;
        }

        bool res = await _articleService.UpdateAsync(articleRes);

        apiResult.Data = res;
        apiResult.Message = res ? "文章更新成功！" : "文章更新出现异常！";

        return Ok(apiResult);
    }

    [HttpPut("{id}/review")]
    public async Task<IActionResult> ReviewArticle(long id, bool isApproved)
    {
        ApiResult<bool> apiResult = new();

        //查询文章是否存在
        Article? article = await _articleService.FindAsync(id);
        if (article != null)
        {
            // 在这里执行审核文章的逻辑
            if (isApproved) article.PublishStatus = 2;
            else article.PublishStatus = 2;

            bool res = await _articleService.UpdateAsync(article);
            apiResult.Data = res;
            apiResult.Message = res ? "文章更新成功！" : "文章更新出现异常！";
            return Ok(apiResult);
        }
        apiResult.Data = false;
        apiResult.Message = "文章更新出现异常！";
        return Ok(apiResult);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteArticle(long id)
    {
        ApiResult<bool> result = new();
        try
        {
            Article? article = await _articleService.FindAsync(id);
            if (article != null)
            {
                if (article.CoverImage != null)
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, article.CoverImage);
                    // 删除文件
                    System.IO.File.Delete(filePath);
                }

                // 执行删除图片逻辑
                // 暂缓
                bool res = await _articleService.DeleteAsync(id);

                result.Code = res ? 0 : -1;
                result.Data = res;
                result.Message = res ? "文章删除成功！" : "文章删除出现异常！";
                return Ok(result);
            }
            else
            {
                result.Code = -1;
                result.Data = false;
                result.Message = "找不到指定的文章！";
                return Ok(result);
            }
        }
        catch (Exception ex)
        {
            result.Code = -1;
            result.Data = false;
            result.Message = ex.Message;
            return Ok(result);
        }

    }
}
