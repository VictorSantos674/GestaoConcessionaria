using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DesafioIntelectah.Models;

namespace DesafioIntelectah.Models
{
    public class Venda : ISoftDelete
    {
        [Key]
        public int VendaId { get; set; }

        [Required(ErrorMessage = "A data da venda é obrigatória.")]
        [CustomValidation(typeof(Venda), nameof(ValidarDataVenda))]
        public DateTime DataVenda { get; set; }

        [Required(ErrorMessage = "O preço de venda é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço de venda deve ser positivo.")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PrecoVenda { get; set; }

    [Required(ErrorMessage = "O protocolo da venda é obrigatório.")]
    [MaxLength(20, ErrorMessage = "O protocolo deve ter no máximo 20 caracteres.")]
    public string ProtocoloVenda { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;


    [Required(ErrorMessage = "O veículo é obrigatório.")]
    public int VeiculoID { get; set; }
    public virtual Veiculo? Veiculo { get; set; }

    [Required(ErrorMessage = "A concessionária é obrigatória.")]
    public int ConcessionariaID { get; set; }
    public virtual Concessionaria? Concessionaria { get; set; }

    [Required(ErrorMessage = "O cliente é obrigatório.")]
    public int ClienteID { get; set; }
    public virtual Cliente? Cliente { get; set; }

        public static ValidationResult ValidarDataVenda(DateTime data, ValidationContext context)
        {
            if (data > DateTime.Now)
                return new ValidationResult("A data da venda não pode ser no futuro.");
            return ValidationResult.Success;
        }
    }
}
