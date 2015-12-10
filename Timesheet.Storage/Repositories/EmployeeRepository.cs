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

        public Employee GetEmployeeByAtomiumAccount(string atomiumAccount)
        {
            var entity = new EmployeeEntity { AtomiumAccount = atomiumAccount };
            var table = GetTable(EmployeesTable);

            var result = table
                        .CreateQuery<EmployeeEntity>()
                        .Where(x => x.PartitionKey == "" && x.RowKey == entity.RowKey)
                        .SingleOrDefault();

            return result?.ToDomain();
        }

        public Employee CreateEmployee(string atomiumAccount, string name, string firstName, string email)
        {
            var table = GetTable(EmployeesTable);
            var entity = new EmployeeEntity
            {
                AtomiumAccount = atomiumAccount,
                Name = name,
                FirstName = firstName,
                Email = email
            };

            var operation = TableOperation.Insert(entity);
            table.Execute(operation);

            return entity.ToDomain();
        }
    }
}