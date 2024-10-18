using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.GetProductByCategory
{
    // not need, we get category information from request parameter
    //public record GetProductByCategoryRequest()
    public record GetProductByCategoryResponse(IEnumerable<Product> Products);

    public class GetProductByCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}", 
                async (string category, ISender sender) =>
            {
                var result = await sender.Send(new GetProductByCategoryRequest(category));

                var response = result.Adapt<GetProductByCategoryResponse>();

                return Results.Ok(response);
            })
            .WithName("GetProductByCategory")
            .Produces<CreateProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Products By Category")
            .WithDescription("Get Products By Category");
        }
    }
}