using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPS.Entity.Business;
using CPS.Entity.SysEntitys;

namespace CPS.EntityFrameworkCore;
/// <param name="options">数据库连接字符串</param>
public class SqlDbContext(DbContextOptions options) : DbContext(options)
{

    #region 系统表
    public virtual DbSet<SysUser>? SysUsers { get; set; }       //用户表
    public virtual DbSet<SysRole>? SysRoles { get; set; }       //角色表
    #endregion

    #region 业务表
    public virtual DbSet<Image>? Images { get; set; }           //图片表
    public virtual DbSet<Video>? Videos { get; set; }           //视频表
    public virtual DbSet<Share>? Shares { get; set; }            //媒体表
    public virtual DbSet<Classify>? Classifys { get; set; }     //分类表
    public virtual DbSet<Article>? Articles { get; set; }       //文章表
    #endregion

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //全局关闭EF Core数据跟踪
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        optionsBuilder.UseSqlServer("server=localhost;database=MYCPS;uid=sa;pwd=Aa111111;Encrypt=True;TrustServerCertificate=True;");
        base.OnConfiguring(optionsBuilder);
    }
}
