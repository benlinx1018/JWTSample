using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using JwtDemoDotNetWeb.Models;


namespace JwtDemoDotNetWeb.Controllers
{
    [RoutePrefix("api/Token")]
    public class TokenController : ApiController
    {

        [Route("Login")]
        //[AllowAnonymous]
        [HttpPost]
        public object Login(TokenRequest request)
        {
            string signKey = ConfigurationManager.AppSettings["SignKey"];
            string issuer = ConfigurationManager.AppSettings["Issuer"];
            string audience = ConfigurationManager.AppSettings["Audience"];
            if (request.Username == "Jon" && request.Password == "123")
            {
                var claims = new[]
                {
                    //自訂payload附帶其他Identity其他屬性的type & value map宣告
                    new Claim(ClaimTypes.Name, request.Username),
                    new Claim(ClaimTypes.Role, "AdminRole")
                };

                var key = new InMemorySymmetricSecurityKey(Encoding.UTF8.GetBytes(signKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                };
              ;
            }

            return BadRequest("Could not verify username and password");
        }
    }
}
