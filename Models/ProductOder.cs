using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForFab_Metals_System.Models
{
    [Table("ProducOrder")]
    public class ProducOrder
    {
        [Key]
        [Column("Id_Op")]
        public int Id { get; set; }

        [Column("Peca")]
        public string Peca { get; set; } = string.Empty;

        [Column("Material")]
        public string? Material { get; set; }

        [Column("DesenhoURL")]
        public string? DesenhoURL { get; set; }

        [Column("Processos")]
        public string? Processos { get; set; } // Pode ser usado para armazenar tarefas serializadas, mas vamos criar tabela separada

        [Column("DataInicio_Plan")]
        public DateTime DataInicioPlan { get; set; }

        [Column("DataFim_Plan")]
        public DateTime DataFimPlan { get; set; }

        [Column("Status")]
        public string Status { get; set; } = "Aguardando";

        [Column("ResponsavelRA")]
        public string? ResponsavelRA { get; set; }

        [ForeignKey("ResponsavelRA")]
        public User? Responsavel { get; set; }

        // Navegação para novas tabelas
        public ICollection<TarefaRoteiro> Tarefas { get; set; } = new List<TarefaRoteiro>();
        public ICollection<Qualidade> Inspecoes { get; set; } = new List<Qualidade>();
    }
}
