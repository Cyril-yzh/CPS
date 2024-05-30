using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CPS.Entity.SysEntitys
{
    /// <summary>
    /// 用户更新密码
    /// </summary>
    public class ChangePasswordDto
    {
        /// <summary>
        /// 角色id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 如果是admin权限即无需旧密码
        /// 如果是其他权限则需输入验证身份
        /// </summary>
        public string? OldPwd { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public required string NewPwd { get; set; }
    }
}