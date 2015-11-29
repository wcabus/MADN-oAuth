using System;
using System.Web.Http;
using Timesheet.Api.Models.Projects;
using Timesheet.Repositories;

namespace Timesheet.Api.Controllers
{
    [RoutePrefix("api/projects")]
    public class ProjectsController : ApiController
    {
        private readonly ProjectRepository _repository;

        public ProjectsController()
        {
            _repository = new ProjectRepository();
        }

        public IHttpActionResult GetProjects()
        {
            return Ok(_repository.GetProjects());
        }

        [Route("{id:guid}", Name = "GetProjectByIdRoute")]
        public IHttpActionResult GetProject(Guid id)
        {
            var project = _repository.GetProjectById(id);
            if (project == null) { return NotFound(); }

            return Ok(project);
        }

        [HttpPost]
        public IHttpActionResult CreateProject(CreateProjectModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = _repository.CreateProject(model.Name, model.Description);
            return CreatedAtRoute("GetProjectByIdRoute", new { project.Id }, project);
        }
    }
}
