using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications.Employee_specs;

namespace Talabat.APIs.Controllers
{
    public class EmployeeController : BaseApiController
    {
        private readonly IGenericRepository<Employee> _employeeRepo;

        public EmployeeController(IGenericRepository<Employee> employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Employee>>> getEmployees()
        {
            var spec = new EmployeeWithDepartmentSpecifications();
            var employees=await _employeeRepo.GetAllWithSpecAsync(spec);
            return Ok(employees);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> getEmployee(int id)
        {
            var spec = new EmployeeWithDepartmentSpecifications(id);
            var employee = await _employeeRepo.GetByIdWithSpecAsync(spec);
            return Ok(employee);
        }
    }
}
