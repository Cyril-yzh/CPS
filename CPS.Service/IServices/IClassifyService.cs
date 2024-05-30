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
    public interface IClassifyService : IBaseService<Classify>
    {
        /// <summary>
        /// 根据key标识，获取分类列表
        /// </summary>
        /// <param name="key">标识</param>
        /// <returns></returns>
        Task<ApiResult<List<Classify>>> GetClassifyListAsync(string? key);
        /// <summary>
        /// 获取文章列表列表数据分页
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="classifyId">分类</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示行</param>
        /// <returns></returns>
        Task<ApiResult<PageData<Classify>>> GetClassifyListPageAsync(int pageIndex = 1, int pageSize = 10);
    }
}
