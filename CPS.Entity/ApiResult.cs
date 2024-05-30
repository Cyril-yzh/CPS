using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS.Entity;
/// <summary>
/// API接口通用返回类
/// </summary>
/// <typeparam name="T"></typeparam>
public class ApiResult<T>
{
    /// <summary>
    /// 状态码，默认200
    /// </summary>
    public int Code { get; set; } = 200;
    /// <summary>
    /// 通用数据
    /// </summary>
    public T? Data { get; set; }
    /// <summary>
    /// 消息
    /// </summary>
    public string? Message { get; set; }

}
