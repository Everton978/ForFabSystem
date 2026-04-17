using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForFab_Metals_System.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("RA")]
        public string RA { get; set; } = string.Empty;

        [Column("Nome")]
        public string Nome { get; set; } = string.Empty;

        [Column("Turma")]
        public string? Turma { get; set; }

        [Column("Senha")]
        public string Senha { get; set; } = string.Empty;

        [Column("DataCadastro")]
        public string? DataCadastro { get; set; }

        // Navegação
        public ICollection<ProducOrder> OrdensResponsavel { get; set; } = new List<ProducOrder>();
    }
}
