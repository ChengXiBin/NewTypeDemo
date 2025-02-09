namespace backend.Models
{
    public class LoginRequest
    {
        public string AccountID { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
