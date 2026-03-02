using LavacarDAL.Data;
using LavacarDAL.Entidades;
using Microsoft.EntityFrameworkCore;
using ClienteEntidad = LavacarDAL.Entidades.Cliente;

namespace LavacarDAL.Repositorios.Cliente
{
    public class ClienteRepositorio : IClienteRepositorio
    {
        private readonly LavacarDbContext _context;
        public ClienteRepositorio(LavacarDbContext context) => _context = context;

        public Task<List<ClienteEntidad>> ListarAsync() =>
            _context.Clientes.AsNoTracking().OrderByDescending(x => x.ClienteId).ToListAsync();

        public Task<ClienteEntidad?> ObtenerAsync(int id) =>
            _context.Clientes.FirstOrDefaultAsync(x => x.ClienteId == id);

        public Task<bool> ExisteIdentificacionAsync(string identificacion, int? excluirId = null) =>
            _context.Clientes.AnyAsync(x => x.Identificacion == identificacion && (!excluirId.HasValue || x.ClienteId != excluirId.Value));

        public void Agregar(ClienteEntidad cliente) => _context.Clientes.Add(cliente);
        public void Actualizar(ClienteEntidad cliente) => _context.Clientes.Update(cliente);

        public void Eliminar(int id)
        {
            var entity = _context.Clientes.Find(id);
            if (entity != null) _context.Clientes.Remove(entity);
        }

        public async Task<bool> GuardarAsync() => (await _context.SaveChangesAsync()) > 0;
    }
}