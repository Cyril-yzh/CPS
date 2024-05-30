using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPS.Entity;
using CPS.Entity.Business;
using CPS.Service.IServices;
using CPS.Service.Base;
using CPS.Repository;

namespace CPS.Service
{
    public class ClassifyService(IBaseRepository<Classify> repository) : BaseService<Classify>(repository), IClassifyService
    {
        public async Task<ApiResult<PageData<Classify>>> GetClassifyListPageAsync(int pageIndex = 1, int pageSize = 10)
        {
            List<Classify> classifyList = await Query(e => e.IsDelete == false).Paging(pageIndex, pageSize, out int count).ToListAsync();
            //构造一个分页返回数据
            PageData<Classify> pageData = new()
            {
                Total = count,
                List = classifyList,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            ApiResult<PageData<Classify>> apiResult = new()
            {
                Data = pageData,
                Message = "获取分页文章数据列表~"
            };

            return apiResult;
        }

        public async Task<ApiResult<List<Classify>>> GetClassifyListAsync(string? key)
        {
            List<Classify> classifyList = await Query(e => e.IsDelete == false).ToListAsync();

            ApiResult<List<Classify>> apiResult = new()
            {
                Data = classifyList,
                Message = "获取分类列表~"
            };

            return apiResult;
        }
    }
}
