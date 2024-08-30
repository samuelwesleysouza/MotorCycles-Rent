using Microsoft.Extensions.Configuration;
using MotorCyclesRentDomain.Entities;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace MotorCyclesRentInfrastructure.Messaging
{
    /// <summary>
    /// Publicador de mensagens para o RabbitMQ.
    /// </summary>
    public class RabbitMQPublisher
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Construtor do publicador RabbitMQ.
        /// </summary>
        /// <param name="configuration">Instância de configuração.</param>
        public RabbitMQPublisher(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Publica uma mensagem quando uma motocicleta é criada.
        /// </summary>
        /// <param name="motorcycle">Objeto Motorcycle a ser publicado.</param>
        public void PublishMotorcycleCreated(Motorcycle motorcycle)
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

            var message = JsonSerializer.Serialize(motorcycle);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                 routingKey: _configuration["RabbitMQ:QueueName"],
                                 basicProperties: null,
                                 body: body);
        }
    }
}
