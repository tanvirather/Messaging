# EmailController

The `EmailController` provides API endpoints to trigger various email notifications. It acts as a gateway to queue notification messages for background processing.

## Base URL
`POST /Email`

## Endpoints

### 1. Order Notification
Trigger an order confirmation email.

* **URL:** `/Email/Order`
* **Method:** `POST`
* **Body:** `OrderMessage` (JSON)
* **Example Payload:**
  ```json
  {
    "OrderId": "a0b1c2d3-e4f5-a6b7-c8d9-e0f1a2b3c4d5"
  }
  ```
* **Success Response:** `202 Accepted`

### 2. Welcome Notification
Trigger a welcome email for a new customer.

* **URL:** `/Email/Welcome`
* **Method:** `POST`
* **Body:** `WelcomeMessage` (JSON)
* **Example Payload:**
  ```json
  {
    "CustomerId": "4b5c6d7e-8f9a-0b1c-2d3e-4f5a6b7c8d9e"
  }
  ```
* **Success Response:** `202 Accepted`

## How it Works
1. The controller receives a POST request with a specific message body.
2. It validates the message and queues it using the `NotificationQueue`.
3. An `Accepted (202)` response is returned immediately to the caller.
4. The `NotificationBackgroundService` picks up the message from the queue and processes it (composes and sends the email).
