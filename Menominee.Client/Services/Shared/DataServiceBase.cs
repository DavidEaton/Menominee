namespace Menominee.Client.Services.Shared
{
    public abstract class DataServiceBase<T>
    {
        protected readonly UriBuilderFactory UriBuilderFactory;
        protected readonly ILogger<T> Logger;

        protected DataServiceBase(UriBuilderFactory uriBuilderFactory, ILogger<T> logger)
        {
            UriBuilderFactory = uriBuilderFactory ?? throw new ArgumentNullException(nameof(uriBuilderFactory));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected UriBuilder CreateBaseUriBuilder(string path)
        {
            return UriBuilderFactory.CreateBaseUriBuilder(path);
        }

        protected Uri BuildUriWithQueryParams(string baseUri, Dictionary<string, long> queryParams)
        {
            var uriBuilder = CreateBaseUriBuilder(baseUri);
            uriBuilder = uriBuilder.CreateUriBuilderWithQueryParams(queryParams);
            return uriBuilder.Uri;
        }
    }
}
