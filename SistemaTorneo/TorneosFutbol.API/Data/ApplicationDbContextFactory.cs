using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProdeMundial.Data;

namespace TorneoFutbol.Data // Ajusta esto al namespace real de tu carpeta Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            // AQUÍ PONES LA MISMA CADENA DE CONEXIÓN QUE USAS EN TU APPSETTINGS.JSON
            optionsBuilder.UseNpgsql("Host=localhost;Database=TU_DB;Username=TU_USER;Password=TU_PASS");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}