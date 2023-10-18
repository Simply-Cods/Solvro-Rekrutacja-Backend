using System.ComponentModel.DataAnnotations;

namespace Solvro_Backend.Models.Database
{
    public class Project
    {
        [Key]
        public long Id { get; set; }
        public User Owner { get; set; }
        public string Name { get; set; }
        public ICollection<ProjectMemberMapping> ProjectMemberMappings { get; set; }
        public ICollection<Task> Tasks { get; set; }
    }
}
