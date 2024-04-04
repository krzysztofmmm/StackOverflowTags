using StackOverflowTags.Services;

namespace StackOverflowTags.Endpoints
{
    public static class StackOverflowTagsEndpoints
    {
        public static void ConfigureStackOverflowTagsEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/tags" , GetTags);
            endpoints.MapPost("/tags/refresh" , RefreshTags);
        }

        public static async Task<IResult> GetTags(IStackOverflowService service , HttpContext context)
        {
            string pageQuery = context.Request.Query["page"];
            string pageSizeQuery = context.Request.Query["pageSize"];
            string sortQuery = context.Request.Query["sort"];
            int.TryParse(pageQuery , out var page);
            int.TryParse(pageSizeQuery , out var pageSize);

            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var tags = await service.FetchAndStoreTagsAsync(page , pageSize);

            if(sortQuery == "name")
            {
                tags = tags.OrderBy(t => t.Name);
            }
            else if(sortQuery == "percentage")
            {
                tags = tags.OrderBy(t => t.Percentage);
            }

            return Results.Ok(tags);
        }

        public static async Task<IResult> RefreshTags(IStackOverflowService service)
        {
            await service.RefreshTagsAsync();
            return Results.Ok("Tags refreshed successfully.");
        }
    }
}
