using System;
using App.Extensions;
using App.Repositories;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace App
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /*if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();*/

            app.UseDefaultFiles().UseStaticFiles()
            .UseSwagger().UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sklyarov API V1");
                c.RoutePrefix = string.Empty;
            })
            //.UseCors(c => c.WithOrigins(Configuration.GetValue<string>("Cors")))
            .UseCors("CorsPolicy")
            .UseRouting()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc");
            })
            .Migrate<ApplicationDbContext>(ApplicationDbContext.Seed);                 
        }
    }
}