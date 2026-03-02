using System.ComponentModel.DataAnnotations;
using LavacarDAL.Entidades;

namespace LavacarBLL.Dtos
{
    public class CitaLavadoDto
    {
        public int CitaLavadoId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un cliente.")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un vehículo.")]
        public int VehiculoId { get; set; }

        [Required(ErrorMessage = "La fecha de la cita es requerida.")]
        public DateTime FechaCita { get; set; }

        public EstadoCita Estado { get; set; } = EstadoCita.Ingresada;
    }
}
