using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Timesheet.Api.Controllers
{
    [RoutePrefix("api/projects")]
    public class ProjectsController : ApiController
    {
        public IHttpActionResult GetProjects()
        {

        }

        [Route("{id:guid}", Name = "GetProjectByIdRoute")]
        public IHttpActionResult GetProject(Guid id)
        {

        }

        [HttpPost]
        public IHttpActionResult CreateProject() {

            return CreatedAtRoute("GetProjectByIdRoute", new { id }, project);
        }
    }
}
