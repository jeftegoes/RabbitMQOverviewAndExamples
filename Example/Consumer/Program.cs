using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

const string url = "amqp://guest:guest@localhost:5672";
const string queueName = "books-queue";

var connectionFactory = new ConnectionFactory()
{
    Uri = new Uri(url)
};

var connection = connectionFactory.CreateConnection();
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

    Console.WriteLine("Waiting for messages...");

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine($"Message received: {message}");
    };
    channel.BasicConsume(queue: queueName,
                         autoAck: true,
                         consumer: consumer);

    Console.WriteLine(" Press [enter] to exit, but wait for listen...");
    Console.ReadLine();
}