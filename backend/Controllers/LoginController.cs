using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly AuthService _authService;
        public LoginController(AuthService authService)
        {
            _authService = authService;
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

            return Ok(new { token });
        }
    }
}
