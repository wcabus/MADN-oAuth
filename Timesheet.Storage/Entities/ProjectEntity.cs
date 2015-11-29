using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Timesheet.Entities
{
    public class ProjectEntity : TableEntity
    {
        public ProjectEntity()
        {
            PartitionKey = "";
        }

        public Guid Id
        {
            get { return Guid.ParseExact(RowKey, "D"); }
            set { RowKey = value.ToString("D"); }
        }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
