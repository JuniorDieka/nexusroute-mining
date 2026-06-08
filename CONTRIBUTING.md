# Contributing to NexusRoute Mining

Thank you for your interest in contributing to NexusRoute Mining! This document provides guidelines for contributing to the project.

## Code of Conduct

- Be respectful and inclusive
- Focus on constructive feedback
- Help others learn and grow

## Getting Started

1. Fork the repository
2. Clone your fork: `git clone https://github.com/yourusername/nexusroute-mining.git`
3. Create a feature branch: `git checkout -b feature/your-feature-name`
4. Make your changes
5. Run tests: `dotnet test`
6. Commit your changes: `git commit -m "Add feature: your feature description"`
7. Push to your fork: `git push origin feature/your-feature-name`
8. Open a Pull Request

## Development Setup

### Prerequisites

- .NET 10 SDK
- SQL Server (or Docker)
- Visual Studio 2026 or VS Code with C# Dev Kit

### Local Development

```bash
# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run tests
dotnet test

# Run the API
cd src/NexusRoute.Api
dotnet run
```

### Docker Development

```bash
# Start all services
docker-compose up -d

# View logs
docker-compose logs -f api

# Stop services
docker-compose down
```

## Coding Standards

- Follow the `.editorconfig` settings
- Use meaningful variable and method names
- Write XML documentation for public APIs
- Keep methods focused and concise
- Write unit tests for business logic
- Use async/await for I/O operations

## Testing

- Write unit tests for domain logic
- Write integration tests for API endpoints
- Aim for meaningful test coverage
- Use descriptive test names: `MethodName_Scenario_ExpectedResult`

## Pull Request Process

1. Ensure all tests pass
2. Update documentation if needed
3. Add a clear description of changes
4. Reference any related issues
5. Wait for code review
6. Address review feedback

## Project Structure

- `Domain/` - Core business logic and entities
- `Application/` - Use cases and DTOs
- `Infrastructure/` - Data access and external services
- `Api/` - REST API and SignalR hubs
- `Simulator/` - Background services for demo mode
- `Tests/` - Unit and integration tests

## Questions?

Open an issue for questions or discussions about the project.

## License

By contributing, you agree that your contributions will be licensed under the MIT License.
