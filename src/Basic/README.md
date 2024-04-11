# Basic Template

This template uses a basic config for the RabbitMQ project and contain two project , one about consumer and another one producer that run in windows console app.


## Projects 
* ProducerConsole: This project serves as the producer, allowing you to send requests to the RabbitMQ server.
* ConsumerConsole: This project acts as the consumer, establishing a connection to the RabbitMQ server to retrieve and print messages previously written to the queue.

## Get Start
From the Solution property, set startup configuration to `Multiple startup projects` and select these two project actions to `start`

After running the projects, input your message in the producer console and observe its appearance in the consumer console.