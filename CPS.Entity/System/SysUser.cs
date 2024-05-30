using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CPS.Entity.SysEntitys
{
    /// <summary>
    /// 用户表
    /// </summary>
    [Table("Sys_User")]
    public class SysUser : BaseEntity<long>
    {
        /// <summary>
        /// 角色id
        /// </summary>
        [ForeignKey(nameof(SysRole))]   //"SysRole"
        public int RoleId { get; set; }
        public virtual SysRole? SysRole { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        [Required, StringLength(100)]
        public string UserPwd { get; set; } = null!;
        /// <summary>
        /// 用户名称
        /// </summary>
        [Required, StringLength(50)]
        public string Name { get; set; } = null!;
        /// <summary>
        /// 邮箱号
        /// </summary>
        [StringLength(20)]
        public string? Email { get; set; }
    }
}