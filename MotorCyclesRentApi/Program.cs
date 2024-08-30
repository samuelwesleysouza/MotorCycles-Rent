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

// Configure the database context
builder.Services.AddDbContext<MotorCyclesContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MotorCyclesConnection")));

// Register services
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<MotorcycleService>();
builder.Services.AddScoped<PersonRegistrationService>(provider =>
{
    var context = provider.GetRequiredService<MotorCyclesContext>();
    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
    return new PersonRegistrationService(context, uploadsFolder);
});
builder.Services.AddScoped<MotorcycleRentalService>();

// Configure RabbitMQ
builder.Services.AddSingleton<RabbitMQPublisher>();
builder.Services.AddHostedService<MotorcycleConsumer>();

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

// Configure MVC and Swagger
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
        Description = "Enter JWT token in the format: 'Bearer {token}'"
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

// Apply database migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MotorCyclesContext>();
    await dbContext.Database.MigrateAsync();
}

// Configure the HTTP request pipeline
app.UseStaticFiles(); // Serve static files from wwwroot

// Ensure Swagger is available in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MotorCycles Rent API V1");
        c.RoutePrefix = string.Empty; // Swagger UI at root
    });
}

// Set default route to serve index.html
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapFallbackToFile("index.html"); // Serve index.html as fallback
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
