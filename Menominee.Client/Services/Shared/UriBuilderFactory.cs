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
            var host = config.Host.TrimEnd('/'); // Ensure no trailing slash
            return new UriBuilder
            {
                Scheme = config.Scheme,
                Host = host,
                Port = config.Port ?? -1, // Set to -1 if null, which omits the port
                Path = path
            };
        }
    }
}
