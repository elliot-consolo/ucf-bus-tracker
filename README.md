# UCF Bus Tracker

A full-stack transit analytics platform that collects and analyzes real-time UCF shuttle data to visualize historical travel times between shuttle stops.

**Live Application:** https://ucf-bus-tracker-production.up.railway.app

The application continuously polls the TransLoc API for active UCF shuttle positions, collecting **140,000+ shuttle position records per day**. Historical data is stored in PostgreSQL and processed using SQL to determine how long individual buses take to travel between selected stops.

Users can select a route, starting stop, and ending stop to visualize historical shuttle travel times throughout the day.

---

## Overview

The UCF Bus Tracker transforms continuously collected transit data into an interactive analytics application.

Every 15 seconds, a background service retrieves the current positions of active UCF shuttle vehicles from the TransLoc API. The resulting data is stored in a PostgreSQL database and connected to a relational route/stop structure designed specifically for the application.

When a user selects a starting and ending stop, the application queries the historical data surrounding those stops, identifies when buses enter each stop, calculates the travel time between them, and returns the results through a REST API for visualization.

The current visualization combines historical observations across multiple days. Day-specific filtering and additional historical analysis are planned as the dataset continues to grow.

---

## Live Demo

**[UCF Bus Tracker](https://ucf-bus-tracker-production.up.railway.app)**

The application is publicly deployed and automatically rebuilt whenever changes are pushed to GitHub.

---

## Features

- **Real-time shuttle data collection**
  - Polls the TransLoc API every 15 seconds.
  - Collects 140,000+ shuttle position records per day.
  - Continuously builds a historical dataset.

- **Historical travel-time analysis**
  - Select a shuttle route and pair of stops.
  - Identifies when a bus enters the starting and ending stops.
  - Calculates travel time using recorded timestamps.
  - Visualizes historical travel times based on the time the trip began.

- **Relational PostgreSQL database**
  - Three application-designed tables organize routes, stops, and shuttle observations.
  - Foreign keys connect shuttle observations and stops to their associated routes.
  - SQL is used for data manipulation, filtering, and analysis.

- **Interactive visualization**
  - JavaScript-based frontend.
  - Chart.js line graphs display historical travel-time data.
  - Route and stop selections dynamically determine the displayed data.

- **Automated data collection**
  - .NET background service collects data independently of user requests.
  - Allows the database to continuously accumulate historical observations.

- **Containerized deployment**
  - Docker multi-stage build minimizes the runtime image.
  - Automatically deployed through Railway from GitHub.

---

# Architecture

```text
                        ┌──────────────────────┐
                        │     TransLoc API     │
                        │  Live Shuttle Data   │
                        └──────────┬───────────┘
                                   │
                             Every 15 seconds
                                   │
                                   ▼
                        ┌──────────────────────┐
                        │ .NET Background      │
                        │     Service          │
                        └──────────┬───────────┘
                                   │
                                   ▼
                   ┌─────────────────────────────────┐
                   │           PostgreSQL            │
                   │                                 │
                   │          ┌─────────┐            │
                   │          │ routes  │            │
                   │          └────┬────┘            │
                   │               │                 │
                   │       ┌───────┴───────┐         │
                   │       │               │         │
                   │       ▼               ▼         │
                   │  ┌─────────┐   ┌─────────────┐  │
                   │  │  stops  │   │bus_snapshots│  │
                   │  └─────────┘   └─────────────┘  │
                   │                                 │
                   │  Both tables reference routes   │
                   │       through foreign keys      │
                   └───────────────┬─────────────────┘
                                   │
                              SQL + Dapper
                                   │
                                   ▼
                        ┌──────────────────────┐
                        │    C# / ASP.NET      │
                        │       REST API       │
                        │                      │
                        │      DTO Models      │
                        └──────────┬───────────┘
                                   │
                              JSON Response
                                   │
                                   ▼
                        ┌──────────────────────┐
                        │      Frontend        │
                        │                      │
                        │ JavaScript / HTML    │
                        │ CSS / Chart.js       │
                        └──────────────────────┘
```

---

# Technology Stack

| Category | Technology |
|---|---|
| Language | C# |
| Runtime | .NET 10 |
| Web Framework | ASP.NET |
| Database | PostgreSQL |
| Database Connectivity | Npgsql |
| Data Access | Dapper |
| Database Language | SQL |
| Frontend | JavaScript, HTML, CSS |
| Visualization | Chart.js |
| API Architecture | REST |
| Data Transfer | DTO Models |
| Background Processing | .NET Background Service |
| Containerization | Docker |
| Hosting | Railway |
| Database Management | pgAdmin |
| Source Control | Git / GitHub |
| External Data Source | TransLoc API |

---

# Database Design

The PostgreSQL database consists of three primary tables designed specifically for the application.

### `routes`

Stores information about shuttle routes.

Both `bus_snapshots` and `stops` reference routes through foreign keys, allowing data from the different tables to be related through the route they belong to.

### `stops`

Stores shuttle stop information, including stop positions and the geographic range used to determine when a bus has entered a stop.

The stop-specific range allows the application to determine whether a recorded shuttle position should be considered an arrival at that stop.

### `bus_snapshots`

Stores the important data collected from the TransLoc API for each shuttle position observation.

With polling occurring every 15 seconds, this table grows by more than **140,000 records per day**, providing the historical dataset used for analysis.

### Relationships

The `routes` table serves as the central reference for route information.

- `stops` contains a foreign key referencing `routes`.
- `bus_snapshots` contains a foreign key referencing `routes`.
- `stops` and `bus_snapshots` do not have a direct foreign-key relationship with each other.

This structure allows shuttle observations and stop definitions to be independently associated with their respective routes while keeping route information normalized.

---

# Travel-Time Calculation

One of the core pieces of the application is determining how long a shuttle took to travel between two selected stops.

When a user selects a starting and ending stop:

### 1. Determine the stop boundaries

Each stop has a position and a configurable geographic range stored in the `stops` table.

The application uses these values to determine when a recorded bus position falls within a stop's range.

### 2. Query historical shuttle positions

SQL retrieves the relevant `bus_snapshots` data for the selected route and stop ranges.

### 3. Identify the start

The first recorded instance of a bus entering the starting stop is treated as the beginning of the trip.

### 4. Identify the end

The first recorded instance of that same trip's bus entering the ending stop is treated as the end of the trip.

### 5. Calculate travel time

The application calculates:

```text
Travel Time = Ending Timestamp - Starting Timestamp
```

### 6. Graph the result

The calculated travel time is associated with the starting timestamp and returned through the REST API.

The frontend then uses Chart.js to display the historical travel-time data as a line graph.

---

# Data Pipeline

```text
TransLoc API
     │
     │ Every 15 seconds
     ▼
Background Service
     │
     │ Shuttle positions
     ▼
PostgreSQL
     │
     │ SQL queries
     ▼
Travel-Time Analysis
     │
     │ DTOs
     ▼
ASP.NET REST API
     │
     │ JSON
     ▼
JavaScript Frontend
     │
     ▼
Chart.js Visualization
```

---

# Engineering Highlights

## High-Frequency Data Collection

The application polls the external TransLoc API every **15 seconds**, resulting in more than **140,000 shuttle position records being collected per day**.

This required designing the database and data-processing layer around a continuously growing dataset rather than a small static collection of records.

## Relational Database Design

The PostgreSQL schema was designed specifically for this application.

Routes, stops, and shuttle observations are separated into their own tables and connected using foreign keys. This provides a structured way to relate historical vehicle data to the routes and stops those vehicles serve.

## SQL-Based Data Analysis

SQL was learned and developed specifically as part of this project.

The project's data manipulation and analysis is performed extensively through SQL queries, including filtering historical observations, relating shuttle data to routes and stops, identifying stop-entry events, and calculating trip information.

Writing the queries directly also made SQL a useful way to experiment with and test different approaches to analyzing the growing dataset.

## Background Processing

A .NET background service continuously collects new data independently of incoming web requests.

This allows the application to function as an ongoing data collection system while users interact with the analytics portion of the application.

## Separation of Application Layers

The application separates database access, application models, API endpoints, and frontend functionality.

Dapper is used with Npgsql for database access, allowing the application to use parameterized SQL queries while mapping query results into C# objects.

DTO models are used to define the data exposed through the REST API rather than directly exposing database structures.

## Containerized Deployment

The application uses a **multi-stage Docker build**.

The first stage uses the .NET SDK to compile the application. The resulting binaries are then transferred into a separate runtime stage containing only what is needed to run the application.

This produces a smaller deployment image and separates the build environment from the production runtime.

## Automated Deployment

The application is hosted on Railway and connected to the project's GitHub repository.

Whenever changes are pushed to GitHub, Railway automatically rebuilds and redeploys the application, keeping the live deployment synchronized with the latest code.

---

# Current Status

The application currently supports **17 shuttle routes**, with stop data being added as the project continues to develop.

The historical visualization currently combines data collected across multiple days. As more data is collected, the graph will provide increasingly complete coverage of shuttle travel times throughout the day.

### Planned Features

- More complete stop coverage across all routes
- Day-specific historical filtering
- Viewing detailed data through each graph point
- Additional filtering and comparison capabilities

Day-specific filtering will use the timestamps stored with the shuttle observations to retrieve and analyze data for a requested date.

---

# Development Environment

The PostgreSQL database is hosted through Railway for the deployed application.

**pgAdmin** is also used during development to inspect the database, test queries, and analyze the collected shuttle data.

The application is developed using C#/.NET on the backend and JavaScript, HTML, and CSS on the frontend.

---

# Project Goals

The goal of the UCF Bus Tracker is to explore how continuously collected real-world transit data can be transformed into useful historical analytics.

The project combines:

- External REST API integration
- High-frequency background data collection
- Relational database design
- PostgreSQL and SQL data analysis
- C# backend development
- REST API development
- DTO-based data transfer
- JavaScript frontend development
- Data visualization
- Docker containerization
- Automated cloud deployment

Rather than relying on a pre-existing dataset, the application continuously builds its own dataset from live shuttle information and uses that data to answer practical questions about shuttle travel times.

---

# Future Development

As the dataset continues to grow, the project will expand from basic travel-time visualization into a broader shuttle analytics platform.

Potential future analysis includes:

- Comparing individual days
- Identifying recurring delays
- Comparing routes and route segments
- Analyzing historical travel-time distributions
