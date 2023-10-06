
using Microsoft.EntityFrameworkCore;
using server.api.Models;

namespace server.api.Services.Context
{
    public class SifocaContext : DbContext
    {
        public SifocaContext(DbContextOptions<SifocaContext> options) : base(options)
        {

        }

        public DbSet<Movimento> Tb_Movimento { get; set; }
        public DbSet<Entrada> Tb_Entrada { get; set; }
        public DbSet<Saida> Tb_Saida { get; set; }
        public DbSet<Fundo> Tb_Fundo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // MOVIMENTOS

            modelBuilder.Entity<Movimento>()
                .HasMany(m => m.Entradas)
                .WithOne(m => m.Movimento)
                .HasForeignKey(e => e.Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Movimento>()
                .HasMany(m => m.Saidas)
                .WithOne(m => m.Movimento)
                .HasForeignKey(s => s.Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<Movimento>()
                .Property(p => p.Descricao)
                .HasMaxLength(250)
                .IsRequired();

            modelBuilder
                .Entity<Movimento>()
                .Property(p => p.Valor)
                .HasPrecision(10, 2)
                .IsRequired();

            modelBuilder
                .Entity<Movimento>()
                .Property(p => p.Categoria)
                .IsRequired();

            //ENTRADAS
            modelBuilder
                .Entity<Entrada>()
                .Property(p => p.Operador)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder
                .Entity<Entrada>()
                .HasOne(p => p.Movimento)
                .WithMany(p => p.Entradas);

            //SAÍDAS
            modelBuilder
                .Entity<Saida>()
                .Property(p => p.Responsável)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder
                .Entity<Saida>()
                .HasOne(p => p.Movimento)
                .WithMany(p => p.Saidas);

            modelBuilder
                .Entity<Fundo>()
                .HasKey(p => p.Id);

            modelBuilder
                .Entity<Fundo>()
                .HasIndex(p => p.Id)
                .IsUnique();

            modelBuilder
                .Entity<Fundo>()
                .Property(p => p.Id)
                .IsRequired();

            modelBuilder
                .Entity<Fundo>()
                .Property(p => p.Total)
                .HasPrecision(10, 2);

            modelBuilder
                .Entity<Fundo>()
                .HasData(new Fundo
                {
                    Id = "caixa",
                    Total = 0
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}