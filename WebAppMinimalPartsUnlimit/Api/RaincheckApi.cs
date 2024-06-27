using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebAppMinimalPartsUnlimit.Data;
using WebAppMinimalPartsUnlimit.Data.Dtos;
using WebAppMinimalPartsUnlimit.Data.Models;

namespace WebAppMinimalPartsUnlimit.Api
{
    internal static class RaincheckApi
    {
        public static RouteGroupBuilder MapRaincheckApi(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/erp")
                .WithTags("Raincheck Api");
            
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                WriteIndented = true,
                MaxDepth = 0,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
            };

            group.MapGet("/rainchecks", async Task<Results<Ok<IList<Raincheck>>, NotFound>> (PartsBdContext db) =>
                await db.Rainchecks
                    .Include(s => s.Product)
                    .Include(s => s.Store)
                    .ToListAsync()
                        is IList<Raincheck> rainchecks
                            ? TypedResults.Ok(rainchecks)
                            : TypedResults.NotFound())
            .WithOpenApi();

            group.MapGet("/rainchecksb", async (PartsBdContext db, int pageSize = 10, int page = 0) =>
            {
                var data = await db.Rainchecks
                    .OrderBy(s => s.RaincheckId)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .Include(s => s.Product)
                        .ThenInclude(s => s.Category)
                    .Include(s => s.Store)
                    .Select(r => new { r.StoreId, r.Name, r.Product, r.Store })
                    .ToListAsync();

                return data.Any()
                    ? Results.Json(data, options)
                    : Results.NotFound();
            })
            .WithOpenApi();

            group.MapGet("/rainchecksc", async (PartsBdContext db, int pageSize = 10, int page = 0) =>
            {
                var data = await db.Rainchecks
                    .OrderBy(s => s.RaincheckId)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .Include(s => s.Product)
                    .Include(s => s.Product.Category)
                    .Include(s => s.Store)
                    .Select(x => new
                    {
                        Name = x.Name,
                        Count = x.Count,
                        SalePrice = x.SalePrice,
                        Store = new
                        {
                            Name = x.Store.Name,
                        },
                        Product = new
                        {
                            Name = x.Product.Title,
                            Category = new
                            {
                                Name = x.Product.Category.Name
                            }
                        }
                    })
                    .ToListAsync();

                return data.Any()
                    ? Results.Json(data, options)
                    : Results.NotFound();
            })
            .WithOpenApi();

            group.MapGet("/rainchecksd", async Task<Results<Ok<List<RaincheckDto>>, NotFound>> (PartsBdContext db, int pageSize = 10, int page = 0) =>
            {
                var data = await db.Rainchecks
                    .OrderBy(s => s.RaincheckId)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .Include(s => s.Product)
                    .Include(s => s.Product.Category)
                    .Include(s => s.Store)
                    .Select(x => new RaincheckDto
                    {
                        Name = x.Name,
                        Count = x.Count,
                        SalePrice = x.SalePrice,
                        Store = new StoreDto
                        {
                            Name = x.Store.Name
                        },
                        Product = new ProductDto
                        {
                            Name = x.Product.Title,
                            Category = new CategoryDto
                            {
                                Name = x.Product.Category.Name
                            }
                        }
                    })
                    .ToListAsync();

                return data.Any()
                    ? TypedResults.Ok(data)
                    : TypedResults.NotFound();

            })
            .WithOpenApi();

            return group;
        }
    }
}
