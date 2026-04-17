using ForFab_Metals_System.Models;
using ForFabio.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ForFabio.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Tabelas existentes (mapeadas para os nomes exatos do SQL)
    public DbSet<User> Users { get; set; }
    public DbSet<ProducOrder> ProducOrders { get; set; }
    public DbSet<Almoxarifado> Almoxarifados { get; set; }
    public DbSet<Qualidade> Qualidades { get; set; }
    public DbSet<FileEntity> Files { get; set; }

    // Novas tabelas (criadas via migration)
    public DbSet<TarefaRoteiro> TarefasRoteiro { get; set; }
    public DbSet<Apontamento> Apontamentos { get; set; }
    public DbSet<DesenhoEngenharia> DesenhosEngenharia { get; set; }
    public DbSet<LaudoQualidadeDetalhado> LaudosQualidadeDetalhados { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuração para tabelas existentes (evitar recriação)
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<ProducOrder>().ToTable("ProducOrder");
        modelBuilder.Entity<Almoxarifado>().ToTable("Almoxarifado");
        modelBuilder.Entity<Qualidade>().ToTable("Qualidade");
        modelBuilder.Entity<FileEntity>().ToTable("Files");

        // Relacionamentos
        modelBuilder.Entity<ProducOrder>()
            .HasMany(o => o.Tarefas)
            .WithOne(t => t.OrdemProducao)
            .HasForeignKey(t => t.OrdemProducaoId);

        modelBuilder.Entity<ProducOrder>()
            .HasOne(o => o.Responsavel)
            .WithMany(u => u.OrdensResponsavel)
            .HasForeignKey(o => o.ResponsavelRA);

        modelBuilder.Entity<Qualidade>()
            .HasOne(q => q.OrdemProducao)
            .WithMany(o => o.Inspecoes)
            .HasForeignKey(q => q.IdOS);
    }
}