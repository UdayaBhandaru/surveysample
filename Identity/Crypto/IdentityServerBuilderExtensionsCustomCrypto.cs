using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Service.Crypto
{
    public static class IdentityServerBuilderExtensionsCustomCrypto
    {
        public static IIdentityServerBuilder AddCustomSigningCredential(this IIdentityServerBuilder builder, string signingKey)
        {
            var signingKeybyte = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            var signingCredentials = new SigningCredentials(signingKeybyte, SecurityAlgorithms.HmacSha256);
            builder.Services.AddSingleton<ISigningCredentialStore>(new DefaultSigningCredentialsStore(signingCredentials));
            builder.Services.AddSingleton<IValidationKeysStore>(new DefaultValidationKeysStore(new[] { signingCredentials.Key }));

            return builder;
        }
    }
}
