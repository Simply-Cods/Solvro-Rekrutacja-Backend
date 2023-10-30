using Solvro_Backend.Enums;
using Solvro_Backend.Logic;
using System.ComponentModel.DataAnnotations;

namespace Solvro_Backend.DTOs
{
    public class CreateTaskDto: IValidatableObject
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name must not be null or empty")]
        public string Name { get; set; }
        public long? AssignedUserId { get; set; }
        [Required(ErrorMessage = "Estimation is required")]
        public int? Estimation { get; set; }
        [Required(ErrorMessage = "Specialization is required")]
        [EnumDataType(typeof(Specialization), ErrorMessage = "Specialization must be a valid enum member")]
        public Specialization? Specialization { get; set; }
        [Required(ErrorMessage = "CreatorId is required")]
        public long? CreatorId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Fibonacci.IsFibo(Estimation!.Value))
            {
                yield return new ValidationResult($"Estimation must be a number from the Fibonacci sequence.", new[] { nameof(Estimation) });
            }
        }
    }
}
