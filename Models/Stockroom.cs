using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForFab_Metals_System.Models
{
 [Table("Almoxarifado")]
    public class Stockroom
    {
        [Key]
        [Column("Id_Item")]
        public int Id { get; set; }

        [Column("Peca")]
        public string Peca { get; set; } = string.Empty;

        [Column("Material")]
        public string? Material { get; set; }

        [Column("Tamanho")]
        public string? Tamanho { get; set; }

        [Column("Quantidade")]
        public int Quantidade { get; set; }

        [Column("Endereco")]
        public string? Endereco { get; set; }

        [Column("Status")]
        public string Status { get; set; } = "Almoxarifado";

        [Column("DataEntrada")]
        public DateTime DataEntrada { get; set; } = DateTime.Now;
    }
}
