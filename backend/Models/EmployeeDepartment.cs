using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class EmployeeDepartment
{
    public Guid EmployeeDepartmentID { get; set; }
    public Guid EmployeeID { get; set; }
    public Guid DepartmentID { get; set; }
    public bool Disable { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime UpdatedTime { get; set; }
    public virtual Employee Employee { get; set; }
    public virtual Department Department { get; set; }
}
