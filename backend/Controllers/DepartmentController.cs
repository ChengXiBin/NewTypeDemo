using backend.Data;
using backend.Dtos.Department;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : Controller
    {
        private readonly AppDbContext _context;
        public DepartmentController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 取得所有部門
        /// </summary>
        /// <returns>所有部門資料</returns>
        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await _context.Departments
                    .Include(d => d.SubDepartments)
                    .Select(d => new
                    {
                        d.DepartmentID,
                        d.Name,
                        SubDepartments = d.SubDepartments.Select(sub => new
                        {
                            sub.DepartmentID,
                            sub.Name
                        }).ToList()
                    })
                    .ToListAsync();

            return Ok(departments);
        }

        /// <summary>
        /// 新增部門
        /// </summary>
        /// <param name="dto">欲新增的部門資料</param>
        /// <returns>
        /// 成功則返回 "新增成功"
        /// 若該部門名稱已存在，則返回 "該部門名稱已存在"
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> AddDepartment([FromBody] AddDepartmentDto dto)
        {
            var departmentExists = await _context.Departments.AnyAsync(d => d.Name == dto.Name);

            if (departmentExists)
            {
                return BadRequest(new { message = "該部門名稱已存在" });
            }

            var department = new Department
            {
                DepartmentID = Guid.NewGuid(),
                Name = dto.Name,
                AffiliatedDepartmentID = dto.AffiliatedDepartmentID,
                Disable = false
            };

            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();

            return Ok(new { message = "新增成功" });
        }

        /// <summary>
        /// 修改指定部門Department的資料
        /// </summary>
        /// <param name="id">部門GUID</param>
        /// <param name="dto">更新後的部門資料</param>
        /// <returns>
        /// 成功則返回 "更新成功"
        /// 
        /// 若部門不存在 則返回 NotFound 404
        /// 若欲修改的部門名稱已存在 則返回 "該部門名稱已存在"
        /// 若父部門ID 為 [Null] or [不存在] 則返回 "無效的父部門ID"
        /// 若父部門ID為自己部門ID 則返回 "部門不能設定自己為父部門"
        /// </returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(Guid id, [FromBody]UpdateDepartment dto)
        {
            //確認該ID部門是否存在
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentID == id);
            if (department == null)
            {
                return NotFound();
            }

            //確認部門名稱是否已存在(排除自己)，避免部門名稱重複
            var departmentExists = await _context.Departments
                .AnyAsync(d => d.Name == dto.Name && d.DepartmentID != id);
            if (departmentExists)
            {
                return BadRequest(new { message = "該部門名稱已存在" });
            }

            //確認父部門ID AffiliatedDepartmentID 是否有效 ，且父部門ID 不能為自己
            if (dto.AffiliatedDepartmentID.HasValue)
            {
                if (dto.AffiliatedDepartmentID == id)
                {
                    return BadRequest(new { message = "部門不能設定自己為父部門" });
                }
                bool parentExist = await _context.Departments
                    .AnyAsync(d => d.DepartmentID == dto.AffiliatedDepartmentID);
                if (!parentExist) 
                {
                    return BadRequest(new { message = "無效的父部門ID"});
                }
            }

            department.Name = dto.Name;
            department.AffiliatedDepartmentID = dto.AffiliatedDepartmentID;
            department.UpdatedTime = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { message = "更新成功" });
        }
    }
}
