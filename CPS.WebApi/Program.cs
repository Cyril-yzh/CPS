using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CPS.Infrastruction.Jwt;
using CPS.Entity.Business;
using CPS.Entity.SysEntitys;
using CPS.EntityFrameworkCore;
using CPS.Repository;
using CPS.Service.IServices;
using CPS.Service;

namespace CPS.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigurationManager configuration = builder.Configuration;

            builder.Services.AddControllers();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Cors", config =>
                {
                    config.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });

            builder.Services.AddDbContext<SqlDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("strConn") ?? throw new Exception("未填写Sql连接字符串"));
            });

            #region 仓储依赖注入
            //用户分享表仓储
            builder.Services.AddScoped<IBaseRepository<Share>, BaseRepository<Share>>();
            //图片表仓储
            builder.Services.AddScoped<IBaseRepository<Image>, BaseRepository<Image>>();
            //视频表仓储
            builder.Services.AddScoped<IBaseRepository<Video>, BaseRepository<Video>>();
            //文章表仓储
            builder.Services.AddScoped<IBaseRepository<Article>, BaseRepository<Article>>();
            //文章类别表仓储
            builder.Services.AddScoped<IBaseRepository<Classify>, BaseRepository<Classify>>();

            //系统用户表仓储
            builder.Services.AddScoped<IBaseRepository<SysUser>, BaseRepository<SysUser>>();
            //系统用户角色表仓储
            builder.Services.AddScoped<IBaseRepository<SysRole>, BaseRepository<SysRole>>();
            //builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            #endregion

            #region 业务Service依赖注入关系
            builder.Services.AddScoped<IArticleService, ArticleService>();   //文章业务
            builder.Services.AddScoped<IClassifyService, ClassifyService>();   //分类业务服务
            builder.Services.AddScoped<IImageService,ImageService>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            #endregion

            #region Jwt授权认证配置

            //读取Jwt配置信息
            var jwtSection = builder.Configuration.GetSection("TokenOptions");
            //Jwt配置信息，注入配置中
            builder.Services.Configure<TokenOptions>(jwtSection);

            TokenOptions tokenManagement = jwtSection.Get<TokenOptions>() ?? throw new AggregateException("初始化 TokenOptions 失败，请检查配置");

            //Jwt鉴权配置
            builder.Services.AddAuthorization(options => { }).AddAuthentication(options =>
            {
                // .net core 官方 JWT 认证,开启 Bearer 认证
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,                                                                //是否验证SecurityKey
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenManagement.Secret)),    //这里的key要进行加密
                    ValidateIssuer = true,                                                                          //验证发布者Issuer
                    ValidIssuer = tokenManagement.Issuer,                                                           //Token颁发机构
                    ValidateAudience = true,                                                                        //验证接收者
                    ValidAudience = tokenManagement.Audience,                                                       //颁发给谁   
                    ValidateLifetime = true,                                                                        //验证token过期时间
                    ClockSkew = TimeSpan.FromSeconds(30),                                                           //时钟缓冲相位差
                    RequireExpirationTime = true,                                                                   //token需要设置过期时间
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        // 如果过期，则把<是否过期>添加到，返回头信息中
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.TryAdd("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            #endregion

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("Cors");

            app.UseStaticFiles();       //静态文件资源

            app.UseAuthentication();    //启用身份认证

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
