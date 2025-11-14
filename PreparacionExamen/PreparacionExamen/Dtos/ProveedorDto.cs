using System.ComponentModel.DataAnnotations;

namespace PreparacionExamen.DTOs
{
    public class ProveedorDto
    {
        [Required(ErrorMessage = "La razón social es obligatoria")]
        [StringLength(200, ErrorMessage = "La razón social no puede exceder los 200 caracteres")]
        public string RazonSocial { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "El contacto no puede exceder los 100 caracteres")]
        public string Contacto { get; set; } = string.Empty;
    }
}