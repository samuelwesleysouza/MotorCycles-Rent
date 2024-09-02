using System;

namespace MotorCyclesRentDomain.Entities
{
    public class Motorcycle
    {
        public int Id { get; set; }
        public string Renavam { get; set; } // Tipo string para aceitar letras e números
        public int Year { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }

        // Método para atualizar propriedades a partir de uma nova instância
        public void UpdateFrom(Motorcycle newMotorcycle)
        {
            if (newMotorcycle == null)
                throw new ArgumentNullException(nameof(newMotorcycle));

            Model = newMotorcycle.Model;
            Year = newMotorcycle.Year;
            Plate = newMotorcycle.Plate;
            // Atualize outros campos conforme necessário
        }
    }
}
