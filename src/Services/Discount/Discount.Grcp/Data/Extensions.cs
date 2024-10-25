using Microsoft.EntityFrameworkCore;

namespace Discount.Grcp.Data
{
    public static class Extensions
    {
        public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
        {
            // create scope that can be used to resolve scoped services
            using var scope = app.ApplicationServices.CreateScope();
            using var dbcontext = scope.ServiceProvider.GetRequiredService<DiscountContext>();
            dbcontext.Database.MigrateAsync();

            return app;
        }
    }
}
