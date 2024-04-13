# Topic Template

This template provides a basic configuration for the RabbitMQ project, consisting of three projects: two for the consumer and the other for the producer, all running as Windows console applications.


## Projects 
* ProducerConsole: This project serves as the producer, allowing you to send requests to the RabbitMQ server.
* ConsumerConsole01: This project acts as the consumer, establishing a connection to the RabbitMQ server to retrieve and print messages previously written to the queue.
* ConsumerConsole02: This project acts as the consumer, establishing a connection to the RabbitMQ server to retrieve and print messages previously written to the queue.

## Get Start
From the Solution property, set startup configuration to `Multiple startup projects` and select these three project actions to `start`

After launching the projects, input your message in the producer console, and observe it appearing in both consumer console windows. In RabbitMQ, exchanges are utilized to receive messages from producers and route them to consumers based on the type of exchange declared. In this example, we use `ExchangeType.Topic`, which means every received message is sent to the queues connected to this exchange and match with route key rule.

## Difference between Direct and Topic


### Direct Exchange

 - Direct exchanges route messages to queues based on an exact match between the routing key of the message and the binding key of the queue.
 - It offers a simple one-to-one message routing mechanism, where each message is delivered to a single queue.

### Topic Exchange

 - Topic exchanges route messages to queues based on wildcard matching of routing keys.
 - It allows for more flexible routing patterns, where messages can be selectively delivered to queues based on matching patterns specified in the routing key.
 - Topics are expressed as a series of words separated by dots, and wildcard characters (* and #) can be used to match one or more words in the routing key.