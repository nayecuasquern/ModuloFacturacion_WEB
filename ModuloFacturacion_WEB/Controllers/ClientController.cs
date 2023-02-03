using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using ModuloFacturacion_WEB.Code;
using ModuloFacturacion_WEB.Models;
using Rotativa.AspNetCore;

namespace ModuloFacturacion_WEB.Controllers
{
    public class ClientController : Controller
    {

        string apiUrl = "https://apifacturacion1.azurewebsites.net/api/FactClients";
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

        public IActionResult Create_partial()
        {
            FactClient client = new FactClient();
            return PartialView("_CreateClientView", client);
        }
        public IActionResult Create_partialm(Models.FactClient client)
        {
            try
            {
                var newdata = APIConsumer.CreateClient(apiUrl, client);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(client);
            }
        }
        public IActionResult detail_partial(string id)
        {
            FactClient client = APIConsumer.Client(apiUrl, id);
            return PartialView("_DetailClientView", client);
        }
        public IActionResult Edit_partial(string id)
        {
            FactClient client = APIConsumer.Client(apiUrl, id);
            return PartialView("_DetailClientView", client);
        }

        [HttpPost]
        public IActionResult Edit_partial(Models.FactClient client)
        {

            try
            {
                APIConsumer.SaveClient(apiUrl, client.CliIdentification, client);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Create");
            }

        }
        public IActionResult ImprimirFacCli(string id)
        {
            string rut = "https://apifacturacion1.azurewebsites.net/api/FactInvoiceHeads/FacturasClientes";
            var data = APIConsumer.InvoiceHeadCli(rut, id);
            return new ViewAsPdf("ImprimirFac", data)
            {
                FileName = $"Venta.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };
        }

        public IActionResult PDF()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 5, 5, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                Paragraph paragraph = new Paragraph("Reporte de Clientes", new Font(Font.FontFamily.TIMES_ROMAN, 16));
                paragraph.Alignment = Element.ALIGN_CENTER;
                document.Add(paragraph);
                Paragraph espacio = new Paragraph("              ");
                document.Add(espacio);
                document.AddTitle("Reporte de Clientes");

                PdfPTable table = new PdfPTable(8);

                PdfPCell cell1 = new PdfPCell(new Phrase("ID", new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                cell1.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                cell1.VerticalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell1);

                PdfPCell cell2 = new PdfPCell(new Phrase("Nombre", new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                cell2.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2.VerticalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell2);

                PdfPCell cell3 = new PdfPCell(new Phrase("Fecha nacimiento", new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                cell3.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell3.HorizontalAlignment = Element.ALIGN_CENTER;
                cell3.VerticalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell3);

                PdfPCell cell4 = new PdfPCell(new Phrase("Dirección", new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                cell4.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                cell4.VerticalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell4);

                PdfPCell cell5 = new PdfPCell(new Phrase("Teléfono", new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                cell5.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell5.HorizontalAlignment = Element.ALIGN_CENTER;
                cell5.VerticalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell5);

                PdfPCell cell6 = new PdfPCell(new Phrase("Correo", new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                cell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell6.HorizontalAlignment = Element.ALIGN_CENTER;
                cell6.VerticalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell6);

                PdfPCell cell7 = new PdfPCell(new Phrase("Estado", new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                cell7.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell7.HorizontalAlignment = Element.ALIGN_CENTER;
                cell7.VerticalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell7);

                PdfPCell cell8 = new PdfPCell(new Phrase("Tipo", new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                cell8.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell8.HorizontalAlignment = Element.ALIGN_CENTER;
                cell8.VerticalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell8);

                foreach (FactClient client in APIConsumer.Clients(apiUrl))
                {
                    string tipoCliente = "";
                    string estadoCliente = "";
                    string fechaFormateada = "";
                    if (client.CliBirthday == null)
                        fechaFormateada = "";
                    else
                        fechaFormateada = ((DateTime)client.CliBirthday).ToString("dd/MM/yyyy");

                    if (client.TypId == 1)
                        tipoCliente = "Efectivo";
                    else if (client.TypId == 2)
                        tipoCliente = "Crédito";

                    if (client.CliStatus == true)
                        estadoCliente = "Activo";
                    else if (client.CliStatus == false)
                        estadoCliente = "Inactivo";

                    PdfPCell cell_1 = new PdfPCell(new Phrase(client.CliIdentification.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                    PdfPCell cell_2 = new PdfPCell(new Phrase(client.CliName.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                    PdfPCell cell_3 = new PdfPCell(new Phrase(fechaFormateada, new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                    PdfPCell cell_4 = new PdfPCell(new Phrase(client.CliAddres.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                    PdfPCell cell_5 = new PdfPCell(new Phrase(client.CliPhone.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                    PdfPCell cell_6 = new PdfPCell(new Phrase(client.CliMail.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                    PdfPCell cell_7 = new PdfPCell(new Phrase(estadoCliente, new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                    PdfPCell cell_8 = new PdfPCell(new Phrase(tipoCliente, new Font(Font.FontFamily.TIMES_ROMAN, 8)));

                    cell_1.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_2.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_3.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_4.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_5.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_6.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_7.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_8.HorizontalAlignment = Element.ALIGN_CENTER;

                    table.AddCell(cell_1);
                    table.AddCell(cell_2);
                    table.AddCell(cell_3);
                    table.AddCell(cell_4);
                    table.AddCell(cell_5);
                    table.AddCell(cell_6);
                    table.AddCell(cell_7);
                    table.AddCell(cell_8);
                }
                document.Add(table);
                document.Close();
                writer.Close();
                var constant = ms.ToArray();
                return File(constant, "application/vnd", "ClientReport.pdf");

            }
            return View();
        }
    }
}
