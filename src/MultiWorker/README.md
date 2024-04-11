# Multi Worker Template

This template provides a basic configuration for the RabbitMQ project, consisting of three projects: two for the consumer and the other for the producer, all running as Windows console applications.


## Projects 
* ProducerConsole: This project serves as the producer, allowing you to send requests to the RabbitMQ server.
* ConsumerConsole01: This project acts as the consumer, establishing a connection to the RabbitMQ server to retrieve and print messages previously written to the queue.
* ConsumerConsole02: This project acts as the consumer, establishing a connection to the RabbitMQ server to retrieve and print messages previously written to the queue.

## Get Start
From the Solution property, set startup configuration to `Multiple startup projects` and select these three project actions to `start`

After launching the projects, enter your message in the producer console and notice its display in the consumers console. Each message sent by the producer is directed to a queue and received by one of the available consumers at that moment.