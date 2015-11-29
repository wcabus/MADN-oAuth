using System;

namespace Timesheet.Domain
{
    public class Employee
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
    }
}
