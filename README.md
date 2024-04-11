<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/hsjalilian/RabbitMQ">
    <img src="/images/rabbitmq_logo.png" alt="RabitMQ Logo" width="80" height="80">
  </a>

  <h3 align="center">RabbitMQ Message Broker</h3>

  <p align="center">   
    <br />
    <a href="https://github.com/hsjalilian/RabbitMQ/tree/master/docs"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <a href="https://github.com/hsjalilian/RabbitMQ/issues">Report Bug</a>
    ·
    <a href="https://github.com/hsjalilian/RabbitMQ/issues">Request Feature</a>
  </p>
</div>


<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li><a href="#rabbitmq-message-broker">RabbitMQ message-broker</a></li>  
    <li><a href="#what-is-an-nessage-broker?">What Is an Message-Broker?</a></li>
    <li><a href="#what-is-rabbitmq">What Is RabbitMQ</a></li>
    <li><a href="#features">Features</a></li>
    <li><a href="#how-to-install">How to install</a></li>    
    <li>
      <a href="#sample-projects">Sample Projects</a>
      <ul>
        <li><a href="#basic">Basic</a></li>
        <li><a href="#multi-worker">Multi Worker</a></li>        
      </ul>
    </li>
  </ol>
</details>

# RabbitMQ message-broker
This repository contains some samples that show RabbitmQ features in different projects from basic to advance config.  


##  What is an Message-Broker?
A message broker is an intermediary software component or service that facilitates communication between different applications or systems by enabling the exchange of messages. It serves as a centralized hub where producers can send messages, and consumers can retrieve and process them.

Benefits of Message broker

* Decoupling of Services
* Asynchronous Communication 
* Reliability and Message Persistence 
* Load Balancing and Scalability 
* Protocol Translation and Interoperability 
* Monitoring and Management 
* Message Routing and Filtering 
* Buffering and Throttling 
* Fault Isolation 


## What Is RabbitMQ
RabbitMQ is a powerful open-source message broker software, designed to facilitate seamless communication between diverse applications. Built upon the Advanced Message Queuing Protocol (AMQP), it acts as a reliable mediator, enabling efficient exchange of messages among different components of distributed systems. Through RabbitMQ, applications can effortlessly send and receive messages, fostering smooth interaction and coordination within complex software architectures.

## Features

For a quick list of RabbitMQ's capabilities for more information see the [documentation](https://www.rabbitmq.com/release-information).

* Message Queues
* Message Durability
* Exchange Types
* Routing
* Publisher-Subscriber Model
* Reliability and Acknowledgments
* Clustering
* Management UI
* Plugins and Extensions
* Security
* Integration


## How to install

RabbitMQ installation comprises two key components: server-side and client-side. The server-side involves configuring and launching RabbitMQ on the server, often through Docker or other deployment methods. Conversely, the client-side entails integrating RabbitMQ client libraries or SDKs into your development environment, facilitating seamless communication between your applications and the RabbitMQ server.

### Server-Side Installation (RabbitMQ)

#### Prerequisites
- Docker installed on your machine. You can download and install Docker Desktop from [https://www.docker.com/products/docker-desktop](https://www.docker.com/products/docker-desktop).

#### Run RabbitMQ Docker Image
Start a RabbitMQ container with the following command. This command will create and run a RabbitMQ server instance inside a Docker container.

`docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.13-management`

#### Access RabbitMQ Management UI
Once the container is running, you can access the RabbitMQ management UI by navigating to http://localhost:15672 in your web browser. Log in with the default credentials (username: guest, password: guest). The management UI allows you to monitor and manage your RabbitMQ server.


### Client-Side Installation (.NET Client using RabbitMQ.Client)

#### Install RabbitMQ.Client NuGet Package
Add the RabbitMQ.Client package to your .NET project using NuGet Package Manager or .NET CLI. This package provides the necessary libraries for connecting to RabbitMQ from your .NET applications.

`dotnet add package RabbitMQ.Client`

#### Connect to RabbitMQ
Use the RabbitMQ.Client library in your .NET project to connect to RabbitMQ and interact with queues, exchanges, and messages.

# Sample Projects

## Basic 

The simplest sample that introduces how to use RabbitMQ here.

Link here : [Basic](https://github.com/hsjalilian/RabbitMQ/tree/main/src/Basic)

## Multi Worker  

Similar to the Basic template, but with the addition of multiple consumers to receive messages from each queue.

Link here : [Mulit Worker](https://github.com/hsjalilian/RabbitMQ/tree/main/src/MulitWorker)

