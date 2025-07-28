# NadinSoftTask

A Clean Architecture-based .NET 8 application built as a technical task for NadinSoft.  
This system allows users to register, create products, and perform full CRUD operations on their own products.

---

## 🚀 Features

- 🔐 **User registration & JWT authentication**
- 🛠️ **Product CRUD** with user ownership
- ⚙️ Clean Architecture: Domain, Application, Infrastructure, API
- 📦 HATEOAS-style pagination
- ✅ TDD with xUnit
- 🧪 Full test coverage for Application, Domain, Infrastructure & Identity
- 🧾 Structured logging via **Serilog** + **Seq**
- 🐳 Dockerized environment

---

## 🛠️ Tech Stack

| Layer        | Tech Used                                                |
|--------------|----------------------------------------------------------|
| **Backend**  | C#, ASP.NET Core 8, EF Core, Fluent API                  |
| **Architecture** | Clean Architecture, MediatR, FluentValidation, HATEOAS |
| **Auth**     | JWT Bearer Tokens                                        |
| **Database** | SQL Server (via Docker)                                  |
| **Docs**     | Swagger/OpenAPI                                          |
| **Logging**  | Serilog + Seq                                            |
| **Tests**    | xUnit, FluentAssertions                                  |
| **Containerization** | Docker, Docker Compose                          |

---

## 🧾 Getting Started

### 🐳 Run with Docker (Recommended)

Make sure [Docker](https://www.docker.com/) is running, then:

```bash
docker-compose up --build
```

> ✅ No additional setup required. Database, API, and logging will start automatically.

### 🔗 Access Points

| Service       | URL                             |
|---------------|----------------------------------|
| **API**       | http://localhost:5000            |
| **Swagger**   | http://localhost:5000/swagger    |
| **Logging (Seq)** | http://localhost:5341         |

---

## 🧪 Running Tests

This project uses **xUnit** for unit and integration testing.

```bash
dotnet test
```

> 🐳 Ensure Docker is running before running tests — SQL Server container is used for testing Infrastructure & Identity.

---

## 📂 Project Structure

```
NadinSoftTask/
│
├── src/
│   ├── Api/                   # ASP.NET Core Web API
│   ├── Application/           # Application logic (CQRS, DTOs, Interfaces)
│   ├── Domain/                # Domain models and interfaces
│   ├── Infrastructure/
│   │   ├── Identity/          # Identity & JWT setup
│   │   ├── Persistence/       # EF Core + Fluent API
│   │   └── CrossCutting/      # Common tools, services, logging, etc.
│   └── WebFramework/          # API setup, filters, middleware
│
├── tests/
│   ├── Application.Tests/
│   ├── Domain.Tests/
│   ├── Infrastructure.Tests/
│   └── Identity.Tests/
│
└── docker-compose.yml         # Full containerized setup
```

---

## 📌 Key Design Patterns

- ✅ **Clean Architecture** (separation of concerns)
- 🧪 **TDD-first development**
- 🔁 **CQRS** with MediatR
- 🧭 **HATEOAS-style pagination**
- 🔐 **JWT-based authentication**
- 📄 **Swagger/OpenAPI** documentation
- 🪵 **Structured Logging** via Serilog & Seq

---

## ✅ Status

> 🔨 This project is production-ready and follows best practices in architecture, testing, and clean code.

---

## 📃 License

This project is licensed for demonstration and technical evaluation purposes.

---
