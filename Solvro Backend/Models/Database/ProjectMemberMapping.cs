using System.ComponentModel.DataAnnotations;

namespace Solvro_Backend.Models.Database
{
    public class ProjectMemberMapping
    {
        [Key]
        public long Id { get; set; }
        public User User {  get; set; }
        public Project Project { get; set; }
    }
}
