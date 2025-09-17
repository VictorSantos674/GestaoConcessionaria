using System.ComponentModel.DataAnnotations;

public class Concessionaria : ISoftDelete
{
    [Key]
    public int ConcessionariaId { get; set; }

    [Required(ErrorMessage = "O nome da concessionária é obrigatório.")]
    [MaxLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O endereço é obrigatório.")]
    [MaxLength(255, ErrorMessage = "O endereço deve ter no máximo 255 caracteres.")]
    public string Endereco { get; set; }

    [Required(ErrorMessage = "A cidade é obrigatória.")]
    [MaxLength(50, ErrorMessage = "A cidade deve ter no máximo 50 caracteres.")]
    public string Cidade { get; set; }

    [Required(ErrorMessage = "O estado é obrigatório.")]
    [MaxLength(50, ErrorMessage = "O estado deve ter no máximo 50 caracteres.")]
    public string Estado { get; set; }

    [Required(ErrorMessage = "O CEP é obrigatório.")]
    [MaxLength(10, ErrorMessage = "O CEP deve ter no máximo 10 caracteres.")]
    [RegularExpression(@"^\d{5}-?\d{3}$", ErrorMessage = "O CEP deve estar no formato 00000-000 ou 00000000.")]
    public string Cep { get; set; }

    [Required(ErrorMessage = "O telefone é obrigatório.")]
    [MaxLength(15, ErrorMessage = "O telefone deve ter no máximo 15 caracteres.")]
    [Phone(ErrorMessage = "O telefone deve ser válido.")]
    public string Telefone { get; set; }

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [MaxLength(100, ErrorMessage = "O e-mail deve ter no máximo 100 caracteres.")]
    [EmailAddress(ErrorMessage = "O e-mail deve ser válido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "A capacidade máxima de veículos é obrigatória.")]
    [Range(1, int.MaxValue, ErrorMessage = "A capacidade máxima deve ser um número positivo.")]
    public int CapacidadeMaximaVeiculos { get; set; }

    public bool IsDeleted { get; set; } = false;
}