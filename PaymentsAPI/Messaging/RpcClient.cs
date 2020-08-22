using Microsoft.Extensions.Options;
using PaymentsAPI.Interfaces;
using PaymentsAPI.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentsAPI.Messaging
{
    public class RpcClient : IRpcClient, IDisposable
    {
        private readonly string hostname;
        private readonly string userName;
        private readonly string password;
        private readonly ConnectionFactory factory;
        private IConnection connection;
        private IModel channel;
        private const string QueueName = "rpc_queue";
        private string replyQueueName;
        private EventingBasicConsumer consumer;
        private IBasicProperties props;
        private ConcurrentDictionary<string, TaskCompletionSource<string>> pendingMessages;

        public RpcClient(IOptions<RabbitMqConfig> options)
        {
            hostname = options.Value.Hostname;
            userName = options.Value.UserName;
            password = options.Value.Password;
            factory = new ConnectionFactory() { HostName = hostname, UserName = userName, Password = password };
            Connect();
        }

        private void Connect()
        {           
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            replyQueueName = channel.QueueDeclare().QueueName;
            pendingMessages = new ConcurrentDictionary<string, TaskCompletionSource<string>>();
            consumer = new EventingBasicConsumer(channel);
            consumer.Received += OnConsumerReceived;
            channel.BasicConsume(replyQueueName, autoAck: true, consumer);       
        }

        private void OnConsumerReceived(object sender, BasicDeliverEventArgs e)
        {
            var correlationId = e.BasicProperties.CorrelationId;
            var message = Encoding.UTF8.GetString(e.Body.ToArray());
            
            Console.WriteLine($"Received: {message} with CorrelationId {correlationId}");
            
            pendingMessages.TryRemove(correlationId, out var taskCompletionSource);
            
            if (taskCompletionSource != null)
            {
                taskCompletionSource.SetResult(message);
            }
        }

        public Task<string> MakePaymentAsync(CardPayment payment)
        {
            var tcs = new TaskCompletionSource<string>();           
            var correlationId = Guid.NewGuid().ToString();
            
            pendingMessages[correlationId] = tcs;

            SendMessage(payment.Serialize(), correlationId);
            
            Console.WriteLine($"Sent: ${payment.Amount} from {payment.Name} with CorrelationId {correlationId}");

            return tcs.Task;
        }

        private void SendMessage(byte[] message, string correlationId)
        {            
            props = channel.CreateBasicProperties();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;
            channel.BasicPublish("", QueueName, props, message);
        }

        public void CloseConnection()
        {
            if (channel.IsOpen)
            {
                channel.Close();
            }
            
            if (connection.IsOpen)
            {
                connection.Close();
            }           
        }

        public void Dispose()
        {
            CloseConnection();
        }
    }
}
