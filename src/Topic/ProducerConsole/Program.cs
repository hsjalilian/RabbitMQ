using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var serverConnection = configuration.GetSection("RabbitMQ");

var factory = new ConnectionFactory
{
    HostName = serverConnection.GetValue<string>("HostName"),
    UserName = serverConnection.GetValue<string>("Username"),
    Password = serverConnection.GetValue<string>("Password")
};

// Establishes a connection to the message broker using the provided factory.
using var connection = factory.CreateConnection();

// Creates a channel within the established connection for communication.
using var channel = connection.CreateModel();

channel.ExchangeDeclare(
        exchange: "request",  // Declare an exchange named "request".
        type: ExchangeType.Topic // This exchange is of type "Topic", allowing for flexible routing based on topic patterns.
    );

Console.WriteLine("You can create and send 10 randomly generated strings using the 'random' keyword as input.");
Console.WriteLine("Your message provides a description of an order created in the US.");
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

            Console.WriteLine($"{message}");
            channel.BasicPublish(
                exchange: "request",
                routingKey: "order.created.us",
                basicProperties: null,
                body: Encoding.UTF8.GetBytes(message)
            );
        }
        continue;
    }

    var body = Encoding.UTF8.GetBytes(message);
    channel.BasicPublish(
        exchange: "request",
        routingKey: "order.created.us",
        basicProperties: null,
        body: body
    );
}