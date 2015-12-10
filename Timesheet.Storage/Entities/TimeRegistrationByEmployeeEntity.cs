using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Timesheet.Entities
{
    public class TimeRegistrationByEmployeeEntity : TableEntity
    {
        public string EmployeeId
        {
            get { return PartitionKey; }
            set { PartitionKey = value; }
        }

        public Guid Id
        {
            get { return Guid.ParseExact(RowKey, "D"); }
            set { RowKey = value.ToString("D"); }
        }

        public Guid TaskId { get; set; }
    }
}
