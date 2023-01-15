using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<ApiScope> ApiScopes(){
    return new List<ApiScope>
                   {
                       new ApiScope("api1", "My API"),
                       new ApiScope("api2","My API2")
                   };
    }
        

    public static IEnumerable<ApiResource> ApiResources()
    {
        return new[]
        {
            new ApiResource("mvc", "My API")
            {
                Scopes = new[] { "api1","api2"},
                UserClaims = new[]{"role"}
            },
            new ApiResource("api1","MyAPI1")
            {
                Scopes = new[]{"api1"}
            }
        };
    }

    public static IEnumerable<IdentityResource> IdentityResources()
    {
        var openIdScope = new IdentityResources.OpenId();
        openIdScope.UserClaims.Add(JwtClaimTypes.Locale);
        return new List<IdentityResource>
        {
            openIdScope,
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new ("roles", new List<string> { JwtClaimTypes.Role })
            {
                Required = true
            },
           new IdentityResources.Address(),
           new IdentityResources.Phone(),
           new IdentityResource("name",new[]{"FirstName","SecondName"})
        };
    }


    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "mvc",
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                
                //AllowAccessTokensViaBrowser = true,

                // where to redirect to after login
                //RedirectUris = { "https://localhost:5002/signin-oidc" },

                // where to redirect to after logout
               //PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },
               
               AccessTokenLifetime = 36000,
               AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "api1",
                    "roles",
                    IdentityServerConstants.StandardScopes.Email,
                    "name"
                }
            }
        };
}