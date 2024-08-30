using System;

namespace MotorCyclesRentDomain.Entities
{
    public class PersonRegistration
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? CNPJ { get; set; }
        public string? CPF { get; set; }
        public DateTime BirthDate { get; set; }
        public string CNHNumber { get; set; }
        public string CNHType { get; set; }
        public string CNHImagePath { get; set; }
        public UserTypeEnum UserTypeEnum { get; set; }
        public string Password { get; set; }
    }

    public enum UserTypeEnum
    {
        User,
        Admin
    }
}
