using LavacarDAL.Data;
using LavacarDAL.Entidades;
using Microsoft.EntityFrameworkCore;

namespace LavacarDAL.Repositorios.Cita
{
    public class CitaRepositorio : ICitaRepositorio
    {
        private readonly LavacarDbContext _context;
        public CitaRepositorio(LavacarDbContext context) => _context = context;

        public Task<List<CitaLavado>> ListarAsync() =>
            _context.CitasLavado.Include(x => x.Cliente).Include(x => x.Vehiculo)
                .AsNoTracking().OrderByDescending(x => x.CitaLavadoId).ToListAsync();

        public Task<CitaLavado?> ObtenerAsync(int id) =>
            _context.CitasLavado.Include(x => x.Cliente).Include(x => x.Vehiculo)
                .FirstOrDefaultAsync(x => x.CitaLavadoId == id);

        public void Agregar(CitaLavado cita) => _context.CitasLavado.Add(cita);
        public void Actualizar(CitaLavado cita) => _context.CitasLavado.Update(cita);

        public void Eliminar(int id)
        {
            var entity = _context.CitasLavado.Find(id);
            if (entity != null) _context.CitasLavado.Remove(entity);
        }

        public async Task<bool> GuardarAsync() => (await _context.SaveChangesAsync()) > 0;
    }
}
