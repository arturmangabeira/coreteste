using Core.Api.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace Core.Api.Data
{
    public class DataContext : DbContext 
    {
        private readonly IConfiguration configuration;
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        /*public DataContext(IConfiguration config)
        {
            this.configuration = config;
        }*/

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ///Configuration.GetConnectionString("DefaultConnection")
            //optionsBuilder.
            optionsBuilder.UseSqlite(this.configuration.GetConnectionString("DefaultConnection"));
        }*/

        public DbSet<Evento> Eventos { get; set; }
    }
}