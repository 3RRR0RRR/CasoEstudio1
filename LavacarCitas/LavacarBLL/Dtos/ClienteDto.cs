using System.ComponentModel.DataAnnotations;

namespace LavacarBLL.Dtos
{
    public class ClienteDto
    {
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "La identificación es requerida.")]
        [StringLength(30)]
        public string Identificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre completo es requerido.")]
        [StringLength(80)]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es requerida.")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El email es requerido.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; } = string.Empty;
    }
}
