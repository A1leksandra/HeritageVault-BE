using HV.DAL;
using Microsoft.EntityFrameworkCore;

namespace HV.WebAPI.Extensions;

public static class HostExtensions
{
    extension(IHost app)
    {
        public async Task MigrateDatabaseAsync()
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ApplicationDbContext>();
            await context.Database.MigrateAsync();
        }
    }
}