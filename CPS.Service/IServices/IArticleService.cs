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
    public interface IArticleService : IBaseService<Article>
    {
        /// <summary>
        /// 获取文章列表列表数据分页
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="classifyId">分类</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示行</param>
        /// <returns></returns>
        Task<ApiResult<PageData<Article>>> GetArticleListPageAsync(string? title, string? content, int? classifyId, int? publishedStatus, int pageIndex = 1, int pageSize = 10);

        /// <summary>
        /// 根据关键字，分页条件获取文章数据列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="classifyId">分类id</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示数</param>
        /// <returns></returns>
        Task<ApiResult<PageData<Article>>> GetArticleListPageAsync(string? keyword, int? classifyId, int? publishedStatus, int pageIndex = 1, int pageSize = 10);

    }
}
