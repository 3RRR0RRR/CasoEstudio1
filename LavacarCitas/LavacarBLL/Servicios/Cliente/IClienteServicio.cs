using LavacarBLL.Dtos;

namespace LavacarBLL.Servicios.Cliente
{
    public interface IClienteServicio
    {
        Task<List<ClienteDto>> ListarAsync();
        Task<ClienteDto?> ObtenerAsync(int id);
        Task CrearAsync(ClienteDto dto);
        Task EditarAsync(ClienteDto dto);
        Task EliminarAsync(int id);
    }
}
