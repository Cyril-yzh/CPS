#nullable disable
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPS.Entity.SysEntitys;
using CPS.Service.IServices;
using CPS.Entity;
using CPS.Entity.Business;
using CPS.Service.Base;
using CPS.Repository;

namespace CPS.Service
{
    public class RoleService(IBaseRepository<SysRole> repository) : BaseService<SysRole>(repository), IRoleService
    {
        public async Task<ApiResult<List<SysRole>>> GetRoleListAsync()
        {
          
            List<SysRole> roles = await Query(e => e.IsDelete == false).ToListAsync();
           
            ApiResult<List<SysRole>> apiResult = new()
            {
                Data = roles,
                Message = "获取用户数据列表~"
            };
            return apiResult;
        }
    }
}
