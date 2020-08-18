using Consumer.Entities;
using Consumer.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PaymentsAPI.Messaging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Consumer.Messaging
{
    public class CardPaymentReceiver : BackgroundService
    {
        private const string CardQueueName = "CardQueue";
        private readonly string hostname;
        private readonly string userName;
        private readonly string password;
        private readonly ConnectionFactory factory;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private IConnection connection;
        private IModel channel;
        
        public CardPaymentReceiver(IOptions<RabbitMqConfig> options, IServiceScopeFactory serviceScopeFactory)
        {
            hostname = options.Value.Hostname;
            userName = options.Value.UserName;
            password = options.Value.Password;
            factory = new ConnectionFactory() { HostName = hostname, UserName = userName, Password = password };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(queue: CardQueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            this.serviceScopeFactory = serviceScopeFactory;
        }

        private void HandleMessage(byte[] message)
        {
            using(var scope = serviceScopeFactory.CreateScope())
            {
                var cardPaymentService = scope.ServiceProvider.GetRequiredService<ICardPaymentService>();
                var payment = message.Deserialize<CardPayment>();
                cardPaymentService.MakePayment(payment);
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (ch, ea) =>
            {
                HandleMessage(ea.Body.ToArray());
                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(CardQueueName, false, consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            channel.Close();
            connection.Close();
            base.Dispose();
        }
    }
}
