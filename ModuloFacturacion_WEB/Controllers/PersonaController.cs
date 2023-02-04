using Microsoft.AspNetCore.Mvc;
using ModuloFacturacion_WEB.Code;
using ModuloFacturacion_WEB.Extensions;
using ModuloFacturacion_WEB.Models;
using Rotativa.AspNetCore;


namespace ModuloFacturacion_WEB.Controllers
{
    public class PersonaController : BaseController
    {

        string apiUrl = "https://apifacturacion1.azurewebsites.net/api/FactClients";

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Crear(string? id)
        {
            FactClient client = new FactClient();
            if (id == null || id.Equals(""))
            {
                return View(client);

            }
            else
            {
                client = APIConsumer.Client(apiUrl, id);
                return View(client);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(FactClient cliente)
        {

            if (ModelState.IsValid)
            {
                var datos = APIConsumer.Clients(apiUrl);
                bool aux = true;

                foreach (var client in datos)
                {                    
                    if(client.CliIdentification.Equals(cliente.CliIdentification))
                    {
                        aux = false;
                        break;
                    }
                }

                if (aux)
                {
                    var newdata = APIConsumer.CreateClient(apiUrl, cliente);
                    return RedirectToAction(nameof(Crear), new { id = "" });
                }
                else
                {
                    APIConsumer.SaveClient(apiUrl, cliente.CliIdentification, cliente);
                    return RedirectToAction(nameof(Crear), new { id = "" });


                }

            }
            return View(cliente);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Crear(FactClient cliente)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        var client = APIConsumer.Client(apiUrl, cliente.);
        //        if (cliente.CliIdentification == null || cliente.CliIdentification == "")
        //        {
        //            var newdata = APIConsumer.CreateClient(apiUrl, cliente);
        //            return RedirectToAction(nameof(Crear));
        //        }
        //        else
        //        {
        //            APIConsumer.SaveClient(apiUrl, cliente.CliIdentification, cliente);
        //            return RedirectToAction(nameof(Crear), new { cliIdentification = "" });


        //        }

        //    }
        //    return View(cliente);
        //}
        public IActionResult ImprimirFacCli(string id)
        {          
            string con = "https://apifacturacion1.azurewebsites.net/api/FactInvoiceHeads/FacturasClientes";
            var data = APIConsumer.InvoiceHeadCli(con, id);

            if (data.Length == 0)
            {
                TempData["notification"] = "Swal.fire('Oops','El cliente no tiene facturas.', 'info')";
                return View("Crear");
            }
            string fecha = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"); ;
            return new ViewAsPdf("ImprimirFacCli", data)
            {
                FileName = $"{data.Last().CliIdentificationNavigation.CliIdentification}"+" "+fecha+".pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };
        }


        [HttpPost]
        public async Task<IActionResult> ObtenerDatos()
        {
            var datos = APIConsumer.Clients(apiUrl);
            return Json(new { data = datos });
        }
    }
}
