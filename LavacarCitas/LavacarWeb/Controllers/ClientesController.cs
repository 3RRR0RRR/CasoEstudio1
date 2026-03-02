using LavacarBLL.Dtos;
using LavacarBLL.Servicios.Cliente;
using Microsoft.AspNetCore.Mvc;

namespace LavacarWeb.Controllers
{
    public class ClientesController : Controller
    {
        private readonly IClienteServicio _servicio;
        public ClientesController(IClienteServicio servicio) => _servicio = servicio;

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var data = await _servicio.ListarAsync();
            return Json(new { ok = true, data });
        }

        [HttpGet]
        public async Task<IActionResult> Form(int? id)
        {
            ClienteDto model = new();
            if (id.HasValue && id.Value > 0)
            {
                var found = await _servicio.ObtenerAsync(id.Value);
                if (found == null) return NotFound();
                model = found;
            }
            return PartialView("_Form", model);
        }

        [HttpPost]
        public async Task<IActionResult> Guardar(ClienteDto model)
        {
        try
        {
            if (!ModelState.IsValid)
                            return BadRequest(new { ok = false, message = "Revise los campos del formulario." });
            
                        if (model.ClienteId == 0) await _servicio.CrearAsync(model);
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
