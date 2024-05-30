using Microsoft.AspNetCore.Mvc;
using CPS.Entity;
using CPS.Entity.Business;
using CPS.Service.IServices;
using System.Security.Cryptography;
using CPS.Entity.SysEntitys;
using CPS.Service;
using System.Text;

namespace CPS.WebApi.Controllers.SystemControllers;
/// <summary>
/// 文章API接口
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    /// <summary>
    /// 获取用户列表数据分页
    /// </summary>
    /// <param name="pageIndex">当前页</param>
    /// <param name="pageSize">每页显示行</param>
    /// <returns></returns>
    [HttpGet, Route("GetUserListPage")]
    public async Task<IActionResult> GetUserListPage(int pageIndex = 1, int pageSize = 10)
    {
        ApiResult<PageData<SysUser>> apiResult = await _userService.GetUserListPageAsync(pageIndex, pageSize);

        return Ok(apiResult);
    }
    /// <summary>
    /// 发布文章
    /// </summary>
    /// <param name="articleDto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> PostClassify(UserDto user)
    {
        ApiResult<bool> apiResult = new();
        if (!string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.UserPwd))
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(user.UserPwd);
            byte[] hashBytes = SHA256.HashData(passwordBytes);
            string encryptedPassword = Convert.ToBase64String(hashBytes);

            SysUser sysUser = new()
            {
                UserPwd = encryptedPassword,
                Name = user.UserName,
                RoleId = user.RoleId,
                CreateUserId = user.CreateUserId,
                CreateUserName = user.CreateUserName,
            };
            bool res = await _userService.AddAsync(sysUser);
            apiResult.Data = res;
            apiResult.Message = res ? "用户创建成功！" : "用户创建出现异常！";
            return Ok(apiResult);
        }
        apiResult.Data = false;
        apiResult.Message = "用户名称为空！";
        return Ok(apiResult);
    }



    /// <summary>
    /// 更新用户信息
    /// </summary>
    /// <param name="articleDto"></param>
    /// <returns></returns>
    [HttpPut("ChangePassword")]
    public async Task<IActionResult> ChangePassword(UserDto user)
    {
        ApiResult<bool> apiResult = new();

        //查询文章是否存在
        SysUser? userRes = await _userService.FindAsync(user.Id);
        if (userRes == null)
        {
            apiResult.Code = 404;
            apiResult.Message = "用户详情信息不存在！";
            return Ok(userRes);
        }

        byte[] passwordBytes = Encoding.UTF8.GetBytes(user.UserPwd);
        byte[] hashBytes = SHA256.HashData(passwordBytes);
        string encryptedPassword = Convert.ToBase64String(hashBytes);

        //更新密码
        userRes.UserPwd = encryptedPassword;

        bool res = await _userService.UpdateAsync(userRes);

        apiResult.Data = res;
        apiResult.Message = res ? "用户更新成功！" : "用户更新出现异常！";

        return Ok(apiResult);
    }

    /// <summary>
    /// 更新用户信息
    /// </summary>
    /// <param name="articleDto"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> PutClassify(UserDto user)
    {
        ApiResult<bool> apiResult = new();

        //查询文章是否存在
        SysUser? userRes = await _userService.FindAsync(user.Id);
        if (userRes == null)
        {
            apiResult.Code = 404;
            apiResult.Message = "用户详情信息不存在！";
            return Ok(userRes);
        }

        //更新文章的信息
        userRes.Name = user.UserName;
        userRes.UpdateTime = DateTime.Now;
        userRes.RoleId = user.RoleId;

        bool res = await _userService.UpdateAsync(userRes);

        apiResult.Data = res;
        apiResult.Message = res ? "用户更新成功！" : "用户更新出现异常！";

        return Ok(apiResult);
    }


    [HttpDelete]
    public async Task<IActionResult> DeleteClassify(long id)
    {
        ApiResult<bool> result = new();
        try
        {
            SysUser? user = await _userService.FindAsync(id);
            if (user != null)
            {
                // 执行删除图片逻辑
                // 暂缓
                bool res = await _userService.DeleteAsync(id);

                result.Code = res ? 0 : -1;
                result.Data = res;
                result.Message = res ? "用户删除成功！" : "用户删除出现异常！";
                return Ok(result);
            }
            else
            {
                result.Code = -1;
                result.Data = false;
                result.Message = "找不到指定的用户！";
                return Ok(result);
            }
        }
        catch (Exception ex)
        {
            result.Code = -1;
            result.Data = false;
            result.Message = ex.Message;
            return Ok(result);
        }
    }
}
