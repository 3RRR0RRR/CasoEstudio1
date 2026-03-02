using LavacarDAL.Entidades;
using ClienteEntidad = LavacarDAL.Entidades.Cliente;

namespace LavacarDAL.Repositorios.Cliente
{
    public interface IClienteRepositorio
    {
        Task<List<ClienteEntidad>> ListarAsync();
        Task<ClienteEntidad?> ObtenerAsync(int id);
        Task<bool> ExisteIdentificacionAsync(string identificacion, int? excluirId = null);
        void Agregar(ClienteEntidad cliente);
        void Actualizar(ClienteEntidad cliente);
        void Eliminar(int id);
        Task<bool> GuardarAsync();
    }
}