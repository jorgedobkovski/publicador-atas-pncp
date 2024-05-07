using System.ComponentModel.DataAnnotations;

namespace PublicadorARP.Models.Dtos
{
    public class RegistroUsuarioDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
    }
}
