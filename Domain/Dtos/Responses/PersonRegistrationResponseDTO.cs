using MotorCyclesRentDomain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace MotorCyclesRentDomain.DTOs.Responses
{
    public class PersonRegistrationResponseDTO
    {
        // ID da pessoa
        public int Id { get; set; } = default;

        public string Name { get; set; } = string.Empty;

        public string? CNPJ { get; set; }

        public string? CPF { get; set; }

        public DateTime? BirthDate { get; set; }

        public string CNHNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "The type of driver's license is mandatory.")]
        [RegularExpression("^(A|B|A\\+B)$", ErrorMessage = "The driver's license type must be A, B or A+B.")]
        public string TypeCNH { get; set; } = string.Empty;

        public UserTypeEnum UserTypeEnum { get; set; }

        public string? CNHImage { get; set; }

        // Adicionando a propriedade Message
        public string Message { get; set; } = string.Empty;
    }
}
