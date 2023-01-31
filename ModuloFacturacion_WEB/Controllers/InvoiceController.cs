using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using ModuloFacturacion_WEB.Code;
using ModuloFacturacion_WEB.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;

namespace ModuloFacturacion_WEB.Controllers
{
    public class InvoiceController : Controller
    {

        string apiUrl = "https://apifacturacion1.azurewebsites.net/api/FactClients";
        string apiUrl2 = "https://apifacturacion1.azurewebsites.net/api/FactInvoiceHeads";
        string apiUrl3 = "https://apisalida.azurewebsites.net/api/Productoes";
        string apiUrl4 = "https://apifacturacion1.azurewebsites.net/api/FactInvoiceDetails";

        public IActionResult Index(string? searchFor)
        {
            ViewBag.SearchFor = "" + searchFor;
            ViewBag.ListaClientes = listaClientes();
            ViewBag.ListaTipoPago = listaTipoPago();
            ViewBag.ListaProductos = listaProductos();


            //return View(APIConsumer.InvoiceHead(apiUrl2));
            return View();

        }


        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.ListaClientes = listaClientes();
            ViewBag.ListaTipoPago = listaTipoPago();
            ViewBag.ListaProductos = listaProductos();

            return View();
        }

        [HttpPost]
        public IActionResult Create(FactInvoiceHead model, IFormCollection formData)
        {
            ViewBag.ListaClientes = listaClientes();
            ViewBag.ListaTipoPago = listaTipoPago();
            ViewBag.ListaProductos = listaProductos();
            ViewBag.textProducto = "" + formData["textProducto"];
            ViewBag.BanderaDetalleVenta = "false";

            var cliente = new FactClient();
            cliente.CliIdentification = formData["txtcedula"];

            if (formData["mostrarDatosCliente"] == "SI")
            {
                ViewBag.BanderaDetalleVenta = "true";
            }

            cliente.CliName = formData["txtnombre"];
            cliente.CliMail = formData["txtcorreo"];
            cliente.CliAddres = formData["txtdireccion"];
            cliente.CliPhone = formData["txttelefono"];
            if (formData["txttipo"] == "Cliente no seleccionado" || formData["txttipo"] == "")
                cliente.TypId = 0;
            else if ((formData["txttipo"]) == "Efectivo")
            {
                cliente.TypId = 1;
            } else if ((formData["txttipo"]) == "Crédito")
            {
                cliente.TypId = 2;
            }        
            ViewBag.ClienteElegido = cliente;


            if (formData["mostrarDatosCliente"] == "SI")
            {
                var datos = APIConsumer.ClienteElegido(apiUrl + "/" + formData["cedulaClienteElegido"]);
                ViewBag.ClienteElegido = datos;
            }
            if (formData["mostrarDatosProducto"] == "SI")
            {
                var datos = APIConsumer.ProductoElegido(apiUrl3 + "/" + formData["idProductoElegido"]);
                ViewBag.ProductoElegido = datos;
                if (datos.prod_iva==true)
                {
                    ViewBag.IVA = "SI";
                } else
                {
                    ViewBag.IVA = "NO";
                }
                
            }

            if (formData["terminarfactura"] == "SI" && model.FactInvoiceDetails!=null)
            {
                try
                {
                    model.CliIdentification = cliente.CliIdentification;
                    model.InvoiceDate = DateTime.Now;
                    model.InvoiceSubtotal = Double.Parse(formData["tdsubtotal"]);
                    model.InvoiceIva = Double.Parse(formData["tdiva"]);
                    model.InvoiceTotal = Double.Parse(formData["tdtotal"]);
                    model.InvoiceStatus = true;
                    int factID = APIConsumer.InsertFactInvoiceHead(apiUrl2, model);

                    foreach (var oC in model.FactInvoiceDetails)
                    {
                        FactInvoiceDetail details = new FactInvoiceDetail();
                        details.InvoiceDetailAmount = oC.InvoiceDetailAmount;
                        details.ProductId = oC.ProductId;
                        details.InvoiceProductName = oC.InvoiceProductName;
                        details.InvoiceDetailSubtotal = Double.Parse(oC.InvoiceDetailSubtotal.ToString());
                        details.InvoiceHeadId = factID;
                        ViewBag.Error = details.InvoiceDetailAmount + details.ProductId + details.InvoiceDetailSubtotal + details.InvoiceHeadId;
                        APIConsumer.InsertFactInvoiceDetail(apiUrl4, details);
                    }
                    ImprimirFac(factID);

                   return RedirectToAction(nameof(Create));
                }
                catch (Exception ex)
                {         
                    ViewBag.ErrorMessage = ex.Message;
                    return View();
                }
            }

            return View();

        }

        public IActionResult AnularFac(int id)
        {
            FactInvoiceHead fac = APIConsumer.InvoiceHead("https://apifacturacion1.azurewebsites.net/api/FactInvoiceHeads/ListaFacturasDetalle", id);
            fac.InvoiceStatus = false;
            APIConsumer.SaveFactInvoiceHead(apiUrl2, id, fac);
            return RedirectToAction("Index");
        }

        private List<Product> listaProductos()
        {
            var productos = APIConsumer.Productos(apiUrl3).Where(p => p.prod_stock > 0);
            var lista = productos.Select(f => new Product
            {
                prod_id = f.prod_id.ToString(),
                prod_nombre = f.prod_nombre,
                prod_descripcion = f.prod_descripcion,
                prod_pvp = f.prod_pvp,
                prod_stock = f.prod_stock,
                prod_iva = f.prod_iva
            })
            .ToList();
            return lista;
        }

        private List<FactClient> listaClientes()
        {
            var clientes = APIConsumer.Clients(apiUrl).Where(p => p.CliStatus == true);
            var lista = clientes.Select(f => new FactClient
            {
                CliIdentification = f.CliIdentification,
                CliName = f.CliName
            }).ToList();

            return lista;
        }



        private List<FactPayType> listaTipoPago()
        {
            var urlPagos = apiUrl.Replace("FactClients", "FactPayTypes");
            var tipoPagos = APIConsumer.PayTypes(urlPagos);
            var lista = tipoPagos.Select(f => new FactPayType
            {
                TypId = f.TypId,
                Typ = f.Typ
            }).ToList();

            return lista;
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerDatos()
        {
            var datos = APIConsumer.InvoiceHead(apiUrl2);
            return Json(new { data = datos });
        }

        [HttpPost]
        public ActionResult Add(FactInvoiceHead model, IFormCollection formData)
        {

            model.FactInvoiceDetails = new List<FactInvoiceDetail>();
            try
            {
                model.InvoiceDate = DateTime.Now;
                int factID = APIConsumer.InsertFactInvoiceHead(apiUrl2, model);
                
                foreach (var oC in model.FactInvoiceDetails)
                {
                    FactInvoiceDetail details = new FactInvoiceDetail();
                    details.InvoiceDetailAmount = oC.InvoiceDetailAmount;
                    details.ProductId = oC.ProductId;
                    details.InvoiceDetailSubtotal = oC.InvoiceDetailSubtotal;
                    details.InvoiceHeadId = factID;
                    APIConsumer.InsertFactInvoiceDetail(apiUrl4, details);
                }

                return RedirectToAction(nameof(Create));
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;  
                return View();
            }
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PDFFact()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 5, 5, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                Paragraph paragraph = new Paragraph("Reporte de Facturas", new Font(Font.FontFamily.TIMES_ROMAN, 16));
                paragraph.Alignment = Element.ALIGN_CENTER;
                document.Add(paragraph);
                Paragraph espacio = new Paragraph("              ");
                document.Add(espacio);
                document.AddTitle("Reporte de Facturas");

                PdfPTable table = new PdfPTable(7);

                PdfPCell cell1 = new PdfPCell(new Phrase("ID", new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                cell1.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                cell1.VerticalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell1);

                PdfPCell cell2 = new PdfPCell(new Phrase("Fecha", new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                cell2.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2.VerticalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell2);

                PdfPCell cell3 = new PdfPCell(new Phrase("Cliente", new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                cell3.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell3.HorizontalAlignment = Element.ALIGN_CENTER;
                cell3.VerticalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell3);

                PdfPCell cell4 = new PdfPCell(new Phrase("Tipo", new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                cell4.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                cell4.VerticalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell4);

                PdfPCell cell5 = new PdfPCell(new Phrase("Subtotal", new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                cell5.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell5.HorizontalAlignment = Element.ALIGN_CENTER;
                cell5.VerticalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell5);

                PdfPCell cell6 = new PdfPCell(new Phrase("Iva", new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                cell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell6.HorizontalAlignment = Element.ALIGN_CENTER;
                cell6.VerticalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell6);

                PdfPCell cell7 = new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                cell7.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell7.HorizontalAlignment = Element.ALIGN_CENTER;
                cell7.VerticalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell7);

                foreach (FactInvoiceHead fact in APIConsumer.InvoiceHead(apiUrl2))
                {
                    PdfPCell cell_1 = new PdfPCell(new Phrase(fact.InvoiceHeadId.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                    PdfPCell cell_2 = new PdfPCell(new Phrase(fact.InvoiceDate.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                    PdfPCell cell_3 = new PdfPCell(new Phrase(fact.CliIdentification.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                    PdfPCell cell_4 = new PdfPCell(new Phrase(fact.TypId.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                    PdfPCell cell_5 = new PdfPCell(new Phrase(fact.InvoiceSubtotal.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                    PdfPCell cell_6 = new PdfPCell(new Phrase(fact.InvoiceIva.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                    PdfPCell cell_7 = new PdfPCell(new Phrase(fact.InvoiceTotal.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8)));

                    cell_1.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_2.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_3.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_4.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_5.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_6.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_7.HorizontalAlignment = Element.ALIGN_CENTER;

                    table.AddCell(cell_1);
                    table.AddCell(cell_2);
                    table.AddCell(cell_3);
                    table.AddCell(cell_4);
                    table.AddCell(cell_5);
                    table.AddCell(cell_6);
                    table.AddCell(cell_7);
                }
                document.Add(table);
                document.Close();
                writer.Close();
                var constant = ms.ToArray();
                return File(constant, "application/vnd", "ReportInvoice.pdf");

            }
            return View();
        }

        public IActionResult ImprimirFac(int id)
        {

            var data = APIConsumer.InvoiceHead("https://apifacturacion1.azurewebsites.net/api/FactInvoiceHeads/ListaFacturasDetalle", id);
            return new ViewAsPdf("ImprimirFac", data)
            {
                FileName = $"Venta {data.InvoiceHeadId}.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };
        }

    }
}
