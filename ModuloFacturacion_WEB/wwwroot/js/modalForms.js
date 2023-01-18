var datatable;

$(document).ready(function () {
    loadDataTable();

    var id = document.getElementById("clientId");

    if (id.value != "") {
        $('#myModal').modal('show');

    }
});

function loadDataTable() {
    datatable = $('#tblData').DataTable({

        "ajax": {

            "url": "/Persona/ObtenerDatos",
            "type": "POST"
        },
        "columns": [
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
                            return "Credito";
                        }
                        return "X";

                    }
            },
            {
                "data": "cliIdentification",
                "render": function (data) {

                    return `
                    <div>
                        <a href="/Persona/Crear/${data}" class="btn btn-success text-white" style="cursor:pointer;">Editar</a>
                    </div>
                    `
                }
            }
        ]
    })

}