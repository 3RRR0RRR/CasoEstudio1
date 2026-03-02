function showResult(r){
  if(r && r.ok){
    Swal.fire("Listo", r.msg || "Proceso realizado correctamente", "success");
  }else{
    Swal.fire("Atención", (r && (r.msg || r.message)) ? (r.msg || r.message) : "Ocurrió un error", "warning");
  }
}

let dtVehiculos;

$(document).ready(function () {
  dtVehiculos = $("#tblVehiculos").DataTable({
    ajax: { url: "/Vehiculos/Listar", dataSrc: json => json.data },
    columns: [
      { data: "vehiculoId" },
      { data: "placa" },
      { data: "marca" },
      { data: "modelo" },
      { data: "clienteNombre" },
      {
        data: null, orderable: false,
        render: row => `
          <button class="btn btn-sm btn-outline-primary me-1" onclick="abrirModalVehiculo(${row.vehiculoId})">Editar</button>
          <button class="btn btn-sm btn-outline-danger" onclick="eliminarVehiculo(${row.vehiculoId})">Eliminar</button>`
      }
    ]
  });

  $("#btnNuevo").click(() => abrirModalVehiculo(0));
});

function abrirModalVehiculo(id) {
  $("#modalVehiculoTitle").text(id ? "Editar Vehículo" : "Nuevo Vehículo");

  $.get("/Vehiculos/Form", { id })
    .done(html => {
      $("#modalVehiculoBody").html(html);
      new bootstrap.Modal(document.getElementById("modalVehiculo")).show();

      $("#frmVehiculo").off("submit").on("submit", function (e) {
        e.preventDefault();
        guardarVehiculo();
      });
    })
    .fail(() => Swal.fire("Error", "No se pudo cargar el formulario.", "error"));
}

function guardarVehiculo() {
  const data = $("#frmVehiculo").serialize();
  $.ajax({
    url: "/Vehiculos/Guardar",
    method: "POST",
    data,
    headers: { "X-Requested-With": "XMLHttpRequest" }
  })
    .done(res => {
      showResult(res);
      bootstrap.Modal.getInstance(document.getElementById("modalVehiculo")).hide();
      dtVehiculos.ajax.reload(null, false);
    })
    .fail(xhr => {
      const r = xhr.responseJSON || { ok: false, msg: "Error al guardar." };
      showResult(r);
    });
}

function eliminarVehiculo(id) {
  Swal.fire({
    title: "¿Eliminar vehículo?",
    text: "Esta acción no se puede deshacer.",
    icon: "warning",
    showCancelButton: true,
    confirmButtonText: "Sí, eliminar",
    cancelButtonText: "Cancelar"
  }).then(r => {
    if (!r.isConfirmed) return;

    $.ajax({
      url: "/Vehiculos/Eliminar",
      method: "POST",
      data: { id },
      headers: { "X-Requested-With": "XMLHttpRequest" }
    })
      .done(res => {
        showResult(res);
        dtVehiculos.ajax.reload(null, false);
      })
      .fail(xhr => {
        const r2 = xhr.responseJSON || { ok: false, msg: "No se pudo eliminar." };
        showResult(r2);
      });
  });
}
