using LavacarBLL.Dtos;
using LavacarDAL.Entidades;

namespace LavacarBLL.Servicios.Cita
{
    public interface ICitaServicio
    {
        Task<List<CitaLavadoListItem>> ListarAsync();
        Task CrearAsync(CitaLavadoDto dto);
        Task CambiarEstadoAsync(int id, EstadoCita estado);
        Task EliminarAsync(int id);
    }

    public class CitaLavadoListItem
    {
        public int CitaLavadoId { get; set; }
        public string Cliente { get; set; } = "";
        public string Vehiculo { get; set; } = "";
        public DateTime FechaCita { get; set; }
        public string Estado { get; set; } = "";
    }
}
