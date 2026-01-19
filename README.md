# DattingService

![Static Badge](https://img.shields.io/badge/language-C%23-red)
![Static Badge](https://img.shields.io/badge/powered_by-.NET_9-blue)
![Static Badge](https://img.shields.io/badge/platforms-Windows-purple)
![Static Badge](https://img.shields.io/badge/version-1.0-orange)
![Static Badge](https://img.shields.io/badge/developer-sergxlove-green)
![Static Badge](https://img.shields.io/badge/year-2025-green)

 ## Architecture 

 ![photo](https://github.com/sergxlove/DattingService/blob/master/resourses/architectureDattingService.png)

 ## About

 The project is a full-featured backend for a dating application built on a microservices architecture. The system includes offline services for user management, algorithmic partner selection, real-time chat, notifications, and media file processing. Each service is implemented on ASP.NET A core with an optimal choice of database (PostgreSQL for transactional data, MongoDB for chats, Redis for caching) and interacts via the REST API, gRPC, and the RabbitMQ message broker. The architecture provides horizontal scalability, fault tolerance, and independent component deployment.
