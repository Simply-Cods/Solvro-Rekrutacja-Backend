using Solvro_Backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace Solvro_Backend.DTOs
{
    public class UpdateTaskStatusDto
    {
        [Required(ErrorMessage = "State is required")]
        public TaskState? State { get; set; }
    }
}
