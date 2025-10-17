# 🎓 University Management Minimal API

A **clean and modern .NET 9 Minimal API** for managing university entities — **Students**, **Courses**, and **Enrollments** — built with a modular architecture and secure **JWT authentication** using **ASP.NET Core Identity**.

---

## ✨ Features

- 🔐 **Authentication & Authorization**
  - JWT-based login and registration.
  - Role-based access control (Admin / User).
- 📚 **Entity Management**
  - CRUD operations for Students, Courses, and Enrollments.
  - Strongly-typed DTOs and input validation with FluentValidation.
- ⚙️ **Clean Architecture**
  - Domain-driven structure: `Domain`, `Persistence`, `Repository`, `Endpoints`, `OpenApiSpecs`.
  - Generic repository pattern (`IGenericRepository<T>`).
- 📄 **Advanced OpenAPI (Swagger)**
  - Separate Swagger docs for each module (Auth, Students, Courses, Enrollments).
  - Rich OpenAPI descriptions and examples via `.WithOpenApi(...)`.
  - Fully typed Minimal API responses for better client SDK generation.
- 📑 **Pagination & Filtering**
  - Built-in pagination for all list endpoints.
  - Query parameters for flexible filtering and sorting.
- 🧩 **Extensible & Maintainable**
  - Easily extend with new entities or endpoints.
  - Ready for containerization and CI/CD pipelines.

---

## 🏗️ Tech Stack

| Layer | Technology |
|--------|-------------|
| **Backend** | .NET 9 Minimal APIs |
| **Database** | SQL Server (Entity Framework Core 9) |
| **Auth** | ASP.NET Core Identity + JWT |
| **Validation** | FluentValidation |
| **Docs** | Swagger / OpenAPI |
| **Patterns** | Repository, DDD, DTO Mapping |

---

## 📁 Project Structure

