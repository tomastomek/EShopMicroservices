var builder = WebApplication.CreateBuilder(args);

// Add services to the container

// Carter scans ICarterModule interface and exposes the minimal APIs
// carter by default loads assemblies of the project only if they depend directly on carter assembly
// we have carter in building blocks and not in Catalog.API, we need to load it manually, so that it
// will be searched for ICarterModule
builder.Services.AddCarter(new DependencyContextAssemblyCatalog(assemblies: typeof(Program).Assembly));
builder.Services.AddMediatR(config =>
{
    // tells the mediator where to find command and query handle classes
    // same situatian as Carter, it is installed in BuildingBlocks but used in Catalog.API
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

builder.Services.AddMarten(opt =>
{
    opt.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapCarter();

app.Run();
