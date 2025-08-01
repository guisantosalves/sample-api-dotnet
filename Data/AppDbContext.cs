using Microsoft.EntityFrameworkCore;
using ProjetoDBZ.Models;

namespace ProjetoDBZ.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        // o nome da table é Personagem, DBZ só representa o todo
        public DbSet<Personagem> DBZ { get; set; }
    }
}