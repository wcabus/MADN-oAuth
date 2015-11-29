using System;
using System.Web.Http;
using Timesheet.Api.Models.Tasks;
using Timesheet.Repositories;

namespace Timesheet.Api.Controllers
{
    [RoutePrefix("api/projects/{projectId:guid}/tasks")]
    public class TasksController : ApiController
    {
        private readonly TaskRepository _repository = new TaskRepository();

        public IHttpActionResult GetTasks(Guid projectId)
        {
            return Ok(_repository.GetTasksByProjectId(projectId));
        }

        [Route("{id:guid}", Name = "GetTaskRoute")]
        public IHttpActionResult GetTask(Guid projectId, Guid id) {
            var task = _repository.GetTaskById(projectId, id);
            if (task == null) { return NotFound(); }

            return Ok(task);
        }

        public IHttpActionResult CreateTask(Guid projectId, CreateTaskModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = _repository.CreateTask(projectId, model.Name, model.Description, model.AvailableTime);
            return CreatedAtRoute("GetTaskRoute", new { projectId, task.Id }, task);
        }
    }
}
