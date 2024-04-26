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

channel.QueueDeclare(
        queue: "rpc",        // Specifies the name of the queue being declared.
        durable: false,        // Indicates whether the queue should survive a broker restart; false means it won't.
        exclusive: false,      // Indicates whether the queue can be accessed by other connections; false means it can.
        autoDelete: false,     // Indicates whether the queue will be deleted when the last consumer unsubscribes; false means it won't.
        arguments: null       // Additional arguments for the queue declaration; in this case, no additional arguments are provided.
    );

channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

Console.WriteLine(" Press [enter] for stop service.");
Console.WriteLine(" Consumer Waiting for messages...");

//Creates a consumer instance tied to the specified channel for handling incoming messages.
var consumer = new EventingBasicConsumer(channel);

channel.BasicConsume(queue: "rpc",
                     autoAck: false,
                     consumer: consumer);

consumer.Received += (model, e) =>
{
    var body = e.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var props = e.BasicProperties;
    var replyProps = channel.CreateBasicProperties();
    replyProps.CorrelationId = props.CorrelationId;

    Console.WriteLine($" Incoming message: {message}");

    string reply = "";
    try
    {
        int wait = new Random().Next(2000, 5000);
        Thread.Sleep( wait );
        reply = $"process pause for {wait} milliseconds about message ({message})...";

    }
    catch(Exception ex)
    {
        reply = $"this error happend: {ex.Message}";
    }
    finally
    {
        var responseBytes = Encoding.UTF8.GetBytes(reply);
        channel.BasicPublish(exchange: string.Empty,
                             routingKey: props.ReplyTo,
                             basicProperties: replyProps,
                             body: responseBytes);

        channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
    }
};

Console.ReadLine();