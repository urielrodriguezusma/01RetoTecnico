## Techical Test
This project is conformed with two Microservices: TransacionService and AntifraudService
- TransactionService: This service is in charge to create a new transaction record into the database. It exposes and API with a controller call Transactions.Controller with 2 Endpoint
  
  - POST /api/Transactions: This endpoint creates a new Transaction record with Status "Pending". Let's see an example of the body
  
            {
                 "sourceAccountId": "b34fcb26-3915-4f06-b50c-a8e1e0a74a9b",
                 "targetAccountId": "863005d1-50a2-4574-a210-60223024149d",
                 "value": 100
            }

  - Get: /api/Transaction/{transferId}: This endpoind gets a record of a specic transfer Id example of the url

         api/Transactions/8b326f60-0901-4562-8761-9d5b1e30c243

- AntifraudService: This service is in charge to validate if the current transaction associated to the SourceAccount does not exceed the limit per day **20.000**. If the value does not exceed the limit the Status will be "Approved"
                    otherwise will be "Rejected". The transaction microservice will be in charge to update this status.


## Diagram
![Diagram](https://github.com/urielrodriguezusma/01RetoTecnico/blob/master/Diagram/Prueba.png)

## How to execute the project
To execute the project you just need to locate the file ***docker-compose.yml*** and run the command

    docker compose --env-file .env.local up -d
By default the project will be executed using RabbitMQ however the configuration is prepering to work with kafka you just need to change the **.env.local** file and set-up the variable KAFKACONNECTIONSTRING with the connection and also 
RABBITMQENABLED in false. The system is prepared to work with RabbitMQ and kafka (WIP), this can be achieve thanks to masstransit library that allows you to abstract the implementation of the the messages system and use Kafka, RabbitMQ, AzureService Bus and so on.





