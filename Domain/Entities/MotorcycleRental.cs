using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorCyclesRentDomain.Entities
{
    public class MotorcycleRental
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int MotorcycleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }
        public decimal TotalCost { get; set; }
        public decimal? PenaltyCost { get; set; }

        public PersonRegistration Person { get; set; }
        public Motorcycle Motorcycle { get; set; }
    }
}
