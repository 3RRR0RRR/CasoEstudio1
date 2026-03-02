using System.ComponentModel.DataAnnotations;

namespace LavacarDAL.Entidades
{
    public class Vehiculo
    {
        public int VehiculoId { get; set; }

        [Required, StringLength(20)]
        public string Placa { get; set; } = string.Empty;

        [Required, StringLength(60)]
        public string Marca { get; set; } = string.Empty;

        [Required, StringLength(60)]
        public string Modelo { get; set; } = string.Empty;

        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
    }
}
