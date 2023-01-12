namespace ModuloFacturacion_WEB.Models
{
    public class Product
    {
        public string prod_id { get; set; } = null!;

        public string? prod_nombre { get; set; }

        public string? prod_descripcion { get; set; }

        public bool? prod_iva { get; set; }

        public double? prod_costo { get; set; }

        public double? prod_pvp { get; set; }
        public bool? prod_estado { get; set; }
        public int? prod_stock { get; set; }
        public string? categoriacat_id { get; set; }
    }
}
