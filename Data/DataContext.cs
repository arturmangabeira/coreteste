using Core.Api.Model;
using Microsoft.EntityFrameworkCore;
namespace Core.Api.Data
{
    public class DataContext : DbContext 
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Evento> Eventos { get; set; }
    }
}