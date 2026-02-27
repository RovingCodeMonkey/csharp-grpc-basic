# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

All commands run from the `WebApplication1/` directory unless otherwise noted.

```bash
# Build
dotnet build

# Run (HTTP on port 5153, HTTPS on port 7247)
dotnet run

# Restore packages
dotnet restore
```

There are no tests in this project yet.

## Architecture

This is an ASP.NET Core **minimal API** project targeting **.NET 10.0** that acts as a **gRPC client** (not a gRPC server). It uses `Grpc.Net.Client` to call an external gRPC service.

**Key design points:**
- `Program.cs` uses minimal API style (no controllers, no `Startup.cs`). The `Controllers/` and `Models/` folders exist but are currently empty.
- gRPC code generation is handled at build time by `Grpc.Tools`. The `.proto` file at [WebApplication1/Services/gRPCClient/greeting.proto](WebApplication1/Services/gRPCClient/greeting.proto) is compiled into C# stubs automatically — do not manually write the generated client classes.
- [greeting.proto](WebApplication1/Services/gRPCClient/greeting.proto) defines the `Greeter` service with two RPCs:
  - `SayHello` — unary request/response
  - `Chat` — bidirectional streaming
- [WebApplication1/Services/gRPCClient/Client.cs](WebApplication1/Services/gRPCClient/Client.cs) is where the gRPC client wrapper logic belongs (currently empty).
- The `/sendquery` HTTP endpoint in `Program.cs` is intended to be the REST-facing trigger that calls the gRPC backend.

**Solution format:** The solution uses the new `.slnx` format (`WebApplication1.slnx`), which requires .NET 10 SDK or Visual Studio 2022 17.12+.

## Proto Code Generation

When adding or modifying `.proto` files, ensure the `.csproj` includes the file via `<Protobuf Include="..." GrpcServices="Client" />`. The current proto is picked up by `Grpc.Tools` automatically if placed in the project tree. Regenerated stubs appear in `obj/` after a build.
