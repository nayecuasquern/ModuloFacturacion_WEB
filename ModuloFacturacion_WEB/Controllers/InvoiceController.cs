using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using ModuloFacturacion_WEB.Code;
using ModuloFacturacion_WEB.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using ClosedXML.Excel;

namespace ModuloFacturacion_WEB.Controllers
{
    public class InvoiceController : Controller
    {

        string apiUrl = "https://apifacturacion1.azurewebsites.net/api/FactClients";
        string apiUrl2 = "https://apifacturacion1.azurewebsites.net/api/FactInvoiceHeads";
        string apiUrl3 = "https://apisalida.azurewebsites.net/api/Productoes";
        string apiUrl4 = "https://apifacturacion1.azurewebsites.net/api/FactInvoiceDetails";
        string apiUrlLastElement = "https://apifacturacion1.azurewebsites.net/api/FactInvoiceHeads/UltimoElemento";

        public IActionResult Index(string? searchFor)
        {
            ViewBag.SearchFor = "" + searchFor;
            ViewBag.ListaClientes = listaClientes();
            ViewBag.ListaTipoPago = listaTipoPago();
            ViewBag.ListaProductos = listaProductos();
            ViewBag.UltimoElemento = ultimoElemento();
            ViewBag.ImprimirFactura = false;


            //return View(APIConsumer.InvoiceHead(apiUrl2));
            return View();

        }


        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.ListaClientes = listaClientes();
            ViewBag.ListaTipoPago = listaTipoPago();
            ViewBag.ListaProductos = listaProductos();
            ViewBag.UltimoElemento = ultimoElemento();
            ViewBag.ImprimirFactura = false;


            return View();
        }

        [HttpPost]
        public IActionResult Create(FactInvoiceHead model, IFormCollection formData)
        {
            ViewBag.ListaClientes = listaClientes();
            ViewBag.ListaTipoPago = listaTipoPago();
            ViewBag.ListaProductos = listaProductos();
            string lastElement = ultimoElemento();
            ViewBag.ImprimirFactura = false;
            ViewBag.UltimoElemento = lastElement;
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

            //if (formData["imprimirFact"] == "SI")
            //{
            //    ViewBag.ImprimirFactura = "SI";
            //}

            if (formData["terminarfactura"] == "SI" && model.FactInvoiceDetails!=null)
            {
                try
                {
                    model.CliIdentification = cliente.CliIdentification;
                    model.InvoiceDate = DateTime.Now;
                    model.InvoiceSubtotal = Double.Parse(formData["tdsubtotal"]);
                    model.InvoiceIva = Double.Parse(formData["tdiva"]);
                    model.InvoiceTotal = Double.Parse(formData["tdtotal"]);
                    model.InvoiceNumber = lastElement;
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

                    ViewBag.ImprimirFactura = "true";
                    //return RedirectToAction(nameof(Create));
                    return View();
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

        private string ultimoElemento()
        {
            FactInvoiceHead resp = APIConsumer.UltimoElemento(apiUrlLastElement);
            string numero = "001-001-" + ((resp.InvoiceHeadId+1).ToString("D9"));
            ViewBag.ultimaFac = resp.InvoiceHeadId + 1;
            return numero;
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

                PdfPTable table = new PdfPTable(8);

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

                PdfPCell cell8 = new PdfPCell(new Phrase("Estado", new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                cell8.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell8.HorizontalAlignment = Element.ALIGN_CENTER;
                cell8.VerticalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell8);

                foreach (FactInvoiceHead fact in APIConsumer.InvoiceHead(apiUrl2))
                {
                    PdfPCell cell_1 = new PdfPCell(new Phrase(fact.InvoiceHeadId.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8)));

                    string fechaFormateada = ((DateTime)(fact.InvoiceDate)).ToString("dd/MM/yyyy");

                    string tipoFactura = "";
                    string estadoFactura = "";

                    if (fact.TypId == 1)
                        tipoFactura = "Efectivo";
                    else if (fact.TypId == 2)
                        tipoFactura = "Crédito";

                    if (fact.InvoiceStatus == true)
                        estadoFactura = "Activo";
                    else if (fact.InvoiceStatus == false)
                        estadoFactura = "Anulada";

                    PdfPCell cell_2 = new PdfPCell(new Phrase(fechaFormateada, new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                    PdfPCell cell_3 = new PdfPCell(new Phrase(fact.CliIdentification, new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                    PdfPCell cell_4 = new PdfPCell(new Phrase(tipoFactura, new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                    PdfPCell cell_5 = new PdfPCell(new Phrase(fact.InvoiceSubtotal.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                    PdfPCell cell_6 = new PdfPCell(new Phrase(fact.InvoiceIva.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                    PdfPCell cell_7 = new PdfPCell(new Phrase(fact.InvoiceTotal.ToString(), new Font(Font.FontFamily.TIMES_ROMAN, 8)));
                    PdfPCell cell_8 = new PdfPCell(new Phrase(estadoFactura, new Font(Font.FontFamily.TIMES_ROMAN, 8)));

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
                return File(constant, "application/vnd", "InvoiceReport.pdf");

            }
            return View();
        }

        public IActionResult ImprimirFac(int id)
        {

            var data = APIConsumer.InvoiceHead("https://apifacturacion1.azurewebsites.net/api/FactInvoiceHeads/ListaFacturasDetalle", id);
            return new ViewAsPdf("ImprimirFac", data)
            {
                FileName = $"Factura Nro {data.InvoiceNumber}.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };
        }
        public IActionResult ImprimirFacAll()
        {
            string con = "https://apifacturacion1.azurewebsites.net/api/FactInvoiceHeads/FacturasClientes";
            var data = APIConsumer.InvoiceHeadCli(con, "");
            string fecha = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"); ;

            return new ViewAsPdf("ImprimirFacAll", data)
            {
                FileName = $"Facturas" + " " + fecha + ".pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };
        }

        public FileResult ExportarExcel()
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection("Data Source=jjmalesg.database.windows.net;Initial Catalog=grupo4DB;User ID=jeffersonmales;Password=Buenhombre1;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("select hd.invoice_number as NÚMERO_FACTURA, hd.invoice_date as FECHA," +
                    "hd.cli_identification as CLIENTE_CÉDULA, cl.cli_name as CLIENTE_NOMBRE," +
                    "ty.typ as TIPO_PAGO, hd.invoice_status as ESTADO, hd.invoice_subtotal as SUBTOTAL," +
                    "hd.invoice_IVA as IVA, hd.invoice_total as TOTAL from fact_client cl inner join fact_invoice_head hd" +
                    " on cl.cli_identification = hd.cli_identification inner join fact_pay_type ty on hd.typ_id = ty.typ_id " +
                    "group by hd.invoice_number, hd.invoice_date, hd.cli_identification, cl.cli_name, ty.typ, hd.invoice_status, hd.invoice_subtotal, " +
                    "hd.invoice_IVA, hd.invoice_total;");
                
                SqlCommand cmd = new SqlCommand(sb.ToString(), cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }

            dt.TableName = "Datos Facturas";
            
            using(XLWorkbook libro = new XLWorkbook())
            {
                var hoja = libro.Worksheets.Add(dt);

                hoja.ColumnsUsed().AdjustToContents();

                using (MemoryStream stream = new MemoryStream())
                {
                    libro.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte Facturas" + ".xlsx");
                }
            }
        }

    }
}
