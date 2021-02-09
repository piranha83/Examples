using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;

namespace App.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder Migrate<TContext>(this IApplicationBuilder app, Action<TContext, IServiceProvider> seeder = null) 
        where TContext : DbContext
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var sp = scope.ServiceProvider;
                var logger = sp.GetRequiredService<ILogger<TContext>>();
                var context = sp.GetRequiredService<TContext>();
                try
                {                    
                    Policy.Handle<SqlException>().WaitAndRetry(new TimeSpan[]
                    {
                        TimeSpan.FromSeconds(3),
                        TimeSpan.FromSeconds(10),
                        TimeSpan.FromSeconds(15),
                    }).Execute(() => {
                        if(context.Database.GetPendingMigrations().Any())
                            context.Database.Migrate();
                        if(seeder != null)
                            seeder(context, sp);
                    });
                }
                catch(Exception ex)
                {
                    logger.LogError("An error occurred while migrating the database used on context: " + ex);
                }
            }
            return app;
        }
    }
}