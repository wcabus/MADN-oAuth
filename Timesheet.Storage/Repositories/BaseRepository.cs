using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Diagnostics;

namespace Timesheet.Repositories
{
    public abstract class BaseRepository
    {
        protected const string ProjectsTable = "Projects";
        protected const string TasksTable = "Tasks";
        protected const string RegistrationsTable = "Registrations";

        protected CloudTable GetTable(string tableName)
        {
            Debug.Assert(!string.IsNullOrEmpty(tableName), "!string.IsNullOrEmpty(tableName)");

            tableName = tableName.ToLowerInvariant();

            // Retrieve the storage account from the connection string.
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the table client.
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(tableName);

            table.CreateIfNotExists();
            return table;
        }
    }
}
