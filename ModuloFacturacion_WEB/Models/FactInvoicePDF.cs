using System;

namespace ModuloFacturacion_WEB.Models
{
    public class FactInvoicePDF
    {
        public int invoiceHeadId { get; set; }
        public DateTime invoiceDate { get; set; }
        public decimal? invoiceSubtotal { get; set; }
        public decimal? invoiceIva { get; set; }
        public decimal? invoiceTotal { get; set; }
        public string invoiceStatus { get; set; }
        public string cliIdentification { get; set; }
        public int? typId { get; set; }
        public string cliIdentificationNavigation { get; set; }
        public List<FactInvoiceDetail>? factInvoiceDetails { get; set; }
        public int typ { get; set; }
    }

    class FactInvoiceDetaila
    {
        public int invoiceDetailId { get; set; }
        public int invoiceDetailAmount { get; set; }
        public decimal invoiceDetailSubtotal { get; set; }
        public int productId { get; set; }
        public int invoiceHeadId { get; set; }
        public FactInvoiceHead invoiceHead { get; set; }
    }
}
