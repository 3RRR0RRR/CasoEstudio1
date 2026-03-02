using LavacarDAL.Data;
using Microsoft.EntityFrameworkCore;

namespace LavacarDAL.Repositorios.Generico
{
    public class RepositorioGenerico<T> : IRepositorioGenerico<T> where T : class
    {
        private readonly LavacarDbContext _context;
        private readonly DbSet<T> _dbSet;

        public RepositorioGenerico(LavacarDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Agregar(T entidad) => _dbSet.Add(entidad);
        public void Actualizar(T entidad) => _dbSet.Update(entidad);

        public void Eliminar(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null) _dbSet.Remove(entity);
        }

        public Task<T?> ObtenerPorIdAsync(int id) => _dbSet.FindAsync(id).AsTask();
        public Task<List<T>> ObtenerTodosAsync() => _dbSet.ToListAsync();

        public async Task<bool> GuardarCambiosAsync()
        {
            var r = await _context.SaveChangesAsync();
            return r > 0;
        }
    }
}
