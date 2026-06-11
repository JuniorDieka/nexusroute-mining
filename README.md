# NexusRoute Mining

**Real-Time Pit-to-Plant Fleet Dispatcher for Mining Operations**

[![CI](https://github.com/JuniorDieka/nexusroute-mining/workflows/CI/badge.svg)](https://github.com/JuniorDieka/nexusroute-mining/actions)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?logo=docker&logoColor=white)](https://www.docker.com/)
[![SignalR](https://img.shields.io/badge/SignalR-Real--time-68217A)](https://dotnet.microsoft.com/apps/aspnet/signalr)

> **Note**: This project is inspired by the author's professional experience in open-pit gold mining operations in eastern DRC. The software, simulated data, and operational scenarios are created for portfolio demonstration purposes and do not represent actual operational systems, proprietary data, or confidential information from any former employer.

## 🎯 Overview

Real-time fleet dispatch and telemetry platform for open-pit mining operations. Demonstrates enterprise .NET architecture with SignalR real-time streaming, domain-driven design, and production-ready patterns.

---

## 📸 Visual Journey

Experience the application through these screenshots showcasing real-time fleet management at Namoya Mine with 38 active assets across 4 open pits.

### Dashboard Overview
Real-time metrics showing **38 active assets**, **5,850 tonnes** produced today, and **77% weekly target** achievement.

![Dashboard Overview](docs/screenshots/01-dashboard-overview.png)

---

### Fleet Status
Complete fleet monitoring across **4 Namoya pits**: Mt. Mwendamboko, Muviringu, Kakula, and Namoya Summit. Track excavators, haul trucks, drill rigs, and support equipment in real-time.

![Fleet Status](docs/screenshots/02-fleet-status.png)

---

### Production Summary
Daily production metrics: **5,850t total tonnage**, **1.88 g/t gold grade**, and **77% target achievement** tracking.

![Production Summary](docs/screenshots/03-production-summary.png)

---

### Alerts System
Active monitoring with **0 critical** and **0 warning** alerts. System operating normally with comprehensive alert management.

![Alerts System](docs/screenshots/04-alerts.png)

---

### Convoy Management
Secure convoy tracking for high-value cargo transport with route monitoring and checkpoint compliance.

![Convoy Management](docs/screenshots/05-convoys.png)

---

## Key Features

- **Real-Time Telemetry** - Equipment health monitoring (temp, pressure, payload, fuel)
- **Cycle Optimization** - Automated cycle time calculation and bottleneck detection
- **Convoy Tracking** - Geofence monitoring for high-value cargo
- **Predictive Alerts** - Threshold-based warnings before failures
- **Production Tracking** - Real-time tonnage and grade control
- **Built-in Simulator** - Demo mode with realistic synthetic data

## 🏗️ Architecture

**Clean Architecture** with clear separation of concerns:
- **Domain** - Entities, value objects, domain services
- **Application** - Use cases, DTOs, validators
- **Infrastructure** - EF Core, repositories, data access
- **API** - REST controllers, SignalR hub, JWT auth
- **Simulator** - Background services for demo data

## 🚀 Quick Start

```bash
# Clone and run with Docker
git clone https://github.com/JuniorDieka/nexusroute-mining.git
cd nexusroute-mining
docker-compose up -d

# Open http://localhost:5000 (wait ~30s for startup)
```

## 🛠️ Local Development

**Prerequisites**: .NET 10 SDK, SQL Server (or Docker)

```bash
dotnet restore
cd src/NexusRoute.Api
dotnet ef database update
dotnet run
# Open http://localhost:5000
```

**Configuration** (`appsettings.json`):
- `ConnectionStrings:DefaultConnection` - Database connection
- `Jwt:SecretKey` - JWT signing key (use User Secrets)
- `Simulator:Enabled` - Enable demo data generator

## 🧪 Testing

```bash
dotnet test
```

Covers domain logic, repositories, API endpoints, and SignalR hubs.

## 📡 API

**Swagger**: `http://localhost:5000/swagger`

**REST Endpoints**: `/api/telemetry`, `/api/assets`, `/api/convoys`, `/api/production`, `/api/alerts`

**SignalR Hub**: `/hubs/dispatch` - Real-time telemetry, alerts, convoy updates

## 🎮 Simulator

Generates realistic telemetry: normal operations, alert triggers (5% chance), GPS drift, convoy monitoring.

Toggle in `appsettings.json`: `"Simulator": { "Enabled": true }`

## 🏭 Technical Highlights

- **Real-Time**: SignalR bi-directional communication, event-driven architecture
- **Clean Architecture**: DDD with rich entities, separation of concerns, DI
- **Domain Logic**: Haversine geofencing, cycle time computation, bottleneck detection
- **Production-Ready**: Serilog, health checks, JWT auth, EF Core migrations, async/await
- **Testing**: Unit + integration tests, Testcontainers, SignalR hub testing
- **DevOps**: Docker multi-stage builds, GitHub Actions CI

## 📊 Tech Stack

**.NET 10** • **ASP.NET Core** • **SignalR** • **EF Core** • **SQL Server** • **FluentValidation** • **Serilog** • **xUnit** • **Docker** • **GitHub Actions**

## 📁 Structure

```
src/
├── NexusRoute.Domain/         # Entities, value objects, domain services
├── NexusRoute.Application/    # Use cases, DTOs, validators
├── NexusRoute.Infrastructure/ # EF Core, repositories, data seeding
├── NexusRoute.Api/            # Controllers, SignalR hub, JWT auth
├── NexusRoute.Simulator/      # Background telemetry generator
└── NexusRoute.Tests/          # Unit & integration tests
```

## 🔐 Security

- JWT Bearer authentication with role-based authorization
- Roles: Dispatcher (full access), Maintenance (health monitoring), Operator (telemetry)
- EF Core parameterized queries, HTTPS enforced, User Secrets for dev

## 📄 License

MIT License - see [LICENSE](LICENSE)

---

Built with ❤️ for Mining real-time fleet dispatch excellence
