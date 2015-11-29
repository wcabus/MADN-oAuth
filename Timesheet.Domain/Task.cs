using System;

namespace Timesheet.Domain
{
    public class Task
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan AvailableTime { get; set; }
    }
}
