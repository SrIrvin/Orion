using Microsoft.EntityFrameworkCore;

namespace Orión.Infrastructure.Persistence;

public static class DbInitializer
{
    public static void Initialize(OrionDbContext context)
    {
        // Asegura que la base de datos existe y aplica todas las migraciones pendientes
        // Si ya existe y está al día, no hace nada.
        context.Database.Migrate();
    }
}
