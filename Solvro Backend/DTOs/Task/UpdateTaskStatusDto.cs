using Solvro_Backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace Solvro_Backend.DTOs
{
    public class UpdateTaskStatusDto
    {
        /// <summary>
        /// The new task state
        /// </summary>
        /// <example>2</example>
        [Required(ErrorMessage = "State is required")]
        public TaskState? State { get; set; }
    }
}
