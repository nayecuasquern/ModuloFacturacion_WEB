var datatable;

$(document).ready(function () {

    loadDataTable();   

});

function loadDataTable() {
    datatable = $('#tblData').DataTable({

        "ajax": {

            "url": "/Invoice/ObtenerDatos",
            "type": "POST"
        },
        "columns": [
            {
                "data": "invoiceHeadId",
                "render": function (data) {
                    return `
                    <div>
                        <a href="/Invoice/ImprimirFac/${data}" class="btn btn-info"">Imprimir</a>
                    </div>
                    `
                }
            },
            { "data": "invoiceHeadId", "whidth": "10" },
            {
                "data": "invoiceDate", "render":
                    function (data) {
                        var hoy = new Date(data);
                        dia = hoy.getDate();
                        mes = hoy.getMonth() + 1;
                        anio = hoy.getFullYear();
                        fecha_actual = String(dia + "/" + mes + "/" + anio);
                        return fecha_actual
                    }
            },
            { "data": "invoiceSubtotal", "whidth": "10" },
            { "data": "invoiceIva", "whidth": "10" },
            { "data": "invoiceTotal", "whidth": "10" },
            { "data": "cliIdentification", "whidth": "10" },
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