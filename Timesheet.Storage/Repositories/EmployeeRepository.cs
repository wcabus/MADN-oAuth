using System;
using System.Collections.Generic;
using System.Linq;
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

            var result = table.CreateQuery<EmployeeEntity>()
                        .Where(x => x.PartitionKey == "" && x.RowKey == entity.RowKey)
                        .SingleOrDefault();

            return result?.ToDomain();
        }
    }
}