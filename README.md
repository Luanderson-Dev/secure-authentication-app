# 🔐 SecureAuthApp | Enterprise Authentication System

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)
![Next.js](https://img.shields.io/badge/Next.js-000000?style=flat&logo=nextdotjs&logoColor=white)
![TypeScript](https://img.shields.io/badge/TypeScript-3178C6?style=flat&logo=typescript&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=flat&logo=postgresql&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2CA5E0?style=flat&logo=docker&logoColor=white)
![Testing](https://img.shields.io/badge/Testing-xUnit-512BD4?style=flat)
![Security](https://img.shields.io/badge/Security-Argon2id-FF4B4B?style=flat)

A full-stack, production-ready containerized application designed to demonstrate state-of-the-art security practices, strict Clean Architecture, and resilient deployment workflows.

## 🎯 The Objective

This project was built to showcase how to properly implement **JWT (JSON Web Token) authentication** while mitigating common web vulnerabilities like **XSS (Cross-Site Scripting)**. Instead of storing tokens insecurely in the browser's `localStorage`, this API manages authentication strictly through **HttpOnly, SameSite Cookies**.

Furthermore, it demonstrates how to securely manage environment secrets, implement advanced password hashing algorithms, and maintain high code quality through automated testing.

## ✨ Key Features

### 🛡️ Advanced Security

- **Argon2id + Pepper:** Passwords are mathematically hashed using the current industry standard combined with a server-side Pepper before hitting the database.
- **HttpOnly JWT & Silent Refresh:** Implements a short-lived Access Token (15 min) and a long-lived Refresh Token (7 days) via HttpOnly cookies. The Next.js client seamlessly intercepts 401 errors and refreshes the session in the background.

### 🏗️ Architecture & Reliability

- **Strict Clean Architecture:** The backend is deeply decoupled into Domain, Application, Infrastructure, and API layers, strictly following SOLID principles and Dependency Inversion.
- **Connection Resiliency:** The API implements automated retry logic to gracefully handle database startup delays inside Docker.

### 💻 Modern Server-Side Frontend

- **Next.js App Router:** Uses Server Components to securely read HttpOnly cookies and fetch protected data before sending HTML to the client, eliminating loading spinners for authenticated routes.
- **Multi-stage Docker Builds:** The frontend runs on a highly optimized, standalone Node.js Docker image.

### 🧪 Automated Testing

- **Unit Testing Suite:** Core business logic, token generation, and password hashing are fully covered using **xUnit**, **Moq**, and **FluentAssertions**.

## 🛠️ Tech Stack

- **Backend:** C#, .NET 10 (Web API), Entity Framework Core
- **Frontend:** Next.js (App Router), TypeScript, Tailwind CSS, Axios
- **Security & Utilities:** `Argon2`, `JwtBearer`, `DotNetEnv`, `Swashbuckle` (Swagger)
- **Testing:** xUnit, Moq, FluentAssertions
- **Database:** PostgreSQL
- **DevOps:** Docker, Docker Compose, Git Flow

## 🚀 How to Run Locally

You only need [Docker](https://www.docker.com/) installed on your machine.

1. Clone this repository:
    ```bash
    git clone https://github.com/Luanderson-Dev/secure-authentication-app.git
    cd secure-authentication-app
    ```
2. Crucial Step - Environment Variables: Create a .env file in the root directory (where the docker-compose.yml is located) and add your secrets:

    ```
    # Database Configuration
    POSTGRES_USER=admin
    POSTGRES_PASSWORD=adminpassword
    POSTGRES_DB=authdb

    # Application Secrets
    PEPPER_SECRET=YourSuperSecretPepperHere123!
    JWT_SECRET=YourVeryLongAndSecureJwtKeyGoesHere2026!
    DB_CONNECTION=Host=db;Database=authdb;Username=admin;Password=adminpassword
    ```

3. Build and spin up the containers:

    ```bash
    docker compose up -d --build
    ```

4. Access the application:
    - Frontend (UI): http://localhost:3000
    - Backend / Swagger API Docs: http://localhost:8080

## 🧪 Running Automated Tests

To ensure the integrity of the core domain and security algorithms, you can run the test suite locally. Open a terminal at the root of the project and execute:

```bash
dotnet test src/SecureAuthApp.Tests/SecureAuthApp.Tests.csproj
```

## 📡 API Endpoints

Interactive documentation is available via Swagger UI at http://localhost:8080 when the containers are running.

| Method | Endpoint             | Description                                                   | Auth Required                |
| ------ | -------------------- | ------------------------------------------------------------- | ---------------------------- |
| `POST` | `/api/auth/register` | Creates a new user with an Argon2 hashed password.            | ❌                           |
| `POST` | `/api/auth/login`    | Validates credentials and injects Access & Refresh Cookies.   | ❌                           |
| `POST` | `/api/auth/refresh`  | Validates the Refresh Cookie and issues a new pair of tokens. | ❌ (Requires Refresh Cookie) |
| `POST` | `/api/auth/logout`   | Instructs the browser to revoke and delete all cookies.       | ✅                           |
| `GET`  | `/api/protected`     | A test endpoint that requires a valid JWT cookie to access.   | ✅                           |
