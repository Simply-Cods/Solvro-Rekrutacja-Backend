using Solvro_Backend.Models.Database;

namespace Solvro_Backend.Models.Views
{
    public class ProjectView
    {
        public long Id { get; set; }
        public string Name { get; set; }
        
        public ProjectView(Project project)
        {
            Id = project.Id;
            Name = project.Name;
        }
    }
}
