# Appendix: Technical Documentation

---

## Service Orchestration and Configuration

This section provides detailed technical documentation for the orchestration and configuration of the cloud-native microservice system, focusing on the `docker-compose.yml` file. The configuration demonstrates how multiple microservices, supporting infrastructure, and observability tools are composed for local development and reproducible experiments.

### C.1 Docker Compose Service Definitions

File: `docker-compose.yml`

```yaml
services:
  basket.api:
    image: basket-api
    container_name: basket.api
    build:
      context: .
      dockerfile: src/Basket.Api/Dockerfile
    ports:
      - "5100:5100"
      - "5101:5101"
    environment:
      - OTEL_EXPORTER_OTLP_ENDPOINT=http://seq:5341/ingest/otlp/v1/traces
      - OTEL_EXPORTER_OTLP_PROTOCOL=http/protobuf
      - OTEL_EXPORTER_OTLP_HEADERS="X-Seq-ApiKey=..."
    depends_on:
      - rabbitmq
      - redis
    networks:
      - thesis-net

  catalog.api:
    image: catalog-api
    container_name: catalog.api
    build:
      context: .
      dockerfile: src/Catalog.Api/Dockerfile
    ports:
      - "5102:5102"
      - "5103:5103"
    environment:
      - OTEL_EXPORTER_OTLP_ENDPOINT=http://seq:5341/ingest/otlp/v1/traces
      - OTEL_EXPORTER_OTLP_PROTOCOL=http/protobuf
      - OTEL_EXPORTER_OTLP_HEADERS="X-Seq-ApiKey=..."
    depends_on:
      - rabbitmq
      - catalog.postgres
    networks:
      - thesis-net

  ordering.api:
    image: ordering-api
    container_name: ordering.api
    build:
      context: .
      dockerfile: src/Ordering.Api/Dockerfile
    ports:
      - "5104:5104"
      - "5105:5105"
    environment:
      - OTEL_EXPORTER_OTLP_ENDPOINT=http://seq:5341/ingest/otlp/v1/traces
      - OTEL_EXPORTER_OTLP_PROTOCOL=http/protobuf
      - OTEL_EXPORTER_OTLP_HEADERS="X-Seq-ApiKey=..."
    depends_on:
      - rabbitmq
      - ordering.postgres
    networks:
      - thesis-net

  paymentprocessor:
    image: paymentprocessor
    container_name: paymentprocessor
    build:
      context: .
      dockerfile: src/PaymentProcessor/Dockerfile
    ports:
      - "5106:5106"
      - "5107:5107"
    environment:
      - OTEL_EXPORTER_OTLP_ENDPOINT=http://seq:5341/ingest/otlp/v1/traces
      - OTEL_EXPORTER_OTLP_PROTOCOL=http/protobuf
      - OTEL_EXPORTER_OTLP_HEADERS="X-Seq-ApiKey=..."
    depends_on:
      - rabbitmq
    networks:
      - thesis-net

  rabbitmq:
    image: rabbitmq:4.0-management
    container_name: rabbitmq
    hostname: rabbitmq
    ports:
      - "15672:15672"
      - "5672:5672"
    volumes:
      - ./.containers/queue/data:/var/lib/rabbitmq
      - ./.containers/queue/log:/var/log/rabbitmq
    environment:
      RABBITMQ_DEFAULT_USER: "guest"
      RABBITMQ_DEFAULT_PASS: "guest"
    networks:
      - thesis-net

  redis:
    image: redis:latest
    container_name: redis
    ports:
      - "6379:6379"
    volumes:
      - ./.containers/redis/data:/data
    networks:
      - thesis-net

  seq:
    image: datalust/seq:latest
    container_name: seq
    volumes:
      - ./.containers/seq:/data
    environment:
      - ACCEPT_EULA=Y
    ports:
      - "5341:5341"
      - "8081:80"
    networks:
      - thesis-net

  pgadmin:
    image: dpage/pgadmin4:latest
    container_name: pgadmin
    environment:
      - PGADMIN_DEFAULT_EMAIL=admin@postgres.com
      - PGADMIN_DEFAULT_PASSWORD=postgres
    ports:
      - "5050:80"
    volumes:
      - ./.containers/pgadmin:/var/lib/pgadmin
    networks:
      - thesis-net

networks:
  thesis-net:
    driver: bridge
```

#### Explanation

- **Microservices**: Each API (e.g., `basket.api`, `catalog.api`, `ordering.api`, `paymentprocessor`) is built from its own Dockerfile and exposes ports for internal and external communication. Environment variables configure distributed tracing via OpenTelemetry and Seq.
- **Databases**: Each domain service (e.g., catalog, ordering) uses a dedicated PostgreSQL instance (not shown in full above for brevity), with persistent volumes for data durability.
- **Message Broker**: RabbitMQ is used for asynchronous communication and event-driven integration between services. Management UI is exposed on port 15672.
- **Cache**: Redis provides distributed caching for performance and resilience.
- **Observability**: Seq aggregates logs and traces from all services. OpenTelemetry exporters are configured in each service for distributed tracing.
- **Admin Tools**: pgAdmin is included for database management and inspection.
- **Networking**: All services are attached to a custom bridge network (`thesis-net`) to enable service discovery and isolation.

### C.2 API Endpoint Registration

File: `src/WebApp/WebApp.Server/Features/Endpoints.cs`

```csharp
// ...existing code...
public static void MapApiEndpoints(this IEndpointRouteBuilder endpoints)
{
    endpoints.MapGet("/api/orders", ...); // Retrieves orders
    endpoints.MapPost("/api/orders", ...); // Creates a new order
    // ...other endpoints for catalog, basket, etc.
}
// ...existing code...
```

#### Explanation

The API endpoint registration pattern centralizes the definition of HTTP endpoints for each microservice. This approach improves maintainability and discoverability of the service contract, and supports automated API documentation and testing.

---

# Notes

- This appendix provides only technical documentation relevant to system orchestration and API surface.
- Listings are excerpted for clarity; see the repository for full implementations and additional configuration.
- Secrets and sensitive values are omitted for security.
