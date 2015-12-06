using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        [Route("~/api/employees/{employeeId:guid}/registrations")]
        public IHttpActionResult GetRegistrationsForEmployee(Guid employeeId)
        {
            return Ok(_repository.GetTimeRegistrationsForEmployee(employeeId));
        }

        [Route("~/api/employees/{employeeId:guid}/registrations/{id:guid}", Name = "GetRegistrationRoute")]
        public IHttpActionResult GetRegistrationForEmployee(Guid employeeId, Guid id)
        {
            var registration = _repository.GetTimeRegistrationByIdForEmployee(employeeId, id);
            if (registration == null)
            {
                return NotFound();
            }

            return Ok(registration);
        }

        [HttpPost]
        [Route("~/api/employees/{employeeId:guid}/registrations")]
        public IHttpActionResult CreateTimeRegistration(Guid employeeId, CreateTimeRegistrationModel model)
        {
            if (employeeId == Guid.Empty)
            {
                ModelState.AddModelError("", "Invalid employee ID.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var registration = _repository.CreateTimeRegistration(model.TaskId, employeeId, model.Start, model.End, model.Remarks);

            return CreatedAtRoute("GetRegistrationRoute", new { employeeId, registration.Id }, registration);
        }
    }
}
