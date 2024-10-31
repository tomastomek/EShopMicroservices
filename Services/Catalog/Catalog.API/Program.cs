using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config =>
{
    // tells the mediator where to find command and query handle classes
    // same situatian as Carter, it is installed in BuildingBlocks but used in Catalog.API
    config.RegisterServicesFromAssemblies(assembly);
    // register validation behavior, for generic we use <,>
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    // register logging behavior
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);

// Carter scans ICarterModule interface and exposes the minimal APIs
// carter by default loads assemblies of the project only if they depend directly on carter assembly
// we have carter in building blocks and not in Catalog.API, we need to load it manually, so that it
// will be searched for ICarterModule
builder.Services.AddCarter(new DependencyContextAssemblyCatalog(assemblies: assembly));

builder.Services.AddMarten(opt =>
{
    opt.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialData>();
}

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapCarter();

app.UseExceptionHandler(options => { });

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
