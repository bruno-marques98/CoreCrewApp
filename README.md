# Employment Management System

The Employment Management System is a web-based application designed to streamline and manage employee data, attendance tracking, role assignments, and overall HR-related tasks within an organization. This application offers secure login via JWT (JSON Web Token) and integrates Angular for a responsive front-end experience.

## Table of Contents
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Database Setup](#database-setup)
  - [Running the Application](#running-the-application)
- [Usage](#usage)
- [API Endpoints](#api-endpoints)
- [Authentication](#authentication)
- [Role Management](#role-management)
- [Contributing](#contributing)
- [License](#license)

---

## Features

- **User Authentication**: Secure login and registration using JWT tokens.
- **Role-based Authorization**: Admin and User roles with different access levels.
- **Employee Management**: Add, update, delete, and view employee details.
- **Attendance Tracking**: Record and monitor employee attendance.
- **Dashboard**: Displays employee and training counts and other key metrics.
- **Real-Time Notifications**: In-app notifications for role assignments and attendance.

## Technologies Used

- **Backend**: ASP.NET Core 6.0, Entity Framework Core, JWT for authentication
- **Frontend**: Angular (for responsive front-end and dynamic pages)
- **Database**: Microsoft SQL Server
- **Identity**: ASP.NET Core Identity for user management and roles

## Getting Started

To run this project locally, follow these instructions. TODO

### Prerequisites

- [.NET SDK 6.0+](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Node.js](https://nodejs.org/) and npm (for Angular)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Installation

1. **Clone the Repository**
   ```bash
   git clone https://github.com/yourusername/employment-management-system.git
   cd employment-management-system
