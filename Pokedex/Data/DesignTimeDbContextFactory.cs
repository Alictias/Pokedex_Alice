using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Pokedex.Data;

// Fábrica usada apenas em design-time (dotnet ef ...)
// Garante que o EF saiba construir o AppDbContext para Add-Migration/Update-Database.
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // Mesma connection string do appsettings.json
        // Pode usar caminho relativo; o SQLite criará o arquivo na raiz do projeto.
        const string connectionString = "Data Source=pokedex.db";

        optionsBuilder.UseSqlite(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}