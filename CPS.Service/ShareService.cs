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
    public class ShareService(IBaseRepository<Share> repository) : BaseService<Share>(repository), IShareService
    {
        /// <summary>
        /// 获取文章列表列表数据分页
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="classifyId">分类</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示行</param>
        public async Task<ApiResult<PageData<Share>>> GetShareListPageAsync(int? publishStatus, int pageIndex = 1, int pageSize = 10)
        {
            //基于 efcore 通过条件检索
            IQueryable<Share> query = _repository.Query()
                .WhereIf(e => e.PublishStatus == publishStatus, publishStatus != null);
            //获取文章数量
            int count = _repository.Count();
            //获取文章分页数据
            List<Share> shares = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            //分页返回类
            PageData<Share> pageData = new()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                List = shares,
                Total = count
            };
            ApiResult<PageData<Share>> apiResult = new()
            {
                Data = pageData,
                Message = "获取分页文章数据列表！"
            };
            return apiResult;
        }
    }
}
