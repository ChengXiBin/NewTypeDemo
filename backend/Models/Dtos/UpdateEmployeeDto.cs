namespace backend.Models.Dtos
{
    public class UpdateEmployeeDto
    {
        public string DisplayName { get; set; } = string.Empty;
        public List<Guid> DepartmentIds { get; set; } = new List<Guid>(); // 部門ID列表
    }
}
