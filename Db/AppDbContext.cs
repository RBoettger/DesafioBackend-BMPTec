using DesafioBackend.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace DesafioBackend.Db
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions _options) : base(_options) { }
        
            public DbSet<PlaylistModel> Playlists { get; set; }
            public DbSet<HistoricoPesquisa> HistoricoPesquisas { get; set; }
    }
}
