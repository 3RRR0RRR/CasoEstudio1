function showResult(r) {
    if (r && r.ok) {
        Swal.fire("Listo", r.msg || "Proceso realizado correctamente", "success");
    } else {
        Swal.fire("Atención", (r && (r.msg || r.message)) ? (r.msg || r.message) : "Ocurrió un error", "warning");
    }
}

let dtCitas;

$(document).ready(function () {

    dtCitas = $("#tblCitas").DataTable({
        ajax: { url: "/Citas/Listar", dataSrc: json => json.data },
        columns: [
            { data: "citaLavadoId" },
            { data: "cliente" },
            { data: "vehiculo" },
            { data: "fechaCita", render: d => new Date(d).toLocaleString() },
            { data: "estado" },
            {
                data: null,
                orderable: false,
                render: row => `
          <div class="d-flex gap-1 flex-wrap">
            <button type="button" class="btn btn-sm btn-outline-success btn-estado"
                    data-id="${row.citaLavadoId}" data-estado="Ingresada">
              Ingresada
            </button>

            <button type="button" class="btn btn-sm btn-outline-warning btn-estado"
                    data-id="${row.citaLavadoId}" data-estado="Cancelada">
              Cancelada
            </button>

            <button type="button" class="btn btn-sm btn-outline-primary btn-estado"
                    data-id="${row.citaLavadoId}" data-estado="Concluida">
              Concluida
            </button>

            <button type="button" class="btn btn-sm btn-outline-danger btn-eliminar"
                    data-id="${row.citaLavadoId}">
              Eliminar
            </button>
          </div>`
            }
        ]
    });

    $("#btnNueva").off("click").on("click", () => abrirModalCita());

    $(document).on("click", ".btn-estado", function () {
        const id = $(this).data("id");
        const nuevoEstado = $(this).data("estado");
        cambiarEstado(id, nuevoEstado);
    });

    $(document).on("click", ".btn-eliminar", function () {
        const id = $(this).data("id");
        eliminarCita(id);
    });
});

function abrirModalCita() {
    $("#modalCitaTitle").text("Nueva Cita");

    $.get("/Citas/Form")
        .done(html => {
            $("#modalCitaBody").html(html);
            new bootstrap.Modal(document.getElementById("modalCita")).show();

            $("#frmCita").off("submit").on("submit", function (e) {
                e.preventDefault();
                guardarCita();
            });
        })
        .fail(() => Swal.fire("Error", "No se pudo cargar el formulario.", "error"));
}

function guardarCita() {
    const data = $("#frmCita").serialize();
    $.ajax({
        url: "/Citas/Guardar",
        method: "POST",
        data,
        headers: { "X-Requested-With": "XMLHttpRequest" }
    })
        .done(res => {
            showResult(res);
            bootstrap.Modal.getInstance(document.getElementById("modalCita")).hide();
            dtCitas.ajax.reload(null, false);
        })
        .fail(xhr => {
            const r = xhr.responseJSON || { ok: false, msg: "Error al guardar." };
            showResult(r);
        });
}

function cambiarEstado(id, nuevoEstado) {
    $.ajax({
        url: "/Citas/CambiarEstado",
        method: "POST",
        data: { id: id, estado: nuevoEstado },
        headers: { "X-Requested-With": "XMLHttpRequest" }
    })
        .done(res => {
            showResult(res);
            dtCitas.ajax.reload(null, false);
        })
        .fail(xhr => {
            const r = xhr.responseJSON || { ok: false, msg: "No se pudo cambiar el estado." };
            showResult(r);
        });
}

function eliminarCita(id) {
    Swal.fire({
        title: "¿Eliminar cita?",
        text: "Esta acción no se puede deshacer.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Sí, eliminar",
        cancelButtonText: "Cancelar"
    }).then(r => {
        if (!r.isConfirmed) return;

        $.ajax({
            url: "/Citas/Eliminar",
            method: "POST",
            data: { id },
            headers: { "X-Requested-With": "XMLHttpRequest" }
        })
            .done(res => {
                showResult(res);
                dtCitas.ajax.reload(null, false);
            })
            .fail(xhr => {
                const r2 = xhr.responseJSON || { ok: false, msg: "No se pudo eliminar." };
                showResult(r2);
            });
    });
}