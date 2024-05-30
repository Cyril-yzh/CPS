using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CPS.Core.Helpers;
using CPS.Entity;
using CPS.Entity.Business;
using CPS.Entity.Dtos.BusDtos.ArticleDtos;
using CPS.Service.BusServices;
using CPS.Service.IServices.IBusService;

namespace CPS.API.Controllers.WebApiControllers;
/// <summary>
/// 文章API接口
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ArticleController : ControllerBase
{
    private readonly IClassifyService _classifyService;
    private readonly IArticleService _articleService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ArticleController(IClassifyService classifyService, IArticleService articleService, IWebHostEnvironment webHostEnvironment)
    {
        this._classifyService = classifyService;
        this._articleService = articleService;
        this._webHostEnvironment = webHostEnvironment;
    }

    /// <summary>
    /// 根据key标识，获取分类列表
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    [HttpGet, Route("GetClassifyList")]
    public async Task<IActionResult> GetClassifyList(string key)
        => Ok(await _classifyService.GetClassifyList(key));


    /// <summary>
    /// 获取文章列表列表数据分页
    /// </summary>
    /// <param name="title">标题</param>
    /// <param name="content">内容</param>
    /// <param name="classifyId">分类</param>
    /// <param name="pageIndex">当前页</param>
    /// <param name="pageSize">每页显示行</param>
    /// <returns></returns>
    [HttpGet, Route("GetArticleListPage")]
    public async Task<IActionResult> GetArticleListPage(string title, string content, int? classifyId
        , int pageIndex = 1, int pageSize = 10)
    {
        ApiResult<PageData<Article>> apiResult = await _articleService.GetArticleListPageAsync(title, content
            , classifyId, pageIndex, pageSize);

        return Ok(apiResult);
    }

    /// <summary>
    /// 发布文章
    /// </summary>
    /// <param name="articleDto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> PostAddArticle(AddEditArticleDto articleDto)
    {
        Article article = new Article();
        article.Title = articleDto.Title;   //AutoMapper框架，他是对象映射关系
        article.Content = articleDto.Content;
        article.BrowseCount = articleDto.BrowseCount;
        article.SupportCount = articleDto.SupportCount;
        article.ShareCount = articleDto.ShareCount;
        article.State = articleDto.State;
        article.CollectCount = articleDto.CollectCount;
        article.ClassifyId = articleDto.ClassifyId;
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
            string fileName = $"images/article/{Guid.NewGuid()}{suffix}";  //Guid.NewGuid() + suffix;

            //要保存的文件路径
            string saveFilePath = Path.Combine(_webHostEnvironment.WebRootPath, fileName);

            //写入字节流到指定路径
            await System.IO.File.WriteAllBytesAsync(saveFilePath, base64Bytes);

            article.CoverImage = fileName;
        }


        bool res = await _articleService.AddAsync(article);
        ApiResult<bool> apiResult = new ApiResult<bool>();
        apiResult.Data = res;
        apiResult.Message = res ? "文章发布成功！" : "文章发布出现异常！";

        return Ok(apiResult);
    }

    /// <summary>
    /// 通过文章编号，获取文章详情信息
    /// </summary>
    /// <param name="id">文章编号</param>
    /// <returns></returns>
    [HttpGet, Route("{id}")] //api/article/12
    public async Task<IActionResult> GetArticle(long id)
    {
        Article article = await _articleService.FindAsync(id);

        ApiResult<Article> apiResult = new();
        apiResult.Data = article;
        apiResult.Code = article == null ? 404 : 200;
        apiResult.Message = article == null ? "文章详情信息不存在！" : "获取文章详情信息";


        return Ok(apiResult);
    }

    /// <summary>
    /// 更新文章信息
    /// </summary>
    /// <param name="articleDto"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> PutArticle(AddEditArticleDto articleDto)
    {
        ApiResult<bool> apiResult = new ApiResult<bool>();

        //查询文章是否存在
        Article articleRes = await _articleService.FindAsync(articleDto.Id);
        if(articleRes == null)
        {
            apiResult.Code = 404;
            apiResult.Message = "文章详情信息不存在！";
            return Ok(articleRes);
        }

        //更新文章的信息
        articleRes.Title = articleDto.Title;
        articleRes.Content = articleDto.Content;
        articleRes.BrowseCount = articleDto.BrowseCount;
        articleRes.SupportCount = articleDto.SupportCount;
        articleRes.ShareCount = articleDto.ShareCount;
        articleRes.State = articleDto.State;
        articleRes.CollectCount = articleDto.CollectCount;
        articleRes.ClassifyId = articleDto.ClassifyId;
        if (!string.IsNullOrEmpty(articleDto.CoverImage) && Base64Helper.IsBase64(articleDto.CoverImage))
        {
            string[] base64Array = articleDto.CoverImage.Split(',');
            string dataType = base64Array[0];
            string base64 = base64Array[1];

            //后缀名
            string suffix = "." + dataType.Replace("data:", "").Replace(";base64", "").Split('/')[1];

            //base64转字节数组
            byte[] base64Bytes = Convert.FromBase64String(base64);

            //生成文件名
            string fileName = $"images/article/{Guid.NewGuid()}{suffix}";  //Guid.NewGuid() + suffix;

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


}
