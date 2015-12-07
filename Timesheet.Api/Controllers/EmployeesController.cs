using System;
using System.Web.Http;
using Timesheet.Api.Models.Employees;
using Timesheet.Repositories;

namespace Timesheet.Api.Controllers
{
    [RoutePrefix("api/employees")]
    public class EmployeesController : ApiController
    {
        private readonly EmployeeRepository _repository = new EmployeeRepository();

        [Route]
        public IHttpActionResult GetEmployees()
        {
            return Ok(_repository.GetEmployees());
        }

        [Route("{id:guid}", Name = "GetEmployeeRoute")]
        public IHttpActionResult GetEmployee(Guid id)
        {
            var employee = _repository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [Route]
        public IHttpActionResult CreateEmployee(CreateEmployeeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _repository.CreateEmployee(model.Name, model.FirstName, model.Email);
            return CreatedAtRoute("GetEmployeeRoute", new { employee.Id }, employee);
        }
    }
}
