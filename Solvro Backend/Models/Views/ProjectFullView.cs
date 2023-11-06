using Solvro_Backend.Models.Database;

namespace Solvro_Backend.Models.Views
{
    public class ProjectFullView
    {
        /// <summary>
        /// Id of the project
        /// </summary>
        /// <example>1</example>
        public long Id { get; set; }

        /// <summary>
        /// Owner of the project
        /// </summary>
        public UserView Owner { get; set; }

        /// <summary>
        /// Name of the project
        /// </summary>
        /// <example>Solvro Backend</example>
        public string Name { get; set; }

        /// <summary>
        /// Members (developers) of the project
        /// </summary>
        public List<UserView> Members { get; set; }

        /// <summary>
        /// List of tasks the project contains
        /// </summary>
        public List<TaskView> Tasks { get; set; }

        public ProjectFullView(Project project)
        {
            Id = project.Id;
            Owner = new UserView(project.Owner);
            Name = project.Name;
            Members = project.ProjectMemberMappings.Select(m => new UserView(m.User)).ToList();
            Tasks = project.Tasks.Select(t => new TaskView(t)).ToList();
        }
    }
}
