using System.Diagnostics.CodeAnalysis;
using Infra.Data.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data
{
    [ExcludeFromCodeCoverage]
    public sealed class Context(DbContextOptions<Context> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PersonConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
