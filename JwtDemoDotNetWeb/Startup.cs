using System;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(JwtDemoDotNetWeb.Startup))]

namespace JwtDemoDotNetWeb
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var audience = ConfigurationManager.AppSettings["Audience"];
            var issuer = ConfigurationManager.AppSettings["Issuer"];
            var secret = ConfigurationManager.AppSettings["SignKey"];
        
            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                //Provider = new MyOAuthBearerAuthenticationProvider(),
                AuthenticationMode = AuthenticationMode.Active,
                AllowedAudiences = new[] { audience },
                IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                {
                    new SymmetricKeyIssuerSecurityTokenProvider(issuer, System.Text.Encoding.UTF8.GetBytes(secret))
                },
                TokenValidationParameters = new TokenValidationParameters()
                {
                    //ValidateAudience = false,
                    ValidAudience = audience,
                    //AudienceValidator = CustomAudienceValidator,

                    //ValidateIssuer = false,
                    ValidIssuer = issuer,
                    //IssuerValidator = CustomIssuerValidator,

                    IssuerSigningKey = new InMemorySymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(secret))
                }
            });
           
        }
    }
}
