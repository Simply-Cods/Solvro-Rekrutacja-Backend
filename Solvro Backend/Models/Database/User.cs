using Solvro_Backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace Solvro_Backend.Models.Database
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        public Specialization Specialization { get; set; }
        public ICollection<Project> OwnedProjects { get; set; }
        public ICollection<ProjectMemberMapping> ProjectMemberMappings { get; set; }
        public ICollection<Task> CreatedTasks { get; set; }
        public ICollection<Task> AssignedTasks { get; set; }
    }
}
