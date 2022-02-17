using Garage_2._0.Data;

namespace Garage_2._0.Extensions
{
    public static class SeedDataAppBuilderExtensions
    {
        public static async Task<IApplicationBuilder> SeedDataAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var _context = provider.GetRequiredService<GarageVehicleContext>();

                try
                {
                    await SeedData.Start(_context);
                }
                catch (Exception ex)
                {
                    var log = provider.GetRequiredService<ILogger<Program>>();
                    log.LogError(String.Join(" ", ex.Message));
                }
            }
            return app;
        }
    }
}
