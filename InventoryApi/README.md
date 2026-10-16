# Inventory API

A small backend API for managing products and orders in a warehouse. I built it to practice ASP.NET Core, Entity Framework Core and SQL Server.

## What it does

- Create, read, update and delete products
- Create orders with several items. The API checks that each product exists and that there is enough stock. If something is wrong, it returns an error and nothing is saved.
- Two reports: products with low stock, and the most ordered products

## Built with

C#, .NET 8, ASP.NET Core Web API, Entity Framework Core, SQL Server (LocalDB), Swagger

## How to run

1. Open the solution in Visual Studio 2022.
2. In the Package Manager Console, run `Update-Database` to create the database.
3. Press play. Swagger opens in the browser and you can test the endpoints there.

The connection string is in `appsettings.json`.

## How it's organized

Controllers receive the requests, services contain the logic (like the stock check), and `AppDbContext` talks to the database through EF Core.

## What I would add next

Authentication, unit tests, and protection against two orders arriving at the same time.