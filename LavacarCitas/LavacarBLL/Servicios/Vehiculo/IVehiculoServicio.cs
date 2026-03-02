using LavacarBLL.Dtos;

namespace LavacarBLL.Servicios.Vehiculo
{
    public interface IVehiculoServicio
    {
        Task<List<VehiculoDto>> ListarAsync();
        Task<VehiculoDto?> ObtenerAsync(int id);
        Task CrearAsync(VehiculoDto dto);
        Task EditarAsync(VehiculoDto dto);
        Task EliminarAsync(int id);
    }
}
