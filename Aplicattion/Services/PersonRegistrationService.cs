using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MotorCyclesRentDomain.Dtos.Requests;
using MotorCyclesRentDomain.DTOs.Responses;
using MotorCyclesRentDomain.Entities;
using MotorCyclesRentInfrastructure;
using System.Linq;

public class PersonRegistrationService
{
    private readonly MotorCyclesContext _context;
    private readonly string _uploadsFolder;
    private readonly PasswordHasher<PersonRegistration> _passwordHasher;

    public PersonRegistrationService(MotorCyclesContext context, string uploadsFolder)
    {
        _context = context;
        _uploadsFolder = uploadsFolder;
        _passwordHasher = new PasswordHasher<PersonRegistration>();
    }

    public async Task<PersonRegistrationResponseDTO> CreatePersonRegistration(PersonRegistrationRequestDTO dto)
    {
        if (string.IsNullOrEmpty(dto.CNPJ) && string.IsNullOrEmpty(dto.CPF))
        {
            throw new ArgumentException("Você deve fornecer pelo menos um dos seguintes campos: CNPJ ou CPF.");
        }

        var existingPersonByCNH = await _context.PersonRegistrations
            .FirstOrDefaultAsync(pr => pr.CNHNumber == dto.CNHNumber);

        if (existingPersonByCNH != null)
        {
            throw new InvalidOperationException("O número da CNH já está registrado.");
        }

        if (!string.IsNullOrEmpty(dto.CNPJ))
        {
            var existingPersonByCNPJ = await _context.PersonRegistrations
                .FirstOrDefaultAsync(pr => pr.CNPJ == dto.CNPJ);

            if (existingPersonByCNPJ != null)
            {
                throw new InvalidOperationException("O CNPJ já está registrado.");
            }
        }

        if (!string.IsNullOrEmpty(dto.CPF))
        {
            var existingPersonByCPF = await _context.PersonRegistrations
                .FirstOrDefaultAsync(pr => pr.CPF == dto.CPF);

            if (existingPersonByCPF != null)
            {
                throw new InvalidOperationException("O CPF já está registrado.");
            }
        }

        string filePath = null;
        if (dto.CNHImage != null && dto.CNHImage.Length > 0)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(dto.CNHImage.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new ArgumentException("Tipo de arquivo inválido.");
            }
            if (dto.CNHImage.Length > 5 * 1024 * 1024) // 5 MB
            {
                throw new ArgumentException("O arquivo é muito grande.");
            }

            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            filePath = Path.Combine(_uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await dto.CNHImage.CopyToAsync(stream);
        }

        if (!Enum.TryParse(dto.UserTypeEnum, true, out UserTypeEnum userTypeEnum))
        {
            userTypeEnum = UserTypeEnum.User; // Default to 'User'
        }

        var personRegistration = new PersonRegistration
        {
            Name = dto.Name,
            CNPJ = dto.CNPJ,
            CPF = dto.CPF,
            BirthDate = dto.BirthDate.GetValueOrDefault(),
            CNHNumber = dto.CNHNumber,
            CNHType = dto.TypeCNH,
            UserTypeEnum = userTypeEnum,
            CNHImagePath = filePath,
            Password = _passwordHasher.HashPassword(null, dto.Password)
        };

        _context.PersonRegistrations.Add(personRegistration);
        await _context.SaveChangesAsync();

        return new PersonRegistrationResponseDTO
        {
            Id = personRegistration.Id,
            Name = personRegistration.Name,
            CNPJ = personRegistration.CNPJ,
            CPF = personRegistration.CPF,
            BirthDate = personRegistration.BirthDate,
            CNHNumber = personRegistration.CNHNumber,
            TypeCNH = personRegistration.CNHType,
            UserTypeEnum = personRegistration.UserTypeEnum,
            CNHImage = personRegistration.CNHImagePath
        };
    }

    public async Task<PersonRegistrationResponseDTO> GetPersonRegistrationById(int id)
    {
        var personRegistration = await _context.PersonRegistrations
            .FindAsync(id);

        if (personRegistration == null)
        {
            throw new KeyNotFoundException("Pessoa não encontrada.");
        }

        return new PersonRegistrationResponseDTO
        {
            Id = personRegistration.Id,
            Name = personRegistration.Name,
            CNPJ = personRegistration.CNPJ,
            CPF = personRegistration.CPF,
            BirthDate = personRegistration.BirthDate,
            CNHNumber = personRegistration.CNHNumber,
            TypeCNH = personRegistration.CNHType,
            UserTypeEnum = personRegistration.UserTypeEnum,
            CNHImage = personRegistration.CNHImagePath
        };
    }

    public async Task<IEnumerable<PersonRegistrationResponseDTO>> GetAllPersonRegistrations()
    {
        return await _context.PersonRegistrations
            .Select(pr => new PersonRegistrationResponseDTO
            {
                Id = pr.Id,
                Name = pr.Name,
                CNPJ = pr.CNPJ,
                CPF = pr.CPF,
                BirthDate = pr.BirthDate,
                CNHNumber = pr.CNHNumber,
                TypeCNH = pr.CNHType,
                UserTypeEnum = pr.UserTypeEnum,
                CNHImage = pr.CNHImagePath
            })
            .ToListAsync();
    }

    public async Task<PersonRegistrationResponseDTO> UpdatePersonRegistration(int id, PersonRegistrationRequestDTO dto)
    {
        var personRegistration = await _context.PersonRegistrations
            .FindAsync(id);

        if (personRegistration == null)
        {
            throw new KeyNotFoundException("Pessoa não encontrada.");
        }

        personRegistration.Name = dto.Name ?? personRegistration.Name;
        personRegistration.CNPJ = dto.CNPJ ?? personRegistration.CNPJ;
        personRegistration.CPF = dto.CPF ?? personRegistration.CPF;
        personRegistration.CNHNumber = dto.CNHNumber ?? personRegistration.CNHNumber;
        personRegistration.CNHType = dto.TypeCNH ?? personRegistration.CNHType;

        if (dto.BirthDate.HasValue)
        {
            personRegistration.BirthDate = dto.BirthDate.Value;
        }

        if (dto.CNHImage != null && dto.CNHImage.Length > 0)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(dto.CNHImage.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new ArgumentException("Tipo de arquivo inválido.");
            }
            if (dto.CNHImage.Length > 5 * 1024 * 1024) // 5 MB
            {
                throw new ArgumentException("O arquivo é muito grande.");
            }

            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(_uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await dto.CNHImage.CopyToAsync(stream);

            personRegistration.CNHImagePath = filePath;
        }

        if (!Enum.TryParse(dto.UserTypeEnum, true, out UserTypeEnum userTypeEnum))
        {
            userTypeEnum = UserTypeEnum.User; // Default to 'User'
        }

        personRegistration.UserTypeEnum = userTypeEnum;
        personRegistration.Password = _passwordHasher.HashPassword(personRegistration, dto.Password);

        _context.PersonRegistrations.Update(personRegistration);
        await _context.SaveChangesAsync();

        return new PersonRegistrationResponseDTO
        {
            Id = personRegistration.Id,
            Name = personRegistration.Name,
            CNPJ = personRegistration.CNPJ,
            CPF = personRegistration.CPF,
            BirthDate = personRegistration.BirthDate,
            CNHNumber = personRegistration.CNHNumber,
            TypeCNH = personRegistration.CNHType,
            UserTypeEnum = personRegistration.UserTypeEnum,
            CNHImage = personRegistration.CNHImagePath
        };
    }

    public async Task DeletePersonRegistration(int id)
    {
        var personRegistration = await _context.PersonRegistrations
            .FindAsync(id);

        if (personRegistration == null)
        {
            throw new KeyNotFoundException("Pessoa não encontrada.");
        }

        _context.PersonRegistrations.Remove(personRegistration);
        await _context.SaveChangesAsync();
    }
}