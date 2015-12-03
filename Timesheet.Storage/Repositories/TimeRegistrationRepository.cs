﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;
using Timesheet.Domain;
using Timesheet.Entities;

namespace Timesheet.Repositories
{
    public class TimeRegistrationRepository : BaseRepository
    {
        public IEnumerable<TimeRegistration> GetTimeRegistrationsForTask(Guid taskId)
        {
            var task = new TimeRegistrationEntity { TaskId = taskId };
            var table = GetTable(RegistrationsTable);
            return table.CreateQuery<TimeRegistrationEntity>()
                .Where(x => x.PartitionKey == task.PartitionKey)
                .Select(x => x.ToDomain());
        }

        public IEnumerable<TimeRegistration> GetTimeRegistrationsForEmployee(Guid employeeId)
        {
            var search = new TimeRegistrationByEmployeeEntity { EmployeeId = employeeId };
            var table = GetTable(RegistrationsEmployeeTable);
            var query = table.CreateQuery<TimeRegistrationByEmployeeEntity>()
                .Where(x => x.PartitionKey == search.PartitionKey)
                .ToList();

            var tasks = query.OrderBy(x => x.TaskId).Select(x => x.TaskId).Distinct();

            var results = new List<TimeRegistration>();
            var tableResults = GetTable(RegistrationsTable);
            foreach (var task in tasks)
            {
                results.AddRange(tableResults.CreateQuery<TimeRegistrationEntity>()
                    .Where(x => x.PartitionKey == task.ToString("D"))
                    .Select(x => x.ToDomain()));
            }

            return results;
        }
        
        public TimeRegistration GetTimeRegistrationByIdForEmployee(Guid employeeId, Guid registrationId)
        {
            var search = new TimeRegistrationByEmployeeEntity { EmployeeId = employeeId, Id = registrationId };
            var table = GetTable(RegistrationsEmployeeTable);
            var key = table.CreateQuery<TimeRegistrationByEmployeeEntity>()
                .Where(x => x.PartitionKey == search.PartitionKey && x.RowKey == search.RowKey)
                .SingleOrDefault();

            if (key == null)
            {
                return null;
            }

            var regKey = new TimeRegistrationEntity { TaskId = key.TaskId, Id = registrationId };
            var regTable = GetTable(RegistrationsTable);
            return regTable.CreateQuery<TimeRegistrationEntity>()
                    .Where(x => x.PartitionKey == regKey.PartitionKey && x.RowKey == regKey.RowKey)
                    .Select(x => x.ToDomain())
                    .Single();
        }

        public TimeRegistration CreateTimeRegistration(Guid taskId, Guid employeeId, DateTimeOffset start, DateTimeOffset end, string remarks)
        {
            var task = new TimeRegistrationEntity
            {
                TaskId = taskId,
                EmployeeId = employeeId,
                Id = Guid.NewGuid(),
                TimeStart = start,
                TimeEnd = end,
                Time = (end - start).Ticks,
                Remarks = remarks
            };

            var taskPerEmployee = new TimeRegistrationByEmployeeEntity
            {
                EmployeeId = employeeId,
                TaskId = taskId,
                Id = task.Id
            };

            var table = GetTable(RegistrationsTable);
            var table2 = GetTable(RegistrationsEmployeeTable);

            var tableOp = TableOperation.Insert(task);
            var tableOp2 = TableOperation.Insert(taskPerEmployee);

            // TODO Move in transaction somehow
            table.Execute(tableOp);
            table2.Execute(tableOp2);
            // End transaction 

            return task.ToDomain();
        }
    }
}