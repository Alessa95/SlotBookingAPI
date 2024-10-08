**Instructions:**

1 - Install Docker Desktop ([Download Docker Desktop | Docker](https://www.docker.com/products/docker-desktop/))

2 - Go to solution's root

3 - Execute the docker command to create image: `docker build -t slotbookingapi -f .\SlotBookingAPI\Dockerfile .`

4 - Execute the docker command to run image: `docker run -p 8080:8080 slotbookingapi`
   
> **Warning:** It is important to mention that, for testing purposes only, we will use port 80 for now.

5 - Get authentication token: Open Postman and add a new POST request:
- url: http://localhost:8080/token
- Body **(Format: x-www-form-urlencoded)**: 
```json
{
    "username": "get from appSettings [AvailabilityApi:ApiUsername]",
    "password": "get from appSettings [AvailabilityApi:ApiPassword]"
}
```
> **Warning:** For testing purposes only, we will use these credentials.

6 - Open Swagger in http://localhost:8080/swagger.

7 - Authenticate

8 - Get available slots using `GET /Slot/{startDay}`

**Ex:** "2024-10-08T00:00:00"

To meet the requirement that *"User should be able to pick a correct week and pick a slot"* the frontend application will request available slots for the current date the user is viewing through this endpoint. This endpoint will return all available slots from the requested date and time onwards (for the rest of the week).

If the user wants to check the availability for the previous or next week, the frontend application should call this endpoint with the date of the Monday of the previous or next week.

9 - Book a slot using `POST /Slot`
```json
{
    "slot": "2024-10-08T09:00:00",
    "comments": "string",
    "patient": {
        "name": "string",
        "secondName": "string",
        "email": "user@example.com",
        "phone": "string"
    }
}
```

This will book the slot if it is available (past dates are not allowed, and availability will be checked before applying the booking).

**Testing:**

1 - Go to SlotBookingAPI -> SlotBooking.ApiTests

Here you will find basic unit tests:
- **SlotControllerTest** verifies that the controller's models are properly validated.
- **AvailabilityServiceTest** verifies the calculation performed on the data obtained from the external API to be used by this API.
- **DateTimeUtilsTest** verifies the functionality of date manipulation methods. 