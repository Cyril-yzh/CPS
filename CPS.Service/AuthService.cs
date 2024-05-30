using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CPS.Infrastruction.Jwt;
using CPS.Entity;
using CPS.Entity.Dtos.SystemDtos;
using CPS.Entity.SysEntitys;
using CPS.Service.IServices;

namespace CPS.Service
{
    public class AuthService(IUserService userService, IOptions<TokenOptions> tokenManagement) : IAuthService
    {
        private readonly IUserService _userService = userService;
        private readonly IOptions<TokenOptions> _tokenManagement = tokenManagement;

        /// <summary>
        /// 后台管理系统登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public async Task<ApiResult<AuthDto>> Login(string userName, string password)
        {
            ApiResult<AuthDto> apiResult = new();
            SysUser user = await _userService.VerifyUser(userName, password);   //验证用户名、密码是否正确
            if (user == null)
            {
                apiResult.Code = -1;    //-1代表出现错误
                apiResult.Message = "用户信息不存在！";
                return apiResult;
            }
            try
            {
                List<Claim> claimList =     //存在用户信息，生成token
                [
                    new Claim(ClaimTypes.Sid, user.Id.ToString()),
                    new Claim(ClaimTypes.Surname, user.Name),
                    new Claim(ClaimTypes.Name, user.Name ?? ""),
                    new Claim(ClaimTypes.Email, user.Email ?? ""),
                    new Claim(ClaimTypes.Role, user.SysRole?.RoleName ?? "")
                ];
                string token = JwtHelper.CreateToken([.. claimList], _tokenManagement.Value);   //生成token
                if (string.IsNullOrEmpty(token))
                {
                    apiResult.Code = -1;    //-1代表出现错误
                    apiResult.Message = "用户信息验证异常！";
                    return apiResult;
                }
                apiResult.Code = 200;
                apiResult.Data = new AuthDto
                {
                    Token = token,
                    VerifyResult = true,
                    Data = new
                    {
                        UserId = user.Id,
                        UserName = user.Name,
                        user.Name,
                        user.Email,
                        user.SysRole?.RoleName,
                    }
                };
                apiResult.Message = "系统用户登录成功！";
                return apiResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }
    }
}
