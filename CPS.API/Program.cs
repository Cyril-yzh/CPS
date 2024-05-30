using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CPS.Core.Jwt;
using CPS.Entity.Business;
using CPS.Entity.SysEntitys;
using CPS.EntityFrameworkCore;
using CPS.Repository;
using CPS.Repository.BusRepository;
using CPS.Repository.IRepository.IBusRepository;
using CPS.Service.BusServices;
using CPS.Service.IServices;
using CPS.Service.IServices.IBusService;
using CPS.Service.SysServices;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration; //��ȡ�����ļ�ʹ�õ���
// Add services to the container.

builder.Services.AddControllers();

#region ��������
builder.Services.AddCors(options =>
{
    options.AddPolicy("Cors",config =>
    {
        config.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});

#endregion

#region WebApi��Ŀ����
builder.Services.AddDbContext<SqlDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("strConn"));
});

#endregion

#region ����ע������

#region �ִ�����ע��
//ҵ����ִ�
builder.Services.AddScoped<IPictureRepository, PictureRepository>();    //ͼƬ���ִ�
builder.Services.AddScoped<IArticleRepository,ArticleRepository>(); //���±��ִ�
builder.Services.AddScoped<IBaseRepository<Classify>,BaseRepository<Classify>>(); //������ִ�


//ϵͳ���ִ�
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBaseRepository<SysMenu>, BaseRepository<SysMenu>>(); //�˵��ִ�
builder.Services.AddScoped<IBaseRepository<SysRole>, BaseRepository<SysRole>>(); //��ɫ�ִ�
//builder.Services.AddScoped<IBaseRepository<SysRoleMenu>, BaseRepository<SysRoleMenu>>(); //��ɫ�˵��м��ϵ��Ӧ���ִ�
#endregion


#region ҵ��Service����ע���ϵ
builder.Services.AddScoped<IArticleService,ArticleService>();   //����ҵ��
builder.Services.AddScoped<IClassifyService,ClassifyService>();   //����ҵ�����

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMenuService, MenuService>();
#endregion



#endregion

#region Jwt��Ȩ��֤����

//��ȡJwt������Ϣ
var jwtSection = builder.Configuration.GetSection("TokenManagement");
//Jwt������Ϣ��ע��������
builder.Services.Configure<TokenManagement>(jwtSection);

TokenManagement tokenManagement = jwtSection.Get<TokenManagement>();

//Jwt��Ȩ����
builder.Services.AddAuthorization(options =>
{

})
    .AddAuthentication(options =>
    {
        //2.1����֤����core�Դ��ٷ�JWT��֤
        // ����Bearer��֤
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,    //�Ƿ���֤SecurityKey
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(tokenManagement.Secret)),    //�����keyҪ���м���
            //��֤������Issuer
            ValidateIssuer = true,
            ValidIssuer = tokenManagement.Issuer,   //Token�䷢����
            //��֤������
            ValidateAudience = true,
            ValidAudience = tokenManagement.Audience,   //�䷢��˭   
            ValidateLifetime = true,    //��֤token����ʱ��
            ClockSkew = TimeSpan.FromSeconds(30),    //ʱ�ӻ�����λ��
            RequireExpirationTime = true,   //token��Ҫ���ù���ʱ��
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                // ������ڣ����<�Ƿ����>���ӵ�������ͷ��Ϣ��
                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                {
                    context.Response.Headers.Add("Token-Expired", "true");
                }
                return Task.CompletedTask;
            }
        };
    });
#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors("Cors");

app.UseStaticFiles();   //��̬�ļ���Դ

app.UseAuthentication();    //����������֤

app.UseAuthorization(); //��Ȩ

app.MapControllers();

app.Run();