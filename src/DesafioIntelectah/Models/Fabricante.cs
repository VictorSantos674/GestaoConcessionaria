using System.ComponentModel.DataAnnotations;
using DesafioIntelectah.Models;

namespace DesafioIntelectah.Models
{
    public class Fabricante : ISoftDelete
    {
        [Key]
    public int FabricanteID { get; set; }

        [Required(ErrorMessage = "O nome do fabricante é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        // Unicidade será validada no controller/service
        public string Nome { get; set; }

        [Required(ErrorMessage = "O país de origem é obrigatório.")]
        [MaxLength(50, ErrorMessage = "O país de origem deve ter no máximo 50 caracteres.")]
        public string PaisOrigem { get; set; }

        [Required(ErrorMessage = "O ano de fundação é obrigatório.")]
        [Range(1800, 2100, ErrorMessage = "Ano de fundação deve ser válido e no passado.")]
        [CustomValidation(typeof(Fabricante), nameof(ValidarAnoFundacao))]
        public int AnoFundacao { get; set; }

        [Required(ErrorMessage = "O website é obrigatório.")]
        [MaxLength(255, ErrorMessage = "O website deve ter no máximo 255 caracteres.")]
        [Url(ErrorMessage = "O website deve ser uma URL válida.")]
        public string Website { get; set; }

        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<Veiculo> Veiculos { get; set; } = new List<Veiculo>();

        public static ValidationResult ValidarAnoFundacao(int ano, ValidationContext context)
        {
            if (ano > DateTime.Now.Year)
                return new ValidationResult("Ano de fundação não pode ser no futuro.");
            return ValidationResult.Success;
        }
    }
}
