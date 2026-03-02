using LavacarDAL.Entidades;

namespace LavacarDAL.Repositorios.Cita
{
    public interface ICitaRepositorio
    {
        Task<List<CitaLavado>> ListarAsync();
        Task<CitaLavado?> ObtenerAsync(int id);
        void Agregar(CitaLavado cita);
        void Actualizar(CitaLavado cita);
        void Eliminar(int id);
        Task<bool> GuardarAsync();
    }
}
