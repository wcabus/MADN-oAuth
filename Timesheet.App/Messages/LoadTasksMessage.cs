using GalaSoft.MvvmLight.Messaging;
using Timesheet.Domain;

namespace Timesheet.App.Messages
{
    public sealed class LoadTasksMessage : MessageBase
    {
        public LoadTasksMessage(Project project)
        {
            Project = project;
        }

        public Project Project { get; }
    }
}
