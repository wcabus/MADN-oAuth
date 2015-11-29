using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Timesheet.Entities
{
    public class TimeRegistrationByEmployeeEntity : TableEntity
    {
        public Guid EmployeeId
        {
            get { return Guid.ParseExact(PartitionKey, "D"); }
            set { PartitionKey = value.ToString("D"); }
        }

        public Guid Id
        {
            get { return Guid.ParseExact(RowKey, "D"); }
            set { RowKey = value.ToString("D"); }
        }

        public Guid TaskId { get; set; }
    }
}
