using Microsoft.EntityFrameworkCore;
using MotorCyclesRentDomain.Entities;

namespace MotorCyclesRentInfrastructure
{
    public class MotorCyclesContext : DbContext
    {
        public MotorCyclesContext(DbContextOptions<MotorCyclesContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da entidade PersonRegistration
            modelBuilder.Entity<PersonRegistration>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(e => e.CNPJ)
                    .IsUnique();

                entity.HasIndex(e => e.CNHNumber)
                    .IsUnique();
            });

            // Configuração da entidade Motorcycle
            modelBuilder.Entity<Motorcycle>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Renavam)
                    .IsRequired()
                    .HasMaxLength(50); // Ajuste o tamanho conforme necessário

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Plate)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasIndex(e => e.Plate)
                    .IsUnique();

                // Adicione outras configurações se necessário
            });
        }

        public DbSet<PersonRegistration> PersonRegistrations { get; set; }
        public DbSet<Motorcycle> Motorcycles { get; set; }

        public DbSet<MotorcycleRental> MotorcycleRentals { get; set; }
    }
}
