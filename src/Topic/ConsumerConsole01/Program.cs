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

channel.ExchangeDeclare(
        exchange: "request",  // Declare an exchange named "request".
        type: ExchangeType.Topic // This exchange is of type "Topic", allowing for flexible routing based on topic patterns.
    );

// Declares a new queue and retrieves its name from the server.
var queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(
         // Binds the newly created queue to the "request" exchange.
         queue: queueName,
         exchange: "request",
         routingKey: "*.*.us"
   );

Console.WriteLine(" Press [enter] for stop service.");
Console.WriteLine(" Consumer Waiting for receive all order that contain '*.*.us'");

//Creates a consumer instance tied to the specified channel for handling incoming messages.
var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, e) =>
{
    var body = e.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" Consumer_01: {message}");
};

channel.BasicConsume(
    queue: queueName,   // Specifies the name of the queue from which messages will be consumed.
    autoAck: true,         // Indicates whether automatic acknowledgment should be enabled; false means manual acknowledgment is required.
    consumer: consumer      // Specifies the object responsible for handling incoming messages.
);

Console.ReadLine();