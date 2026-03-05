# 🔐 SecureAuthApp | Authentication System

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)
![React](https://img.shields.io/badge/React-20232A?style=flat&logo=react&logoColor=61DAFB)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=flat&logo=postgresql&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2CA5E0?style=flat&logo=docker&logoColor=white)

A full-stack, containerized application designed to demonstrate security practices, Clean Architecture, and deployment workflows.

## 🎯 The Objective

This project was built to showcase how to properly implement **JWT (JSON Web Token) authentication** while mitigating common web vulnerabilities like **XSS (Cross-Site Scripting)**. Instead of storing tokens insecurely in the browser's `localStorage`, this API manages authentication strictly through **HttpOnly Cookies**.

## ✨ Key Features

- **Security:** Passwords are mathematically hashed using `BCrypt` before hitting the database. Tokens are handled via HttpOnly, SameSite cookies.
- **Clean Architecture:** The backend is decoupled into Domain, Application, Infrastructure, and API layers, strictly following SOLID principles.
- **Resilient Infrastructure:** The .NET API implements automated retry logic (Connection Resiliency) to gracefully handle database startup delays inside Docker.
- **Dockerized Environment:** The entire stack (Frontend, Backend, and Database) runs seamlessly with a single Docker Compose command.
- **Interactive UI:** A clean, educational React frontend that visually explains the hashing and token-handling processes.

## 🛠️ Tech Stack

- **Backend:** C#, .NET 10 (Minimal APIs), Entity Framework Core
- **Frontend:** React, Vite, CSS Flexbox
- **Database:** PostgreSQL
- **DevOps & Tooling:** Docker, Docker Compose, Git Flow

## 🏗️ Architecture Overview

The backend solution is structured following Clean Architecture:

1. **Domain:** Contains core business entities (`User`) and interfaces (`IUserRepository`).
2. **Application:** Orchestrates business rules, handles password hashing, and generates JWTs (`AuthService`).
3. **Infrastructure:** Implements database connections and data access using EF Core and PostgreSQL.
4. **API:** The entry point. Handles HTTP routing, Dependency Injection, CORS, and Cookie injection.

## 🚀 How to Run Locally

You only need [Docker](https://www.docker.com/) installed on your machine.

1. Clone this repository:
    ```bash
    git clone https://github.com/Luanderson-Dev/secure-authentication-app.git
    cd SecureAuthApp
    ```
2. Build and spin up the containers:

    ```bash
    docker-compose up --build
    ```

3. Access the application:
    - Frontend (UI): http://localhost:3000
    - Backend (API): http://localhost:8080

## 📡 API Endpoints

| Method | Endpoint             | Description                                                 | Auth Required |
| ------ | -------------------- | ----------------------------------------------------------- | ------------- |
| `POST` | `/api/auth/register` | Creates a new user with a hashed password.                  | ❌            |
| `POST` | `/api/auth/login`    | Validates credentials and injects the HttpOnly Cookie.      | ❌            |
| `POST` | `/api/auth/logout`   | Instructs the browser to delete the authentication cookie.  | ✅            |
| `GET`  | `/api/protected`     | A test endpoint that requires a valid JWT cookie to access. | ✅            |
