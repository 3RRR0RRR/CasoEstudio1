using System.ComponentModel.DataAnnotations;

namespace LavacarBLL.Dtos
{
    public class VehiculoDto
    {
        public int VehiculoId { get; set; }

        [Required(ErrorMessage = "La placa es requerida.")]
        [StringLength(20)]
        public string Placa { get; set; } = string.Empty;

        [Required(ErrorMessage = "La marca es requerida.")]
        [StringLength(60)]
        public string Marca { get; set; } = string.Empty;

        [Required(ErrorMessage = "El modelo es requerido.")]
        [StringLength(60)]
        public string Modelo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar un cliente.")]
        public int ClienteId { get; set; }
    }
}
