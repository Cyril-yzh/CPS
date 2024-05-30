using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPS.Entity.Business;
using CPS.Entity;
using CPS.Entity.SysEntitys;
using System.Data;
using CPS.Service.Base;

namespace CPS.Service.IServices
{
    public interface IRoleService : IBaseService<SysRole>
    {
        Task<ApiResult<List<SysRole>>> GetRoleListAsync();
    }
}
