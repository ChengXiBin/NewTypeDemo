using backend.Data;
using backend.Models;
using backend.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult>GetEmployees()
        {
            var employees = await _context.Employees
                .Select(e => new
                {
                    e.EmployeeID,
                    e.DisplayName,
                    e.Email,
                    e.Disable,
                    Departments = e.EmployeeDepartments
                        .Select(ed => new 
                        {
                            ed.DepartmentID,
                            ed.Department.Name
                        })
                        .ToList()
                })
                .ToListAsync();

            return Ok(employees);
        }
        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody]UpdateEmployeeDto dto)
        {
            //確認是否有該員工
            var employee = await _context.Employees
                            .Include(e => e.EmployeeDepartments)
                            .FirstOrDefaultAsync(e => e.EmployeeID == id);
            
            if (employee == null)
            {
                return NotFound("查無該員工");
            }

            #region 更新員工基本資料
            employee.DisplayName = dto.DisplayName;
            employee.UpdatedTime = DateTime.UtcNow;

            #endregion

            #region 更新員工所屬部門
            //取得目前關聯部門
            var currentDepartmentIds = employee.EmployeeDepartments
                                    .Select(ed => ed.EmployeeDepartmentID)
                                    .ToList();

            //移除目標關聯部門
            var removeRelation = employee.EmployeeDepartments
                            .Where(ed => !dto.DepartmentIds.Contains(ed.DepartmentID))
                            .ToList();
            _context.EmployeeDepartments.RemoveRange(removeRelation);

            //建立目標關聯部門
            var addRelation = dto.DepartmentIds
                            .Where(deptId => !currentDepartmentIds.Contains(deptId))
                            .Select(deptId => new EmployeeDepartment
                            {
                                EmployeeID = id,
                                DepartmentID = deptId,
                                Disable = false,
                                CreatedTime = DateTime.UtcNow,
                                UpdatedTime = DateTime.UtcNow,
                            })
                            .ToList();
            _context.EmployeeDepartments.AddRange(addRelation);
            #endregion

            await _context.SaveChangesAsync();

            return Ok(new { message = "員工資料更新成功" });
        }
    }
}
