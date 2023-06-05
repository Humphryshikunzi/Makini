using Digipap.IRepositories; 
using Microsoft.EntityFrameworkCore; 

namespace Digipap.Persistent
{
    internal static class DbInitializer
    {
        internal static IApplicationBuilder Initialize(this IApplicationBuilder app)
        {
            // use this to genarate migrations and update db at runtime when not done manualy in terminal

            var serviceScope = app.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<DigipapDbContext>();
            var pendingMigrations = context.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
                context.Database.Migrate();

            var initializers = serviceScope.ServiceProvider.GetServices<IDbInitializerRepository>();
            foreach (var initializer in initializers)
            {
                initializer.Initialize();
            }

            return app;
        }
    }
}
