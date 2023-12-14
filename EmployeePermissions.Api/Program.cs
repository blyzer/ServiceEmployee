using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.AspNetCore.Http;
using EmployeePermissions.Application.Commands;
using EmployeePermissions.Application.Queries;
using EmployeePermissions.Domain;
using EmployeePermissions.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Nest;
using Prometheus;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
var builder = WebApplication.CreateSlimBuilder(args);
builder.Host.UseSerilog();

builder.Services.AddDbContext<EmployeeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeePermissionsDatabase")));

builder.Services.AddScoped(typeof(EmployeePermissions.Infrastructure.IRepository<Employee>), typeof(Repository<Employee>));
builder.Services.AddScoped(typeof(EmployeePermissions.Infrastructure.IRepository<EmployeePermission>), typeof(Repository<EmployeePermission>));
builder.Services.AddScoped(typeof(EmployeePermissions.Infrastructure.IRepository<PermissionType>), typeof(Repository<PermissionType>));
builder.Services.AddScoped(typeof(EmployeePermissions.Infrastructure.IRepository<Permission>), typeof(Repository<Permission>));
// Repeat for other repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// Register MediatR services
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
// Create an instance of the Elasticsearch client
var settings = new ConnectionSettings(new Uri(builder.Configuration["ElasticsearchSettings:Uri"]))
    .DefaultIndex("permissions");

var elasticClient = new ElasticClient(settings);
builder.Services.AddSingleton<IElasticClient>(elasticClient);
// Create Elasticsearch index before app starts
CreateIndex(elasticClient);

var kafkaBootstrapServers = builder.Configuration["Kafka:BootstrapServers"];
var kafkaTopicName = builder.Configuration["Kafka:TopicName"];
builder.Services.AddSingleton(new KafkaProducerService(kafkaBootstrapServers, kafkaTopicName));


builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy());

var app = builder.Build();

app.UseHealthChecks("/health");
app.UseMetricServer();
app.MapPost("/permissions", async (CreatePermissionCommand command, IMediator mediator) =>
{
    var permissionId = await mediator.Send(command);
    if (permissionId == Guid.Empty)
    {
        return Results.Problem("Unable to create permission."); // Or your preferred error response
    }
    // Assuming you want to return the ID of the created permission
    return Results.Created($"/permissions/{permissionId}", new { Id = permissionId });
});

// Endpoint to get a permission by ID
// app.MapGet("/permissions/{id}", async (Guid id, IMediator mediator) =>
// {
//     var query = new GetPermissionByIdQuery(id);
//     var permission = await mediator.Send(query);
//     return permission is not null ? Results.Ok(permission) : Results.NotFound();
// })
// The tracking endpoint
app.MapPost("/trackActivity", (string activity) => 
{
    Log.Information($"User performed: {activity}");
    // Other business logic
    return Results.Ok($"Tracked: {activity}");
});

app.Run();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

static void CreateIndex(IElasticClient elasticClient)
{
    var createIndexResponse = elasticClient.Indices.Create("permissions", c => c
        .Map<PermissionDocument>(m => m
                .AutoMap()
            // Further configuration such as custom analyzers, mappings, etc.
        )
    );

    if (!createIndexResponse.IsValid)
    {
        // Handle the error of index creation...
    }
}