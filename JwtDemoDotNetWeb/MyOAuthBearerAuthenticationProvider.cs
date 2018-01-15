using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security.OAuth;

namespace JwtDemoDotNetWeb
{
    public class MyOAuthBearerAuthenticationProvider : IOAuthBearerAuthenticationProvider
    {
        public Task RequestToken(OAuthRequestTokenContext context)
        {
            if (context.Request.Method.Equals("GET") && context.Request.Query["token"] !=null)
                context.Token = context.Request.Query["token"];
            return Task.CompletedTask;
        }

        public Task ValidateIdentity(OAuthValidateIdentityContext context)
        {
            return Task.CompletedTask;
        }

        public Task ApplyChallenge(OAuthChallengeContext context)
        {
            return Task.CompletedTask;
        }
    }
}