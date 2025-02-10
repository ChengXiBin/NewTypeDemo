using backend.Data;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly AuthService _authService;
        private readonly AppDbContext _context;
        public LoginController(AuthService authService, AppDbContext context)
        {
            _authService = authService;
            _context = context;
        }


        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrWhiteSpace(loginRequest.AccountID) && string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                return BadRequest(new { message = "帳號和密碼為必填"});
            }

            var token = await _authService.Authenticate(loginRequest.AccountID,loginRequest.Password);
            if (token == null) 
            {
                return Unauthorized(new { message = "帳號或密碼錯誤"});
            }

            var employeeData = await _context.Employees
                .Where(e => e.AccountID == loginRequest.AccountID)
                .Select(e => new
                {
                    employeeName = e.DisplayName,
                    departmentName = e.EmployeeDepartments
                        .Select(ed => ed.Department.Name)
                        .FirstOrDefault() ?? "未分配部門"
                })
                .FirstOrDefaultAsync();

            if (employeeData == null)
            {
                return NotFound(new { message = "找不到員工資料" });
            }

            return Ok(new
            {
                token,
                employeeData?.employeeName,
                employeeData?.departmentName
            });
        }
    }
}
