using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForFab_Metals_System.Models
{
    [Table("Files")]
    public class FileEntity
    {
        [Key]
        [Column("Id_File")]
        public int Id { get; set; }

        [Column("Nome")]
        public string? Nome { get; set; }

        [Column("Files_Drawing")]
        public byte[]? Conteudo { get; set; }
    }
}
