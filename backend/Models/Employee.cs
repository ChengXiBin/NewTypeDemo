using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Employee
{
    public Guid EmployeeID { get; set; }
    public string AccountID { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool Disable { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime UpdatedTime { get; set; }
    public virtual ICollection<EmployeeDepartment> EmployeeDepartments { get; set; } = new List<EmployeeDepartment>();
    public string Role { get; set; } = "User"; //預設為一般使用者
}
