namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);

    public class StoreBasketCommandValidar : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidar()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }

    internal class StoreBasketCommandHandler(IBasketRepository repository)
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle
            (StoreBasketCommand command, CancellationToken cancellationToken)
        {
            var  storedCart = await repository.StoreBasket(command.Cart, cancellationToken);
            //TODO: update cache

            return new StoreBasketResult(command.Cart.UserName);
        }
    }
}