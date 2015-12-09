using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;
using Timesheet.Domain;
using Timesheet.Entities;

namespace Timesheet.Repositories
{
    public class EmployeeRepository : BaseRepository
    {
        public IEnumerable<Employee> GetEmployees()
        {
            var table = GetTable(EmployeesTable);
            return table.CreateQuery<EmployeeEntity>()
                        .ToList()
                        .Select(x => x.ToDomain());
        }

        public Employee GetEmployeeById(Guid id)
        {
            var entity = new EmployeeEntity { Id = id };
            var table = GetTable(EmployeesTable);

            var result = table
                        .CreateQuery<EmployeeEntity>()
                        .SingleOrDefault(x => x.PartitionKey == "" && x.RowKey == entity.RowKey);

            return result?.ToDomain();
        }

        public Employee CreateEmployee(string name, string firstName, string email)
        {
            var table = GetTable(EmployeesTable);
            var entity = new EmployeeEntity { Id = Guid.NewGuid(), Name = name, FirstName = firstName, Email = email };
            var operation = TableOperation.Insert(entity);
            table.Execute(operation);

            return entity.ToDomain();
        }
    }
}