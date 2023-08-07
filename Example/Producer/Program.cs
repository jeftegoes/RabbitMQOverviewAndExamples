using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

const string url = "amqp://guest:guest@localhost:5672";
const string queueName = "books-queue";

var connectionFactory = new ConnectionFactory()
{
    Uri = new Uri(url)
};

var connection = connectionFactory.CreateConnection();
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare(queueName, false, false, false, null);

    var book = new { Name = "Capitães da Areia" };
    var bookSerialized = JsonSerializer.Serialize(book);
    var bytes = Encoding.UTF8.GetBytes(bookSerialized);

    channel.BasicPublish(string.Empty, queueName, false, null, bytes);
}