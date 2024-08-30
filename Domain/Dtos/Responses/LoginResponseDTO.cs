using MotorCyclesRentDomain.Entities;

namespace MotorCyclesRentDomain.Dtos.Responses
{
    public class LoginResponseDTO
    {
        // Token gerado após o login
        public string Token { get; set; } = string.Empty;

        // Identificador do usuário
        public int UserId { get; set; }

        // Nome do usuário
        public string Name { get; set; } = string.Empty;

        // Tipo de usuário
        public UserTypeEnum UserTypeEnum { get; set; }

        // Adicionando a propriedade Message
        public string Message { get; set; } = string.Empty;
    }
}
