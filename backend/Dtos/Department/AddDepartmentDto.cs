namespace backend.Dtos.Department
{
    public class AddDepartmentDto
    {
        public string Name {  get; set; }
        public Guid AffiliatedDepartmentID {  get; set; }
    }
}
