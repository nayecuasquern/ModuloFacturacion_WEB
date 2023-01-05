using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using ModuloFacturacion_WEB.Code;
using ModuloFacturacion_WEB.Models;

namespace ModuloFacturacion_WEB.Controllers
{
    public class InvoiceController : Controller
    {

        string apiUrl = "https://apifacturacion1.azurewebsites.net/api/FactClients";
        string apiUrl2 = "https://apifacturacion1.azurewebsites.net/api/FactInvoiceHeads";
        public IActionResult Index()
        {
            var clientes = APIConsumer.Clients(apiUrl);

            return View(clientes);
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
    }
}
