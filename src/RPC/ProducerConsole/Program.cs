using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
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

string replyQueueName = channel.QueueDeclare().QueueName;

#region consumer scope for process reply
ConcurrentDictionary<string, TaskCompletionSource<string>> callbackMapper = new();
var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) =>
{
    if (!callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out var tcs))
        return;

    var body = ea.Body.ToArray();
    var response = Encoding.UTF8.GetString(body);
    
    Console.WriteLine($"reply from consumer: {response}");

    tcs.TrySetResult(response);
};

channel.BasicConsume(consumer: consumer,
                     queue: replyQueueName,
                     autoAck: true);

#endregion

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
            IBasicProperties propsLoop = channel.CreateBasicProperties();
            var correlationIdLoop = Guid.NewGuid().ToString();
            propsLoop.CorrelationId = correlationIdLoop;
            propsLoop.ReplyTo = replyQueueName;
            var tcsLoop = new TaskCompletionSource<string>();
            callbackMapper.TryAdd(correlationIdLoop, tcsLoop);

            message = new string(Enumerable.Repeat(chars, random.Next(5, 15))
                .Select(s => s[random.Next(s.Length)]).ToArray());

            Console.WriteLine($"{message}");
           
            channel.BasicPublish(exchange: string.Empty,
                            routingKey: "rpc",
                            basicProperties: propsLoop,
                            body: Encoding.UTF8.GetBytes(message));
        }
        continue;
    }

    IBasicProperties props = channel.CreateBasicProperties();
    var correlationId = Guid.NewGuid().ToString();
    props.CorrelationId = correlationId;
    props.ReplyTo = replyQueueName;
    var tcs = new TaskCompletionSource<string>();
    callbackMapper.TryAdd(correlationId, tcs);

    channel.BasicPublish(exchange: string.Empty,
                            routingKey: "rpc",
                            basicProperties: props,
                            body: Encoding.UTF8.GetBytes(message));
}