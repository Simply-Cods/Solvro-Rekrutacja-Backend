using Solvro_Backend.Enums;

using STask = Solvro_Backend.Models.Database.Task;

namespace Solvro_Backend.Models.Views
{
    public class TaskView
    {
        /// <summary>
        /// Id of the task
        /// </summary>
        /// <example>1</example>
        public long Id { get; set; }

        /// <summary>
        /// Basic information of the related project
        /// </summary>
        public ProjectView Project { get; set; }

        /// <summary>
        /// Name of the task
        /// </summary>
        /// <example>Create the entire API</example>
        public string Name { get; set; }

        /// <summary>
        /// Assigned user (if present)
        /// </summary>
        public UserView? AssignedUser { get; set; }

        /// <summary>
        /// Estimation of the task
        /// </summary>
        /// <example>8</example>
        public int Estimation { get; set; }

        /// <summary>
        /// Specialization of the task
        /// </summary>
        /// <example>1</example>
        public Specialization Specialization { get; set; }

        /// <summary>
        /// State of the task
        /// </summary>
        /// <example>1</example>
        public TaskState State { get; set; }

        /// <summary>
        /// The user who created the task
        /// </summary>
        public UserView Creator { get; set; }

        /// <summary>
        /// Date of creation
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Date of completion - present only when <code>State</code> is set to <code>TaskState.Done (2)</code>
        /// </summary>
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
