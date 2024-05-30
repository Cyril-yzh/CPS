using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CPS.Service.IServices.IBusService;

namespace CPS.API.Controllers.WebApiControllers;
[Route("api/[controller]")]
[ApiController]
public class MiniAppController : ControllerBase
{
    private readonly IClassifyService _classifyService;
    private readonly IArticleService _articleService;

    public MiniAppController(IClassifyService classifyService,IArticleService articleService)
    {
        this._classifyService = classifyService;
        this._articleService = articleService;
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
    /// 根据条件关键字、分类id，获取分页文章数据列表
    /// </summary>
    /// <param name="keyword">关键字</param>
    /// <param name="classifyId">分类id</param>
    /// <param name="pageIndex">当前页码</param>
    /// <param name="pageSize">每页显示数</param>
    /// <returns></returns>
    [HttpGet,Route("GetArticleListPage")]
    public async Task<IActionResult> GetArticleListPage(string keyword,int? classifyId, int pageIndex = 1
        , int pageSize = 10)
        => Ok(await _articleService.GetArticleListPageAsync(keyword,classifyId,pageIndex,pageSize));

}
