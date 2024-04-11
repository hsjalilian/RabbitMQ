using System.Text;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

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

channel.QueueDeclare(
        queue: "multiworker",        // Specifies the name of the queue being declared.
        durable: true,        // Indicates whether the queue should survive a broker restart; false means it won't.
        exclusive: false,      // Indicates whether the queue can be accessed by other connections; false means it can.
        autoDelete: false,     // Indicates whether the queue will be deleted when the last consumer unsubscribes; false means it won't.
        arguments: null       // Additional arguments for the queue declaration; in this case, no additional arguments are provided.
    );

Console.WriteLine(" Type exit for stop! ");
string message = "";

var properties = channel.CreateBasicProperties();
properties.Persistent = true;

while (true)
{
    Console.Write(" Type your message: ");
    message = Console.ReadLine() ?? "exit";
    if (message.Equals("exit"))
        break;

    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(
         exchange: string.Empty,  // Specifies the exchange to which the message will be published; an empty string means default exchange.
         routingKey: "multiworker",     // Specifies the routing key for the message; determines which queues will receive the message.
         basicProperties: properties,   // Specifies additional properties for the message; in this case, no additional properties are provided.
         body: body               // Specifies the message body; the actual content of the message being published.
     );
}