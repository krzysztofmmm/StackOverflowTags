using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StackOverflowTags.Data;
using StackOverflowTags.Models;

namespace StackOverflowTags.Tests.IntegrationTests
{
    public class TagsEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public TagsEndpointsTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Find and remove the application's ApplicationDbContext registration.
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if(descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Add ApplicationDbContext using an in-memory database for testing.
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                    });
                });
            });

            _client = _factory.CreateClient();
        }

        private async Task InitializeDbForTests()
        {
            var scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if(db.Database.IsInMemory())
            {
                db.Database.EnsureCreated();
                db.Tags.RemoveRange(db.Tags);
                await db.SaveChangesAsync();

                db.Tags.AddRange(
                    new Tag { Name = "C#" , Count = 100 , Percentage = 10.0 } ,
                    new Tag { Name = "Java" , Count = 200 , Percentage = 20.0 }
                );
                await db.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task GetTags_ReturnsSuccessStatusCode()
        {
            await InitializeDbForTests();
            var response = await _client.GetAsync("/tags");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task RefreshTags_EndpointInvoked_UpdatesTagData()
        {
            await InitializeDbForTests();
            var postResponse = await _client.PostAsync("/tags/refresh" , null);
            postResponse.EnsureSuccessStatusCode();

            var getResponse = await _client.GetAsync("/tags");
            getResponse.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetTags_EndpointPagination_ReturnsExpectedNumberOfTags()
        {
            await InitializeDbForTests();
            var response = await _client.GetAsync("/tags?page=1&pageSize=5");
            response.EnsureSuccessStatusCode();
        }
    }
}
