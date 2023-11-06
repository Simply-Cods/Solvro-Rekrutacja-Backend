using Solvro_Backend.Models.Database;

namespace Solvro_Backend.Models.Views
{
    public class ProjectView
    {
        /// <summary>
        /// Id of the project
        /// </summary>
        /// <example>1</example>
        public long Id { get; set; }

        /// <summary>
        /// Name of the project
        /// </summary>
        /// <example>Solvro Backend</example>
        public string Name { get; set; }
        
        public ProjectView(Project project)
        {
            Id = project.Id;
            Name = project.Name;
        }
    }
}
