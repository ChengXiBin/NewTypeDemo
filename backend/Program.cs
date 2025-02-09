using backend.Data;
using backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

//���U��Ʈw EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ���UService
builder.Services.AddScoped<AuthService>();

//���UController
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//*�ݭn��Authentication�~��Authorization
//���U ����Authentication
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
        ValidateIssuerSigningKey = true, //�T�OToekn���Q«��
        IssuerSigningKey = new SymmetricSecurityKey(jwtKey), // �Ψӳ]�w���Ҫ�key
        ValidateIssuer = true, //�T�OToken �O�ѫ��w��Issue�o�e
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true, //�T�OToken�u��ѯS�w Audience �ϥ�
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true, //�T�OToken�S���L��
        ClockSkew = TimeSpan.Zero //�����ɶ��w�ġA�� Token �L���ɶ��ǽT
    };
});

//���U ���vAuthorization
builder.Services.AddAuthorization();

var app = builder.Build();

//�޲z����l�ơA�T�O �޲z���b�� �s�b
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetService<AppDbContext>();
    DataSeeder.SeedAdminUser(dbContext); //��l��
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//�ϥΨ������� (�ѪRToken)   *�ݭn��Authentication�~��Authorization
app.UseAuthentication();

//�ϥα��v  �t�d�ˬd Authorize �ݩʻP�v��
app.UseAuthorization();

//�]�wAPI����
app.MapControllers();

app.Run();
