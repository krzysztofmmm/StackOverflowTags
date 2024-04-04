using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using StackOverflowTags.Data;
using StackOverflowTags.DTOs;
using StackOverflowTags.Models;
using System.Text.Json;

namespace StackOverflowTags.Services
{
    public class StackOverflowService : IStackOverflowService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "StackOverflowTags";
        private readonly HttpClient _httpClient;

        public StackOverflowService(ApplicationDbContext context , IHttpClientFactory clientFactory , IMemoryCache cache)
        {
            _context = context;
            _clientFactory = clientFactory;
            _cache = cache;
            _httpClient = _clientFactory.CreateClient("StackOverflowClient");
        }

        public async Task<IEnumerable<Tag>> FetchAndStoreTagsAsync(int page = 1 , int pageSize = 100)
        {
            if(!_cache.TryGetValue(CacheKey , out List<Tag> cachedTags))
            {
                cachedTags = new List<Tag>();
                bool hasMore = true;
                int pageCount = 0;

                while(hasMore && cachedTags.Count < 1000)
                {
                    var response = await _httpClient.GetAsync($"tags?order=desc&sort=popular&site=stackoverflow&pagesize={pageSize}&page={page + pageCount}");
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponseWrapper>(content , new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if(apiResponse != null && apiResponse.Items.Any())
                    {
                        var tagsToAddOrUpdate = apiResponse.Items.Select(i => new Tag
                        {
                            Name = i.Name ,
                            Count = i.Count ,
                            Percentage = 0 // Will be calculated below
                        });

                        foreach(var tag in tagsToAddOrUpdate)
                        {
                            var existingTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tag.Name);
                            if(existingTag == null)
                            {
                                await _context.Tags.AddAsync(tag);
                            }
                            else
                            {
                                existingTag.Count = tag.Count;
                            }
                        }

                        await _context.SaveChangesAsync();

                        // Recalculate percentages after all updates
                        var totalTagCount = await _context.Tags.SumAsync(t => t.Count);
                        var allTags = await _context.Tags.ToListAsync();
                        allTags.ForEach(t => t.Percentage = Math.Round((double)t.Count / totalTagCount * 100 , 2));

                        _context.Tags.UpdateRange(allTags);
                        await _context.SaveChangesAsync();

                        cachedTags.AddRange(allTags);
                        hasMore = apiResponse.HasMore;
                        pageCount++;
                    }
                    else
                    {
                        hasMore = false;
                    }
                }

                _cache.Set(CacheKey , cachedTags , TimeSpan.FromHours(1));
            }

            return cachedTags;
        }

        public async Task RefreshTagsAsync()
        {
            _cache.Remove(CacheKey);
            await FetchAndStoreTagsAsync(); // Implicitly updates the cache
        }
    }
}
