var datatable;
var clientId = document.getElementById("clientId")

$(document).ready(function () {
    loadDataTable();

    var id = document.getElementById("clientId");

    if (id.value != "" && id.value != "in") {
        alert(id.value)
        $('#editModal').modal('show');
    }
    if (id.value == "in") {
        alert(id.value)
        $('#myModal').modal('show');
    }
});
function Clean() {
    var clientName = document.getElementById("clientName")
    var clientAddress = document.getElementById("clientAddress")
    var clientMail = document.getElementById("clientMail")
    var clientPhone = document.getElementById("clientPhone")
    var clientDate = document.getElementById("clientDate")
    var clientStatus = document.getElementById("clientStatus")
    var clientTyp = document.getElementById("clientTyp")

    clientId.value = "";
    clientName.value = "";
    clientAddress.value = "";
    clientMail.value = "";
    clientPhone.value = "";
    clientDate.value = "";
    clientStatus.value = "True";
    clientTyp.value = "1";
}

function loadDataTable() {
    var table = $('#tblData').DataTable();
    table.destroy();

    datatable = $('#tblData').DataTable({
        language: {
            "decimal": "",
            "emptyTable": "No hay información",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
            "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
            "infoFiltered": "(Filtrado de _MAX_ total entradas)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ Entradas",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            "zeroRecords": "Sin resultados encontrados",
            "paginate": {
                "first": "Primero",
                "last": "Ultimo",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "ajax": {

            "url": "/Persona/ObtenerDatos",
            "type": "POST"
        },
        "columns": [


            {
                "data": "cliIdentification",
                "render": function (data) {
                    return `
                    <div>

                        <a onclick="Identi()" href="/Persona/Crear/${data}"  class="btn btn-success text-white" style="cursor:pointer;">Editar</a>
                    </div>
                    `
                }, "whidth": "5" 
            },
            { "data": "cliIdentification", "whidth": "10" },
            { "data": "cliName", "whidth": "10" },
            { "data": "cliAddres", "whidth": "10" },
            { "data": "cliMail", "whidth": "10" },
            { "data": "cliPhone", "whidth": "10" },
            //{ "data": "cliBirthday", "whidth": "10" },
            {
                "data": "cliBirthday", "render":
                    function (data) {
                        var hoy = new Date(data);
                        dia = hoy.getDate();
                        mes = hoy.getMonth() + 1;
                        anio = hoy.getFullYear();
                        fecha_actual = String(dia + "/" + mes + "/" + anio);
                        return fecha_actual
                    }
            },
            {
                "data": "cliStatus", "render":
                    function (data) {
                        if (data == true) {
                            return "Activo";
                        }
                        else {
                            return "Inactivo";
                        }
                    }

            },
            {
                "data": "typId", "whidth": "10", "render":
                    function (data) {

                        if (data == 1) {
                            return "Efectivo";
                        }
                        if (data == 2) {
                            return "Crédito";
                        }
                        return "X";

                    }
            }

        ]
    })

}