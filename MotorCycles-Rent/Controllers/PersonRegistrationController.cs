using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorCyclesRentDomain.Dtos.Requests;

[ApiController]
[Route("api/[controller]")]
public class PersonRegistrationsController : ControllerBase
{
    private readonly PersonRegistrationService _personRegistrationService;

    public PersonRegistrationsController(PersonRegistrationService personRegistrationService)
    {
        _personRegistrationService = personRegistrationService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePersonRegistration([FromForm] PersonRegistrationRequestDTO dto)
    {
        try
        {
            var result = await _personRegistrationService.CreatePersonRegistration(dto);
            return CreatedAtAction(nameof(GetPersonRegistrationById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPersonRegistrationById(int id)
    {
        try
        {
            var result = await _personRegistrationService.GetPersonRegistrationById(id);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPersonRegistrations()
    {
        var result = await _personRegistrationService.GetAllPersonRegistrations();
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")] // Correção aqui
    public async Task<IActionResult> UpdatePersonRegistration(int id, [FromForm] PersonRegistrationRequestDTO dto)
    {
        try
        {
            var result = await _personRegistrationService.UpdatePersonRegistration(id, dto);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // Correção aqui
    public async Task<IActionResult> DeletePersonRegistration(int id)
    {
        try
        {
            await _personRegistrationService.DeletePersonRegistration(id);
            return Ok(new { message = "Usuário excluído com sucesso." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

}
