using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModuloFacturacion_WEB.Code;
using ModuloFacturacion_WEB.Models;

namespace ModuloFacturacion_WEB.Controllers
{
    public class ClientController : Controller
    {
        string apiUrl1 = "https://localhost:7039/api/FactClients";
        string apiUrl = "https://localhost:7164/api/FactClients";
        // GET: ClientController
        public ActionResult Index()
        {
            var data = APIConsumer.Clients(apiUrl);
            return View(data);
        }

        // GET: ClientController/Details/5
        public ActionResult Details(string id)
        {
            var data = APIConsumer.Client(apiUrl, id);
            return View(data);
        }

        // GET: ClientController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.FactClient client)
        {
            try
            {
                var newdata = APIConsumer.CreateClient(apiUrl, client);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(client);
            }
        }

        // GET: ClientController/Edit/5
        public ActionResult Edit(string id)
        {
            var data = APIConsumer.Client(apiUrl, id);
            return View(data);
        }

        // POST: ClientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, Models.FactClient client)
        {
            try
            {
                APIConsumer.SaveClient(apiUrl, id, client);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(client);
            }
        }

        // GET: ClientController/Delete/5
        public ActionResult Delete(string id)
        {
            var data = APIConsumer.Client(apiUrl, id);
            return View(data);
        }

        // POST: ClientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                APIConsumer.DeleteClient(apiUrl, id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
