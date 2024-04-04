namespace StackOverflowTags.Services
{
    public class TagFetchingBackgroundService : BackgroundService
    {
        private readonly IStackOverflowService _stackOverflowService;

        public TagFetchingBackgroundService(IStackOverflowService stackOverflowService)
        {
            _stackOverflowService = stackOverflowService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Logic to fetch tags at startup
            await _stackOverflowService.FetchAndStoreTagsAsync();
        }
    }
}