using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    [Table("Post")]
    public class Post
    {
        [Key] // Indicação explícita de primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Indica que é gerado automaticamente pelo banco.
        public int Id { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string Body { get; set; }

        public string Slug { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastUpdateDate { get; set; }

        [ForeignKey("CategoryId")] // Por padrão é separado em classe e propriedade [Category, Id] pelo mapeamento.
        public int CategoryId { get; set; }
        public Category Category { get; set; } // Propriedade de navegação.


        [ForeignKey("AuthorId")]
        public int AuthorId { get; set; }
        public User Author { get; set; }
    }
}
