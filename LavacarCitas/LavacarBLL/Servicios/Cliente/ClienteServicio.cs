using AutoMapper;
using LavacarBLL.Dtos;
using LavacarDAL.Entidades;
using LavacarDAL.Repositorios.Cliente;
using ClienteEntidad = LavacarDAL.Entidades.Cliente;

namespace LavacarBLL.Servicios.Cliente
{
    public class ClienteServicio : IClienteServicio
    {
        private readonly IClienteRepositorio _repo;
        private readonly IMapper _mapper;

        public ClienteServicio(IClienteRepositorio repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<ClienteDto>> ListarAsync()
        {
            var list = await _repo.ListarAsync();
            return _mapper.Map<List<ClienteDto>>(list);
        }

        public async Task<ClienteDto?> ObtenerAsync(int id)
        {
            var entity = await _repo.ObtenerAsync(id);
            return entity == null ? null : _mapper.Map<ClienteDto>(entity);
        }

        public async Task CrearAsync(ClienteDto dto)
        {
            ValidarMayorDeEdad(dto.FechaNacimiento);

            if (await _repo.ExisteIdentificacionAsync(dto.Identificacion))
                throw new Exception("No se puede registrar un cliente con la misma identificación.");

            var entity = _mapper.Map<ClienteEntidad>(dto);
            _repo.Agregar(entity);

            if (!await _repo.GuardarAsync())
                throw new Exception("No se pudo guardar el cliente.");
        }

        public async Task EditarAsync(ClienteDto dto)
        {
            ValidarMayorDeEdad(dto.FechaNacimiento);

            if (await _repo.ExisteIdentificacionAsync(dto.Identificacion, dto.ClienteId))
                throw new Exception("No se puede registrar un cliente con la misma identificación.");

            var entity = await _repo.ObtenerAsync(dto.ClienteId);
            if (entity == null) throw new Exception("Cliente no encontrado.");

            entity.Identificacion = dto.Identificacion;
            entity.NombreCompleto = dto.NombreCompleto;
            entity.FechaNacimiento = dto.FechaNacimiento;
            entity.Email = dto.Email;

            _repo.Actualizar(entity);

            if (!await _repo.GuardarAsync())
                throw new Exception("No se pudo actualizar el cliente.");
        }

        public async Task EliminarAsync(int id)
        {
            _repo.Eliminar(id);
            if (!await _repo.GuardarAsync())
                throw new Exception("No se pudo eliminar el cliente.");
        }

        private static void ValidarMayorDeEdad(DateTime fechaNacimiento)
        {
            var hoy = DateTime.Today;
            int edad = hoy.Year - fechaNacimiento.Year;
            if (fechaNacimiento.Date > hoy.AddYears(-edad)) edad--;

            if (edad < 18) throw new Exception("No puede registrar clientes menores de edad");
        }
    }
}