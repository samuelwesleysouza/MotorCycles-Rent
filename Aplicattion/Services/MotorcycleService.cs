using Microsoft.EntityFrameworkCore;
using MotorCyclesRentDomain.Dtos.Requests;
using MotorCyclesRentDomain.DTOs.Responses;
using MotorCyclesRentDomain.Entities;
using MotorCyclesRentInfrastructure;
using MotorCyclesRentInfrastructure.Messaging; 

/// <summary>
/// Serviço para gerenciar operações relacionadas a motocicletas.
/// </summary>
public class MotorcycleService
{
    private readonly MotorCyclesContext _context;
    private readonly RabbitMQPublisher _publisher;

    /// <summary>
    /// Construtor do serviço de motocicletas.
    /// </summary>
    /// <param name="context">Instância do contexto do banco de dados.</param>
    /// <param name="publisher">Instância do publicador RabbitMQ.</param>
    public MotorcycleService(MotorCyclesContext context, RabbitMQPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
    }

    /// <summary>
    /// Cria uma nova motocicleta no banco de dados e publica um evento.
    /// </summary>
    /// <param name="dto">Objeto DTO contendo as informações da motocicleta.</param>
    /// <returns>Objeto DTO com as informações da motocicleta criada.</returns>
    /// <exception cref="InvalidOperationException">Lançado se a placa já estiver registrada.</exception>
    public async Task<MotorcycleResponseDTO> CreateMotorcycle(MotorcycleRequestDTO dto)
    {
        // Verifica se já existe uma motocicleta com a mesma placa
        var existingMotorcycle = await _context.Motorcycles
            .FirstOrDefaultAsync(m => m.Plate == dto.Plate);

        if (existingMotorcycle != null)
        {
            throw new InvalidOperationException("A placa já está registrada.");
        }

        // Cria uma nova entidade Motorcycle com base nos dados fornecidos
        var motorcycle = new Motorcycle
        {
            Renavam = dto.Renavam, // Certifique-se de que o tipo é string no DTO e na entidade
            Year = dto.Year,
            Model = dto.Model,
            Plate = dto.Plate
        };

        // Adiciona a nova motocicleta ao contexto e salva as alterações no banco de dados
        _context.Motorcycles.Add(motorcycle);
        await _context.SaveChangesAsync();

        // Publicar evento de motocicleta criada
        _publisher.PublishMotorcycleCreated(motorcycle);

        // Retorna um DTO com as informações da motocicleta criada
        return new MotorcycleResponseDTO
        {
            Id = motorcycle.Id,
            Renavam = motorcycle.Renavam,
            Year = motorcycle.Year,
            Model = motorcycle.Model,
            Plate = motorcycle.Plate
        };
    }

/// <summary>
/// Obtém uma motocicleta pelo ID.
/// </summary>
/// <param name="id">ID da motocicleta a ser recuperada.</param>
/// <returns>Objeto DTO com as informações da motocicleta encontrada.</returns>
/// <exception cref="KeyNotFoundException">Lançado se a motocicleta não for encontrada.</exception>
public async Task<MotorcycleResponseDTO> GetMotorcycleById(int id)
    {
        // Recupera a motocicleta com o ID especificado
        var motorcycle = await _context.Motorcycles
            .FindAsync(id);

        if (motorcycle == null)
        {
            throw new KeyNotFoundException("Moto não encontrada.");
        }

        // Retorna um DTO com as informações da motocicleta encontrada
        return new MotorcycleResponseDTO
        {
            Id = motorcycle.Id,
            Renavam = motorcycle.Renavam,
            Year = motorcycle.Year,
            Model = motorcycle.Model,
            Plate = motorcycle.Plate
        };
    }

    /// <summary>
    /// Obtém todas as motocicletas do banco de dados.
    /// </summary>
    /// <returns>Uma lista de objetos DTO com as informações de todas as motocicletas.</returns>
    public async Task<IEnumerable<MotorcycleResponseDTO>> GetAllMotorcycles()
    {
        // Recupera todas as motocicletas e projeta os resultados para DTOs
        return await _context.Motorcycles
            .Select(m => new MotorcycleResponseDTO
            {
                Id = m.Id,
                Renavam = m.Renavam,
                Year = m.Year,
                Model = m.Model,
                Plate = m.Plate
            })
            .ToListAsync();
    }

    /// <summary>
    /// Obtém motocicletas com base na placa.
    /// </summary>
    /// <param name="plate">Placa da motocicleta a ser pesquisada.</param>
    /// <returns>Uma lista de objetos DTO com as informações das motocicletas que correspondem à placa.</returns>
    public async Task<IEnumerable<MotorcycleResponseDTO>> GetMotorcyclesByPlate(string plate)
    {
        // Filtra as motocicletas que contêm a placa especificada e projeta os resultados para DTOs
        return await _context.Motorcycles
            .Where(m => m.Plate.Contains(plate))
            .Select(m => new MotorcycleResponseDTO
            {
                Id = m.Id,
                Renavam = m.Renavam,
                Year = m.Year,
                Model = m.Model,
                Plate = m.Plate
            })
            .ToListAsync();
    }

    /// <summary>
    /// Atualiza uma motocicleta existente.
    /// </summary>
    /// <param name="id">ID da motocicleta a ser atualizada.</param>
    /// <param name="dto">Objeto DTO contendo as novas informações da motocicleta.</param>
    /// <returns>Objeto DTO com as informações da motocicleta atualizada.</returns>
    /// <exception cref="KeyNotFoundException">Lançado se a motocicleta não for encontrada.</exception>
    /// <exception cref="InvalidOperationException">Lançado se a placa já estiver registrada por outra motocicleta.</exception>
    public async Task<MotorcycleResponseDTO> UpdateMotorcycle(int id, MotorcycleRequestDTO dto)
    {
        // Recupera a motocicleta com o ID especificado
        var motorcycle = await _context.Motorcycles
            .FindAsync(id);

        if (motorcycle == null)
        {
            throw new KeyNotFoundException("Moto não encontrada.");
        }

        // Verifica se já existe outra motocicleta com a mesma placa (excluindo a atual)
        var existingMotorcycle = await _context.Motorcycles
            .Where(m => m.Id != id)
            .FirstOrDefaultAsync(m => m.Plate == dto.Plate);

        if (existingMotorcycle != null)
        {
            throw new InvalidOperationException("A placa já está registrada.");
        }

        // Atualiza os detalhes da motocicleta com os novos dados
        motorcycle.Renavam = dto.Renavam; // Atualizar o Renavam
        motorcycle.Year = dto.Year;
        motorcycle.Model = dto.Model;
        motorcycle.Plate = dto.Plate;

        // Marca a motocicleta como atualizada e salva as alterações no banco de dados
        _context.Motorcycles.Update(motorcycle);
        await _context.SaveChangesAsync();

        // Retorna um DTO com as informações da motocicleta atualizada
        return new MotorcycleResponseDTO
        {
            Id = motorcycle.Id,
            Renavam = motorcycle.Renavam,
            Year = motorcycle.Year,
            Model = motorcycle.Model,
            Plate = motorcycle.Plate
        };
    }

    /// <summary>
    /// Remove uma motocicleta do banco de dados.
    /// </summary>
    /// <param name="id">ID da motocicleta a ser removida.</param>
    /// <exception cref="KeyNotFoundException">Lançado se a motocicleta não for encontrada.</exception>
    public async Task DeleteMotorcycle(int id)
    {
        // Recupera a motocicleta com o ID especificado
        var motorcycle = await _context.Motorcycles
            .FindAsync(id);

        if (motorcycle == null)
        {
            throw new KeyNotFoundException("Moto não encontrada.");
        }

        // Remove a motocicleta do contexto e salva as alterações no banco de dados
        _context.Motorcycles.Remove(motorcycle);
        await _context.SaveChangesAsync();
    }
}
