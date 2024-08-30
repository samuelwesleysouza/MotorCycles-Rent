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
    /// <summary>
    /// Consumidor de mensagens do RabbitMQ para motocicletas.
    /// </summary>
    public class MotorcycleConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Construtor do consumidor de motocicletas.
        /// </summary>
        /// <param name="serviceProvider">Instância do provedor de serviços.</param>
        /// <param name="configuration">Instância de configuração.</param>
        public MotorcycleConsumer(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        /// <summary>
        /// Executa o serviço em segundo plano.
        /// </summary>
        /// <param name="stoppingToken">Token de cancelamento para o serviço.</param>
        /// <returns>Tarefa representando a operação assíncrona.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:HostName"],
                UserName = _configuration["RabbitMQ:UserName"],
                Password = _configuration["RabbitMQ:Password"]
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

            await Task.CompletedTask;
        }

        private async Task SaveMotorcycleToDatabase(Motorcycle motorcycle)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MotorCyclesContext>();

            // Armazena a motocicleta no banco de dados
            context.Motorcycles.Add(motorcycle);
            await context.SaveChangesAsync();
        }
    }
}
