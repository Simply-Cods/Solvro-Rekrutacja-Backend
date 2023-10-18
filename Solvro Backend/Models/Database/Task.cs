using Solvro_Backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace Solvro_Backend.Models.Database
{
    public class Task
    {
        [Key]
        public long Id { get; set; }
        public Project Project { get; set; }
        public string Name { get; set; }
        public User? AssignedUser { get; set; }
        public int Estimation { get; set; }
        public Specialization Specialization { get; set; }
        public TaskState State { get; set; }
        public User Creator { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
