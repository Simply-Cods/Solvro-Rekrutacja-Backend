using Solvro_Backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace Solvro_Backend.DTOs
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Specialization must be present")]
        public Specialization Specialization { get; set; }
    }
}
