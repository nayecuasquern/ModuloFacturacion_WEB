var datatable;
var clientId = document.getElementById("clientId")

$(document).ready(function () {
    loadDataTable();

    var id = document.getElementById("clientId");
    
    if (id.value != "") {
        $('#myModal').modal('show');

    }
});
function Clean() {
    clientId.readOnly = false;
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
function Identi() {
    clientId.readOnly = true;
}
function loadDataTable() {
    datatable = $('#tblData').DataTable({

        "ajax": {

            "url": "/Persona/ObtenerDatos",
            "type": "POST"
        },
        "columns": [
            {
                "data": "cliIdentification",
                "render": function (data) {
                    clientId.readOnly = true;
                    return `
                    <div>
                        <a onclick="Identi()" href="/Persona/Crear/${data}"  class="btn btn-success text-white" style="cursor:pointer;">Editar</a>
                    </div>
                    `
                }
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
                            return "Credito";
                        }
                        return "X";

                    }
            }
            
        ]
    })

}