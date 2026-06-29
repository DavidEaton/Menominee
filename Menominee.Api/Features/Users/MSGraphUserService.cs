using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Menominee.Shared.Models.Tenants;
using Menominee.Shared.Models.Users;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;

namespace Menominee.Api.Features.Users
{
    public class MSGraphUserService : IMSGraphUserService
    {
        private readonly GraphServiceClient _graphClient;
        private readonly GraphConfiguration _graphConfig;
        private readonly ILogger<MSGraphUserService> _logger;
        public string ShopRoleAttributeName => CustomAttributeName("shopRole");
        public string ShopIdAttributeName => CustomAttributeName("shopId");
        public string ShopNameAttributeName => CustomAttributeName("shopName");

        public MSGraphUserService(GraphServiceClient graphClient, GraphConfiguration graphConfig, ILogger<MSGraphUserService> logger)
        {
            _graphClient = graphClient;
            _graphConfig = graphConfig;
            _logger = logger;
        }

        public async Task<List<UserResponse>> GetAllAsync() =>
            throw new NotImplementedException("GetAllAsync is not implemented yet.");

        public async Task<RegisterUserResult> RegisterAsync(RegisterUserRequest user) =>
            throw new NotImplementedException("RegisterAsync is not implemented yet.");

        public string CustomAttributeName(string baseName)
        {
            return $"extension_{_graphConfig.B2CExtensionAppClientId.Replace("-", "")}_{baseName}";
        }
    }
}