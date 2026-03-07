# 🔐 SecureAuthApp | Authentication System

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)
![React](https://img.shields.io/badge/React-20232A?style=flat&logo=react&logoColor=61DAFB)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=flat&logo=postgresql&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2CA5E0?style=flat&logo=docker&logoColor=white)
![Security](https://img.shields.io/badge/Security-Argon2id-FF4B4B?style=flat)

A full-stack, containerized application designed to demonstrate state-of-the-art security practices, strict Clean Architecture, and secure deployment workflows.

## 🎯 The Objective

This project was built to showcase how to properly implement **JWT (JSON Web Token) authentication** while mitigating common web vulnerabilities like **XSS (Cross-Site Scripting)**. Instead of storing tokens insecurely in the browser's `localStorage`, this API manages authentication strictly through **HttpOnly, SameSite Cookies**.

Furthermore, it demonstrates how to securely manage environment secrets and implement advanced password hashing algorithms.

## ✨ Key Features

- **Advanced Security:** Passwords are mathematically hashed using **Argon2id** (the current industry standard) combined with a server-side **Pepper** before hitting the database.
- **HttpOnly JWT:** Tokens are securely generated and injected into HttpOnly cookies, making them inaccessible to malicious JavaScript.
- **Strict Clean Architecture:** The backend is deeply decoupled into Domain, Application, Infrastructure, and API layers, strictly following SOLID principles and Dependency Inversion.
- **Secure Secrets Management:** Database credentials, JWT secrets, and hashing peppers are safely isolated in a `.env` file, loaded dynamically via `DotNetEnv` and Docker Compose.
- **Resilient Infrastructure:** The .NET API implements automated retry logic (Connection Resiliency) to gracefully handle database startup delays inside Docker.
- **Dockerized Environment:** The entire stack (Frontend, Backend, and Database) runs seamlessly with a single Docker Compose command.

## 🛠️ Tech Stack

- **Backend:** C#, .NET 10 (Web API with Controllers), Entity Framework Core
- **Security Libraries:** `Konscious.Security.Cryptography.Argon2`, `Microsoft.AspNetCore.Authentication.JwtBearer`, `DotNetEnv`
- **Frontend:** React, Vite, CSS Flexbox
- **Database:** PostgreSQL
- **DevOps & Tooling:** Docker, Docker Compose, Git Flow

## 🏗️ Architecture Overview

The backend solution is structured following Clean Architecture:

1.  **Domain:** Contains pure business entities (e.g., `User`). It has no dependencies on external frameworks.
2.  **Application:** Defines the interfaces/contracts (`IUserRepository`, `IPasswordHasher`, `IJwtProvider`) and DTOs (`LoginRequest`, `RegisterRequest`).
3.  **Infrastructure:** Implements the contracts. This is where the heavy lifting happens: EF Core PostgreSQL connections, `Argon2PasswordHasher` logic, and `JwtProvider` token generation.
4.  **API (Presentation):** The entry point. Handles HTTP routing via Controllers (`AuthController`, `ProtectedController`), loads `.env` variables, configures CORS, and manages Dependency Injection.

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
    - Backend (API): http://localhost:8080

## 📡 API Endpoints

| Method | Endpoint             | Description                                                 | Auth Required |
| ------ | -------------------- | ----------------------------------------------------------- | ------------- |
| `POST` | `/api/auth/register` | Creates a new user with a hashed password.                  | ❌            |
| `POST` | `/api/auth/login`    | Validates credentials and injects the HttpOnly Cookie.      | ❌            |
| `POST` | `/api/auth/logout`   | Instructs the browser to delete the authentication cookie.  | ✅            |
| `GET`  | `/api/protected`     | A test endpoint that requires a valid JWT cookie to access. | ✅            |
