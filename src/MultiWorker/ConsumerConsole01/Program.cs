using System.Text;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

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
        queue: "multiworker",   // Specifies the name of the queue being declared.
        durable: true,          // Indicates whether the queue should survive a broker restart.
        exclusive: false,       // Indicates whether the queue can be accessed by other connections.
        autoDelete: false,      // Indicates whether the queue will be deleted when the last consumer unsubscribes.
        arguments: null         // Additional arguments for the queue declaration.
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

    channel.BasicAck(
        deliveryTag: e.DeliveryTag,   // Specifies the delivery tag of the message being acknowledged.
        multiple: false               // Indicates whether to acknowledge only the specified message (`false`) or all messages up to the specified one (`true`).
    );
};

channel.BasicConsume(
    queue: "multiworker",   // Specifies the name of the queue from which messages will be consumed.
    autoAck: false,         // Indicates whether automatic acknowledgment should be enabled; false means manual acknowledgment is required.
    consumer: consumer      // Specifies the object responsible for handling incoming messages.
);

Console.ReadLine();