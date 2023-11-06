using Solvro_Backend.Enums;
using Solvro_Backend.Logic;
using System.ComponentModel.DataAnnotations;

namespace Solvro_Backend.DTOs
{
    public class CreateTaskDto: IValidatableObject
    {
        /// <summary>
        /// Name of the task
        /// </summary>
        /// <example>Create the entire API</example>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name must not be null or empty")]
        public string Name { get; set; }

        /// <summary>
        /// Assigned User ID
        /// </summary>
        /// <example>1</example>
        public long? AssignedUserId { get; set; }

        /// <summary>
        /// Estimation of the task - must be a number from the fibonacci sequence
        /// </summary>
        /// <example>8</example>
        [Required(ErrorMessage = "Estimation is required")]
        public int? Estimation { get; set; }

        /// <summary>
        /// Specialization of the task
        /// </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "Specialization is required")]
        [EnumDataType(typeof(Specialization), ErrorMessage = "Specialization must be a valid enum member")]
        public Specialization? Specialization { get; set; }

        /// <summary>
        /// User ID of the task creator
        /// </summary>
        /// <example>1</example>
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
