# Restaurants API

.NET 8.0 web API to manage restaurants.

## Project Structure

- `src/Restaurants.API`: This is the main API project, entry point of the application.
- `src/Restaurants.Application`: It contains the application logic which is responsible for the application's behavior and policies.
- `src/Restaurants.Domain`: It contains enterprise logic and types which is the core layer of an application.
- `src/Restaurants.Infrastructure`: It contains infrastructure-related code such as database and file system interactions.
- `tests/Restaurants.API.Tests`: It contains unit tests for the API.

## Getting Started

### Required

- .NET 8.0
- Visual Studio 2022 or later

### Building

Open the `Restaurants.sln` file in Visual Studio and build the solution.

### Running

Set `Restaurants.API` as the startup project and start the application.

## API Endpoints

1. `GET /api/restaurants`
   - Parameters: `offset`, `limit`
   - Authorization Not required

2. `GET /api/restaurants/{id}`
   - Parameters: `id`
   - Authorization: Bearer token

3 . `GET /api/restaurants/search`
   - Parameters: `searchTerm`, `offset`, `limit`
   - Authorization Not required

4 . `POST /api/restaurants/{id}/logo`
   - Parameters: `id`
   - Body: File `file`
   - Authorization: Bearer token

5. `GET /api/restaurants/{id}/dishes`
   - Parameters: `id`
   - Authorization: Bearer token

6. `POST /api/restaurants{id}/dishes`
   - Parameters: `id`
   - Body: JSON object with properties `name`, `description`, `price`, `kiloCalories`
   - Authorization: Bearer token, User must be in Owner role and must have passport = true

7. `DELETE /api/restaurants/{id}/dishes`
   - Parameters: `id`
   - Authorization: Bearer token

8. `GET /api/restaurants/{id}/dishes/{dishId}`
   - Parameters: `id`, `dishId`
   - Authorization: Bearer token

9. `DELETE /api/restaurants/{id}`
   - Parameters: `id`
   - Authorization: Bearer token, User's age must be atleast 18

10. `POST /api/restaurants`
   - Body: JSON object with properties `name`, `description`, `category`, `hasDelivery`, `contactEmail`, `contactNumber`, `city`, `street`, `postalCode`
   - Authorization: Bearer token, User must be in Owner role and must have passport = true

## Testing

The tests are located in the `tests` directory. You can run them in Visual Studio using the test runner.