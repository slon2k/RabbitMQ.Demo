using Microsoft.Extensions.Options;
using PaymentsAPI.Interfaces;
using PaymentsAPI.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;


namespace PaymentsAPI.Messaging
{
    public class MessageSender : IMessageSender
    {
        private readonly string hostname;
        private readonly string userName;
        private readonly string password;
        private readonly ConnectionFactory factory;
        private const string ExchangeName = "TopicExchange";
        private const string OrderQueueName = "OrderQueue";
        private const string CardQueueName = "CardQueue";
        private const string AllQueueName = "AllPaymentsQueue";
        private const string OrderRoutingKey = "payment.order";
        private const string CardRoutingKey = "payment.card";
        private const string AllRoutingKey = "payment.*";

        public MessageSender(IOptions<RabbitMqConfig> options)
        {
            hostname = options.Value.Hostname;
            userName = options.Value.UserName;
            password = options.Value.Password;
            factory = new ConnectionFactory() { HostName = hostname, UserName = userName, Password = password };
        }

        private void SendMessage(byte[] message, string routingKey)
        {
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(ExchangeName, "topic");
                
                channel.QueueDeclare(queue: OrderQueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueDeclare(queue: CardQueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueDeclare(queue: AllQueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                channel.QueueBind(OrderQueueName, ExchangeName, OrderRoutingKey);
                channel.QueueBind(CardQueueName, ExchangeName, CardRoutingKey);
                channel.QueueBind(AllQueueName, ExchangeName, AllRoutingKey);

                channel.BasicPublish(ExchangeName, routingKey, null, message);
            };
        }
        
        public void SendOrder(Order order)
        {
            SendMessage(order.Serialize(), OrderRoutingKey);
        }

        public void SendCardPayment(CardPayment cardPayment)
        {
            SendMessage(cardPayment.Serialize(), CardRoutingKey);
        }

    }
}
