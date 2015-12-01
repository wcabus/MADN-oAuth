using System;
using System.Web.Http;
using Timesheet.Repositories;

namespace Timesheet.Api.Controllers
{
    [RoutePrefix("api/employees")]
    public class EmployeesController : ApiController
    {
        private readonly EmployeeRepository _repository = new EmployeeRepository();

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
    }
}
