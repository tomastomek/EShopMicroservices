namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryRequest(string Category) : IQuery<GetProductByCategoryResult>;
    public record GetProductByCategoryResult(IEnumerable<Product> Products);

    internal class GetProductByCategoryQueryHandler
        (IDocumentSession session)
        : IQueryHandler<GetProductByCategoryRequest, GetProductByCategoryResult>
    {
        public async Task<GetProductByCategoryResult> Handle
            (GetProductByCategoryRequest query, CancellationToken cancellationToken)
        {

            var products = await session.Query<Product>()
                .Where(p => p.Category.Contains(query.Category))
                .ToListAsync(cancellationToken);

            return new GetProductByCategoryResult(products);
        }
    }
}