using Blog.Data.Mappings;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data
{
    public class BlogDataContext : DbContext
    {
        private const string CONNECTION_STRING = "Server=localhost,1433;Database=my_blog;User ID=sa;Password=1q2w3e4r@#$;TrustServerCertificate=True";

        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }

        // Configuração de conexão com o DB.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(CONNECTION_STRING);

        // Configuração para arquivos de mapeamento.
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CategoryMap());
            builder.ApplyConfiguration(new UserMap());
            builder.ApplyConfiguration(new PostMap());
        }
    }
}
