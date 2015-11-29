using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Timesheet.Entities
{
    public class TaskEntity : TableEntity
    {
        public Guid ProjectId
        {
            get { return Guid.ParseExact(PartitionKey, "D"); }
            set { PartitionKey = value.ToString("D"); }
        }

        public Guid Id
        {
            get { return Guid.ParseExact(RowKey, "D"); }
            set { RowKey = value.ToString("D"); }
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public long? AvailableTime { get; set; }
    }
}
