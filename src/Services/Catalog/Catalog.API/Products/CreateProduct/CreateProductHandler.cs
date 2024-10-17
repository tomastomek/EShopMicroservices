namespace Catalog.API.Products.CreateProduct
{
    // comes from model
    // represents the data we need to create a product
    public record CreateProductCommand(
        string Name,
        List<string> Category,
        string Description,
        string ImageFile,
        decimal Price) : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);

    // using primary constructor inject IDocumentSession
    internal class CreateProductCommandHandler(IDocumentSession session)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        // command triggers command handler
        public async Task<CreateProductResult> Handle
            (CreateProductCommand command, CancellationToken cancellationToken)
        {
            // Create product entity from command object
            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price
            };

            // TODO save to database
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            // return CreateProductResult result
            // id will be generated from document database
            return new CreateProductResult(product.Id);
        }
    }
}

