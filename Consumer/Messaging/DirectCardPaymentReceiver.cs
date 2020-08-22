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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Consumer.Messaging
{
    public class DirectCardPaymentReceiver : BackgroundService
    {
        private string hostname;
        private const string QueueName = "rpc_queue";
        private readonly string userName;
        private readonly string password;
        private readonly ConnectionFactory factory;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private IConnection connection;
        private IModel channel;
        private static Random random = new Random();

        public DirectCardPaymentReceiver(IOptions<RabbitMqConfig> options, IServiceScopeFactory serviceScopeFactory)
        {
            hostname = options.Value.Hostname;
            userName = options.Value.UserName;
            password = options.Value.Password;
            factory = new ConnectionFactory() { HostName = hostname, UserName = userName, Password = password };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += ConsumerReceived;

            channel.BasicConsume(QueueName, false, consumer);

            return Task.CompletedTask;
        }

        private void ConsumerReceived(object sender, BasicDeliverEventArgs e)
        {
            var payment = e.Body.ToArray().Deserialize<CardPayment>();
            var props = e.BasicProperties;
            var replyProps = channel.CreateBasicProperties();
            replyProps.CorrelationId = props.CorrelationId;

            var authCode = HandlePayment(payment);
            
            var message = Encoding.UTF8.GetBytes(authCode.ToString());

            channel.BasicPublish("", props.ReplyTo, replyProps, message);
            channel.BasicAck(e.DeliveryTag, false);
        }

        private int HandlePayment(CardPayment payment)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var cardPaymentService = scope.ServiceProvider.GetRequiredService<ICardPaymentService>();
                cardPaymentService.MakePayment(payment);
            }
            return random.Next(100000, 999999);
        }

        public override void Dispose()
        {
            channel.Close();
            connection.Close();
            base.Dispose();
        }
    }
}
