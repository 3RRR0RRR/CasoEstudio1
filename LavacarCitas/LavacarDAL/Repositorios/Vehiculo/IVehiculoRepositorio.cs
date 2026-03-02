using LavacarDAL.Entidades;
using VehiculoEntidad = LavacarDAL.Entidades.Vehiculo;

namespace LavacarDAL.Repositorios.Vehiculo
{
    public interface IVehiculoRepositorio
    {
        Task<List<VehiculoEntidad>> ListarAsync();
        Task<VehiculoEntidad?> ObtenerAsync(int id);
        Task<bool> ExistePlacaAsync(string placa, int? excluirId = null);
        void Agregar(VehiculoEntidad vehiculo);
        void Actualizar(VehiculoEntidad vehiculo);
        void Eliminar(int id);
        Task<bool> GuardarAsync();
    }
}