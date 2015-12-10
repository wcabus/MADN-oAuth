using System;
using System.Web.Http;
using Timesheet.Api.Models.TimeRegistrations;
using Timesheet.Repositories;

namespace Timesheet.Api.Controllers
{
    public class TimeRegistrationsController : ApiController
    {
        private readonly TimeRegistrationRepository _repository = new TimeRegistrationRepository();

        [Route("~/api/tasks/{taskId:guid}/registrations")]
        public IHttpActionResult GetRegistrations(Guid taskId)
        {
            return Ok(_repository.GetTimeRegistrationsForTask(taskId));
        }

        [Route("~/api/employees/{employeeId}/registrations")]
        public IHttpActionResult GetRegistrationsForEmployee(string employeeId)
        {
            return Ok(_repository.GetTimeRegistrationsForEmployee(employeeId.ToUpperInvariant()));
        }

        [Route("~/api/employees/{employeeId}/registrations/{id:guid}", Name = "GetRegistrationRoute")]
        public IHttpActionResult GetRegistrationForEmployee(string employeeId, Guid id)
        {
            var registration = _repository.GetTimeRegistrationByIdForEmployee(employeeId.ToUpperInvariant(), id);
            if (registration == null)
            {
                return NotFound();
            }

            return Ok(registration);
        }

        [HttpPost]
        [Route("~/api/employees/{employeeId}/registrations")]
        public IHttpActionResult CreateTimeRegistration(string employeeId, CreateTimeRegistrationModel model)
        {
            if (string.IsNullOrEmpty(employeeId))
            {
                ModelState.AddModelError("", "Invalid employee ID.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            employeeId= employeeId.ToUpperInvariant();
            var registration = _repository.CreateTimeRegistration(model.TaskId, employeeId, model.Start, model.End, model.Remarks);

            return CreatedAtRoute("GetRegistrationRoute", new { employeeId, registration.Id }, registration);
        }
    }
}
