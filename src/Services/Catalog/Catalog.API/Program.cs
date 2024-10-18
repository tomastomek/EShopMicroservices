using BuildingBlocks.Behaviors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

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

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapCarter();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        // first we get the exception
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (exception == null)
        {
            return;
        }

        // used for formatting the error response
        var problemDetails = new ProblemDetails
        {
            Title = exception.Message,
            Status = StatusCodes.Status500InternalServerError,
            Detail = exception.StackTrace
        };

        // we log the exception
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(exception, exception.Message);

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";

        // then write all the details into response as json
        await context.Response.WriteAsJsonAsync(problemDetails);
    });
});

app.Run();
