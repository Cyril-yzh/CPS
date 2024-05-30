using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CPS.Infrastruction.Jwt;
public static class JwtHelper
{
    /// <summary>
    /// 生成token令牌
    /// </summary>
    /// <param name="claimInfo">要存储在token中的信息</param>
    /// <returns></returns>
    public static string CreateToken(Claim[] claimInfo, TokenOptions options)
    {
        //1、获取我们的TokenManagement类中，Secret密钥字符串
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Secret));
        //2、通过key加密方式
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //3、生成token的时间
        var notTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")); //开始时间
        //4、token到期时间，相对于生成token的开始时间
        var expiresTime = notTime.AddMinutes(options.AccessExpiration);    //到期时间

        //5、设置生成token的一些配置信息
        var jwtToken = new JwtSecurityToken(options.Issuer
            , options.Audience
            , claimInfo
            , notTime
            , expiresTime
            , credentials);

        string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return token;
    }

    /// <summary>
    /// 获取请求上下文Claims中的UserId
    /// </summary>
    /// <param name="httpContext">请求上下文</param>
    public static string? GetUserId(HttpContext httpContext)
    {
        //if (httpContext.User is null|| !httpContext.User.Claims.Any())
        //    return null;
        return httpContext.User.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Sid)?.Value;
    }

    /// <summary>
    /// 获取请求上下文Claims中的Name名称
    /// </summary>
    /// <param name="httpContext">请求上下文</param>
    public static string? GetName(HttpContext httpContext)
    {
        //if (httpContext.User is null|| !httpContext.User.Claims.Any())
        //    return null;
        return httpContext.User.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Name)?.Value;
    }

    /// <summary>
    /// 解析token获取JwtSecurityToken对象
    /// </summary>
    /// <param name="token">token</param>
    public static JwtSecurityToken GetSecurityToken(string token) => new JwtSecurityTokenHandler().ReadJwtToken(GetRemoveBearerToken(token));

    /// <summary>
    /// 获取token中存储的信息
    /// </summary>
    /// <param name="httpContext">http数据上下文</param>
    public static TokenPayload GetTokenPayload(HttpContext httpContext)
    {
        //解析token
        JwtSecurityToken jwtToken = GetSecurityToken(GetRemoveBearerToken(httpContext.Request.Headers.Authorization.ToString()));
      
        //获取token中的payload信息
        TokenPayload tokenPayload = new()
        {
            Id = Convert.ToInt32(jwtToken.Payload[ClaimTypes.Sid]),
            Email = jwtToken.Payload[ClaimTypes.Email].ToString(),
            UserName = jwtToken.Payload[ClaimTypes.Surname].ToString(),
            Name = jwtToken.Payload[ClaimTypes.Name].ToString(),
            Role = jwtToken.Payload[ClaimTypes.Role]?.ToString(),
        };

        return tokenPayload;
    }

    /// <summary>
    /// 获取token中存储的信息
    /// </summary>
    /// <param name="token">token令牌</param>
    public static TokenPayload GetTokenPayload(string token)
    {
        //解析token
        JwtSecurityToken jwtToken = GetSecurityToken(token);
        //获取token中的payload信息
        TokenPayload tokenPayload = new()
        {
            Id = Convert.ToInt32(jwtToken.Payload[ClaimTypes.Sid]),
            Email = jwtToken.Payload[ClaimTypes.Email].ToString(),
            UserName = jwtToken.Payload[ClaimTypes.Surname].ToString(),
            Name = jwtToken.Payload[ClaimTypes.Name].ToString(),
            Role = jwtToken.Payload[ClaimTypes.Role]?.ToString(),
        };

        return tokenPayload;
    }

    /// <summary>
    /// 获取去除字符串开头Bearer的token令牌
    /// </summary>
    /// <param name="token">token令牌</param>
    public static string GetRemoveBearerToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            throw new ArgumentNullException(token, "传入的token为空!");

        return token.StartsWith("Bearer ")? token.Replace("Bearer ", ""):token;
    }
}
