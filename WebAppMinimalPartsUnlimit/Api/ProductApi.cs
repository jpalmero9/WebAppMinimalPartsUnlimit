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


            // TODO: Mover a config
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                //PropertyNameCaseInsensitive = false,
                //PropertyNamingPolicy = null,
                WriteIndented = true,
                //IncludeFields = false,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                //ReferenceHandler = ReferenceHandler.Preserve
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
