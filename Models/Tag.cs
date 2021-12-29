using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    [Table("Tag")]
    public class Tag
    {
        // [Key] é um annotation para indicar chave primária.
        [Key] public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
    }
}
