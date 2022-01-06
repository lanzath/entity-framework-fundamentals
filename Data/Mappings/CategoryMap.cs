using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // O EF tenta "pluralizar" o nome das tabelas, então o map foi configurado para a tabela Category e não Categories como faria por padrão.
            builder.ToTable("Category");

            // Id é a chave primária.
            builder.HasKey(x => x.Id);

            // Identity
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(); // PRIMARY KEY IDENTITY(1, 1).

            #region Propriedades
            builder.Property(x => x.Name) // Propriedade Name
                .IsRequired() // Obrigatória
                .HasColumnName("Name") // Nome da coluna
                .HasColumnType("NVARCHAR") // Tipo de dados
                .HasMaxLength(80); // Validação de quantidade max de carácteres.

            builder.Property(x => x.Slug)
                .IsRequired()
                .HasColumnName("Slug")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(80);
            #endregion

            // Índices
            builder.HasIndex(x => x.Slug, "IX_Category_Slug") // Criação de index (Propriedade que recebe Index, nome do index).
                .IsUnique(); // O index é único.
        }
    }
}
