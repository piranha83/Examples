using System;
using System.Text;
using App.Extensions;
using App.Models;
using App.Repositories;
using App.Services;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;

namespace App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private IServiceProvider _serviceProvider;
       
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Authentication 
            var key = Encoding.UTF8.GetBytes(Configuration["TokenKey"]);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _serviceProvider.GetRequiredService<SecurityKey>(),
                    ValidateIssuer = true,
                    ValidIssuer = Configuration.GetValue<string>("JwtServer"),
                    ValidateAudience = true,
                    ValidAudience = Configuration.GetValue<string>("Client"),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromDays(5)
                };                                
            });
                        
            // HttpContextServiceProviderValidatorFactory requires access to HttpContext
            services.AddHttpContextAccessor();
            services.AddControllers()
                // Adds fluent validators to Asp.net
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssemblyContaining<Startup>();
                    // Optionally set validator factory if you have problems with scope resolve inside validators.
                    options.ValidatorFactoryType = typeof(HttpContextServiceProviderValidatorFactory);
                });
            // This line uses 'UseSqlServer' in the 'options' parameter
            // with the connection string defined above.
            services.AddDbContext<ApplicationDbContext>(options => {
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(nameof(App));
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 4, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    })
                    .LogTo(Console.Write);
                })
                .AddScoped(typeof(ISession), typeof(EfSession<ApplicationDbContext>))
                .AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
                .AddSingleton(typeof(IIdentityRepository), typeof(IdentityRepository))
                .AddSingleton(typeof(IIdentityService), typeof(JwtService))                
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sklyarov API", Version = "v1" });
                    // Adds fluent validation rules to swagger
                    c.AddFluentValidationRules();
                })
                .AddLogging(builder => builder.AddConsole())
                .AddHealthChecks().AddSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                    name: "db-check",
                    tags: new string[] { "master" }); 

            services.AddSingleton<SecurityKey>(new SymmetricSecurityKey(key));                  
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            _serviceProvider = app.ApplicationServices;
            /*if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();*/
            app.UseDeveloperExceptionPage();
            app.UseDefaultFiles().UseStaticFiles();
            if (!env.IsDevelopment())
                app.UseSpaStaticFiles();

            app.UseSwagger().UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sklyarov API V1");
                c.RoutePrefix = string.Empty;
            })
            .UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader())
            .UseRouting()
            .UseAuthentication().UseAuthorization()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc");
            })
            .Migrate<ApplicationDbContext>(ApplicationDbContext.Seed)
            .UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
                if (env.IsDevelopment())
                    spa.UseAngularCliServer(npmScript: "start");
            });                
        }
    }
}