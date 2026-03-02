namespace LavacarDAL.Repositorios.Generico
{
    public interface IRepositorioGenerico<T> where T : class
    {
        void Agregar(T entidad);
        void Actualizar(T entidad);
        void Eliminar(int id);
        Task<T?> ObtenerPorIdAsync(int id);
        Task<List<T>> ObtenerTodosAsync();
        Task<bool> GuardarCambiosAsync();
    }
}
