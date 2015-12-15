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

        public string AtomiumAccount
        {
            get { return RowKey; }
            set { RowKey = value; }
        }

        public string Name { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
    }
}
