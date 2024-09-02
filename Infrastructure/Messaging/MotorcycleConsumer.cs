using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MotorCyclesRentDomain.Entities;
using Microsoft.Extensions.Configuration;

namespace MotorCyclesRentInfrastructure.Consumers
{
    public class MotorcycleConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public MotorcycleConsumer(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var factory = new ConnectionFactory()
                    {
                        HostName = _configuration["RabbitMQ:HostName"],
                        UserName = _configuration["RabbitMQ:UserName"],
                        Password = _configuration["RabbitMQ:Password"],
                        Port = 5672
                    };

                    using var connection = factory.CreateConnection();
                    using var channel = connection.CreateModel();

                    channel.QueueDeclare(queue: _configuration["RabbitMQ:QueueName"],
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += async (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        var motorcycle = JsonSerializer.Deserialize<Motorcycle>(message);

                        if (motorcycle != null && motorcycle.Year == 2024)
                        {
                            await SaveMotorcycleToDatabase(motorcycle);
                        }
                    };

                    channel.BasicConsume(queue: _configuration["RabbitMQ:QueueName"],
                                         autoAck: true,
                                         consumer: consumer);

                    await Task.Delay(Timeout.Infinite, stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                }
            }
        }

        private async Task SaveMotorcycleToDatabase(Motorcycle motorcycle)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MotorCyclesContext>();

            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var existingMotorcycle = await context.Motorcycles
                    .FindAsync(motorcycle.Id);

                if (existingMotorcycle == null)
                {
                    context.Motorcycles.Add(motorcycle);
                }
                else
                {
                    existingMotorcycle.UpdateFrom(motorcycle);
                }

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Erro ao salvar motocicleta: {ex.Message}");
            }
        }
    }
}
