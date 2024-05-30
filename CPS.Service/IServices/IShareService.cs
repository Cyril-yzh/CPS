using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPS.Entity;
using CPS.Entity.Business;
using CPS.Service.Base;

namespace CPS.Service.IServices
{
    public interface IShareService : IBaseService<Share>
    {
        /// <summary>
        /// 根据key标识，获取分享列表
        /// </summary>
        /// <param name="key">标识</param>
        Task<ApiResult<PageData<Share>>> GetShareListPageAsync(int? publishStatus, int pageIndex = 1, int pageSize = 10);
    }
}
