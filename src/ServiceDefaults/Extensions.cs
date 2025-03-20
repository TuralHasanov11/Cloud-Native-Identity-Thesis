using System.IO.Compression;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Compliance.Classification;
using Microsoft.Extensions.Compliance.Redaction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ServiceDefaults.Middleware;

namespace ServiceDefaults;

public static partial class Extensions
{
    public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
    {
        builder.AddBasicServiceDefaults();

        builder.Services.AddServiceDiscovery();

        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.SerializerOptions.WriteIndented = false;
            options.SerializerOptions.Encoder = JavaScriptEncoder.Default;
            options.SerializerOptions.AllowTrailingCommas = true;
            options.SerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
        });

        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            http.AddStandardResilienceHandler();

            http.AddServiceDiscovery();
        });

        return builder;
    }

    public static IHostApplicationBuilder AddBasicServiceDefaults(this IHostApplicationBuilder builder)
    {
        builder.ConfigureHost();

        builder.AddDefaultHealthChecks();

        builder.ConfigureDiagnostics();

        builder.AddDefaultErrorHandler();

        builder.AddDefaultCompression();

        builder.AddDefaultResiliency();

        return builder;
    }

    public static IHostApplicationBuilder ConfigureDiagnostics(this IHostApplicationBuilder builder)
    {
        builder.Logging.EnableEnrichment();
        builder.Logging.EnableRedaction();

        builder.Services.AddSerilog((services, options) =>
        {
            options.ReadFrom.Configuration(builder.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext();
        });

        builder.Services.AddServiceLogEnricher(options =>
        {
            options.ApplicationName = true;
            options.BuildVersion = true;
            options.DeploymentRing = true;
            options.EnvironmentName = true;
        });

        builder.Services.AddStaticLogEnricher<MachineNameEnricher>();

        builder.Services.AddRedaction(options =>
        {
            options.SetRedactor<ErasingRedactor>(new DataClassificationSet(ApplicationLoggingTaxonomy.SensitiveData));

            options.SetRedactor<SecretRedactor>(new DataClassificationSet(ApplicationLoggingTaxonomy.PersonalData));
        });

        builder.Services.AddScoped<RequestContextMiddleware>();
        builder.Services.AddScoped<RequestTimeMiddleware>();

        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        builder.AddOpenTelemetryExporters();

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource =>
            {
                resource.AddService(
                        builder.Environment.ApplicationName,
                        serviceInstanceId: Environment.MachineName);

                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                if (environment is not null)
                {
                    resource.AddAttributes(new Dictionary<string, object>
                    {
                        ["environment.name"] = environment,
                    });
                }
            })
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddMeter(
                        //DiagnosticsConfiguration.Meter.Name,
                        //MassTransit.Monitoring.InstrumentationOptions.MeterName, // MassTransit Meter
                        "Microsoft.AspNetCore.Hosting",
                        "System.Net.Http",
                        "Microsoft.AspNetCore.Server.Kestrel",
                        "Experimental.Microsoft.Extensions.AI",
                        builder.Environment.ApplicationName);
            })
            .WithTracing(tracing =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    tracing.SetSampler(new AlwaysOnSampler());
                }

                tracing.AddAspNetCoreInstrumentation()
                    .AddGrpcClientInstrumentation()
                    .AddHttpClientInstrumentation()
                    //.AddSource(DiagnosticsConfiguration.Source.Name)
                    //.AddSource(MassTransit.Logging.DiagnosticHeaders.DefaultListenerName) // MassTransit ActivitySource
                    .AddSource("Experimental.Microsoft.Extensions.AI");
            });

        return builder;
    }

    private static IHostApplicationBuilder AddOpenTelemetryExporters(this IHostApplicationBuilder builder)
    {
        var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

        if (useOtlpExporter)
        {
            builder.Services.Configure<OpenTelemetryLoggerOptions>(logging => logging.AddOtlpExporter());
            builder.Services.ConfigureOpenTelemetryMeterProvider(metrics => metrics.AddOtlpExporter());
            builder.Services.ConfigureOpenTelemetryTracerProvider(tracing => tracing.AddOtlpExporter());
        }

        return builder;
    }

    public static IHostApplicationBuilder AddDefaultHealthChecks(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

        return builder;
    }

    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        app.UseMiddleware<RequestContextMiddleware>();
        // Uncomment the following line to enable the Prometheus endpoint (requires the OpenTelemetry.Exporter.Prometheus.AspNetCore package)
        // app.MapPrometheusScrapingEndpoint();

        if (app.Environment.IsDevelopment())
        {
            app.MapHealthChecks("/health");

            app.MapHealthChecks("/live", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("live"),
            });

            return app;
        }

        app.UseMiddleware<RequestTimeMiddleware>();

        return app;
    }

    private static void AddDefaultErrorHandler(this IHostApplicationBuilder builder)
    {
        builder.Services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

                var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
            };
        });

        builder.Services.AddExceptionHandler<ProblemExceptionHandler>();
    }

    private static void ConfigureHost(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHsts(options =>
        {
            options.Preload = true;
            options.IncludeSubDomains = true;
        });

        builder.Services.Configure<HostOptions>(builder.Configuration.GetSection("Host"));

        builder.Services.AddScoped<ContentTypeOptionsMiddleware>();
    }

    private static void AddDefaultCompression(this IHostApplicationBuilder builder)
    {
        builder.Services.AddRequestDecompression();

        builder.Services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
        });

        builder.Services.Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

        builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.SmallestSize);
    }

    private static void AddDefaultResiliency(this IHostApplicationBuilder builder)
    {
        builder.Services.AddLoadShedding((_, options) =>
        {
            options.SubscribeEvents(events =>
            {
                events.ItemEnqueued.Subscribe(LoadShedding.SubscribeToItemEnqueued);
                events.ItemDequeued.Subscribe(LoadShedding.SubscribeToItemDequeued);
                events.ItemProcessing.Subscribe(LoadShedding.SubscribeToItemProcessing);
                events.ItemProcessed.Subscribe(LoadShedding.SubscribeToItemProcessed);
                events.Rejected.Subscribe(LoadShedding.SubscribeToRejected);
            });
        });

        builder.Services.AddRequestTimeouts(options =>
        {
            options.DefaultPolicy = new RequestTimeoutPolicy
            {
                Timeout = TimeSpan.FromMilliseconds(2000),
                TimeoutStatusCode = 503,
            };
        });

        builder.Services.AddRateLimiter(limiterOptions =>
        {
            limiterOptions.AddFixedWindowLimiter(
                policyName: "FixedRateLimitingPolicy",
                options =>
                {
                    options.PermitLimit = 4;
                    options.Window = TimeSpan.FromSeconds(12);
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = 2;
                });
        });
    }
}
