using System;
using Timesheet.Domain;

namespace Timesheet.Entities
{
    internal static class Converters
    {
        public static ProjectEntity ToEntity(this Project model)
        {
            return new ProjectEntity
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description
            };
        }

        public static Project ToDomain(this ProjectEntity entity)
        {
            return new Project
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };
        }

        public static TaskEntity ToEntity(this Task model)
        {
            return new TaskEntity
            {
                Id = model.Id,
                ProjectId = model.ProjectId,
                Name = model.Name,
                Description = model.Description,
                AvailableTime = model.AvailableTime?.Ticks
            };
        }

        public static Task ToDomain(this TaskEntity entity)
        {
            return new Task
            {
                Id = entity.Id,
                ProjectId = entity.ProjectId,
                Name = entity.Name,
                Description = entity.Description,
                AvailableTime = entity.AvailableTime == null ? (TimeSpan?)null : new TimeSpan(entity.AvailableTime.Value)
            };
        }

        public static EmployeeEntity ToEntity(this Employee model)
        {
            return new EmployeeEntity
            {
                Id = model.Id,
                Name = model.Name,
                FirstName = model.FirstName,
                Email = model.Email
            };
        }

        public static Employee ToDomain(this EmployeeEntity entity)
        {
            return new Employee
            {
                Id = entity.Id,
                Name = entity.Name,
                FirstName = entity.FirstName,
                Email = entity.Email
            };
        }

        public static TimeRegistrationEntity ToEntity(this TimeRegistration model)
        {
            return new TimeRegistrationEntity
            {
                Id = model.Id,
                TaskId = model.TaskId,
                EmployeeId = model.EmployeeId,
                TimeStart = model.TimeStart,
                TimeEnd = model.TimeEnd,
                Time = model.Time.Ticks,
                Remarks = model.Remarks
            };
        }

        public static TimeRegistration ToDomain(this TimeRegistrationEntity model)
        {
            return new TimeRegistration
            {
                Id = model.Id,
                TaskId = model.TaskId,
                EmployeeId = model.EmployeeId,
                TimeStart = model.TimeStart,
                TimeEnd = model.TimeEnd,
                Time = new TimeSpan(model.Time),
                Remarks = model.Remarks
            };
        }
    }
}
