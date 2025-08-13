# FCG - FIAP Cloud Games

A comprehensive game management system built with .NET 8, following Clean Architecture principles and Domain-Driven Design (DDD) patterns.

## 🎯 Project Overview

FCG (FIAP Cloud Games) is a modern web API application designed to manage games, users, and their game libraries. The system provides functionality for user authentication, game catalog management, and user game library operations.

## 🏗️ Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

```
├── FCG.Api/                    # Presentation Layer (Web API)
├── FCG.Application/             # Application Layer (Services, DTOs, Interfaces)
├── FCG.Core/                    # Domain Layer (Entities, Value Objects, Interfaces)
├── FCG.Infrastructure/          # Infrastructure Layer (Data Access, Repositories)
└── Tests/                       # Test Projects
    ├── FCG.Api.Tests/
    ├── FCG.Application.Tests/
    ├── FCG.Core.Tests/
    └── FCG.Infrastructure.Tests/
```

### Key Design Patterns

- **Domain-Driven Design (DDD)**: Rich domain models with business logic encapsulation
- **Repository Pattern**: Data access abstraction
- **Unit of Work**: Transaction management
- **CQRS**: Command Query Responsibility Segregation
- **Dependency Injection**: Loose coupling and testability
- **Value Objects**: Email and Password with built-in validation

## 🚀 Technologies Used

- **.NET 8**: Latest .NET framework
- **ASP.NET Core Web API**: RESTful API development
- **Entity Framework Core**: Object-Relational Mapping (ORM)
- **FluentValidation**: Input validation
- **xUnit**: Unit testing framework
- **Moq**: Mocking framework for testing
- **SQL Server**: Database (configurable)

## 📋 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)

## ⚙️ Setup Instructions

### 1. Clone the Repository

```bash
git clone <repository-url>
cd FCG
```

### 2. Configure Database Connection

Update the connection string in `FCG/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FCGDatabase;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### 3. Apply Database Migrations

```bash
cd FCG.Infrastructure
dotnet ef database update --startup-project ../FCG
```

### 4. Build the Solution

```bash
dotnet build
```

### 5. Run the Application

```bash
cd FCG
dotnet run
```

The API will be available at:
- HTTPS: `https://localhost:7001`
- HTTP: `http://localhost:5000`
- Swagger UI: `https://localhost:7001/swagger`

## 🧪 Running Tests

### Run All Tests

```bash
dotnet test
```

### Run Specific Test Project

```bash
dotnet test FCG.Application.Tests
dotnet test FCG.Core.Tests
dotnet test FCG.Infrastructure.Tests
dotnet test FCG.Api.Tests
```

## 📚 API Documentation

### Authentication Endpoints

- `POST /api/auth/login` - User authentication
- `POST /api/auth/register` - User registration

### User Management

- `GET /api/users` - Get all users
- `GET /api/users/{id}` - Get user by ID
- `POST /api/users` - Create new user
- `PUT /api/users/{id}` - Update user
- `DELETE /api/users/{id}` - Delete user
- `PUT /api/users/{id}/password` - Update user password

### Game Management

- `GET /api/games` - Get all games
- `GET /api/games/{id}` - Get game by ID
- `POST /api/games` - Create new game
- `PUT /api/games/{id}` - Update game
- `DELETE /api/games/{id}` - Delete game
- `PUT /api/games/{id}/discount` - Apply discount to game
- `PUT /api/games/{id}/sale-status` - Update sale status

### User Game Library

- `GET /api/user-game-library/user/{userId}` - Get user's game library
- `POST /api/user-game-library` - Add game to user library
- `DELETE /api/user-game-library/{userId}/{gameId}` - Remove game from library

### Role Management

- `GET /api/roles` - Get all roles
- `GET /api/roles/{id}` - Get role by ID
- `POST /api/roles` - Create new role
- `PUT /api/roles/{id}` - Update role
- `DELETE /api/roles/{id}` - Delete role

## 🏛️ Domain Model

### Core Entities

#### User
- Properties: Name, Email, Password, Role
- Business Rules: Email uniqueness, password encryption
- Value Objects: Email, Password

#### Game
- Properties: Name, Description, Price, OriginalPrice, Discount, IsOnSale
- Business Rules: Price validation, discount calculations

#### Role
- Properties: Name, Description
- Business Rules: Role-based authorization

#### UserGameLibrary
- Properties: User, Game, PurchaseDate
- Business Rules: User-game relationship management

### Value Objects

#### Email
- Validates email format
- Ensures email uniqueness

#### Password
- Handles password hashing
- Enforces password complexity rules

## 🔧 Configuration

### Environment Variables

The application supports the following environment-specific configurations:

- `ASPNETCORE_ENVIRONMENT`: Development, Staging, Production
- `ConnectionStrings__DefaultConnection`: Database connection string

### Logging

Logging is configured in `appsettings.json` and includes:
- Console logging for development
- File logging for production
- Custom middleware for request/response logging

## 🧩 Project Structure Details

### FCG.Core (Domain Layer)
```
├── Entity/
│   ├── EntityBase.cs          # Base entity class
│   ├── User.cs                # User aggregate
│   ├── Game.cs                # Game aggregate
│   ├── Role.cs                # Role entity
│   ├── UserGameLibrary.cs     # User-Game relationship
│   └── ValueObjects/
│       ├── Email.cs           # Email value object
│       └── Password.cs        # Password value object
└── Interfaces/
    ├── IRepository.cs         # Generic repository interface
    ├── IUserRepository.cs     # User repository interface
    ├── IGameRepository.cs     # Game repository interface
    ├── IRoleRepository.cs     # Role repository interface
    ├── IUserGameLibraryRepository.cs
    └── IUnitOfWork.cs         # Unit of work interface
```

### FCG.Application (Application Layer)
```
├── DTOs/                      # Data Transfer Objects
├── Interfaces/                # Service interfaces
├── Services/                  # Service implementations
└── Validators/                # FluentValidation validators
```

### FCG.Infrastructure (Infrastructure Layer)
```
├── Data/
│   ├── ApplicationDbContext.cs # EF Core DbContext
│   └── UnitOfWork.cs          # Unit of work implementation
├── Repository/                # Repository implementations
└── Migrations/                # EF Core migrations
```

### FCG.Api (Presentation Layer)
```
├── Controllers/               # API controllers
├── Middleware/                # Custom middleware
├── Attributes/                # Custom attributes
└── Program.cs                 # Application entry point
```

## 🔒 Security Features

- **Password Hashing**: Secure password storage using bcrypt
- **Role-Based Authorization**: Custom authorization attributes
- **Input Validation**: FluentValidation for request validation
- **SQL Injection Prevention**: Entity Framework parameterized queries

## 🚀 Deployment

### Docker Support

The application can be containerized using Docker:

```dockerfile
# Example Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["FCG/FCG.Api.csproj", "FCG/"]
RUN dotnet restore "FCG/FCG.Api.csproj"
COPY . .
WORKDIR "/src/FCG"
RUN dotnet build "FCG.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FCG.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FCG.Api.dll"]
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Coding Standards

- Follow C# coding conventions
- Write unit tests for new features
- Update documentation as needed
- Ensure all tests pass before submitting PR

## 👥 Authors

- Tetsuo - Initial work and ongoing maintenance
