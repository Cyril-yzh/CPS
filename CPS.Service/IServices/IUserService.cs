using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPS.Entity.Business;
using CPS.Entity;
using CPS.Entity.SysEntitys;
using CPS.Service.Base;

namespace CPS.Service.IServices
{
    public interface IUserService : IBaseService<SysUser>
    {
        Task<SysUser> VerifyUser(string userName, string password);
        Task<ApiResult<PageData<SysUser>>> GetUserListPageAsync(int pageIndex = 1, int pageSize = 10);
    }
}
