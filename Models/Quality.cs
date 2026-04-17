using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForFab_Metals_System.Models
{
   
    [Table("Qualidade")]
    public class Quality
    {
        [Key]
        [Column("IdInspecao")]
        public int Id { get; set; }

        [Column("IdOS")]
        public int IdOS { get; set; }

        [ForeignKey("IdOS")]
        public ProducOrder OrdemProducao { get; set; } = null!;

        [Column("Parametro")]
        public string? Parametro { get; set; }

        [Column("ValorNominal")]
        public string? ValorNominal { get; set; }

        [Column("ValorReal")]
        public decimal? ValorReal { get; set; }

        [Column("StatusFinal")]
        public string? StatusFinal { get; set; }

        [Column("Observacoes")]
        public string? Observacoes { get; set; }

        [Column("DataInspecao")]
        public DateTime DataInspecao { get; set; } = DateTime.Now;
    }
}
