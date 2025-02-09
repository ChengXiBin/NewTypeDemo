using backend.Data;
using backend.Models;

namespace backend.Services
{
    //DataSeeder資料種子初始化器  用來表示初始化資料的類別
    public static class DataSeeder
    {
        /// <summary>
        /// 若資料庫不存在系統管理員Admin，系統啟動時會建立系統管理員帳號Admin
        /// </summary>
        /// <param name="dbContext"></param>
        public static void SeedAdminUser(AppDbContext dbContext) 
        {
            //確保資料庫存在
            dbContext.Database.EnsureCreated();

            //檢查是否已存在管理員
            if(!dbContext.Employees.Any(e => e.Role == "Admin"))
            {
                var admin = new Employee
                {
                    EmployeeID = Guid.NewGuid(),
                    AccountID = "admin",
                    Password = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    DisplayName = "系統管理員",
                    Email = "admin@test.com",
                    Disable = false,
                    Role = "Admin"
                };
                dbContext.Employees.Add(admin);
                dbContext.SaveChanges();
                Console.WriteLine("預設系統管理員帳號已建立");
            }
        }

    }
}
