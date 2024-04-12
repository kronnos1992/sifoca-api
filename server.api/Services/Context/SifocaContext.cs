
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
        public DbSet<Entrada> Tb_Entrada { get; set; }
        public DbSet<Saida> Tb_Saida { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // MOVIMENTOS
            var passwordHasher = new PasswordHasher<IdentityUser>();
            var hashedPassword = passwordHasher.HashPassword(null, "master");

            //ENTRADAS
            modelBuilder
                .Entity<Entrada>()
                .Property(p => p.Operador)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder
                .Entity<Entrada>()
                .Property(p => p.ValorEntrada)
                .HasPrecision(18,2)
                .IsRequired();

            //SAÍDAS
            modelBuilder
                .Entity<Saida>()
                .Property(p => p.Responsável)
                .HasMaxLength(100)
                .IsRequired();
            
            modelBuilder
                .Entity<Saida>()
                .Property(p => p.ValorSaida)
                .HasPrecision(18,2)
                .IsRequired();

            modelBuilder
                .Entity<AppUserRole>()
                .HasKey(p => new { p.UserId, p.RoleId });

            modelBuilder
                .Entity<AppUserRole>()
                .HasOne(r => r.Role)
                .WithMany(u => u.Users)
                .HasForeignKey(fk => fk.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<AppUserRole>()
                .HasOne(r => r.User)
                .WithMany(u => u.Roles)
                .HasForeignKey(fk => fk.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AppUser>()
                .HasData(new AppUser
                {
                    Id = 1, 
                    NomeCompleto = "MASTER USER",
                    UserName = "master",
                    Departamento = "Geral",
                    NormalizedUserName = "MASTER",
                    SecurityStamp="JFLMT5UKYWNBGTZPC3VK57BSJJDHU",
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
                    NormalizedName = "MASTER",
                    DataRegistro = Generic.GetCurrentAngolaDateTime(),
                });

            modelBuilder
                .Entity<AppUserRole>()
                .HasData(new AppUserRole
                {
                    UserId = 1,
                    RoleId = 1,
                    DataRegistro = Generic.GetCurrentAngolaDateTime(),
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}