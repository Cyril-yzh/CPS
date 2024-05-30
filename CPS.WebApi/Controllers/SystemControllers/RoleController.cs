using Microsoft.AspNetCore.Mvc;
using CPS.Entity;
using CPS.Entity.Business;
using CPS.Service.IServices;
using CPS.Entity.Dtos.BusinessDtos;
using CPS.Entity.SysEntitys;

namespace CPS.WebApi.Controllers.SystemControllers;
/// <summary>
/// 文章API接口
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class RoleController(IRoleService roleService) : ControllerBase
{
    private readonly IRoleService _roleService = roleService;

    /// <summary>
    /// 获取用户列表数据分页
    /// </summary>
    /// <param name="pageIndex">当前页</param>
    /// <param name="pageSize">每页显示行</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        ApiResult<List<SysRole>> apiResult = await _roleService.GetRoleListAsync();

        return Ok(apiResult);
    }

}
