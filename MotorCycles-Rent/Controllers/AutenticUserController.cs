using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MotorCyclesRentDomain.Dtos.Requests;
using MotorCyclesRentDomain.Dtos.Responses;
using MotorCyclesRentAplicattion.Interfaces;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AutenticUserController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<AutenticUserController> _logger;

    public AutenticUserController(IAuthenticationService authenticationService, ILogger<AutenticUserController> logger)
    {
        _authenticationService = authenticationService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid ModelState: {ModelState}", ModelState);
            return BadRequest(ModelState);
        }

        try
        {
            var loginResponse = await _authenticationService.VerifyLoginAsync(loginRequest);

            if (!string.IsNullOrEmpty(loginResponse.Message))
            {
                return Unauthorized(new { Error = loginResponse.Message });
            }

            return Ok(loginResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao realizar login");
            return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao realizar login: {ex.Message}");
        }
    }
}
