# Controllers

This directory contains the API controllers for the Messaging service.

## OrderController

The `OrderController` handles email notifications related to orders.

### Send Email for Order

Sends an email for a specific order identified by its ID.

**URL:** `POST /Email/Order`

**Body:** A JSON string containing the Order GUID.

#### Example using curl

```bash
curl -X POST http://localhost:5000/Email/Order \
     -H "Content-Type: application/json" \
     -d "\"00000000-0000-0000-0000-000000000000\""
```

*Note: Replace `http://localhost:5000` with the actual base URL of your running API and the GUID with a valid `orderId`.*
