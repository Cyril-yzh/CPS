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

    public class ArticleService(IBaseRepository<Article> repository) : BaseService<Article>(repository), IArticleService
    {
        /// <summary>
        /// 获取文章列表列表数据分页
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="classifyId">分类</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示行</param>
        public async Task<ApiResult<PageData<Article>>> GetArticleListPageAsync(string? title, string? content
            , int? classifyId, int? publishedStatus, int pageIndex = 1, int pageSize = 10)
        {
            //基于 efcore 通过条件检索
            IQueryable<Article> query = _repository.Query()
                .WhereIf(e => e.PublishStatus == publishedStatus, publishedStatus != null)
                .WhereIf(e => e.Title.Contains(title ?? ""), !string.IsNullOrEmpty(title))
                .WhereIf(e => e.Content.Contains(content ?? ""), !string.IsNullOrEmpty(content))
                .WhereIf(e => e.ClassifyId == classifyId, classifyId != null);
            //获取文章数量
            int count = query.Count();
            //获取文章分页数据
            List<Article> articles = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            //分页返回类
            PageData<Article> pageData = new()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                List = articles,
                Total = count
            };
            ApiResult<PageData<Article>> apiResult = new()
            {
                Data = pageData,
                Message = "获取分页文章数据列表！"
            };
            return apiResult;
        }

        public async Task<ApiResult<PageData<Article>>> GetArticleListPageAsync(string? keyword, int? classifyId
            , int? publishedStatus, int pageIndex = 1, int pageSize = 10)
        {
            //获取分页文章数据列表、数据总数
            List<Article> articleList = await Query(e => !e.IsDelete ?? false)
                .WhereIf(e => e.PublishStatus == publishedStatus, publishedStatus != null)
                .WhereIf(e => e.Title.Contains(keyword ?? "")
                    || e.Content.Contains(keyword ?? ""), !string.IsNullOrEmpty(keyword))
                .WhereIf(e => e.ClassifyId == classifyId, classifyId != null)
                .Paging(pageIndex, pageSize, out int count)   //合理使用扩展方法
                .ToListAsync();

            //构造一个分页返回数据
            PageData<Article> pageData = new()
            {
                Total = count,
                List = articleList,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            ApiResult<PageData<Article>> apiResult = new()
            {
                Data = pageData,
                Message = "获取分页文章数据列表~"
            };

            return apiResult;
        }


    }
}
