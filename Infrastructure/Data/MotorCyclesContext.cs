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
                    .HasMaxLength(50);

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Plate)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasIndex(e => e.Plate)
                    .IsUnique();

                // Configure o auto-incremento do ID
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
            });

            // Configuração da entidade MotorcycleRental
            modelBuilder.Entity<MotorcycleRental>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Person)
                    .WithMany()
                    .HasForeignKey(e => e.PersonId);

                entity.HasOne(e => e.Motorcycle)
                    .WithMany()
                    .HasForeignKey(e => e.MotorcycleId);
            });
        }

        public DbSet<PersonRegistration> PersonRegistrations { get; set; }
        public DbSet<Motorcycle> Motorcycles { get; set; }
        public DbSet<MotorcycleRental> MotorcycleRentals { get; set; }
    }
}
