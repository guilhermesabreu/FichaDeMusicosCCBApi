using FichaDeMusicosCCB.Domain.Entities;
using FichaDeMusicosCCB.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FichaDeMusicosCCB.Persistence
{
    public class FichaDeMusicosCCBContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>,
                                                    UserRole, IdentityUserLogin<int>,
                                                    IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public FichaDeMusicosCCBContext(DbContextOptions<FichaDeMusicosCCBContext> options) : base(options) { }

        public DbSet<Ocorrencia> Ocorrencias { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Hino> Hinos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region User
            #region N Users para N Roles
            modelBuilder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                 .WithMany(r => r.UserRoles)
                 .HasForeignKey(ur => ur.RoleId)
                 .IsRequired();

                userRole.HasOne(ur => ur.User)
                 .WithMany(r => r.UserRoles)
                 .HasForeignKey(ur => ur.UserId)
                 .IsRequired();
            }
            );
            #endregion

            #region 1 User para 1 Pessoa
            modelBuilder.Entity<User>()
            .HasOne(a => a.Pessoa)
            .WithOne(b => b.User)
            .HasForeignKey<Pessoa>(b => b.IdUser);
            #endregion
            #endregion

            #region Ocorrencias
            #region Chave Primária
            modelBuilder.Entity<Ocorrencia>()
                .HasKey(o => o.IdOcorrencia);
            #endregion

            #region 1 Ocorrencia para N Hinos
            modelBuilder.Entity<Ocorrencia>()
                .HasMany(h => h.Hinos)
                .WithOne(h => h.Ocorrencia);
            #endregion

            #region N Ocorrências para N Pessoas
            modelBuilder.Entity<PessoaOcorrencia>()
            .HasKey(bc => new { bc.IdPessoa, bc.IdOcorrencia });

            modelBuilder.Entity<PessoaOcorrencia>()
                .HasOne(bc => bc.Pessoa)
                .WithMany(b => b.PessoaOcorrencias)
                .HasForeignKey(bc => bc.IdPessoa);

            modelBuilder.Entity<PessoaOcorrencia>()
                .HasOne(bc => bc.Ocorrencia)
                .WithMany(c => c.PessoaOcorrencias)
                .HasForeignKey(bc => bc.IdOcorrencia);
            #endregion
            #endregion

            #region Hino
            #region Chave Primária
            modelBuilder.Entity<Hino>()
                .HasKey(h => h.IdHino);
            #endregion
            #endregion

            #region Pessoa
            #region Chave Primária
            modelBuilder.Entity<Pessoa>()
                .HasKey(h => h.IdPessoa);
            #endregion
            #endregion

        }
    }
}