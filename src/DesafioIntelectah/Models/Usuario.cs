using Microsoft.AspNetCore.Identity;
using DesafioIntelectah.Models;

namespace DesafioIntelectah.Models
{
    // Estende a classe de usuário padrão do Identity para adicionar
    // o nível de acesso customizado.
    public class ApplicationUser : IdentityUser
    {
        public NivelAcesso NivelAcesso { get; set; }
    }
}
