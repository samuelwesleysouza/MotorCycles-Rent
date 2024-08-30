using Microsoft.AspNetCore.Mvc;
using MotorCyclesRentAplicattion.Services;
using MotorCyclesRentDomain.Dtos.Requests;

namespace MotorCycles_Rent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotorcycleRentalController : ControllerBase
    {
        private readonly MotorcycleRentalService _rentalService;

        public MotorcycleRentalController(MotorcycleRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpPost]
        [Route("rent")]
        public async Task<IActionResult> RentMotorcycle([FromBody] RentalRequestDto request)
        {
            try
            {
                var rental = await _rentalService.RentMotorcycle(request.PersonId, request.MotorcycleId, request.StartDate, request.EndDate, request.ExpectedReturnDate);
                return Ok(rental);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
