using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Menominee.Shared.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;

//using Microsoft.Graph;

namespace Menominee.Api.Users
{
    public class MSGraphUserService : IMSGraphUserService
    {
        private readonly GraphServiceClient _graphClient;
        private readonly GraphConfig _graphConfig;
        private readonly ILogger<MSGraphUserService> _logger;
        public string ShopRoleAttributeName => CustomAttributeName("shopRole");
        public string ShopIdAttributeName => CustomAttributeName("shopId");
        public string ShopNameAttributeName => CustomAttributeName("shopName");

        public MSGraphUserService(GraphServiceClient graphClient, GraphConfig graphConfig, ILogger<MSGraphUserService> logger)
        {
            _graphClient = graphClient;
            _graphConfig = graphConfig;
            _logger = logger;
        }

        public async Task<List<UserToRead>> GetUsers()
        {
            _logger.LogTrace("GetUsers");
            var users = new List<UserToRead>();
            try
            {
                var result = await _graphClient.Users
                    .Request()
                    .Select($"id,displayName,identities,{ShopRoleAttributeName}")
                    .GetAsync();
                
                foreach (var user in result.CurrentPage)
                {
                    var email = user.Identities.FirstOrDefault(i => i.SignInType.Equals("emailAddress"))
                                    ?.IssuerAssignedId ??
                                user.Identities.FirstOrDefault(i =>
                                        i.SignInType.Equals("userPrincipalName") && i.IssuerAssignedId.Contains('@'))
                                    ?.IssuerAssignedId;
                    var shopRole = user.AdditionalData?.ContainsKey(ShopRoleAttributeName) == true
                        ? user.AdditionalData[ShopRoleAttributeName].ToString()
                        : string.Empty;
                    
                    if (!string.IsNullOrEmpty(email))
                    {
                        users.Add(new UserToRead
                        {
                            Id = user.Id,
                            Username = email,
                            Email = email,
                            Name = user.DisplayName,
                            ShopRole = shopRole
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetUsers-Catch");
            }

            _logger.LogTrace("GetUsers - DONE");
            return users;
        }

        public async Task<RegisterUserResult> CreateUser(RegisterUser user)
        {
            _logger.LogTrace("CreateUser");
            var result = new RegisterUserResult { Successful = false };
            try
            {
                IDictionary<string, object> extensionInstance = new Dictionary<string, object>();
                extensionInstance.Add(ShopRoleAttributeName, user.ShopRole);
                // TODO: shop name
                // TODO: shop ID

                var graphResult = await _graphClient.Users
                    .Request()
                    .AddAsync(new User
                    {
                        DisplayName = user.Email,
                        Identities = new List<ObjectIdentity>
                        {
                            new ObjectIdentity
                            {
                                SignInType = "emailAddress",
                                Issuer = _graphConfig.TenantId,
                                IssuerAssignedId = user.Email
                            }
                        },
                        PasswordProfile = new PasswordProfile
                        {
                            Password = user.Password
                        },
                        PasswordPolicies = "DisablePasswordExpiration",
                        AdditionalData = extensionInstance
                    });
                var userId = graphResult.Id;
                result.Successful = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateUser-Catch");
                result.Successful = false;
                // TODO: get more helpful here?
                result.Errors = new[] { ex.Message };
            }
            _logger.LogTrace("CreateUser - DONE");
            return result;
        }
        
        public string CustomAttributeName(string baseName)
        {
            return $"extension_{_graphConfig.B2CExtensionAppClientId.Replace("-", "")}_{baseName}";
        }
    }
}