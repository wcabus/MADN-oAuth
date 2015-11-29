using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Timesheet.Entities
{
    public class EmployeeEntity : TableEntity
    {
        public EmployeeEntity()
        {
            PartitionKey = "";
        }

        public Guid Id
        {
            get { return Guid.ParseExact(RowKey, "D"); }
            set { RowKey = value.ToString("D"); }
        }

        public string Name { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
    }
}
