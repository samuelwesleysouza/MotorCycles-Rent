using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using MotorCyclesRentAplicattion.Interfaces;
using MotorCyclesRentAplicattion.Services;
using MotorCyclesRentInfrastructure;
using MotorCyclesRentInfrastructure.Messaging; 
using MotorCyclesRentInfrastructure.Consumers; 
using System.Text;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Configure o contexto do banco de dados
builder.Services.AddDbContext<MotorCyclesContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MotorCyclesConnection")));

// Registre os serviços
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<MotorcycleService>();
builder.Services.AddScoped<PersonRegistrationService>(provider =>
{
    var context = provider.GetRequiredService<MotorCyclesContext>();
    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
    return new PersonRegistrationService(context, uploadsFolder);
});

// Adicione o MotorcycleRentalService
builder.Services.AddScoped<MotorcycleRentalService>();

// Configuração do RabbitMQ
builder.Services.AddSingleton<RabbitMQPublisher>();
builder.Services.AddHostedService<MotorcycleConsumer>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // Correção aqui
            ValidAudience = builder.Configuration["Jwt:Audience"], // Correção aqui
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Correção aqui
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin")); 
});

// Configure serviços MVC e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MotorCycles Rent API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no formato: 'Bearer {token}'"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new[] { "readAccess", "writeAccess" }
        }
    });
});

var app = builder.Build();

// Aplicar as migrações do banco de dados
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MotorCyclesContext>();
    await dbContext.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MotorCycles Rent API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();  
app.UseAuthorization();
app.MapControllers();

app.Run();
