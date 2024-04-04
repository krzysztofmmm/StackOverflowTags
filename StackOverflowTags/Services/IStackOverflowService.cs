using StackOverflowTags.Models;

namespace StackOverflowTags.Services
{
    public interface IStackOverflowService
    {
        Task<IEnumerable<Tag>> FetchAndStoreTagsAsync(int page = 1 , int pageSize = 100);
        Task RefreshTagsAsync();
    }
}
