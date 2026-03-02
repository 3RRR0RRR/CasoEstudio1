function showResult(r){
  if(r && r.ok){
    Swal.fire("Listo", r.msg || "Proceso realizado correctamente", "success");
  }else{
    Swal.fire("Atención", (r && (r.msg || r.message)) ? (r.msg || r.message) : "Ocurrió un error", "warning");
  }
}

let dtClientes;

$(document).ready(function () {
  dtClientes = $("#tblClientes").DataTable({
    ajax: { url: "/Clientes/Listar", dataSrc: json => json.data },
    columns: [
      { data: "clienteId" },
      { data: "identificacion" },
      { data: "nombreCompleto" },
      { data: "fechaNacimiento", render: d => new Date(d).toLocaleDateString() },
      { data: "email" },
      {
        data: null, orderable: false,
        render: row => `
          <button class="btn btn-sm btn-outline-primary me-1" onclick="abrirModalCliente(${row.clienteId})">Editar</button>
          <button class="btn btn-sm btn-outline-danger" onclick="eliminarCliente(${row.clienteId})">Eliminar</button>`
      }
    ]
  });

  $("#btnNuevo").click(() => abrirModalCliente(0));
});

function abrirModalCliente(id) {
  $("#modalClienteTitle").text(id ? "Editar Cliente" : "Nuevo Cliente");

  $.get("/Clientes/Form", { id })
    .done(html => {
      $("#modalClienteBody").html(html);
      new bootstrap.Modal(document.getElementById("modalCliente")).show();

      $("#frmCliente").off("submit").on("submit", function (e) {
        e.preventDefault();
        guardarCliente();
      });
    })
    .fail(() => Swal.fire("Error", "No se pudo cargar el formulario.", "error"));
}

function guardarCliente() {
  const data = $("#frmCliente").serialize();
  $.ajax({
    url: "/Clientes/Guardar",
    method: "POST",
    data,
    headers: { "X-Requested-With": "XMLHttpRequest" }
  })
    .done(res => {
      showResult(res);
      bootstrap.Modal.getInstance(document.getElementById("modalCliente")).hide();
      dtClientes.ajax.reload(null, false);
    })
    .fail(xhr => {
      const r = xhr.responseJSON || { ok: false, msg: "Error al guardar." };
      showResult(r);
    });
}

function eliminarCliente(id) {
  Swal.fire({
    title: "¿Eliminar cliente?",
    text: "Esta acción no se puede deshacer.",
    icon: "warning",
    showCancelButton: true,
    confirmButtonText: "Sí, eliminar",
    cancelButtonText: "Cancelar"
  }).then(r => {
    if (!r.isConfirmed) return;

    $.ajax({
      url: "/Clientes/Eliminar",
      method: "POST",
      data: { id },
      headers: { "X-Requested-With": "XMLHttpRequest" }
    })
      .done(res => {
        showResult(res);
        dtClientes.ajax.reload(null, false);
      })
      .fail(xhr => {
        const r2 = xhr.responseJSON || { ok: false, msg: "No se pudo eliminar." };
        showResult(r2);
      });
  });
}
