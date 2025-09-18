using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DesafioIntelectah.Models;

namespace DesafioIntelectah.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = true;
        }

    public DbSet<Fabricante> Fabricantes { get; set; } = null!;
    public DbSet<Veiculo> Veiculos { get; set; } = null!;
    public DbSet<Concessionaria> Concessionarias { get; set; } = null!;
    public DbSet<Cliente> Clientes { get; set; } = null!;
    public DbSet<Venda> Vendas { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 

            modelBuilder.Entity<Fabricante>().HasIndex(f => f.Nome).IsUnique();
            modelBuilder.Entity<Concessionaria>().HasIndex(c => c.Nome).IsUnique();
            modelBuilder.Entity<Cliente>().HasIndex(c => c.CPF).IsUnique();
            modelBuilder.Entity<Venda>().HasIndex(v => v.ProtocoloVenda).IsUnique();

            modelBuilder.Entity<Veiculo>()
                .HasOne(v => v.Fabricante)
                .WithMany(f => f.Veiculos)
                .HasForeignKey(v => v.FabricanteID);

            modelBuilder.Entity<Fabricante>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Veiculo>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Concessionaria>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Cliente>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Venda>().HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
