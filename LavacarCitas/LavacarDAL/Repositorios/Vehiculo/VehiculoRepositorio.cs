using LavacarDAL.Data;
using LavacarDAL.Entidades;
using Microsoft.EntityFrameworkCore;
using VehiculoEntidad = LavacarDAL.Entidades.Vehiculo;

namespace LavacarDAL.Repositorios.Vehiculo
{
    public class VehiculoRepositorio : IVehiculoRepositorio
    {
        private readonly LavacarDbContext _context;
        public VehiculoRepositorio(LavacarDbContext context) => _context = context;

        public Task<List<VehiculoEntidad>> ListarAsync() =>
            _context.Vehiculos.Include(x => x.Cliente)
                .AsNoTracking()
                .OrderByDescending(x => x.VehiculoId)
                .ToListAsync();

        public Task<VehiculoEntidad?> ObtenerAsync(int id) =>
            _context.Vehiculos.Include(x => x.Cliente).FirstOrDefaultAsync(x => x.VehiculoId == id);

        public Task<bool> ExistePlacaAsync(string placa, int? excluirId = null) =>
            _context.Vehiculos.AnyAsync(x => x.Placa == placa && (!excluirId.HasValue || x.VehiculoId != excluirId.Value));

        public void Agregar(VehiculoEntidad vehiculo) => _context.Vehiculos.Add(vehiculo);
        public void Actualizar(VehiculoEntidad vehiculo) => _context.Vehiculos.Update(vehiculo);

        public void Eliminar(int id)
        {
            var entity = _context.Vehiculos.Find(id);
            if (entity != null) _context.Vehiculos.Remove(entity);
        }

        public async Task<bool> GuardarAsync() => (await _context.SaveChangesAsync()) > 0;
    }
}