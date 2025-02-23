using backend.Data;
using backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

/*
設定CORS
AllowAnyOrigin() => 允許任何來源 (開發環境用)  若正式環境則改為使用WithOrigins(網域)
AllowAnyMethod() => 允許所有 HTTP 方法 (GET、POST、PUT、DELETE等)
AllowAnyHeader() => 允許所有請求標頭
AllowCredentials() => 允許 Cookies / HTTP Authorization Header  正式環境使用  須搭配WithOrigins()
*/
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

//註冊資料庫 EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 註冊Service
builder.Services.AddScoped<AuthService>();

//註冊Controller
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s => 
{
    /*
    讓Swagger有註解
    記得csproj檔 要加
    <GenerateDocumentationFile>True</GenerateDocumentationFile>  //讓 .NET產生專案的 xml註解文件
    <NoWarn>1591</NoWarn>  //避免沒有 XML 註解時，編譯器顯示警告
     */
    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    s.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,xmlFilename));
});


//*需要先Authentication才能Authorization
//註冊 驗證Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("Jwt");
    var jwtKey = AuthService.GetJwtKey(builder.Configuration);

    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, //確保Toekn不被竄改
        IssuerSigningKey = new SymmetricSecurityKey(jwtKey), // 用來設定驗證的key
        ValidateIssuer = true, //確保Token 是由指定的Issue發送
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true, //確保Token只能由特定 Audience 使用
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true, //確保Token沒有過期
        ClockSkew = TimeSpan.Zero //取消時間緩衝，讓 Token 過期時間準確
    };
});

//註冊 授權Authorization
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors("AllowAll");  //要寫在app.UseRouting() 和 app.UseAuthorization() 之前

//管理員初始化，確保 管理員帳號 存在
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetService<AppDbContext>();
    DataSeeder.SeedAdminUser(dbContext); //初始化
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//使用身分驗證 (解析Token)   *需要先Authentication才能Authorization
app.UseAuthentication();

//使用授權  負責檢查 Authorize 屬性與權限
app.UseAuthorization();

//設定API路由
app.MapControllers();

app.Run();
