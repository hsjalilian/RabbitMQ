using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
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
        exchange: "logs",  // Declare an exchange named "logs".
        type: ExchangeType.Direct // This exchange type is "Direct",  ExchangeType.Direct indicates a direct exchange.
    );

// Declares a new queue and retrieves its name from the server.
var queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(
         // Binds the newly created queue to the "logs" exchange with an 'info' routing key.
         queue: queueName,
         exchange: "logs",
         routingKey: "info"
   );

Console.WriteLine(" Press [enter] for stop service.");
Console.WriteLine(" Consumer Waiting for receive messages with 'info' route key...");

//Creates a consumer instance tied to the specified channel for handling incoming messages.
var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, e) =>
{
    var body = e.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" Consumer_02: {message}");
};

channel.BasicConsume(
    queue: queueName,   // Specifies the name of the queue from which messages will be consumed.
    autoAck: true,         // Indicates whether automatic acknowledgment should be enabled; false means manual acknowledgment is required.
    consumer: consumer      // Specifies the object responsible for handling incoming messages.
);

Console.ReadLine();