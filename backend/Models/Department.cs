using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Department
{
    public Guid DepartmentID { get; set; }
    public string Name { get; set; } = null!;
    public Guid? AffiliatedDepartmentID { get; set; }
    public bool Disable { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime UpdatedTime { get; set; }
    public virtual Department ParentDepartment { get; set; }
    public virtual ICollection<Department> SubDepartments { get; set; }
    public virtual ICollection<EmployeeDepartment> EmployeeDepartments { get; set; } = new List<EmployeeDepartment>();
}
