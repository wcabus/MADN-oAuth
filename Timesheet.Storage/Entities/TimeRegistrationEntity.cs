using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Timesheet.Entities
{
    public class TimeRegistrationEntity : TableEntity
    {
        public Guid TaskId
        {
            get { return Guid.ParseExact(PartitionKey, "D"); }
            set { PartitionKey = value.ToString("D"); }
        }

        public Guid Id
        {
            get { return Guid.ParseExact(RowKey, "D"); }
            set { RowKey = value.ToString("D"); }
        }        

        public Guid EmployeeId { get; set; }

        public DateTimeOffset TimeStart { get; set; }
        public DateTimeOffset TimeEnd { get; set; }
        public long Time { get; set; }

        public string Remarks { get; set; }
    }
}
