using CPS.Entity;
using CPS.Entity.Dtos.SystemDtos;
using CPS.Entity.SysEntitys;

namespace CPS.Service.IServices
{
    public interface IAuthService
    {
        /// <summary>
        /// 后台管理系统登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        Task<ApiResult<AuthDto>> Login(string userName, string password);


    }
}
