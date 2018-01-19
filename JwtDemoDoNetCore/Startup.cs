using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JwtDemoDoNetCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddCors();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        //ValidateAudience = false,
                        ValidAudience = Configuration["JWT:Audience"],
                        //AudienceValidator = CustomAudienceValidator,

                        //ValidateIssuer = false,
                        ValidIssuer = Configuration["JWT:Issuer"],
                        //IssuerValidator = CustomIssuerValidator,

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["JWT:SignKey"]))
                    };

                    #region Events

                    ////允許來自Query String的token
                    //options.Events = new JwtBearerEvents()
                    //{
                    //    OnMessageReceived = ctx =>
                    //    {
                    //        
                    //        if (ctx.Request.Method.Equals("GET") && ctx.Request.Query.ContainsKey("token"))
                    //            ctx.Token = ctx.Request.Query["token"];
                    //        return Task.CompletedTask;
                    //    }
                    //};

                    #endregion

                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            }
        
            app.UseAuthentication();
            app.UseMvc();
        }

        private string CustomIssuerValidator(string orgIssuer, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            var issuer = Configuration["JWT:Issuer"];
            return issuer;
        }

        private bool CustomAudienceValidator(IEnumerable<string> audiences, SecurityToken securityToken,
            TokenValidationParameters validationParameters)
        {
            string[] validAud = { Configuration["JWT:Audience"] };
            var isValid = validAud.Any(x => audiences.Any(y => y.Equals(x)));

            return isValid;
        }
    }
}