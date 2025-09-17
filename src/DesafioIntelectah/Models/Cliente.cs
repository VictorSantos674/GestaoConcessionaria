using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class Cliente : ISoftDelete
{
    [Key]
    public int ClienteId { get; set; }

    [Required(ErrorMessage = "O nome do cliente é obrigatório.")]
    [MaxLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O CPF é obrigatório.")]
    [MaxLength(14, ErrorMessage = "O CPF deve ter no máximo 14 caracteres.")]
    [CustomValidation(typeof(Cliente), nameof(ValidarCpf))]
    public string CPF { get; set; }

    [Required(ErrorMessage = "O telefone é obrigatório.")]
    [MaxLength(15, ErrorMessage = "O telefone deve ter no máximo 15 caracteres.")]
    [Phone(ErrorMessage = "O telefone deve ser válido.")]
    public string Telefone { get; set; }

    public bool IsDeleted { get; set; } = false;

    public static ValidationResult ValidarCpf(string cpf, ValidationContext context)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return new ValidationResult("O CPF é obrigatório.");
        var cpfNumeros = Regex.Replace(cpf, "[^0-9]", "");
        if (cpfNumeros.Length != 11)
            return new ValidationResult("O CPF deve conter 11 dígitos.");
        if (new string(cpfNumeros[0], 11) == cpfNumeros)
            return new ValidationResult("CPF inválido.");
        int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        string tempCpf, digito;
        int soma, resto;
        tempCpf = cpfNumeros.Substring(0, 9);
        soma = 0;
        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;
        digito = resto.ToString();
        tempCpf += digito;
        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;
        digito += resto.ToString();
        if (!cpfNumeros.EndsWith(digito))
            return new ValidationResult("CPF inválido.");
        return ValidationResult.Success;
    }
}

