﻿namespace Basket.API.Basket.GetBasket
{
    // we will get username from request parameter
    // public record GetBasketRequest(string UserName)
    public record GetBasketResponse(ShoppingCart Cart);

    public class GetBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/basket/{userName}", async (string userName, ISender sender) =>
            {
                var result = await sender.Send(new GetBasketQuery(userName));

                var response = result.Adapt<GetBasketResponse>();

                return Results.Ok(response);
            })
            .WithName("GetBasket")
            .Produces<GetBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Basket by UserName")
            .WithDescription("Get Basket by UserName");
        }
    }
}