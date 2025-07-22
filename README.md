# ✈️ Multi-Stop Travel Planner

A full-featured, cross-platform travel planning application built with .NET MAUI and ASP.NET Core. It allows users to manage trips, add multiple stops, track budgets, and visualize their journey on a map.

![App Screenshot](https://github.com/rasoolzadh/TravelPlannerSolution/blob/master/TravelPlanner.App/Resources/Images/screenshot.png)

---

## ✨ Features

- **Trip Management:** Create, edit, and delete multi-day trips with titles, descriptions, and budgets.
- **Multi-Stop Planning:** Add detailed stops to each trip, including location, dates, costs, and notes.
- **Interactive Map View:** Visualize all stops for a trip on an interactive OpenStreetMap, with a route connecting all locations.
- **Budget Tracking:** Automatically calculates the total cost of all stops and compares it against the trip's budget.
- **Calendar View:** See your stops organized on a simple calendar.
- **Share Itinerary:** Generate and share a formatted summary of your trip plan via email or other apps.
- **Cross-Platform:** A single codebase for Android, iOS, and Windows.

---

## 🛠️ Technology Stack

### Frontend (.NET MAUI App)
- **.NET 8**
- **C# & XAML**
- **.NET MAUI** for cross-platform UI
- **MVVM Architecture** using the CommunityToolkit.Mvvm
- **WebView** with **Leaflet.js** for OpenStreetMap integration

### Backend (ASP.NET Core Web API)
- **.NET 8**
- **C#**
- **ASP.NET Core** for creating the RESTful API
- **Entity Framework Core 8** as the Object-Relational Mapper (ORM)
- **SQL Server** as the database

---

## 🚀 Getting Started

Follow these steps to get the project running locally.

### 1. Backend Setup (`TravelPlanner.Api`)
1.  Open `appsettings.json`.
2.  Update the `DefaultConnection` string to point to your SQL Server instance.
3.  Open the **Package Manager Console**, set the default project to `TravelPlanner.Api`, and run the following commands:
    ```powershell
    Add-Migration InitialCreate
    Update-Database
    ```
4.  Run the API project. It should launch a Swagger page in your browser.

### 2. Frontend Setup (`TravelPlanner.App`)
1.  If you plan to run on a **physical Android device**, open `MauiProgram.cs` and change the `baseApiUrl` to your computer's local IP address.
2.  In Visual Studio, right-click the solution and select **Set Startup Projects...**.
3.  Choose **Multiple startup projects** and set the **Action** for both `TravelPlanner.Api` and `TravelPlanner.App` to **Start**.
4.  Select your target device (Windows Machine or an Android device) and run the solution.

---

## 📝 API Endpoints

The backend provides the following key endpoints:

| Method | Route                       | Description                  |
| :----- | :-------------------------- | :--------------------------- |
| `GET`  | `/api/Trips`                | Get all trips with their stops. |
| `GET`  | `/api/Trips/{id}`           | Get a single trip by its ID. |
| `POST` | `/api/Trips`                | Create a new trip.           |
| `PUT`  | `/api/Trips/{id}`           | Update an existing trip.     |
| `DELETE`| `/api/Trips/{id}`           | Delete a trip.               |
| `POST` | `/api/Stops`                | Create a new stop for a trip.|
| `PUT`  | `/api/Stops/{id}`           | Update an existing stop.     |
| `DELETE`| `/api/Stops/{id}`           | Delete a stop.               |

---

## 📄 License

This project is distributed under the MIT License.
