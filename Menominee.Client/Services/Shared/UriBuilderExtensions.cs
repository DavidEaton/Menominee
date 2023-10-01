using System.Web;

namespace Menominee.Client.Services.Shared
{
    public static class UriBuilderExtensions
    {
        public static UriBuilder CreateUriBuilderWithQueryParams(
            this UriBuilder uriBuilder,
            Dictionary<string, long> queryParams)
        {
            var initialQuery = HttpUtility.ParseQueryString(uriBuilder.Query);

            var updatedQuery = queryParams.Aggregate(initialQuery, (currentQuery, param) =>
            {
                currentQuery[param.Key] = param.Value.ToString();
                return currentQuery;
            });

            uriBuilder.Query = updatedQuery.ToString();
            return uriBuilder;
        }
    }
}
