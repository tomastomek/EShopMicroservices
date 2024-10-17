namespace Catalog.API.Products.CreateProduct;

// captures all the details needed to create a product
public record CreateProductRequest(
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price);

public record CreateProductResponse(Guid Id);

public class CreateProductEndpoint : ICarterModule
{
    // here we define endpoints
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
        {
            // convert request to command object
            // mediator needs command object to trigger handler
            var command = request.Adapt<CreateProductCommand>();

            // send command object using mediator (sender)
            var result = await sender.Send(command);

            // after we get result from handler, we convert it to response
            var response = result.Adapt<CreateProductResponse>();

            return Results.Created($"/products/{response.Id}", response);
        })
        .WithName("CreateProduct") // name of HTTP post method
        .Produces<CreateProductResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest) // if there's problem return this
        .WithSummary("Create Product") // summary of HTTP post method
        .WithDescription("Create Product"); // description of HTTP post method
    }
}
