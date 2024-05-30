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
                options.UseSqlServer(configuration.GetConnectionString("strConn") ?? throw new Exception("δ��дSql�����ַ���"));
            });

            #region �ִ�����ע��
            //�û������ִ�
            builder.Services.AddScoped<IBaseRepository<Share>, BaseRepository<Share>>();
            //ͼƬ��ִ�
            builder.Services.AddScoped<IBaseRepository<Image>, BaseRepository<Image>>();
            //��Ƶ��ִ�
            builder.Services.AddScoped<IBaseRepository<Video>, BaseRepository<Video>>();
            //���±�ִ�
            builder.Services.AddScoped<IBaseRepository<Article>, BaseRepository<Article>>();
            //��������ִ�
            builder.Services.AddScoped<IBaseRepository<Classify>, BaseRepository<Classify>>();

            //ϵͳ�û���ִ�
            builder.Services.AddScoped<IBaseRepository<SysUser>, BaseRepository<SysUser>>();
            //ϵͳ�û���ɫ��ִ�
            builder.Services.AddScoped<IBaseRepository<SysRole>, BaseRepository<SysRole>>();
            //builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            #endregion

            #region ҵ��Service����ע���ϵ
            builder.Services.AddScoped<IArticleService, ArticleService>();   //����ҵ��
            builder.Services.AddScoped<IClassifyService, ClassifyService>();   //����ҵ�����
            builder.Services.AddScoped<IImageService,ImageService>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            #endregion

            #region Jwt��Ȩ��֤����

            //��ȡJwt������Ϣ
            var jwtSection = builder.Configuration.GetSection("TokenOptions");
            //Jwt������Ϣ��ע��������
            builder.Services.Configure<TokenOptions>(jwtSection);

            TokenOptions tokenManagement = jwtSection.Get<TokenOptions>() ?? throw new AggregateException("��ʼ�� TokenOptions ʧ�ܣ���������");

            //Jwt��Ȩ����
            builder.Services.AddAuthorization(options => { }).AddAuthentication(options =>
            {
                // .net core �ٷ� JWT ��֤,���� Bearer ��֤
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,                                                                //�Ƿ���֤SecurityKey
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenManagement.Secret)),    //�����keyҪ���м���
                    ValidateIssuer = true,                                                                          //��֤������Issuer
                    ValidIssuer = tokenManagement.Issuer,                                                           //Token�䷢����
                    ValidateAudience = true,                                                                        //��֤������
                    ValidAudience = tokenManagement.Audience,                                                       //�䷢��˭   
                    ValidateLifetime = true,                                                                        //��֤token����ʱ��
                    ClockSkew = TimeSpan.FromSeconds(30),                                                           //ʱ�ӻ�����λ��
                    RequireExpirationTime = true,                                                                   //token��Ҫ���ù���ʱ��
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        // ������ڣ����<�Ƿ����>��ӵ�������ͷ��Ϣ��
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

            app.UseStaticFiles();       //��̬�ļ���Դ

            app.UseAuthentication();    //���������֤

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
