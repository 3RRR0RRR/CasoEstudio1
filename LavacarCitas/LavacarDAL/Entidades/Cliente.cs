using System.ComponentModel.DataAnnotations;

namespace LavacarDAL.Entidades
{
    public class Cliente
    {
        public int ClienteId { get; set; }

        [Required, StringLength(30)]
        public string Identificacion { get; set; } = string.Empty;

        [Required, StringLength(80)]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required]
        public DateTime FechaNacimiento { get; set; }

        [Required, StringLength(120)]
        public string Email { get; set; } = string.Empty;

        public ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
        public ICollection<CitaLavado> Citas { get; set; } = new List<CitaLavado>();
    }
}
