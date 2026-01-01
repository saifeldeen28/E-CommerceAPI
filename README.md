E-Commerce API
A robust and scalable RESTful API built for an E-Commerce platform. This project handles everything from product management and user authentication to shopping carts and order processing.

🚀 Features
Product Management: Full CRUD operations for products, categories, and brands.

Identity & Security: Secure user registration and login using ASP.NET Core Identity and JWT (JSON Web Tokens).

Shopping Basket: High-performance basket management (optionally backed by Redis).

Order Processing: Workflow for creating orders, calculating totals, and tracking status.

Pagination & Filtering: Efficient data retrieval with support for searching, sorting, and paging.

Generic Repository Pattern: Clean and maintainable data access layer.

API Documentation: Fully interactive documentation using Swagger UI.

🛠️ Tech Stack
Framework: ASP.NET Core 8.0 (or 6.0/7.0)

Database: Microsoft SQL Server

ORM: Entity Framework Core

Mapping: AutoMapper

Validation: FluentValidation

Logging: Serilog / Built-in Logger

📋 Prerequisites
Before you begin, ensure you have the following installed:

.NET SDK (Version 6.0 or later)

SQL Server (Express or Developer edition)

Visual Studio 2022 or VS Code

⚙️ Getting Started
1. Clone the Repository
git clone https://github.com/saifeldeen28/E-CommerceAPI.git
cd E-CommerceAPI

3. Update Connection String
Open appsettings.json in the API project and update the DefaultConnection string to point to your local SQL Server instance:
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=EcommerceDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}

4. Apply Migrations
Open your terminal or Package Manager Console and run:
dotnet ef database update

6. Run the Application
 dotnet run --project E-CommerceAPI.csproj
The API will be available at https://localhost:5001 (or the port specified in your launchSettings.json).

📖 API Documentation
Once the application is running, you can explore the endpoints via Swagger UI:

URL: https://localhost:5001/swagger/index.html

📂 Project Structure
Layer,Responsibility,Key Components
📂 E-Commerce.Core,Domain Layer,"Entities, Interfaces, Specifications"
📂 E-Commerce.Repository,Data Access Layer,"Data Context, Migrations, Repositories"
📂 E-Commerce.Service,Business Logic Layer,"Application Services, Business Logic"
🚀 E-Commerce.API,Presentation Layer,"Controllers, DTOs, Middleware, Extensions"

🔐 Authentication
To access protected endpoints:

Register a user via /api/account/register.

Login via /api/account/login to receive a JWT token.

Include the token in the Authorization header: Authorization: Bearer {your_token}.

🤝 Contributing
Contributions are welcome! Please feel free to submit a Pull Request.

Fork the Project

Create your Feature Branch (git checkout -b feature/AmazingFeature)

Commit your Changes (git commit -m 'Add some AmazingFeature')

Push to the Branch (git push origin feature/AmazingFeature)

Open a Pull Request

📧 Contact
Saifeldeen - GitHub Profile

Project Link: https://github.com/saifeldeen28/E-CommerceAPI

Developed with ❤️ using .NET
