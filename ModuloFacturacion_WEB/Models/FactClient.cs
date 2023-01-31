using System.ComponentModel.DataAnnotations;


namespace ModuloFacturacion_WEB.Models
{
    public class FactClient
    {
        [Required(ErrorMessage = "Ingrese la Cédula")]
        [StringLength(maximumLength: 10, MinimumLength = 10, ErrorMessage = "Cédula Inválida")]
        public string CliIdentification { get; set; } = null!;

        [Required(ErrorMessage = "Debe ingresar el Nombre del Cliente")]
        [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessage = "El Nombre es muy corto")]
        public string? CliName { get; set; }

        public DateTime? CliBirthday { get; set; }

        [Required(ErrorMessage = "Ingrese una Dirección")]
        public string? CliAddres { get; set; }

        [StringLength(maximumLength: 10, MinimumLength = 10, ErrorMessage = "El número es inválido")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Número Invalido")]
        public string? CliPhone { get; set; }

        [EmailAddress(ErrorMessage = "El correo es inválido")]
        public string? CliMail { get; set; }

        public bool? CliStatus { get; set; }
        [Required]
        public int? TypId { get; set; }

        public virtual ICollection<FactInvoiceHead> FactInvoiceHeads { get; } = new List<FactInvoiceHead>();

        public virtual FactPayType? Typ { get; set; }
    }
}
