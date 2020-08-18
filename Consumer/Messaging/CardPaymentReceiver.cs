using Consumer.Entities;
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
        private IConnection connection;
        private IModel channel;

        public CardPaymentReceiver(IOptions<RabbitMqConfig> options)
        {
            hostname = options.Value.Hostname;
            userName = options.Value.UserName;
            password = options.Value.Password;
            factory = new ConnectionFactory() { HostName = hostname, UserName = userName, Password = password };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(queue: CardQueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        private void HandleMessage(byte[] message)
        {
            var payment = message.Deserialize<CardPayment>();
            Console.WriteLine($"Payment received {payment.Name}, {payment.CardNumber}, ${payment.Amount}");
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
