using backend.Data;
using backend.Dtos.Employee;
using backend.Models;
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

        /// <summary>
        /// 取得所有員工
        /// </summary>
        /// <returns>所有員工資料集合</returns>
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
                        }).ToList()
                })
                .ToListAsync();

            return Ok(employees);
        }

        /// <summary>
        /// 新增員工
        /// </summary>
        /// <param name="dto">欲新增的員工資料</param>
        /// <returns>
        /// 成功則返回 "新增成功"
        /// 若帳號已存在則返回 "該員工帳號已存在"
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody]AddEmployeeDto dto)
        {
            bool accountExists = await _context.Employees.AnyAsync(e => e.AccountID == dto.AccountID);
            
            //確認該帳號是否已建立，若存在則拒絕建立
            if(accountExists) 
            {
                return BadRequest(new { message = "該員工帳號已存在" }); 
            }

            var newEmployee = new Employee
            {
                EmployeeID = Guid.NewGuid(),
                AccountID = dto.AccountID,
                Password = dto.Password,
                DisplayName = dto.DisplayName,
                Email = dto.Email
            };

            if (dto.DepartmentIDs.Any())
            {
                foreach (var departmentID in dto.DepartmentIDs)
                {
                    newEmployee.EmployeeDepartments.Add(new EmployeeDepartment
                    {
                        EmployeeDepartmentID = Guid.NewGuid(),
                        EmployeeID = newEmployee.EmployeeID,
                        DepartmentID = departmentID,
                        Disable = false,
                    });
                }
            }

            await _context.Employees.AddAsync(newEmployee);
            await _context.SaveChangesAsync();

            return Ok(new {message = "新增成功"});
        }

        /// <summary>
        /// 修改指定員工Employee的資料
        /// </summary>
        /// <param name="id">Employee GUID</param>
        /// <param name="dto">更新後的員工資料</param>
        /// <returns>
        /// 成功回傳 "員工資料更新成功"
        /// 若找不到對應的員工則回傳 "查無該員工"
        /// </returns>
        [HttpPut("{id}")]
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
                            .Where(ed => !dto.DepartmentIDs.Contains(ed.DepartmentID))
                            .ToList();
            _context.EmployeeDepartments.RemoveRange(removeRelation);

            //建立目標關聯部門
            var addRelation = dto.DepartmentIDs
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

            return Ok(new { message = "更新成功" });
        }
    }
}
