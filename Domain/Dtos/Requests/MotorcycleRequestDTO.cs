﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorCyclesRentDomain.Dtos.Requests
{
    public class MotorcycleRequestDTO
    {
        public string Renavam { get; set; } // Tipo string para aceitar letras e números
        public int Year { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }
    }


}