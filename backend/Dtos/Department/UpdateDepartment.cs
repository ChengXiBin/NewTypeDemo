namespace backend.Dtos.Department
{
    public class UpdateDepartment
    {
        public string Name { get; set; }
        public Guid? AffiliatedDepartmentID { get; set; }
    }
}
