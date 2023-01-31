namespace ModuloFacturacion_WEB.Models
{
    public class FactInvoiceHead
    {
        public int InvoiceHeadId { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public double? InvoiceSubtotal { get; set; }

        public double? InvoiceIva { get; set; }

        public double? InvoiceTotal { get; set; }

        public string? CliIdentification { get; set; }

        public int? TypId { get; set; }
        public bool? InvoiceStatus { get; set; }


        public virtual FactClient? CliIdentificationNavigation { get; set; }

        public List<FactInvoiceDetail>? FactInvoiceDetails { get; set; }

        public virtual FactPayType? Typ { get; set; }
    }
}
