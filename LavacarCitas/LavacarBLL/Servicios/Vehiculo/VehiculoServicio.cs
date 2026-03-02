using AutoMapper;
using LavacarBLL.Dtos;
using LavacarDAL.Entidades;
using LavacarDAL.Repositorios.Vehiculo;
using VehiculoEntidad = LavacarDAL.Entidades.Vehiculo;

namespace LavacarBLL.Servicios.Vehiculo
{
    public class VehiculoServicio : IVehiculoServicio
    {
        private readonly IVehiculoRepositorio _repo;
        private readonly IMapper _mapper;

        public VehiculoServicio(IVehiculoRepositorio repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<VehiculoDto>> ListarAsync()
        {
            var list = await _repo.ListarAsync();
            return _mapper.Map<List<VehiculoDto>>(list);
        }

        public async Task<VehiculoDto?> ObtenerAsync(int id)
        {
            var entity = await _repo.ObtenerAsync(id);
            return entity == null ? null : _mapper.Map<VehiculoDto>(entity);
        }

        public async Task CrearAsync(VehiculoDto dto)
        {
            if (await _repo.ExistePlacaAsync(dto.Placa))
                throw new Exception("No se puede registrar un vehículo con la misma placa.");

            var entity = _mapper.Map<VehiculoEntidad>(dto);
            _repo.Agregar(entity);

            if (!await _repo.GuardarAsync())
                throw new Exception("No se pudo guardar el vehículo.");
        }

        public async Task EditarAsync(VehiculoDto dto)
        {
            if (await _repo.ExistePlacaAsync(dto.Placa, dto.VehiculoId))
                throw new Exception("No se puede registrar un vehículo con la misma placa.");

            var entity = await _repo.ObtenerAsync(dto.VehiculoId);
            if (entity == null) throw new Exception("Vehículo no encontrado.");

            if (entity.ClienteId != dto.ClienteId)
                throw new Exception("Un vehículo no puede tener 2 clientes. No se permite reasignarlo a otro cliente.");

            entity.Placa = dto.Placa;
            entity.Marca = dto.Marca;
            entity.Modelo = dto.Modelo;

            _repo.Actualizar(entity);

            if (!await _repo.GuardarAsync())
                throw new Exception("No se pudo actualizar el vehículo.");
        }

        public async Task EliminarAsync(int id)
        {
            _repo.Eliminar(id);
            if (!await _repo.GuardarAsync())
                throw new Exception("No se pudo eliminar el vehículo.");
        }
    }
}