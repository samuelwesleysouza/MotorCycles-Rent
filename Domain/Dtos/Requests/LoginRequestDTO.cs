using System.ComponentModel.DataAnnotations;

namespace MotorCyclesRentDomain.Dtos.Requests
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "CPF or CNPJ is required.")]
        public string CPF { get; set; } = string.Empty;

        [Required(ErrorMessage = "CPF or CNPJ is required.")]
        public string CNPJ { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;
    }
}
