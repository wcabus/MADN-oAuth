using System;

namespace Timesheet.Domain
{
    public class TimeRegistration
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public string EmployeeId { get; set; }
        
        public DateTimeOffset TimeStart { get; set; }
        public DateTimeOffset TimeEnd { get; set; }
        public TimeSpan Time { get; set; }

        public string Remarks { get; set; }
    }
}
