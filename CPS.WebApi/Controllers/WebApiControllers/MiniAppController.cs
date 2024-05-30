using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CPS.Service.IServices;

namespace CPS.API.Controllers.WebApiControllers;
[Route("api/[controller]")]
[ApiController]
public class MiniAppController(IClassifyService classifyService, IArticleService articleService) : ControllerBase
{
    private readonly IClassifyService _classifyService = classifyService;
    private readonly IArticleService _articleService = articleService;

    /// <summary>
    /// 根据key标识，获取分类列表
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    [HttpGet, Route("GetClassifys")]
    public async Task<IActionResult> GetClassifys(string? key)
        => Ok(await _classifyService.GetClassifyListAsync(key));

    /// <summary>
    /// 根据条件关键字、分类id，获取分页文章数据列表
    /// </summary>
    /// <param name="keyword">关键字</param>
    /// <param name="classifyId">分类id</param>
    /// <param name="pageIndex">当前页码</param>
    /// <param name="pageSize">每页显示数</param>
    /// <returns></returns>
    [HttpGet, Route("GetArticles")]
    public async Task<IActionResult> GetArticles(string? keyword, int? classifyId, int? publishedStatus, int pageIndex = 1
        , int pageSize = 10)
        => Ok(await _articleService.GetArticleListPageAsync(keyword, classifyId, publishedStatus, pageIndex, pageSize));

}
