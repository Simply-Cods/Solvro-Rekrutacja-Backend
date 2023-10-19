using Solvro_Backend.Enums;

using STask = Solvro_Backend.Models.Database.Task;

namespace Solvro_Backend.Models.Views
{
    public class TaskView
    {
        public long Id { get; set; }
        public ProjectView Project { get; set; }
        public string Name { get; set; }
        public UserView? AssignedUser { get; set; }
        public int Estimation { get; set; }
        public Specialization Specialization { get; set; }
        public TaskState State { get; set; }
        public UserView Creator { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public TaskView(STask task)
        {
            Id = task.Id;
            Project = new ProjectView(task.Project);
            Name = task.Name;
            AssignedUser = task.AssignedUser == null ? null : new UserView(task.AssignedUser);
            Estimation = task.Estimation;
            Specialization = task.Specialization;
            State = task.State;
            Creator = new UserView(task.Creator);
            CreatedAt = task.CreatedAt;
            CompletedAt = task.CompletedAt;
        }
    }
}
