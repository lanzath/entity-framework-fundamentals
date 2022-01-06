using System.Collections.Generic;
using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings
{
    public class PostMap : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            // Tabela
            builder.ToTable("Post");

            // Chave Primária
            builder.HasKey(x => x.Id);

            // Identity
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            // Propriedades
            builder.Property(x => x.LastUpdateDate)
                .IsRequired()
                .HasColumnName("LastUpdateDate")
                .HasColumnType("SMALLDATETIME")
                .HasMaxLength(60)
                .HasDefaultValueSql("GETDATE()"); // Gerar data pelo SQL.
            // .HasDefaultValue(DateTime.Now.ToUniversalTime()); // Gera data pelo C#.

            // Índices
            builder
                .HasIndex(x => x.Slug, "IX_Post_Slug")
                .IsUnique();

            #region Relacionamentos

            builder
                .HasOne(x => x.Author) // Post pertence a um autor
                .WithMany(x => x.Posts) // e este autor têm muitos posts.
                .HasConstraintName("FK_Post_Author") // nome da chave estrangeira.
                .OnDelete(DeleteBehavior.Cascade); // Efeito cascata ao deletar, para deletar registros "filhos".

            builder
                .HasOne(x => x.Category) // Post tem uma categoria
                .WithMany(x => x.Posts) // e esta categoria têm muitos posts.
                .HasConstraintName("FK_Post_Category")
                .OnDelete(DeleteBehavior.Cascade);


            builder
                .HasMany(x => x.Tags) // Post tem muitas tags
                .WithMany(x => x.Posts) // e estas tags estão associadas a muitos posts.
                .UsingEntity<Dictionary<string, object>>( // Entidade "virtual" para associar as tags com os posts
                    "PostTag",
                    post => post
                        .HasOne<Tag>()
                        .WithMany()
                        .HasForeignKey("PostId")
                        .HasConstraintName("FK_PostRole_PostId")
                        .OnDelete(DeleteBehavior.Cascade),
                    tag => tag
                        .HasOne<Post>()
                        .WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("FK_PostTag_TagId")
                        .OnDelete(DeleteBehavior.Cascade));

            #endregion
        }
    }
}
