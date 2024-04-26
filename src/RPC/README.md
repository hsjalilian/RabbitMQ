# RPC Template

This template provides a basic configuration for the RabbitMQ project, consisting of two projects: one for the consumer and the other for the producer, all running as Windows console applications.


## Projects 
* ProducerConsole: This project serves as the producer, allowing you to send requests to the RabbitMQ server and get response.
* ConsumerConsole: This project acts as the consumer, establishing a connection to the RabbitMQ server to retrieve and print messages previously written to the queue.

## Get Start
From the Solution property, set startup configuration to `Multiple startup projects` and select these three project actions to `start`

After launching both projects, input your message in the producer console, then witness it displayed in the consumer console windows. Upon processing the request in the consumer, the response is sent to the reply queue for the producer to retrieve, enabling seamless continuation of the processing.

RPC in RabbitMQ involves clients sending requests to a server via a designated queue, with correlation IDs and reply-to queues specified. The server processes requests and sends responses back to the client's designated queue, using the correlation ID for identification. This pattern enables synchronous communication between distributed systems, facilitating seamless interaction and data exchange.