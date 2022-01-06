using System;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data
{
    public class BlogDataContext : DbContext
    {
        private const string CONNECTION_STRING = "Server=localhost,1433;Database=Blog;User ID=sa;Password=1q2w3e4r@#$;TrustServerCertificate=True";

        #region Representação das tabelas
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        // public DbSet<Tag> Tags { get; set; }
        // public DbSet<PostTag> PostTags { get; set; }
        // public DbSet<Role> Roles { get; set; }
        // public DbSet<UserRole> UserRoles { get; set; }
        #endregion

        // Configuração de conexão com o DB.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(CONNECTION_STRING);
            options.LogTo(Console.WriteLine); // Exibe um log de queries executadas no console.
        }
    }
}
