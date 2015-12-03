using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;
using Timesheet.Domain;
using Timesheet.Entities;

namespace Timesheet.Repositories
{
    public class TaskRepository : BaseRepository
    {
        public IEnumerable<Task> GetTasksByProjectId(Guid projectId)
        {
            var entity = new TaskEntity { ProjectId = projectId };
            var table = GetTable(TasksTable);

            return table.CreateQuery<TaskEntity>()
                        .Where(x => x.PartitionKey == entity.PartitionKey)
                        .ToList()
                        .Select(x => x.ToDomain());
        }

        public Task GetTaskById(Guid projectId, Guid id)
        {
            var entity = new TaskEntity { ProjectId = projectId, Id = id };
            var table = GetTable(TasksTable);

            var result = table.CreateQuery<TaskEntity>()
                        .Where(x => x.PartitionKey == entity.PartitionKey && x.RowKey == entity.RowKey)
                        .SingleOrDefault();

            return result?.ToDomain();
        }

        public Task CreateTask(Guid projectId, string name, string description, TimeSpan? availableTime)
        {
            var table = GetTable(TasksTable);
            var entity = new TaskEntity
            {
                ProjectId = projectId,
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                AvailableTime = availableTime?.Ticks
            };
            var operation = TableOperation.Insert(entity);
            table.Execute(operation);

            return entity.ToDomain();
        }
    }
}
