namespace ModuloFacturacion_WEB.Models
{
    public class FactInvoiceDetail
    {
        public int InvoiceDetailId { get; set; }

        public int? InvoiceDetailAmount { get; set; }

        public double? InvoiceDetailSubtotal { get; set; }

        public string? ProductId { get; set; }

        public string? InvoiceProductName { get; set; }

        public int? InvoiceHeadId { get; set; }

        public virtual FactInvoiceHead? InvoiceHead { get; set; }
    }
}
