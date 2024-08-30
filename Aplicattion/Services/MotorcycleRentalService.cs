using MotorCyclesRentDomain.Entities;
using MotorCyclesRentInfrastructure;

public class MotorcycleRentalService
{
    private readonly MotorCyclesContext _context;

    public MotorcycleRentalService(MotorCyclesContext context)
    {
        _context = context;
    }

    public async Task<MotorcycleRental> RentMotorcycle(int personId, int motorcycleId, DateTime startDate, DateTime endDate, DateTime expectedReturnDate)
    {
        var person = await _context.PersonRegistrations.FindAsync(personId);
        if (person == null || person.CNHType != "A")
        {
            throw new InvalidOperationException("O entregador não está habilitado para alugar motos.");
        }

        if (startDate.Date <= DateTime.UtcNow.Date)
        {
            throw new ArgumentException("A data de início deve ser o dia seguinte à data de criação.");
        }

        if (endDate.Date <= startDate.Date)
        {
            throw new ArgumentException("A data de término deve ser posterior à data de início.");
        }

        if (expectedReturnDate.Date > endDate.Date)
        {
            throw new ArgumentException("A data prevista para devolução deve estar dentro do período de locação.");
        }

        var motorcycle = await _context.Motorcycles.FindAsync(motorcycleId);
        if (motorcycle == null)
        {
            throw new KeyNotFoundException("Moto não encontrada.");
        }

        var rental = new MotorcycleRental
        {
            PersonId = personId,
            MotorcycleId = motorcycleId,
            StartDate = startDate,
            EndDate = endDate,
            ExpectedReturnDate = expectedReturnDate
        };

        // Calculate total cost
        rental.TotalCost = CalculateTotalCost(startDate, endDate);

        // Calculate penalty or additional cost if any
        rental.PenaltyCost = CalculatePenaltyOrAdditionalCost(expectedReturnDate, endDate, rental.TotalCost);

        _context.MotorcycleRentals.Add(rental);
        await _context.SaveChangesAsync();

        return rental;
    }

    private decimal CalculateTotalCost(DateTime startDate, DateTime endDate)
    {
        var rentalDays = (endDate.Date - startDate.Date).Days;
        decimal dailyRate;

        if (rentalDays <= 7)
        {
            dailyRate = 30;
        }
        else if (rentalDays <= 15)
        {
            dailyRate = 28;
        }
        else if (rentalDays <= 30)
        {
            dailyRate = 22;
        }
        else if (rentalDays <= 45)
        {
            dailyRate = 20;
        }
        else
        {
            dailyRate = 18;
        }

        return dailyRate * rentalDays;
    }

    private decimal? CalculatePenaltyOrAdditionalCost(DateTime expectedReturnDate, DateTime endDate, decimal totalCost)
    {
        if (endDate < expectedReturnDate)
        {
            var daysEarly = (expectedReturnDate.Date - endDate.Date).Days;
            var penaltyRate = GetPenaltyRate((expectedReturnDate.Date - endDate.Date).Days);
            return penaltyRate * totalCost;
        }
        else if (endDate > expectedReturnDate)
        {
            var daysLate = (endDate.Date - expectedReturnDate.Date).Days;
            return 50 * daysLate; // R$50 por day adicional
        }

        return null;
    }

    private decimal GetPenaltyRate(int days)
    {
        if (days <= 7)
        {
            return 0.2m; // 20% penalty
        }
        else if (days <= 15)
        {
            return 0.4m; // 40% penalty
        }

        return 0; // No penalty for other durations
    }
}