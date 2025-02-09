using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace backend.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context,IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        /// <summary>
        /// 驗證登入，成功則方送JWT Token ， 失敗則回傳null
        /// </summary>
        /// <param name="accountId">帳號</param>
        /// <param name="password">密碼</param>
        /// <returns>JWT Token</returns>
        public async Task<string?> Authenticate(string accountId, string password)
        {
            var user = await _context.Employees.SingleOrDefaultAsync(e => e.AccountID == accountId);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password,user.Password))  //Verify 加密參數1與參數2做比對
            {
                return null; //帳號密碼錯誤
            }

            return GenerateJwtToken(user);
        }

        /// <summary>
        /// 產生JWT Token
        /// </summary>
        /// <param name="employee">員工</param>
        /// <returns>返回Token 字串</returns>
        private string GenerateJwtToken(Employee employee)
        {
            var key = GetJwtKey(_config);  

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,employee.EmployeeID.ToString()),
                new Claim(ClaimTypes.Name,employee.AccountID)
            };

            var token = new JwtSecurityToken(
                issuer : _config["Jwt:Issuer"], // 對應JWT的iss : Token 發送者
                audience : _config["Jwt:Audience"], // 對應JWT的aud Audience Token 接收者
                claims : claims, //Token內的資訊(使用者ID、使用者名稱)   可以自訂義放些不敏感資訊 ex:ID、名稱、角色
                expires : DateTimeOffset.UtcNow.AddHours(2).UtcDateTime, //對應JWT的exp 有效期限
                signingCredentials : new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256) // 用 Key 簽署 Token
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// 取得JwtKey
        /// </summary>
        /// <returns>Byt[] key</returns>
        public static byte[] GetJwtKey(IConfiguration config)
        {
            //若正式環境變數存在JWT_SECRET 則取該值 建議該值為[64字元隨機字串]  最少也要32字元
            //若沒有則給預設值_config["JWT:key"] ex:開發環境 
            string? keyString = Environment.GetEnvironmentVariable("JWT_SECRET") ?? config["JWT:Key"]!;  // ! 代表忽略NET 的null警告

            if (string.IsNullOrEmpty(keyString))
            {
                throw new InvalidOperationException("JWT Key is missing.");
            }

            return Encoding.UTF8.GetBytes(keyString); //Tokem key型別 需用byte[]
        }

    }
}
