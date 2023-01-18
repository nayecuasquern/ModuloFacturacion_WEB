using Microsoft.AspNetCore.Mvc;
using ModuloFacturacion_WEB.Code;
using ModuloFacturacion_WEB.Models;

namespace ModuloFacturacion_WEB.Controllers
{
    public class PersonaController : Controller
    {

        string apiUrl = "https://apifacturacion1.azurewebsites.net/api/FactClients";

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Crear(string? id)
        {
            FactClient client = new FactClient();
            if (id == "" || id == null)
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
                if (cliente.CliIdentification == null || cliente.CliIdentification == "")
                {
                    var newdata = APIConsumer.CreateClient(apiUrl, cliente);
                    return RedirectToAction(nameof(Crear));
                }
                else
                {
                    APIConsumer.SaveClient(apiUrl, cliente.CliIdentification, cliente);
                    return RedirectToAction(nameof(Crear), new { cliIdentification = "" });


                }

            }
            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerDatos()
        {
            var datos = APIConsumer.Clients(apiUrl);
            return Json(new { data = datos });
        }
    }
}
