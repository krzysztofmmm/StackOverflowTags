public static class ServiceExtensions
{
    public static IServiceCollection AddStackOverflowClient(this IServiceCollection services)
    {
        services.AddHttpClient("StackOverflowClient" , c =>
        {
            c.BaseAddress = new Uri("https://api.stackexchange.com/2.2/");
            c.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("gzip"));
        }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
        });

        return services;
    }
}
