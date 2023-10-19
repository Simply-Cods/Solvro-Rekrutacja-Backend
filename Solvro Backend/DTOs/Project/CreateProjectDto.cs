using System.ComponentModel.DataAnnotations;

namespace Solvro_Backend.DTOs
{
    public class CreateProjectDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name field cannot be null or empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "OwnerId cannot be null")]
        public long? OwnerId { get; set; }
        [Required(ErrorMessage = "MemberIds must be present")]
        public List<long> MemberIds {get; set; }
    }
}
