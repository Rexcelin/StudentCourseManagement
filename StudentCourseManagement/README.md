# Student Course Management System

## Overview

The Student Course Management System is a console-based application developed using C# and .NET. It manages student details, course information, department records, and student enrollments using SQL Server as the database.

The project follows a layered architecture to separate database operations, business logic, and data models, making the application easier to maintain and extend.

## Technologies Used

- **Programming Language:** C#
- **Framework:** .NET
- **Database:** Microsoft SQL Server
- **Database Connectivity:** Microsoft.Data.SqlClient
- **Architecture:** Repository and Service Pattern
- **IDE:** Visual Studio

## Features

- **Student Management** – Add and retrieve student information.
- **Course Management** – Manage course details.
- **Department Management** – Maintain department records.
- **Enrollment Management** – Enroll students in courses and manage enrollment status.
- **Database Integration** – Store and retrieve information using SQL Server.
- **Asynchronous Operations** – Use async/await for database operations.
- **Layered Architecture** – Separate models, repositories, services, and helper classes.

## Project Structure

```text
StudentCourseManagement/
│
├── Helpers/
│   └── DatabaseHelper.cs
│
├── Models/
│   ├── Student.cs
│   ├── Course.cs
│   ├── Department.cs
│   └── Enrollment.cs
│
├── Repository/
│   ├── StudentRepository.cs
│   ├── CourseRepository.cs
│   ├── DepartmentRepository.cs
│   └── EnrollmentRepository.cs
│
├── Services/
│   ├── StudentService.cs
│   ├── CourseService.cs
│   ├── DepartmentService.cs
│   └── EnrollmentService.cs
│
├── Program.cs
├── appsettings.json
├── database_schema.sql
├── .gitignore
└── README.md
```

*Note: Update the file and folder names above to match your actual project structure.*

## Prerequisites

Before running the application, install:

1. .NET SDK compatible with your project.
2. Microsoft SQL Server.
3. SQL Server Management Studio (SSMS), if you want a graphical interface to manage the database.
4. Visual Studio or Visual Studio Code.

## Database Setup

1. Open SQL Server Management Studio.
2. Connect to your SQL Server instance.
3. Create the database required by the project.
4. Execute your database schema SQL script to create the necessary tables.
5. Update the connection string in your configuration or database helper.

Example connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=StudentCourseManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Replace `YOUR_SERVER_NAME` with your actual SQL Server instance name.

## Installation and Execution

### 1. Clone the repository

```bash
git clone YOUR_GITHUB_REPOSITORY_URL
```

### 2. Navigate to the project directory

```bash
cd StudentCourseManagement
```

### 3. Restore dependencies

```bash
dotnet restore
```

### 4. Build the project

```bash
dotnet build
```

### 5. Run the application

```bash
dotnet run
```

## Architecture

The application uses a layered architecture:

- **Models:** Represent entities such as students, courses, departments, and enrollments.
- **Helpers:** Provide shared database connection functionality.
- **Repositories:** Handle SQL queries and database operations.
- **Services:** Implement business logic and validations.
- **Program.cs:** Provides the console entry point and application navigation.

## Future Enhancements

- Convert the console application into a RESTful Web API.
- Add authentication and role-based authorization.
- Implement input validation and improved exception handling.
- Add logging and audit trail functionality.
- Develop a web-based user interface.

## Author

**Rexcelin J**

Computer Science and Engineering

## License

This project is intended for educational and learning purposes.
