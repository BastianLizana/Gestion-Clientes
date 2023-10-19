using Microsoft.EntityFrameworkCore;

namespace ApiGestionClientes.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        { 
            
        }

        public DbSet<Clientes> Cliente => Set<Clientes>();
    }
}
