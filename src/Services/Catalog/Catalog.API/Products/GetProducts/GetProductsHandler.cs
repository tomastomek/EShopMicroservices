namespace Catalog.API.Products.GetProducts
{
    public record GetProductsQuery() : IQuery<GetProductsResult>;
    public record GetProductsResult(IEnumerable<Product> Products);

    // ILogger parameter is name we want to appear in logs for this item
    internal class GetProductsQueryHandler
        (IDocumentSession session, ILogger<GetProductsQueryHandler> logger)
        : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            // @Query - tells logging framework to serialize query object using ToString(), but also
            // include additional metadata about the object such as its property values
            logger.LogInformation("GetProductsQueryHandler.Handle called with {@Query}", query);
            // Query returns IQueryable<T> object
            var products = await session.Query<Product>().ToListAsync(cancellationToken);

            return new GetProductsResult(products);
        }
    }
}
