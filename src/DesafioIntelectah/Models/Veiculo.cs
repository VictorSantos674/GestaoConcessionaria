using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DesafioIntelectah.Models;

namespace DesafioIntelectah.Models
{
    public class Veiculo : ISoftDelete
    {
        [Key]
    public int VeiculoID { get; set; }

        [Required(ErrorMessage = "O modelo do veículo é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O modelo deve ter no máximo 100 caracteres.")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "O ano de fabricação é obrigatório.")]
        [Range(1900, 2100, ErrorMessage = "Ano de fabricação deve ser válido.")]
        [CustomValidation(typeof(Veiculo), nameof(ValidarAnoFabricacao))]
        public int AnoFabricacao { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser um valor positivo.")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "O tipo de veículo é obrigatório.")]
        public TipoVeiculo TipoVeiculo { get; set; }

        [MaxLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O fabricante é obrigatório.")]
    public int FabricanteID { get; set; }
        public virtual Fabricante Fabricante { get; set; }

        public bool IsDeleted { get; set; } = false;

        public static ValidationResult ValidarAnoFabricacao(int ano, ValidationContext context)
        {
            if (ano > DateTime.Now.Year)
                return new ValidationResult("Ano de fabricação não pode ser no futuro.");
            return ValidationResult.Success;
        }
    }
}
