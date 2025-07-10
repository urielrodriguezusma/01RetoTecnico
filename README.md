## Techical Test
This project is conformed with two Microservices: TransacionService and AntifraudService
- TransactionService: This service is in charge to create a new transaction record into the database. It exposes and API with a controller call Transactions.Controller with 2 Endpoint
  
  - POST /api/Transactions: This endpoint creates a new Transaction record. Let's see an example of the body
  
            {
                 "sourceAccountId": "b34fcb26-3915-4f06-b50c-a8e1e0a74a9b",
                 "targetAccountId": "863005d1-50a2-4574-a210-60223024149d",
                 "value": 100
            }

  - Get: /api/Transaction/{transferId}: This endpoind gets a record of a specic transfer Id example of the url

         `api/Transactions/8b326f60-0901-4562-8761-9d5b1e30c243
