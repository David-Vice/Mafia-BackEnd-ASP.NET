using back_end.Data;
using back_end.Security;
using back_end.Services.Abstract;
using back_end.Services.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace back_end
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

            services.AddControllers();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IGameSessionsUsersRoleService, GameSessionsUsersRoleService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IBotResponceService, BotResponceService>();
            services.AddScoped<IPlayerInGameStatusService, PlayerInGameStatusService>();
            services.Configure<DbConnectionInfo>(settings => Configuration.GetSection("ConnectionStrings").Bind(settings));
            services.AddScoped<IDataContext, DataContext>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = Configuration.GetSection("Token").GetValue<bool>("CheckIssuer"),
                    ValidIssuers = Configuration.GetSection("Token").GetValue<IEnumerable<string>>("Issuers"),
                    ValidateAudience = Configuration.GetSection("Token").GetValue<bool>("CheckAudience"),
                    ValidAudiences = Configuration.GetSection("Token").GetValue<IEnumerable<string>>("Audiences"),
                    ValidateIssuerSigningKey = Configuration.GetSection("Token").GetValue<bool>("CheckSigningKey"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("Token").GetValue<string>("SuperSecretKey")))
                };
            });
            services.AddCors();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "back_end", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert your token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                      },
                      new List<string>()
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            // {
            //    app.UseDeveloperExceptionPage();
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
             app.UseSwaggerUI(c =>
             {

                 //c.SwaggerEndpoint("/swagger/v1/swagger.json", "back_end v1");
                 string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                 c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "back_end");

             }
                 );
          //  }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(x => x
               .AllowAnyMethod()
               .AllowAnyHeader()
               .SetIsOriginAllowed(origin => true) // allow any origin
               .AllowCredentials()); // allow credentials
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
