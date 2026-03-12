using EventBooking.Infrastructure.Persistence;
using EventBooking.Infrastructure.Persistence.RuntimeConfigurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventBooking.Infrastructure.Extensions
{
    public static class SeedDataExtensions
    {
        public static async Task<IHost> SeedIdentityDataAsync(this IHost app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    await DefaultAdminUserConfiguration.SeedAsync(services);
                    await DefaultNormalUserConfiguration.SeedAsync(services);
                }
                catch (Exception ex)
                {
                    //if logger system will be added log will be here.
                    Console.WriteLine($"Seed Exception: {ex.Message}");
                }
            }

            return app;
        }
    }
}
