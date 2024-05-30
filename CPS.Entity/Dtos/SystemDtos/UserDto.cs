using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CPS.Entity.SysEntitys
{
    /// <summary>
    /// 用户更新密码
    /// </summary>
    public class UserDto
    {
        public long Id { get; set; }
        /// <summary>
        /// 角色id
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public required string UserName { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public required string UserPwd { get; set; }
        public long? CreateUserId { get; set; }
        public string? CreateUserName { get; set; }
    }
}