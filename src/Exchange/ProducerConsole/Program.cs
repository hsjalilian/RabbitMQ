using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var serverConnection1 = configuration.GetSection("RabbitMQ");

var factory = new ConnectionFactory
{
    HostName = serverConnection1.GetValue<string>("HostName"),
    UserName = serverConnection1.GetValue<string>("Username"),
    Password = serverConnection1.GetValue<string>("Password")
};

// Establishes a connection to the message broker using the provided factory.
using var connection = factory.CreateConnection();

// Creates a channel within the established connection for communication.
using var channel = connection.CreateModel();

channel.ExchangeDeclare(
        exchange: "notifications",  // Declare an exchange named "notifications".
        type: ExchangeType.Fanout // This exchange type is "fanout", broadcasting messages to all bound queues.
    );

Console.WriteLine("You can create and send 10 randomly generated strings using the 'random' keyword as input.");
Console.WriteLine(" Type exit for stop! ");
string message = "";

while (true)
{
    Console.Write(" Type your message: ");
    message = Console.ReadLine() ?? "exit";
    if (message.Equals("exit"))
        break;

    if (message.Equals("random"))
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var random = new Random();
        for (int i = 0; i < 10; i++)
        {
            message = new string(Enumerable.Repeat(chars, random.Next(5, 15))
                .Select(s => s[random.Next(s.Length)]).ToArray());
            Console.WriteLine(message);
            channel.BasicPublish(
                exchange: "notifications",
                // The message is sent with an empty routing key, indicating it should be broadcasted to all queues bound to the exchange.
                routingKey: string.Empty,
                basicProperties: null,
                body: Encoding.UTF8.GetBytes(message)
            );
        }
        continue;
    }

    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(
        exchange: "notifications",
        // The message is sent with an empty routing key, indicating it should be broadcasted to all queues bound to the exchange.
        routingKey: string.Empty,
        basicProperties: null,
        body: body
    );
}

