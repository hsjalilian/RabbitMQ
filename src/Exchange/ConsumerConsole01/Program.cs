﻿using Microsoft.Extensions.Configuration;
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
        exchange: "notifications",  // Declare an exchange named "notifications".
        type: ExchangeType.Fanout // This exchange type is "fanout", broadcasting messages to all bound queues.
    );

// Declares a new queue and retrieves its name from the server.
var queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(
         // Binds the newly created queue to the "notifications" exchange with an empty routing key.
         queue: queueName,
         exchange: "notifications",
         routingKey: string.Empty
   );

Console.WriteLine(" Press [enter] for stop service.");
Console.WriteLine(" Consumer Waiting for messages...");

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