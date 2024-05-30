using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS.Entity.Business;
/// <summary>
/// 分类表
/// </summary>
public class Classify:BaseEntity<int>
{
    /// <summary>
    /// 分类名称
    /// </summary>
    [StringLength(100)]
    public required string Name { get; set; }

    /// <summary>
    /// 是否显示在导航栏
    /// </summary>
    public bool IsShowNav { get; set; }


}
