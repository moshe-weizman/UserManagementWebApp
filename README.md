
---

# User Management Web Application

## Overview

This is a web application built using ASP.NET Core, jQuery, and SQL Server. It allows users to register with their details, including username, email, date of birth, and a photo. The photo is uploaded to the server and the details are stored in a SQL Server database. The application also displays a list of existing users, and clicking on a user in the list will display their full details.

## Features

- User registration form with fields for username, email, date of birth, and photo upload.
- Photo upload with image preview.
- Store user details in a SQL Server database.
- Display a list of registered users.
- View full details of a user by clicking on a row in the users list.
- Prevent duplicate usernames.
- Date of birth cannot be later than today.
- Clear button to reset the form.

## Prerequisites

- .NET 6.0 SDK or later
- SQL Server
- Visual Studio or another suitable IDE

## Installation

1. Clone the repository:

    ```sh
    git clone https://github.com/your-username/UserManagementWebApp.git
    cd UserManagementWebApp
    ```

2. Set up the SQL Server database:


1. Create a new database named `UserManagement`.
2. Execute the following scripts in the provided order:

   - `create_table.sql`: This script creates the necessary tables for the application.
  
    - Update the connection string in `appsettings.json`:

    ```json
    "ConnectionStrings": {
        "AppDbContext": "Server=your_server_name;Database=your_database_name; Integrated Security=True; TrustServerCertificate=True;"
    }
    ```



3. Run the application:


## Usage

1. Fill in the registration form with your details:
    - Username
    - Email
    - Date of Birth (cannot be later than today)
    - Photo (image preview available)

2. Click the "Submit" button to register. If the username already exists, an error message will be displayed.

3. The registered user will be displayed in the users list. Click on a user in the list to view their full details.

4. Use the "Clear" button to reset the form fields.

## Project Structure

- **Controllers**: Contains the API controllers for handling HTTP requests.
- **Models**: Contains the data models used in the application.
- **Data**: Contains the database context and repository classes.
- **wwwroot**: Contains static files such as HTML, CSS, JavaScript, and uploaded photos.

## Dependencies

- ASP.NET Core
- Entity Framework Core
- jQuery



---

