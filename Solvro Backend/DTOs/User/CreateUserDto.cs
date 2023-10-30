﻿using Solvro_Backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace Solvro_Backend.DTOs
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Specialization must be present")]
        [EnumDataType(typeof(Specialization), ErrorMessage = "Specialization must be a valid enum member")]
        public Specialization? Specialization { get; set; }
    }
}
