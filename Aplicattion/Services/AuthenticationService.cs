using System.Threading.Tasks;
using MotorCyclesRentDomain.Dtos.Requests;
using MotorCyclesRentDomain.Dtos.Responses;
using MotorCyclesRentAplicattion.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using MotorCyclesRentDomain.Entities;
using MotorCyclesRentInfrastructure;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MotorCyclesRentAplicattion.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly MotorCyclesContext _context;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<PersonRegistration> _passwordHasher;

        public AuthenticationService(MotorCyclesContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<PersonRegistration>();
        }

        public async Task<LoginResponseDTO> VerifyLoginAsync(LoginRequestDTO request)
        {
            try
            {
                var personRegistration = await _context.PersonRegistrations
                    .FirstOrDefaultAsync(pr => pr.CPF == request.CPF); 

                if (personRegistration == null)
                {
                    return new LoginResponseDTO { Message = "Número do CPF não encontrado." };
                }

                var result = _passwordHasher.VerifyHashedPassword(personRegistration, personRegistration.Password, request.Password);

                if (result != PasswordVerificationResult.Success)
                {
                    return new LoginResponseDTO { Message = "Senha incorreta." };
                }

                var token = GenerateJwtToken(personRegistration);

                return new LoginResponseDTO
                {
                    Token = token,
                    UserId = personRegistration.Id,
                    Name = personRegistration.Name,
                    UserTypeEnum = personRegistration.UserTypeEnum
                };
            }
            catch (Exception ex)
            {
              
                return new LoginResponseDTO { Message = $"Erro ao verificar login: {ex.Message}" };
            }
        }

        private string GenerateJwtToken(PersonRegistration personRegistration)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, personRegistration.CPF),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.Name, personRegistration.Name),
        new Claim(ClaimTypes.Role, personRegistration.UserTypeEnum.ToString()) // Correção aqui
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
