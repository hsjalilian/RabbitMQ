using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
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

channel.QueueDeclare(
        queue: "basic",        // Specifies the name of the queue being declared.
        durable: false,        // Indicates whether the queue should survive a broker restart; false means it won't.
        exclusive: false,      // Indicates whether the queue can be accessed by other connections; false means it can.
        autoDelete: false,     // Indicates whether the queue will be deleted when the last consumer unsubscribes; false means it won't.
        arguments: null       // Additional arguments for the queue declaration; in this case, no additional arguments are provided.
    );

Console.WriteLine(" Press [enter] for stop service.");
Console.WriteLine(" Consumer Waiting for messages...");

//Creates a consumer instance tied to the specified channel for handling incoming messages.
var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, e) =>
{
    var body = e.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" Incoming message: {message}");
};

channel.BasicConsume(
        queue: "basic",     // Specifies the name of the queue from which messages will be consumed.
        autoAck: true,      // Indicates whether automatic acknowledgment should be enabled; true means messages are automatically acknowledged upon delivery.
        consumer: consumer  // Specifies the object responsible for handling incoming messages; typically an instance of a class implementing the IBasicConsumer interface.
    );

Console.ReadLine();