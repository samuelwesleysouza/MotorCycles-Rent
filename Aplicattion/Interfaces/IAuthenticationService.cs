using System.Threading.Tasks;
using MotorCyclesRentDomain.Dtos.Requests;
using MotorCyclesRentDomain.Dtos.Responses;

namespace MotorCyclesRentAplicattion.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LoginResponseDTO> VerifyLoginAsync(LoginRequestDTO request);
    }
}

