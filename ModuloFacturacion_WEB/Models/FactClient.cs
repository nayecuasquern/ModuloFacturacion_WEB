using System.ComponentModel.DataAnnotations;


namespace ModuloFacturacion_WEB.Models
{
    public class FactClient
    {
        [Required(ErrorMessage = "Ingrese una Cédula")]
        [StringLength(maximumLength: 10, MinimumLength = 10, ErrorMessage = "Cédula Inválida")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Solo se permiten números")]
        [ValidateIdentification(ErrorMessage = "Cédula Inválida")]
        public string CliIdentification { get; set; } = null!;

        [Required(ErrorMessage = "Ingrese el Nombre del Cliente")]
        [StringLength(maximumLength: 100, MinimumLength = 2, ErrorMessage = "El nombre es muy corto")]
        [RegularExpression(@"^[a-zA-ZñÑáéíóúüÁÉÍÓÚÜ\s]+$", ErrorMessage = "Solo se permiten letras")]
        public string? CliName { get; set; }

        [Required(ErrorMessage = "Ingrese una Fecha de nacimiento")]
        [ValidateBirthday(ErrorMessage = "La Fecha de Nacimiento NO puede ser posterior a la fecha actual")]
        public DateTime? CliBirthday { get; set; }

        [Required(ErrorMessage = "Ingrese una Dirección")]
        public string? CliAddres { get; set; }

        [Required(ErrorMessage = "Ingrese un Número de Celular")]
        [StringLength(maximumLength: 10, MinimumLength = 10, ErrorMessage = "Número de Celular Inválido")]
        [RegularExpression(@"^09\d+$", ErrorMessage = "Número de Celular Inválido")]
        public string? CliPhone { get; set; }

        [Required(ErrorMessage = "Ingrese un Correo electrónico")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)\s{0,}$", ErrorMessage = "Correo electrónico inválido")]
        public string? CliMail { get; set; }

        [Required]
        public bool? CliStatus { get; set; }
        [Required]
        public int? TypId { get; set; }

        public virtual ICollection<FactInvoiceHead> FactInvoiceHeads { get; } = new List<FactInvoiceHead>();

        public virtual FactPayType? Typ { get; set; }
    }

    public class ValidateBirthdayAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime date = (DateTime)value;
            return date <= DateTime.Now;
        }
    }

    public class ValidateIdentificationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string id = (string)value;

            int total = 0;
            int[] coefficients = new int[] { 2, 1, 2, 1, 2, 1, 2, 1, 2 };

            for (int i = 0; i < coefficients.Length; i++)
            {
                int quantity = int.Parse(id[i].ToString()) * coefficients[i];

                if (quantity > 9)
                {
                    quantity = quantity - 9;
                }

                total += quantity;
            }

            if (total == 0)
            {
                return false;
            }

            int residue = total % 10;

            if (residue != 0)
                residue = 10 - residue;

            return residue == int.Parse(id[9].ToString());
        }
    }

}
