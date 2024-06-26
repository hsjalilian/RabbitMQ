﻿using Microsoft.Extensions.Configuration;
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


channel.QueueDeclare(
        queue: "basic",        // Specifies the name of the queue being declared.
        durable: false,        // Indicates whether the queue should survive a broker restart; false means it won't.
        exclusive: false,      // Indicates whether the queue can be accessed by other connections; false means it can.
        autoDelete: false,     // Indicates whether the queue will be deleted when the last consumer unsubscribes; false means it won't.
        arguments: null       // Additional arguments for the queue declaration; in this case, no additional arguments are provided.
    );

Console.WriteLine(" Type exit for stop! ");
string message = "";

while (true)
{
    Console.Write(" Type your message: ");
    message = Console.ReadLine() ?? "exit";
    if (message.Equals("exit"))
        break;

    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(
         exchange: string.Empty,  // Specifies the exchange to which the message will be published; an empty string means default exchange.
         routingKey: "basic",     // Specifies the routing key for the message; determines which queues will receive the message.
         basicProperties: null,   // Specifies additional properties for the message; in this case, no additional properties are provided.
         body: body               // Specifies the message body; the actual content of the message being published.
     );
}