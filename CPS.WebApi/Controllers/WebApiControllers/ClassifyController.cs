using Microsoft.AspNetCore.Mvc;
using CPS.Entity;
using CPS.Entity.Business;
using CPS.Service.IServices;
using CPS.Entity.Dtos.BusinessDtos;
using CPS.Service;
using static System.Net.Mime.MediaTypeNames;

namespace CPS.API.Controllers.WebApiControllers;
/// <summary>
/// 文章API接口
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ClassifyController(IClassifyService classifyService) : ControllerBase
{
    private readonly IClassifyService _classifyService = classifyService;

    /// <summary>
    /// 根据key标识，获取分类列表
    /// </summary>
    /// <param name="key"></param>
    [HttpGet("All")]
    public async Task<IActionResult> GetClassifys(string? key)
        => Ok(await _classifyService.GetClassifyListAsync(key));

    /// <summary>
    /// 根据key标识，获取分类列表
    /// </summary>
    /// <param name="key"></param>
    [HttpGet]
    public async Task<IActionResult> GetClassifyListPage(int pageIndex = 1, int pageSize = 10)
        => Ok(await _classifyService.GetClassifyListPageAsync(pageIndex, pageSize));

    /// <summary>
    /// 发布文章
    /// </summary>
    /// <param name="articleDto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> PostClassify(Classify classify)
    {
        ApiResult<bool> apiResult = new();
        
        if (!string.IsNullOrEmpty(classify.Name))
        {
            classify.IsDelete = false;
            bool res = await _classifyService.AddAsync(classify);
            apiResult.Data = res;
            apiResult.Message = res ? "类别创建成功！" : "类别创建出现异常！";
            return Ok(apiResult);
        }
        apiResult.Data = false;
        apiResult.Message = "类别名称为空！";
        return Ok(apiResult);
    }

    /// <summary>
    /// 更新文章信息
    /// </summary>
    /// <param name="articleDto"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> PutClassify(Classify classify)
    {
        ApiResult<bool> apiResult = new();

        //查询文章是否存在
        Classify? classifyRes = await _classifyService.FindAsync(classify.Id);
        if (classifyRes == null)
        {
            apiResult.Code = 404;
            apiResult.Message = "类别详情信息不存在！";
            return Ok(classifyRes);
        }
        //更新文章的信息
        classifyRes.Name = classify.Name;
        classifyRes.UpdateTime = DateTime.Now;
        classifyRes.IsShowNav = classify.IsShowNav;

        bool res = await _classifyService.UpdateAsync(classifyRes);

        apiResult.Data = res;
        apiResult.Message = res ? "文章更新成功！" : "文章更新出现异常！";

        return Ok(apiResult);
    }


    [HttpDelete]
    public async Task<IActionResult> DeleteClassify(int id)
    {
        ApiResult<bool> result = new();
        try
        {
            Classify? classify = await _classifyService.FindAsync(id);
            if (classify != null)
            {
                // 执行删除图片逻辑
                // 暂缓
                bool res = await _classifyService.DeleteAsync(id);

                result.Code = res ? 0 : -1;
                result.Data = res;
                result.Message = res ? "类别删除成功！" : "类别删除出现异常！";
                return Ok(result);
            }
            else
            {
                result.Code = -1;
                result.Data = false;
                result.Message = "找不到指定的类别！";
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
