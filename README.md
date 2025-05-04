# Educational Portal

Educational Portal is a web application designed to facilitate in-company learning. Employees can create and share courses with their colleagues, making it particularly beneficial for training interns and junior developers. Course authors earn in-company currency, which can be utilized in other company services. The application is built using the Onion (Clean) architecture, with an ASP.NET Core backend and an Angular frontend.

## Table of Contents

- [Features](#features)
- [Stack](#stack)
- [Installation](#installation)
  - [Prerequisites](#prerequisites)
  - [Setup Instructions](#setup-instructions)
- [Usage](#usage)

## Features

- **Course Creation**: Employees can create and manage courses for their colleagues.
- **In-Company Currency**: Authors earn in-company currency for course creation, usable in other company services.
- **User Management**: Manage user roles and permissions within the portal.
- **Responsive Design**: Accessible on various devices with a responsive user interface.

## Stack

- **Backend**: ASP.NET Core
- **Frontend**: Angular
- **Architecture**: Onion (Clean)
- **Database**: SQL Server
- **Containerization**: Docker

## Installation

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/en/download/)
- [Angular CLI](https://angular.io/cli)
- [Docker](https://www.docker.com/get-started)

### Setup Instructions

1. **Clone the Repository**:

   ```bash
   git clone https://github.com/Shchoholiev/educational-portal.git
   cd educational-portal
   ```

2. **Backend Setup**:

   - Navigate to the API project directory:

     ```bash
     cd EducationalPortal.API
     ```

   - Restore dependencies and build the project:

     ```bash
     dotnet restore
     dotnet build
     ```

   - Apply database migrations:

     ```bash
     dotnet ef database update
     ```

   - Run the backend server:

     ```bash
     dotnet run
     ```

3. **Frontend Setup**:

   - Navigate to the frontend project directory:

     ```bash
     cd ../EducationalPortal.Web
     ```

   - Install dependencies:

     ```bash
     npm install
     ```

   - Start the frontend development server:

     ```bash
     ng serve
     ```

4. **Access the Application**:

   - Open your browser and navigate to `http://localhost:4200` to access the frontend.
   - The backend API will be running at `http://localhost:5000`.

## Usage

- **Create Courses**: Employees can create courses by navigating to the "Create Course" section and filling out the necessary details.
- **Enroll in Courses**: Employees can browse available courses and enroll in those that interest them.
- **Earn In-Company Currency**: Course authors earn in-company currency upon course creation, which can be used in other company services.
- **Manage Users**: Admins can manage user roles and permissions through the "User Management" section.
