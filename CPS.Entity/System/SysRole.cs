using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS.Entity.SysEntitys
{
    /// <summary>
    /// 角色表
    /// </summary>
    [Table("Sys_Role")]
    public class SysRole : BaseEntity<int>
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [Required, StringLength(30)]
        public string RoleName { get; set; } = null!;

        /// <summary>
        /// 角色介绍
        /// </summary>
        [Required, StringLength(100)]
        public string Desc { get; set; } = null!;

    }
}
