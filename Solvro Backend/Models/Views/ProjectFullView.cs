using Solvro_Backend.Models.Database;

namespace Solvro_Backend.Models.Views
{
    public class ProjectFullView
    {
        public long Id { get; set; }
        public UserView Owner { get; set; }
        public string Name { get; set; }
        public List<UserView> Members { get; set; }
        //public List<TaskView> Tasks { get; set; }

        public ProjectFullView(Project project)
        {
            Id = project.Id;
            Owner = new UserView(project.Owner);
            Name = project.Name;
            Members = project.ProjectMemberMappings.Select(m => new UserView(m.User)).ToList();
        }
    }
}
