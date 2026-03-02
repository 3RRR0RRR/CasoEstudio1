using LavacarBLL.Dtos;
using LavacarBLL.Servicios.Cita;
using LavacarBLL.Servicios.Cliente;
using LavacarBLL.Servicios.Vehiculo;
using LavacarDAL.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace LavacarWeb.Controllers
{
    public class CitasController : Controller
    {
        private readonly ICitaServicio _servicio;
        private readonly IClienteServicio _clientes;
        private readonly IVehiculoServicio _vehiculos;

        public CitasController(ICitaServicio servicio, IClienteServicio clientes, IVehiculoServicio vehiculos)
        {
            _servicio = servicio;
            _clientes = clientes;
            _vehiculos = vehiculos;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var data = await _servicio.ListarAsync();
            return Json(new { ok = true, data });
        }

        [HttpGet]
        public async Task<IActionResult> Form()
        {
            ViewBag.Clientes = await _clientes.ListarAsync();
            ViewBag.Vehiculos = await _vehiculos.ListarAsync();

            var model = new CitaLavadoDto { FechaCita = DateTime.Now.AddDays(1) };
            return PartialView("_Form", model);
        }

        [HttpPost]
        public async Task<IActionResult> Guardar(CitaLavadoDto model)
        {
        try
        {
            if (!ModelState.IsValid)
                            return BadRequest(new { ok = false, message = "Revise los campos del formulario." });
            
                        await _servicio.CrearAsync(model);

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
        public async Task<IActionResult> CambiarEstado(int id, EstadoCita estado)
        {
        try
        {
            await _servicio.CambiarEstadoAsync(id, estado);

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
