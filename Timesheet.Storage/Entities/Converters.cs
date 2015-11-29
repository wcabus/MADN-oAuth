using Timesheet.Domain;

namespace Timesheet.Entities
{
    internal static class Converters
    {
        public static ProjectEntity ToEntity(this Project model)
        {
            return new ProjectEntity
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description
            };
        }

        public static Project ToDomain(this ProjectEntity entity)
        {
            return new Project
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };
        }
    }
}
