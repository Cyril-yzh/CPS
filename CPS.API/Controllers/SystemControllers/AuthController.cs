using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CPS.Core.Jwt;
using CPS.Entity.SysEntitys;
using CPS.Service.IServices;
using CPS.Service.SysServices;

namespace CPS.API.Controllers.SystemControllers;
/// <summary>
/// 系统授权验证API接口
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMenuService _menuService;
    //private readonly IUserService _userService;

    public AuthController(IAuthService authService, IMenuService menuService/*, IUserService userService*/)
    {
        this._authService = authService;
        this._menuService = menuService;
        //this._userService = userService;
    }

    //[HttpPost("AddAdmin")]
    //public async Task<IActionResult> AddAdmin(string userName, string password)
    //{
    //    SysUser user = new SysUser()
    //    {
    //        Email = "sivicyu95@gmail.com",
    //        Name = userName,
    //        RoleId = 1,
    //        UserPwd = password
    //    };
    //    return Ok(await _userService.AddAsync(user));
    //}

    /// <summary>
    /// 小鱼记后台管理系统登录
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <param name="password">密码</param>
    /// <returns></returns>
    [HttpPost("AdminLogin")]    //api/auth/AdminLogin   API调用链接
    public async Task<IActionResult> AdminLogin(string userName, string password)
    {
        return Ok(await _authService.AdminLogin(userName, password));
    }

    [HttpGet, Route("GetSystemMenu")]
    [Authorize]
    public async Task<IActionResult> GetSystemMenu()
    {
        TokenPayload payload = JwtHelper.GetTokenPayload(HttpContext);

        var apiResult = await _menuService.GetSystemMenu(payload.Id);

        return Ok(apiResult);
    }

}
