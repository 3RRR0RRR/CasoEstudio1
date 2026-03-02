using LavacarBLL.Dtos;
using LavacarBLL.Servicios.Cliente;
using LavacarBLL.Servicios.Vehiculo;
using Microsoft.AspNetCore.Mvc;

namespace LavacarWeb.Controllers
{
    public class VehiculosController : Controller
    {
        private readonly IVehiculoServicio _servicio;
        private readonly IClienteServicio _clientes;

        public VehiculosController(IVehiculoServicio servicio, IClienteServicio clientes)
        {
            _servicio = servicio;
            _clientes = clientes;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var vehs = await _servicio.ListarAsync();
            var clientes = await _clientes.ListarAsync();
            var map = clientes.ToDictionary(x => x.ClienteId, x => x.NombreCompleto);

            var data = vehs.Select(v => new
            {
                v.VehiculoId, v.Placa, v.Marca, v.Modelo, v.ClienteId,
                ClienteNombre = map.ContainsKey(v.ClienteId) ? map[v.ClienteId] : ""
            });

            return Json(new { ok = true, data });
        }

        [HttpGet]
        public async Task<IActionResult> Form(int? id)
        {
            VehiculoDto model = new();
            if (id.HasValue && id.Value > 0)
            {
                var found = await _servicio.ObtenerAsync(id.Value);
                if (found == null) return NotFound();
                model = found;
            }

            ViewBag.Clientes = await _clientes.ListarAsync();
            return PartialView("_Form", model);
        }

        [HttpPost]
        public async Task<IActionResult> Guardar(VehiculoDto model)
        {
        try
        {
            if (!ModelState.IsValid)
                            return BadRequest(new { ok = false, message = "Revise los campos del formulario." });
            
                        if (model.VehiculoId == 0) await _servicio.CrearAsync(model);
                        else await _servicio.EditarAsync(model);

    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        return Json(new { ok = true, msg = "Proceso realizado correctamente" });

    TempData["Mensaje"] = "Proceso realizado correctamente";
    TempData["Ok"] = true;
    return RedirectToAction("Index");
}
catch (Exception ex)
{
    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        return Json(new { ok = false, msg = ex.Message });

    TempData["Mensaje"] = ex.Message;
    TempData["Ok"] = false;
    return RedirectToAction("Index");
}

        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
        try
        {
            await _servicio.EliminarAsync(id);

    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        return Json(new { ok = true, msg = "Proceso realizado correctamente" });

    TempData["Mensaje"] = "Proceso realizado correctamente";
    TempData["Ok"] = true;
    return RedirectToAction("Index");
}
catch (Exception ex)
{
    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        return Json(new { ok = false, msg = ex.Message });

    TempData["Mensaje"] = ex.Message;
    TempData["Ok"] = false;
    return RedirectToAction("Index");
}

        }
    }
}
