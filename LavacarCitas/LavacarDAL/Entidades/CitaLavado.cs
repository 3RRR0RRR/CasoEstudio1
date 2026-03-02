using System.ComponentModel.DataAnnotations;

namespace LavacarDAL.Entidades
{
    public enum EstadoCita
    {
        Ingresada = 1,
        Cancelada = 2,
        Concluida = 3
    }

    public class CitaLavado
    {
        public int CitaLavadoId { get; set; }

        [Required]
        public DateTime FechaCita { get; set; }

        [Required]
        public EstadoCita Estado { get; set; } = EstadoCita.Ingresada;

        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        public int VehiculoId { get; set; }
        public Vehiculo? Vehiculo { get; set; }
    }
}
