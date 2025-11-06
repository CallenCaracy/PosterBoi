# PosterBoi

**PosterBoi** is a lightweight social media backend built with **ASP.NET Core 8**, designed to handle user posts, comments, and image uploads via **Cloudinary**. It follows a clean architecture pattern with **Controllers → Services → Interfaces → Data Layer**, making it scalable and maintainable.  

---

## Features

- **User posts and comments**  
- **Image uploads** using Cloudinary with folder organization  
- **API versioning** for future enhancements  
- **Dependency Injection (DI)** for services  
- **SQL Server database** integration with Entity Framework Core  
- **Swagger UI** for API exploration  

_Planned features:_  

- WebSocket chat  
- User authentication  
- Likes and reactions system  

---

## Project Structure

```bash
PosterBoi/
├─ API/                 # Controllers, API entry point
│  ├─ Controllers/      # Versioned controllers (v1, v2)
│  ├─ Extensions/       # DI, Logging, CORS, Cloudinary config
├─ Core/                # Domain entities and interfaces
│  ├─ Configs/          # Cloudinary and other 3rd party services configurations
│  ├─ DTOs/             # Data Transfer Objects
│  ├─ Models/           # User, Post, Comment
│  ├─ Interfaces/       # Service interfaces
├─ Infrastructure/      # Implementation of services, DB context
│  ├─ Data/             # AppDbContext
│  ├─ Services/         # CloudinaryService, PostService
├─ Program.cs           # Entry point with DI, middleware setup
├─ Startup.cs           # Entry point with DI, middleware setup
├─ appsettings.json     # Configuration
```

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)  
- [SQL Server](https://www.microsoft.com/en-us/sql-server) (LocalDB works)  
- [Cloudinary account](https://cloudinary.com/) for image uploads  

---

### Setup

1. Clone the repository:

    ```
    git clone https://github.com/YourUsername/PosterBoi.git
    cd PosterBoi
    ```

2. Add your Cloudinary credentials to `appsettings.Development.json`:

    ```
    "Cloudinary": {
      "CloudName": "your-cloud-name",
      "ApiKey": "your-api-key",
      "ApiSecret": "your-api-secret"
    }
    ```

3. Configure SQL Server connection string in `appsettings.json`:

    ```
    "ConnectionStrings": {
      "PosterBoiDBConnection": "Server=localhost;Database=PosterBoiDB;User Id=sa;Password=YourPassword;"
    }
    ```

4. Restore packages and run the application:

    ```
    dotnet restore
    dotnet run
    ```

5. Open Swagger UI at:

    ```
    https://localhost:{port}/swagger/index.html
    ```

6. Database Operations
    ## Normal
    ```bash
    dotnet ef database drop
    dotnet ef migrations remove
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```

    ## Fully Purge
    ```bash
    Remove-Item -Recurse -Force .\Migrations
    Remove-Item -Recurse -Force .\bin
    Remove-Item -Recurse -Force .\obj
    dotnet clean
    dotnet build
    dotnet ef migrations add Init
    dotnet ef database update
    ```
---
