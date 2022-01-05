using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    [Table("User")]
    public class User
    {
        [Key] // Indicação explícita de primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Indica que é gerado automaticamente pelo banco.
        public int Id { get; set; }

        [Required] // Indica que é Not Null.
        [MinLength(2)] // Indica quantidade mínima de caracteres.
        [MaxLength(80)] // Indica a quantidade máx de caracteres.
        [Column("Name", TypeName = "NVARCHAR")] // Indicação do nome e tipo da coluna.
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("Email", TypeName = "NVARCHAR")]
        public string Email { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("PasswordHash", TypeName = "NVARCHAR")]
        public string PasswordHash { get; set; }

        [Required]
        [Column("Bio", TypeName = "NVARCHAR")]
        public string Bio { get; set; }

        [Required]
        [MaxLength(2000)]
        [Column("Image", TypeName = "NVARCHAR")]
        public string Image { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(80)]
        [Column("Slug", TypeName = "NVARCHAR")]
        public string Slug { get; set; }
    }
}
