
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using server.api.Models;
using server.api.Services.Functions;

namespace server.api.Services.Context
{
    public class SifocaContext : IdentityDbContext<AppUser, AppRole, int,
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public SifocaContext(DbContextOptions<SifocaContext> options) : base(options)
        {

        }


        public DbSet<Movimento> Tb_Movimento { get; set; }
        public DbSet<Entrada> Tb_Entrada { get; set; }
        public DbSet<Saida> Tb_Saida { get; set; }
        public DbSet<Fundo> Tb_Fundo { get; set; }

        protected override async void OnModelCreating(ModelBuilder modelBuilder)
        {
            // MOVIMENTOS
            var passwordHasher = new PasswordHasher<IdentityUser>();
            var hashedPassword = passwordHasher.HashPassword(null, "master");

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
                .HasPrecision(18, 2)
                .IsRequired();

            modelBuilder
                .Entity<Movimento>()
                .Property(p => p.Caixa)
                .HasPrecision(18, 2)
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


            // FUNDO

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

            modelBuilder
                .Entity<AppUserRole>()
                .HasKey(p => new { p.UserId, p.RoleId });

            modelBuilder
                .Entity<AppUserRole>()
                .HasOne(r => r.Role)
                .WithMany(u => u.Users)
                .HasForeignKey(fk => fk.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<AppUserRole>()
                .HasOne(r => r.User)
                .WithMany(u => u.Roles)
                .HasForeignKey(fk => fk.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppUser>()
                .HasData(new AppUser
                {
                    Id = 1,
                    NomeCompleto = "MASTER USER",
                    UserName = "master",
                    Departamento = "Geral",
                    NormalizedUserName= "MASTER",
                    Email = "master@sifoca.ao",
                    PhoneNumber = "0000000",
                    PasswordHash = hashedPassword,
                    DataNascimento = Generic.GetCurrentAngolaDateTime(),
                    DataRegistro = Generic.GetCurrentAngolaDateTime(),
                    DataAtualizacao = null
                });

            //await Generic.SeedInitialDataAsync();

            modelBuilder
                .Entity<AppRole>()
                .HasData(new AppRole
                {
                    Id = 1,
                    Name = "MASTER",
                    DataRegistro = Generic.GetCurrentAngolaDateTime(),
                });

            modelBuilder
                .Entity<AppUserRole>()
                .HasData(new AppUserRole
                {
                    UserId = 1,
                    RoleId = 1,
                    
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}