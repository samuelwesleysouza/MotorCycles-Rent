using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorCyclesRentDomain.Dtos.Requests
{
    public class RentalRequestDto
    {
        public int PersonId { get; set; }
        public int MotorcycleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
    }
}
