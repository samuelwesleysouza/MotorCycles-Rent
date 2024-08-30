using Microsoft.AspNetCore.Http;
using System;

namespace MotorCyclesRentDomain.Dtos.Requests
{
    public class PersonRegistrationRequestDTO
    {
        public string Name { get; set; }
        public string? CNPJ { get; set; }
        public string? CPF { get; set; }
        public DateTime? BirthDate { get; set; }  // Permitindo valores nulos
        public string CNHNumber { get; set; }
        public string TypeCNH { get; set; }
        public IFormFile? CNHImage { get; set; }
        public string UserTypeEnum { get; set; }
        public string Password { get; set; }
    }
}
