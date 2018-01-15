using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JwtDemoDoNetCore.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JwtDemoDoNetCore.Controllers
{
    [Produces("application/json")]
    [Route("api/Token")]
    public class TokenController : Controller
    {
        private readonly IConfiguration _config;

        public TokenController(IConfiguration config)
        {
            _config = config;
        }

        [Route("Login")]
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody]TokenRequest request)
        {
            if (request.Username == "Jon" && request.Password == "123")
            {
                var claims = new[]
                {
                    //自訂payload附帶其他Identity其他屬性的type & value map宣告
                    new Claim(ClaimTypes.Name, request.Username),
                    new Claim(ClaimTypes.Role, "AdminRole")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SignKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _config["JWT:Issuer"],
                    audience: _config["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

            return BadRequest("Could not verify username and password");
        }
    }
}