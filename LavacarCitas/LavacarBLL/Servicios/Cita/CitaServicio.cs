using AutoMapper;
using LavacarBLL.Dtos;
using LavacarDAL.Entidades;
using LavacarDAL.Repositorios.Cita;
using LavacarDAL.Repositorios.Cliente;
using LavacarDAL.Repositorios.Vehiculo;

namespace LavacarBLL.Servicios.Cita
{
    public class CitaServicio : ICitaServicio
    {
        private readonly ICitaRepositorio _repo;
        private readonly IClienteRepositorio _clientes;
        private readonly IVehiculoRepositorio _vehiculos;
        private readonly IMapper _mapper;

        public CitaServicio(ICitaRepositorio repo, IClienteRepositorio clientes, IVehiculoRepositorio vehiculos, IMapper mapper)
        {
            _repo = repo;
            _clientes = clientes;
            _vehiculos = vehiculos;
            _mapper = mapper;
        }

        public async Task<List<CitaLavadoListItem>> ListarAsync()
        {
            var list = await _repo.ListarAsync();
            return list.Select(x => new CitaLavadoListItem
            {
                CitaLavadoId = x.CitaLavadoId,
                Cliente = x.Cliente?.NombreCompleto ?? "",
                Vehiculo = x.Vehiculo != null ? $"{x.Vehiculo.Placa} - {x.Vehiculo.Marca} {x.Vehiculo.Modelo}" : "",
                FechaCita = x.FechaCita,
                Estado = x.Estado.ToString()
            }).ToList();
        }

        public async Task CrearAsync(CitaLavadoDto dto)
        {
            var cliente = await _clientes.ObtenerAsync(dto.ClienteId);
            if (cliente == null) throw new Exception("Debe seleccionar un cliente válido.");

            var vehiculo = await _vehiculos.ObtenerAsync(dto.VehiculoId);
            if (vehiculo == null) throw new Exception("Debe seleccionar un vehículo válido.");

            if (vehiculo.ClienteId != dto.ClienteId)
                throw new Exception("El vehículo seleccionado no pertenece al cliente indicado.");

            var entity = _mapper.Map<CitaLavado>(dto);
            entity.Estado = EstadoCita.Ingresada;

            _repo.Agregar(entity);
            if (!await _repo.GuardarAsync()) throw new Exception("No se pudo registrar la cita.");
        }

        public async Task CambiarEstadoAsync(int id, EstadoCita estado)
        {
            var entity = await _repo.ObtenerAsync(id);
            if (entity == null) throw new Exception("Cita no encontrada.");

            entity.Estado = estado;
            _repo.Actualizar(entity);

            if (!await _repo.GuardarAsync()) throw new Exception("No se pudo cambiar el estado de la cita.");
        }

        public async Task EliminarAsync(int id)
        {
            _repo.Eliminar(id);
            if (!await _repo.GuardarAsync()) throw new Exception("No se pudo eliminar la cita.");
        }
    }
}
