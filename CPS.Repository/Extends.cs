using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CPS.Repository
{
    public static class EFExtends
    {
        /// <summary>
        /// 扩展WhereIf动态条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="expression"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> expression, bool condition)
        {
            return condition ? queryable.Where(expression) : queryable;
        }
        /// <summary>
        /// 通用分页扩展方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable">查询对象</param>
        /// <param name="skipNum">跳过多少行</param>
        /// <param name="takeNum">取多少行</param>
        /// <param name="count">数据总数</param>
        /// <returns></returns>
        public static IQueryable<T> Paging<T>(this IQueryable<T> queryable, int pageIndex, int pageSize, out int count)
        {
            count = queryable.Count();

            return queryable.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

    }
}
