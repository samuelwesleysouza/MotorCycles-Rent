using Microsoft.AspNetCore.Mvc;
using MotorCyclesRentDomain.Dtos.Requests;
using MotorCyclesRentDomain.Dtos.Responses;
using System;
using System.Threading.Tasks;

/// <summary>
/// Controlador para gerenciar operações relacionadas às motocicletas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MotorcyclesController : ControllerBase
{
    private readonly MotorcycleService _motorcycleService;

    /// <summary>
    /// Construtor do controlador de motocicletas.
    /// </summary>
    /// <param name="motorcycleService">Instância do serviço de motocicletas.</param>
    public MotorcyclesController(MotorcycleService motorcycleService)
    {
        _motorcycleService = motorcycleService;
    }

    /// <summary>
    /// Cria uma nova motocicleta.
    /// </summary>
    /// <param name="dto">Objeto DTO contendo as informações da motocicleta a ser criada.</param>
    /// <returns>Resposta HTTP com status 201 (Created) se a motocicleta for criada com sucesso.</returns>
    /// <response code="409">Lançado se a placa já estiver registrada.</response>
    [HttpPost]
    public async Task<IActionResult> CreateMotorcycle([FromBody] MotorcycleRequestDTO dto)
    {
        try
        {
            // Cria a motocicleta usando o serviço
            var result = await _motorcycleService.CreateMotorcycle(dto);
            // Retorna a resposta com status 201 (Created) e localização do recurso criado
            return CreatedAtAction(nameof(GetMotorcycleById), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            // Retorna a resposta com status 409 (Conflict) se a placa já estiver registrada
            return Conflict(ex.Message);
        }
    }

    /// <summary>
    /// Obtém uma motocicleta pelo ID.
    /// </summary>
    /// <param name="id">ID da motocicleta a ser recuperada.</param>
    /// <returns>Resposta HTTP com status 200 (OK) se a motocicleta for encontrada.</returns>
    /// <response code="404">Lançado se a motocicleta não for encontrada.</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMotorcycleById(int id)
    {
        try
        {
            // Obtém a motocicleta pelo ID usando o serviço
            var result = await _motorcycleService.GetMotorcycleById(id);
            // Retorna a resposta com status 200 (OK) e os dados da motocicleta
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            // Retorna a resposta com status 404 (Not Found) se a motocicleta não for encontrada
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Obtém todas as motocicletas.
    /// </summary>
    /// <returns>Resposta HTTP com status 200 (OK) e uma lista de todas as motocicletas.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllMotorcycles()
    {
        // Obtém todas as motocicletas usando o serviço
        var result = await _motorcycleService.GetAllMotorcycles();
        // Retorna a resposta com status 200 (OK) e a lista de motocicletas
        return Ok(result);
    }

    /// <summary>
    /// Obtém motocicletas com base na placa.
    /// </summary>
    /// <param name="plate">Placa das motocicletas a serem recuperadas.</param>
    /// <returns>Resposta HTTP com status 200 (OK) e uma lista de motocicletas que correspondem à placa.</returns>
    [HttpGet("plate/{plate}")]
    public async Task<IActionResult> GetMotorcyclesByPlate(string plate)
    {
        // Obtém motocicletas com base na placa usando o serviço
        var result = await _motorcycleService.GetMotorcyclesByPlate(plate);
        // Retorna a resposta com status 200 (OK) e a lista de motocicletas
        return Ok(result);
    }

    /// <summary>
    /// Atualiza uma motocicleta existente.
    /// </summary>
    /// <param name="id">ID da motocicleta a ser atualizada.</param>
    /// <param name="dto">Objeto DTO contendo as novas informações da motocicleta.</param>
    /// <returns>Resposta HTTP com status 200 (OK) se a motocicleta for atualizada com sucesso.</returns>
    /// <response code="400">Lançado se o estado do modelo não for válido.</response>
    /// <response code="404">Lançado se a motocicleta não for encontrada.</response>
    /// <response code="409">Lançado se a placa já estiver registrada por outra motocicleta.</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMotorcycle(int id, [FromBody] MotorcycleRequestDTO dto)
    {
        if (!ModelState.IsValid)
        {
            // Retorna uma resposta com status 400 (Bad Request) se o estado do modelo não for válido
            return BadRequest(ModelState);
        }

        try
        {
            // Atualiza a motocicleta usando o serviço
            var result = await _motorcycleService.UpdateMotorcycle(id, dto);
            // Retorna a resposta com status 200 (OK) e os dados da motocicleta atualizada
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            // Retorna a resposta com status 404 (Not Found) se a motocicleta não for encontrada
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            // Retorna a resposta com status 409 (Conflict) se a placa já estiver registrada
            return Conflict(ex.Message);
        }
    }

    /// <summary>
    /// Remove uma motocicleta.
    /// </summary>
    /// <param name="id">ID da motocicleta a ser removida.</param>
    /// <returns>Resposta HTTP com status 204 (No Content) se a motocicleta for removida com sucesso.</returns>
    /// <response code="404">Lançado se a motocicleta não for encontrada.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMotorcycle(int id)
    {
        try
        {
            // Remove a motocicleta usando o serviço
            await _motorcycleService.DeleteMotorcycle(id);
            // Retorna uma resposta com status 204 (No Content) se a motocicleta for removida com sucesso
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            // Retorna a resposta com status 404 (Not Found) se a motocicleta não for encontrada
            return NotFound(ex.Message);
        }
    }
}
