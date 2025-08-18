# 🧾 Basic Payment System Microservices

A modern event-driven payment system built with **C# ASP.NET**, **Apache Kafka**, **MSSQL**, and **Docker**. The system demonstrates asynchronous communication between decoupled services using Apache Kafka as a message broker.

## 🔧 Microservices Overview

This project consists of three primary components:
- **ApiGateway** - Central entry point with Ocelot for routing requests
- **OrderService** - Handles order creation and manages order status
- **PaymentService** - Processes payments and updates payment status

Each service has its own dedicated MSSQL database, and they communicate asynchronously via Kafka topics.

## 📦 Project Structure

PaymentSystem/
│
├── Services/
│ ├── ApiGateway/
│ │ ├── Connected Services/
│ │ ├── Dependencies/
│ │ ├── Properties/
│ │ ├── ApiGateway.http
│ │ ├── appsettings.json
│ │ ├── Dockerfile
│ │ ├── ocelot.json
│ │ └── Program.cs
│ │
│ ├── OrderService/
│ │ ├── Controllers/
│ │ ├── Data/
│ │ ├── DTOs/
│ │ ├── Exceptions/
│ │ ├── Interfaces/
│ │ ├── MessageBroker/
│ │ ├── Migrations/
│ │ ├── Models/
│ │ ├── Services/
│ │ ├── appsettings.json
│ │ ├── Dockerfile
│ │ ├── OrderService.http
│ │ └── Program.cs
│ │
│ └── PaymentService/
│ ├── Controllers/
│ ├── Data/
│ ├── DTOs/
│ ├── Exceptions/
│ ├── Interfaces/
│ ├── MessageBroker/
│ ├── Migrations/
│ ├── Models/
│ ├── Services/
│ ├── appsettings.json
│ ├── Dockerfile
│ ├── PaymentService.http
│ └── Program.cs
│
└── docker-compose.yml


## 🐳 Docker Architecture

The system runs in a Docker environment with:
- Each service in its own container
- Dedicated MSSQL databases for Order and Payment services
- Apache Kafka for message brokering
- Ocelot API Gateway as the entry point

## 🔁 Flow of Operations

### 📝 Order Creation
1. Client sends POST request to ApiGateway to create an order
2. OrderService creates order with Status = PENDING, PaymentStatus = NOT_PAID
3. OrderService publishes `PaymentRequestEvent` to Kafka's `payment-requests` topic

### 💳 Payment Processing
1. PaymentService consumes `PaymentRequestEvent` from `payment-requests` topic
2. PaymentService processes payment (success/failure)
3. Updates payment status in its database
4. Publishes `PaymentProcessedEvent` to Kafka's `order-updates` topic

### 🔄 Order Status Update
1. OrderService consumes `PaymentProcessedEvent` from `order-updates` topic
2. Updates order status based on payment result
3. Persists changes to its database

### 🔍 Retrieval
Clients can GET order details through ApiGateway to view:
- OrderId, TotalAmount, Status, and PaymentStatus

## 🛠 Service Implementation Highlights

### ApiGateway
- Ocelot configuration for routing
- Centralized request handling
- Service aggregation

### OrderService
- **Controllers**: REST endpoints for order operations
- **Data**: Database context and repositories
- **DTOs**: Data transfer objects for API contracts
- **MessageBroker**: Kafka producer/consumer implementations
- **Models**: Order, OrderStatus, PaymentStatus entities
- **Services**: Core business logic for order processing

### PaymentService
- **Controllers**: Payment processing endpoints
- **MessageBroker**: Kafka event handlers
- **Models**: Payment entity and status enums
- **Services**: Payment validation and processing logic

## 🚀 Getting Started

1. Ensure Docker Desktop is running
2. Clone the repository
3. Run: `docker-compose up --build`
4. Services will be available through the API Gateway (typically http://localhost:5000)

## 📄 API Documentation

Test API endpoints using the included `.http` files in each service directory with REST Client extension.

## 🔧 Technologies Used
- ASP.NET Core
- Apache Kafka
- MSSQL Server
- Entity Framework Core
- Ocelot API Gateway
- Docker
- Docker Compose
