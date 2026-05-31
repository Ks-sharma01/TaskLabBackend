# Task Management System

## Overview

Task Management System is a full-stack web application that helps teams manage and track tasks efficiently. Team Leads can assign tasks to team members, while members can view and update only their assigned tasks.

The application includes authentication, role-based authorization, task assignment, Redis caching, rate limiting, and a responsive React frontend.

---

## Features

### Authentication & Authorization

* User Registration and Login
* JWT Authentication
* Refresh Token Support
* Role-Based Access Control (Admin, Team Lead, Team Member)

### Task Management

* Create Tasks
* Assign Tasks to Team Members
* Update Task Status
* Delete Tasks
* View Task Details

### Team Lead Features

* Create and Assign Tasks
* View All Team Tasks
* Track Task Progress

### Team Member Features

* View Assigned Tasks Only
* Update Task Status
* View Task Details

### Security Features

* JWT Authentication
* Refresh Tokens
* Rate Limiting for Login and OTP APIs
* OTP Verification
* Password Hashing
* Role-Based Authorization

### Performance Optimization

* Redis Caching
* Repository Pattern
* Dependency Injection
* Asynchronous Programming

---

## Technology Stack

### Frontend

* React.js
* Tailwind CSS
* Axios
* React Router DOM

### Backend

* ASP.NET Core Web API
* Entity Framework Core
* JWT Authentication
* ASP.NET Core Identity
* Redis Cache

### Database

* SQL Server

### Tools

* Visual Studio
* VS Code
* SSMS
* Postman
* Git & GitHub

---

## Architecture

```text
React Frontend
       │
       ▼
ASP.NET Core Web API
       │
 ┌─────┴─────┐
 ▼           ▼
SQL Server   Redis
```

---

## Project Structure

```text
TaskManagementSystem
│
├── Frontend (React)
│   ├── Components
│   ├── Pages
│   ├── Services
│   └── Routing
│
├── Backend (ASP.NET Core Web API)
│   ├── Controllers
│   ├── Services
│   ├── Repositories
│   ├── Models
│   ├── DTOs
│   ├── Middleware
│   └── Authentication
│
└── Database
    └── SQL Server
```

---

## API Features

### Authentication APIs

* Register User
* Login User
* Refresh Token
* Logout

### Task APIs

* Create Task
* Update Task
* Delete Task
* Get All Tasks
* Get Task By Id
* Get Assigned Tasks

### User APIs

* Get User Profile
* Update Profile

---

## Design Patterns Used

* Repository Pattern
* Dependency Injection
* JWT Authentication
* Role-Based Authorization
* Caching Pattern

---

## Installation

### Clone Repository

```bash
git clone <repository-url>
```

### Backend Setup

```bash
cd Backend
```

Restore packages:

```bash
dotnet restore
```

Apply migrations:

```bash
dotnet ef database update
```

Run API:

```bash
dotnet run
```

### Frontend Setup

```bash
cd frontend
```

Install packages:

```bash
npm install
```

Run application:

```bash
npm run dev
```

---

## Future Enhancements

* Email Notifications
* Task Comments
* File Attachments
* Dashboard Analytics
* Real-Time Notifications using SignalR
* Team Performance Reports

---

## Learning Outcomes

This project demonstrates practical implementation of:

* ASP.NET Core Web API
* React.js
* SQL Server
* JWT Authentication
* Role-Based Authorization
* Redis Caching
* Rate Limiting
* Repository Pattern
* RESTful API Development

---

## Author

Kiran Sharma

Aspiring Full Stack .NET Developer
