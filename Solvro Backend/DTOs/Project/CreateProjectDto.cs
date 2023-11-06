using System.ComponentModel.DataAnnotations;

namespace Solvro_Backend.DTOs
{
    public class CreateProjectDto
    {
        /// <summary>
        /// Name of the project
        /// </summary>
        /// <example>Solvro Backend</example>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name field cannot be null or empty")]
        public string Name { get; set; }

        /// <summary>
        /// User ID of the owner
        /// </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "OwnerId cannot be null")]
        public long? OwnerId { get; set; }

        /// <summary>
        /// IDs of the members, Owner is not a member by default and must be added manually if desired
        /// </summary>
        /// <example>[1]</example>
        [Required(ErrorMessage = "MemberIds must be present")]
        public List<long> MemberIds {get; set; }
    }
}
