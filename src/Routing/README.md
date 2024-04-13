# Routing Template

This template provides a basic configuration for the RabbitMQ project, consisting of three projects: two for the consumer and the other for the producer, all running as Windows console applications.


## Projects 
* ProducerConsole: This project serves as the producer, allowing you to send requests to the RabbitMQ server.
* ConsumerConsole01: This project acts as the consumer, establishing a connection to the RabbitMQ server to retrieve and print messages previously written to the queue.
* ConsumerConsole02: This project acts as the consumer, establishing a connection to the RabbitMQ server to retrieve and print messages previously written to the queue.

## Get Start
From the Solution property, set startup configuration to `Multiple startup projects` and select these three project actions to `start`

After launching the projects, input your message in the producer console, and observe it appearing in both consumer console windows. In RabbitMQ, exchanges are utilized to receive messages from producers and route them to consumers based on the type of exchange declared. In this example, we use `ExchangeType.Direct`, which means every received message is sent to the queues connected to this exchange and match with route key rule.

## All Exchange Types

*  Direct: Messages are delivered to queues based on exact matching of the routing key with the binding key of the queue.

* Fanout: Messages are broadcasted to all queues bound to the exchange, disregarding the routing key.

* Topic: Messages are routed to queues based on wildcard matching of routing keys, allowing for flexible routing patterns.

* Headers: Messages are routed to queues based on matching criteria defined in message headers, offering a high degree of customization in routing.