using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CPS.Infrastruction.Jwt;
using CPS.Entity.SysEntitys;
using CPS.Service.IServices;
using CPS.Entity.Business;
using CPS.Entity;
using CPS.Service;

namespace CPS.API.Controllers.SystemControllers;
/// <summary>
/// 系统授权验证API接口
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <param name="password">密码</param>
    /// <returns></returns>
    [HttpPost("Login")]    //api/auth/Login   API调用链接
    public async Task<IActionResult> Login(string userName, string password)
    {
        return Ok(await _authService.Login(userName, password));
    }

    //[HttpGet, Route("GetSystemMenu")]
    //[Authorize]
    //public async Task<IActionResult> GetSystemMenu()
    //{
    //    TokenPayload payload = JwtHelper.GetTokenPayload(HttpContext);

    //    var apiResult = await _menuService.GetSystemMenu(payload.Id);

    //    return Ok(apiResult);
    //}

}
