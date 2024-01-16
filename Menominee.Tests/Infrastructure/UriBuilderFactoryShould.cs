using FluentAssertions;
using Menominee.Client.Services.Shared;
using Microsoft.Extensions.Options;
using Xunit;

namespace Menominee.Tests.Infrastructure
{
    public class UriBuilderFactoryShould
    {
        [Fact]
        public void CreateBaseUriBuilder_With_Null_Port_Should_Omit_Port()
        {
            // Arrange
            var config = new UriBuilderConfiguration { Scheme = "https", Host = "example.com", Port = null };
            var factory = new UriBuilderFactory(Options.Create(config));

            // Act
            var uriBuilder = factory.CreateBaseUriBuilder("");
            var uri = uriBuilder.Uri;

            // Assert
            uri.ToString().Should().Be("https://example.com/");
        }

        [Fact]
        public void CreateBaseUriBuilder_With_Null_Port_Should_Include_Port()
        {
            var config = new UriBuilderConfiguration { Scheme = "https", Host = "example.com", Port = 8080 };
            var factory = new UriBuilderFactory(Options.Create(config));

            var uriBuilder = factory.CreateBaseUriBuilder("");
            var uri = uriBuilder.Uri;

            uri.ToString().Should().Be("https://example.com:8080/");
        }

        [Fact]
        public void CreateBaseUriBuilder_With_Trailing_Slash_In_Host_Should_Remove_Trailing_Slash()
        {
            var config = new UriBuilderConfiguration { Scheme = "https", Host = "example.com/", Port = null };
            var factory = new UriBuilderFactory(Options.Create(config));

            var uriBuilder = factory.CreateBaseUriBuilder("");
            var uri = uriBuilder.Uri;

            uri.ToString().Should().Be("https://example.com/");
        }

    }
}
