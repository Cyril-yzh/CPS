#nullable disable
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPS.Entity.SysEntitys;
using CPS.Service.IServices;
using CPS.Entity;
using CPS.Entity.Business;
using CPS.Service.Base;
using CPS.Repository;

namespace CPS.Service
{
    public class UserService(IBaseRepository<SysUser> repository) : BaseService<SysUser>(repository), IUserService
    {
        public async Task<ApiResult<PageData<SysUser>>> GetUserListPageAsync(int pageIndex = 1, int pageSize = 10)
        {
            List<SysUser> userList = await _repository.Query().Paging(pageIndex, pageSize, out int count).ToListAsync();

            //构造一个分页返回数据
            PageData<SysUser> pageData = new()
            {
                Total = count,
                List = userList,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            ApiResult<PageData<SysUser>> apiResult = new()
            {
                Data = pageData,
                Message = "获取用户数据列表~"
            };

            return apiResult;
        }

        public async Task<SysUser> VerifyUser(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException(nameof(userName), "用户名不能为空！");

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password), "密码不能为空！");

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = SHA256.HashData(passwordBytes);
            string encryptedPassword = Convert.ToBase64String(hashBytes);

            //查询用户
            SysUser user = await _repository.Query(e => e.Name.Equals(userName)
                && e.UserPwd.Equals(encryptedPassword) && e.IsDelete == false)
                .Include(e => e.SysRole)
                .SingleOrDefaultAsync();

            //SysUser user = await _repository.Query(e => e.Name.Equals(userName)
            //    && e.UserPwd.Equals(password) && e.IsDelete == false)
            //    .Include(e => e.SysRole)
            //    .SingleOrDefaultAsync();

            return user;
        }
    }
}
