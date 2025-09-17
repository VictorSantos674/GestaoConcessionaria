using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// --- MODELS ---
// É uma boa prática separar cada classe em seu próprio arquivo na pasta Models,
// mas para facilitar a visualização, elas estão aqui.

// Interface para Deleção Lógica
public interface ISoftDelete
{
    public bool IsDeleted { get; set; }
}

public enum TipoVeiculo
{
    Carro,
    Moto,
    Caminhao
}

public enum NivelAcesso
{
    Administrador,
    Vendedor,
    Gerente
}

// Classe de Usuário customizada que herda do Identity
public class ApplicationUser : IdentityUser
{
    public NivelAcesso NivelAcesso { get; set; }
}

public class Fabricante : ISoftDelete
{
    [Key]
    public int FabricanteId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; }

    [MaxLength(50)]
    public string PaisOrigem { get; set; }
    public int AnoFundacao { get; set; }

    [MaxLength(255)]
    public string Website { get; set; }

    public bool IsDeleted { get; set; } = false;

    // Propriedade de navegação: um fabricante tem muitos veículos
    public virtual ICollection<Veiculo> Veiculos { get; set; }
}

public class Veiculo : ISoftDelete
{
    [Key]
    public int VeiculoId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Modelo { get; set; }
    public int AnoFabricacao { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Preco { get; set; }

    public TipoVeiculo TipoVeiculo { get; set; }
    public string? Descricao { get; set; }
    public bool IsDeleted { get; set; } = false;

    // Chave Estrangeira para Fabricante
    public int FabricanteId { get; set; }
    public virtual Fabricante Fabricante { get; set; }
}

public class Concessionaria : ISoftDelete
{
    [Key]
    public int ConcessionariaId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; }

    [MaxLength(255)]
    public string Endereco { get; set; }
    [MaxLength(50)]
    public string Cidade { get; set; }
    [MaxLength(50)]
    public string Estado { get; set; }
    [MaxLength(10)]
    public string Cep { get; set; }
    [MaxLength(15)]
    public string Telefone { get; set; }
    [MaxLength(100)]
    public string Email { get; set; }
    public int CapacidadeMaximaVeiculos { get; set; }
    public bool IsDeleted { get; set; } = false;
}

public class Cliente : ISoftDelete
{
    [Key]
    public int ClienteId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; }

    [Required]
    [MaxLength(11)]
    public string Cpf { get; set; }

    [MaxLength(15)]
    public string Telefone { get; set; }
    public bool IsDeleted { get; set; } = false;
}

public class Venda : ISoftDelete
{
    [Key]
    public int VendaId { get; set; }
    public DateTime DataVenda { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal PrecoVenda { get; set; }

    [Required]
    [MaxLength(20)]
    public string ProtocoloVenda { get; set; }
    public bool IsDeleted { get; set; } = false;

    // Chaves Estrangeiras
    public int VeiculoId { get; set; }
    public virtual Veiculo Veiculo { get; set; }

    public int ConcessionariaId { get; set; }
    public virtual Concessionaria Concessionaria { get; set; }

    public int ClienteId { get; set; }
    public virtual Cliente Cliente { get; set; }
}


// --- DBCONTEXT ---

// Herda de IdentityDbContext para integrar com o sistema de autenticação do ASP.NET
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Mapeamento das suas classes para tabelas no banco de dados
    public DbSet<Fabricante> Fabricantes { get; set; }
    public DbSet<Veiculo> Veiculos { get; set; }
    public DbSet<Concessionaria> Concessionarias { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Venda> Vendas { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Essencial ao usar Identity

        // Configurações usando Fluent API

        // Configurar chaves únicas conforme especificado no desafio
        modelBuilder.Entity<Fabricante>().HasIndex(f => f.Nome).IsUnique();
        modelBuilder.Entity<Concessionaria>().HasIndex(c => c.Nome).IsUnique();
        modelBuilder.Entity<Cliente>().HasIndex(c => c.Cpf).IsUnique();
        modelBuilder.Entity<Venda>().HasIndex(v => v.ProtocoloVenda).IsUnique();
        
        // Configurar o relacionamento entre Veiculo e Fabricante (um-para-muitos)
        modelBuilder.Entity<Veiculo>()
            .HasOne(v => v.Fabricante)
            .WithMany(f => f.Veiculos)
            .HasForeignKey(v => v.FabricanteId);
            
        // Configuração Global para Deleção Lógica (Soft Delete)
        // Isso adiciona um filtro em TODAS as consultas para as entidades que implementam ISoftDelete,
        // garantindo que registros com IsDeleted = true nunca sejam retornados.
        modelBuilder.Entity<Fabricante>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Veiculo>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Concessionaria>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Cliente>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Venda>().HasQueryFilter(e => !e.IsDeleted);
    }
}
