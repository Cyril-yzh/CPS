using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS.Entity
{
    /// <summary>
    /// 通用分页返回类
    /// </summary>
    public class PageData<TEntity>
    {
        public required List<TEntity> List { get; set; }
        public int Total { get; set; }
        public int PageIndex { get; set; }

        public int PageSize { get; set; }


    }
}