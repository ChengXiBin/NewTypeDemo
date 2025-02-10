namespace backend.Dtos.Employee
{
    public class UpdateEmployeeDto
    {
        public string DisplayName { get; set; } = string.Empty;
        public List<Guid> DepartmentIDs { get; set; } = new List<Guid>(); // 部門ID列表
    }
}
