using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;
using Timesheet.Domain;
using Timesheet.Entities;

namespace Timesheet.Repositories
{
    public class ProjectRepository : BaseRepository
    {
        public IEnumerable<Project> GetProjects()
        {
            var table = GetTable(ProjectsTable);
            return table.CreateQuery<ProjectEntity>()
                        .Select(x => x.ToDomain());
        }

        public Project GetProjectById(Guid id)
        {
            var entity = new ProjectEntity { Id = id };
            var table = GetTable(ProjectsTable);

            return table.CreateQuery<ProjectEntity>()
                        .Where(x=> x.PartitionKey == "" && x.RowKey == entity.RowKey)
                        .Select(x => x.ToDomain())
                        .SingleOrDefault();
        }

        public Project CreateProject(string name, string description)
        {
            var table = GetTable(ProjectsTable);
            var entity = new ProjectEntity { Id = Guid.NewGuid(), Name = name, Description = description };
            var operation = TableOperation.Insert(entity);
            table.Execute(operation);

            return entity.ToDomain();
        }
    }
}
