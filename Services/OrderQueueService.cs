using System;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using OrderService.Data;
using OrderService.Models;


namespace OrderService.Services
{
    public class OrderQueueService : IDisposable
    {
        private readonly IModel _channel;
        private readonly OrderContext _dbContext;
        private readonly IConnection _connection;

        public OrderQueueService(OrderContext dbContext)
        {
            _dbContext = dbContext;
            var factory = new ConnectionFactory() { HostName = "rabbitmq" }; // baglanti kurulmasi

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.QueueDeclare(queue: "orders",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
            }
            catch (Exception ex)
            {
                // log yapisi dusun
                Console.WriteLine($"RabbitMQ'ya bağlanırken hata oluştu: {ex.Message}");
                throw;
            }
        }

        public void StartListening()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body;
                    var orderJson = Encoding.UTF8.GetString(body.ToArray());
                    var order = JsonSerializer.Deserialize<Order>(orderJson);

                    if (order != null)
                    {
                        await _dbContext.Orders.AddAsync(order);
                        await _dbContext.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    // log yapisi dusun
                    Console.WriteLine($"Mesaj işlenirken hata oluştu: {ex.Message}");
                }
            };

            _channel.BasicConsume(queue: "orders",
                                 autoAck: true, // normalde false? cunku msj gonderilirse onaylanmali
                                 consumer: consumer);
        }

        // order creation process sends message to RabbitMQ
        public void Publish(Order order)
        {
            var orderJson = JsonSerializer.Serialize(order);
            var body = Encoding.UTF8.GetBytes(orderJson);

            _channel.BasicPublish(exchange: "",
                          routingKey: "orders",
                          basicProperties: null,
                          body: body);
        }


        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
