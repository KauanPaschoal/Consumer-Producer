using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Consumer.Services
{
    public class RabbitMqConsumerService : BackgroundService
    {
        private readonly ILogger<RabbitMqConsumerService> _logger;
        private readonly List<string> _messages = new();
        private IConnection? _connection;
        private IModel? _channel;

        public RabbitMqConsumerService(ILogger<RabbitMqConsumerService> logger)
        {
            _logger = logger;
            InitRabbitMq();
        }

        private void InitRabbitMq()
        {
            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq", // nome do serviço no docker-compose
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "horizon_queue",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                lock (_messages)
                {
                    _messages.Add(message);
                }

                _logger.LogInformation($"Mensagem recebida: {message}");
            };

            _channel!.BasicConsume(queue: "horizon_queue",
                                 autoAck: true,
                                 consumer: consumer);

            return Task.CompletedTask;
        }

        public List<string> GetMessages()
        {
            lock (_messages)
            {
                return _messages.ToList();
            }
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}
