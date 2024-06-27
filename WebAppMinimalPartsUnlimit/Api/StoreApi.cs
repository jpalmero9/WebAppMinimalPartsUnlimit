using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebAppMinimalPartsUnlimit.Data;
using WebAppMinimalPartsUnlimit.Data.Dtos;
using WebAppMinimalPartsUnlimit.Data.Models;

namespace WebAppMinimalPartsUnlimit.Api
{
    internal static class StoreApi
    {
        public static RouteGroupBuilder MapStoreApi(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/erp")
                .WithTags("Store Api");

            group.MapGet("/user", (ClaimsPrincipal user) =>
            {
                return user.Identity;

            })
            .WithOpenApi();

            group.MapGet("/store/{storeid}", async Task<Results<Ok<StoreDto>, NotFound>> (int storeid, PartsBdContext db, IMapper mapper) =>
            {
                return mapper.Map<StoreDto>(await db.Stores.FirstOrDefaultAsync(m => m.StoreId == storeid))
                    is StoreDto store
                        ? TypedResults.Ok(store)
                        : TypedResults.NotFound();
            })
            .WithOpenApi();


            group.MapGet("/storea", async Task<Results<Ok<IList<Store>>, NotFound>> (PartsBdContext db) =>
                await db.Stores.ToListAsync()
                    is IList<Store> stores
                        ? TypedResults.Ok(stores)
                        : TypedResults.NotFound())
                .WithOpenApi();


            group.MapGet("/storeb", async Task<Results<Ok<IList<Store>>, NotFound>> (PartsBdContext db, int pageSize = 10, int page = 0) =>
                await db.Stores.Skip(page * pageSize).Take(pageSize).ToListAsync()
                    is IList<Store> stores
                        ? TypedResults.Ok(stores)
                        : TypedResults.NotFound())
                .WithOpenApi();

            group.MapGet("/storec1", async Task<Results<Ok<IList<Store>>, NotFound>> (PartsBdContext db, int pageSize = 10, int page = 0) =>
                await db.Stores
                .Skip(page * pageSize)
                .Take(pageSize)
                .Select(store => new { store.StoreId, store.Name })
                .ToListAsync()
                    is IList<Store> stores
                        ? TypedResults.Ok(stores)
                        : TypedResults.NotFound())
                .WithOpenApi();

            group.MapGet("/storec2", async (PartsBdContext db, int pageSize = 10, int page = 0) =>
            {
                var data = await db.Stores
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .Include(s => s.Rainchecks)
                    .Select(store => new { store.StoreId, store.Name })
                    .ToListAsync();

                return data.Any()
                    ? Results.Ok(data)
                    : Results.NotFound();
            })
            .WithOpenApi();
                        
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

            group.MapGet("/stored", async Task<Results<Ok<IList<Store>>, NotFound>> (PartsBdContext db) =>
                await db.Stores.Include(s => s.Rainchecks).ToListAsync()
                    is IList<Store> stores
                        ? TypedResults.Ok(stores)
                        : TypedResults.NotFound())
                .WithOpenApi();

            group.MapGet("/storee", async (PartsBdContext db) =>
                await db.Stores.Include(s => s.Rainchecks).ToListAsync()
                    is IList<Store> stores
                        ? Results.Json(stores, options)
                        : Results.NotFound())
                .WithOpenApi();

            group.MapGet("/storef", async (PartsBdContext db) =>
                await db.Stores.Include(s => s.Rainchecks).ToListAsync()
                    is IList<Store> stores
                        ? Results.Json(stores, options)
                        : Results.NotFound())
                .WithOpenApi();

            return group;
        }
    }
}
