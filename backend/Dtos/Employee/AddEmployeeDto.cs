namespace backend.Dtos.Employee
{
    public class AddEmployeeDto
    {
        public string AccountID { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public List<Guid> DepartmentIDs { get; set; } = new List<Guid>(); // 部門ID列表
    }
}
