using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebAppMinimalPartsUnlimit.Data;
using WebAppMinimalPartsUnlimit.Data.Models;

namespace WebAppMinimalPartsUnlimit.Api
{
    internal static class ProductApi
    {
        public static RouteGroupBuilder MapProductApi(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/erp")
                .WithTags("Product Api");

            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };

            group.MapGet("/products", async (PartsBdContext db) =>
                await db.Products.ToListAsync()
                    is IList<Product> products
                        ? Results.Json(products, options)
                        : Results.NotFound())
                .WithOpenApi();

            return group;
        }
    }
}
