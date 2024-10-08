﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MotorCyclesRentInfrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MotorCyclesContext>
    {
        public MotorCyclesContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MotorCyclesContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Database=motorcycles;Username=motorcycles;Password=motorcycles$#@!");

            return new MotorCyclesContext(optionsBuilder.Options);
        }
    }
}
