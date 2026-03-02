using AutoMapper;
using LavacarBLL.Dtos;
using LavacarDAL.Entidades;

namespace LavacarBLL
{
    public class MapeoClases : Profile
    {
        public MapeoClases()
        {
            CreateMap<Cliente, ClienteDto>().ReverseMap();
            CreateMap<Vehiculo, VehiculoDto>().ReverseMap();
            CreateMap<CitaLavado, CitaLavadoDto>().ReverseMap();
        }
    }
}
