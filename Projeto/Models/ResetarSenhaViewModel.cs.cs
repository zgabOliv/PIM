using System.ComponentModel.DataAnnotations;

namespace Projeto.Models
{
    public class ResetarSenhaViewModel
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha precisa ter pelo menos 6 caracteres.")]
        [DataType(DataType.Password)]
        public string NovaSenha { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(NovaSenha), ErrorMessage = "As senhas não conferem.")]
        public string ConfirmarSenha { get; set; }
    }
}
