using Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Extentions;
public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<Context>();

        context.Database.EnsureCreated();
        context.Database.Migrate();
    }
}
