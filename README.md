# Employee Management System

## Overview
This project is a **.NET 8** Employee Management System built using Domain-Driven Design (DDD) principles, adhering to SOLID design principles and implementing best practices such as the **Repository Pattern**, **Entity Framework Core**, and **JWT-based Authentication**. The project includes both an **MVC/Razor frontend** and an **API backend**, and handles **CRUD operations** for employees and departments.

### Features
- **Employee and Department CRUD** operations.
- **JWT-based Authentication** for secure API access.
- **Separation of Concerns** with a well-structured solution divided into multiple projects: `Domain`, `Infrastructure`, `Application`, `API`, `Web`, and `Tests`.
- **Entity Framework Core** with **PostgreSQL** or **SQL Server** as the database.
- **Logging** using `ILogger` for detailed error and info tracking.
- **Unit Testing** with **XUnit** and **FluentAssertions** for ensuring code quality.

---

## Table of Contents
1. [Solution Structure](#solution-structure)
2. [Setup Instructions](#setup-instructions)
3. [Design Choices](#design-choices)
4. [Running the Application](#running-the-application)
5. [Testing](#testing)
6. [API Endpoints](#api-endpoints)

---

## Solution Structure

The project is divided into multiple layers and follows **Domain-Driven Design (DDD)**:

- **Domain Layer**: Contains the core business logic and domain entities such as `Employee`, `Department`, and value objects like `EmployeeCode` and `EmailAddress`.
  
- **Infrastructure Layer**: Responsible for database access and implementations of the repository interfaces. Uses **Entity Framework Core** for database interactions.

- **Application Layer**: Contains services like `EmployeeService` and `DepartmentService`, which orchestrate business logic and serve as an intermediary between the domain layer and the infrastructure.

- **API Layer**: Exposes the functionality of the application as **RESTful APIs**. This layer includes controllers for `Employee` and `Department`, handling requests and responses.

- **MVC Web Layer**: The **Razor Pages/MVC application** that consumes the API and provides a UI for the system. It manages **Employee** and **Department** forms and lists.

- **Tests Layer**: Contains unit tests written with **XUnit** and **FluentAssertions** for ensuring the correctness of repository and service classes.

---

## Setup Instructions

### Prerequisites
1. **.NET 8 SDK** installed on your system.
2. **PostgreSQL** or **SQL Server** installed and running for the database.
3. **NuGet Package Restore** to ensure all dependencies are installed.

### Configuration

1. **Clone the repository**:
    ```bash
    git clone https://github.com/your-repo/employee-management-system.git
    cd employee-management-system
    ```

2. **Database Setup**:
    - Set up your connection string in `appsettings.json` for the database you're using (either PostgreSQL or SQL Server):
      ```json
      "ConnectionStrings": {
        "DefaultConnection": "Server=localhost;Database=EmployeeMgmtDb;User Id=your-username;Password=your-password;"
      }
      ```

3. **Apply Migrations**:
    Run the following commands to apply the database migrations:
    ```bash
    cd src/EmployeeMgmt.API
    dotnet ef database update
    ```

4. **Seeding Data**:
    - The project includes an `AppDbInitializer` class for seeding initial data. This will run automatically on the first application startup.

---

## Design Choices

### 1. **Domain-Driven Design (DDD)**
   - **Entities** like `Employee` and `Department` represent core business concepts.
   - **Value Objects** like `EmployeeCode` and `EmailAddress` enforce business rules around domain values, ensuring consistent validation and immutability.
   - **Repositories** abstract the data access layer, allowing flexibility in changing the database without impacting business logic.

### 2. **Repository Pattern**
   - Encapsulates data access logic, promoting a clean separation of concerns between the domain layer and data storage. This pattern also improves testability by allowing repository methods to be mocked in tests.

### 3. **Service Layer**
   - Services orchestrate complex business logic, ensuring a clear separation between domain logic and the infrastructure layer. This provides scalability and adaptability for future changes or feature enhancements.

### 4. **JWT Authentication**
   - The API layer implements JWT-based authentication to secure endpoints. This ensures that only authenticated users can perform operations, adding an extra layer of security to the system.

### 5. **Logging & Error Handling**
   - `ILogger` is used for logging errors and information. Logs follow a structured pattern to ensure easy debugging, including method names and error details.

### 6. **Unit Testing with Fluent Assertions**
   - Comprehensive unit tests have been written using **XUnit** and **FluentAssertions**. This ensures that critical functionality like CRUD operations and business rules are covered and behave as expected.

---

## Running the Application

1. **Running the API Project**:
    ```bash
    cd src/EmployeeMgmt.API
    dotnet run
    ```

2. **Running the MVC Web Application**:
    ```bash
    cd src/EmployeeMgmt.Web
    dotnet run
    ```

3. Open the API in your browser at `https://localhost:7054/api` for accessing API documentation through **Swagger**.

---

## Testing

1. **Run Unit Tests**:
    ```bash
    cd tests/EmployeeMgmt.Tests
    dotnet test
    ```

- Unit tests cover repository and service logic. The tests ensure correctness of CRUD operations, business rule enforcement, and error handling.

---

## API Endpoints

1. **Employee Endpoints**:
    - `GET /api/employees`: Retrieve all employees.
    - `GET /api/employees/{id}`: Retrieve employee by ID.
    - `POST /api/employees`: Add a new employee.
    - `PUT /api/employees/{id}`: Update an existing employee.
    - `DELETE /api/employees/{id}`: Delete an employee.

2. **Department Endpoints**:
    - Similar CRUD operations for departments.

Each endpoint is secured with JWT, and responses include proper HTTP status codes and error messages.

---

## Conclusion

This Employee Management System demonstrates a scalable and maintainable architecture using best practices like Domain-Driven Design, Repository Pattern, and JWT-based authentication. The solution's modular approach ensures a clean separation of concerns and ease of maintenance, making it suitable for future enhancements and feature additions.
