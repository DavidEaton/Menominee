using Microsoft.Extensions.Options;

namespace Menominee.Client.Services.Shared
{
    public class UriBuilderFactory
    {
        private readonly UriBuilderConfiguration config;
        public UriBuilderFactory(IOptions<UriBuilderConfiguration> config)
        {
            this.config = config.Value;
        }

        public UriBuilder CreateBaseUriBuilder(string path)
        {
            return new UriBuilder
            {
                Scheme = config.Scheme,
                Host = config.Host,
                Port = config.Port ?? 0,
                Path = path
            };
        }
    }
}
