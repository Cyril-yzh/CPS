using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS.Infrastruction.Jwt;
/// <summary>
/// 存储Jwt配置信息
/// </summary>
public class TokenOptions
{
    /// <summary>
    /// 令牌密钥
    /// </summary>
    [JsonProperty("secret")]
    public string Secret { get; set; } = null!;
    /// <summary>
    /// 颁发者
    /// </summary>
    [JsonProperty("issuer")]
    public string? Issuer { get; set; }
    /// <summary>
    /// 接收者
    /// </summary>
    [JsonProperty("audience")]
    public string? Audience { get; set; }
    /// <summary>
    /// Token令牌过期时间（分钟）
    /// </summary>
    [JsonProperty("accessExpiration")]
    public int AccessExpiration { get; set; }
    /// <summary>
    /// Token刷新令牌过期时间（分钟）
    /// </summary>
    [JsonProperty("refreshExpiration")]
    public int RefreshExpiration { get; set; }

}
