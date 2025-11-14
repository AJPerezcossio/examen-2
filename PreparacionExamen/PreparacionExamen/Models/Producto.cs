using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PreparacionExamen.Models
{
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "La descripción corta no puede exceder los 200 caracteres")]
        public string DescripcionCorta { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }

        // Claves foráneas para las relaciones
        [Required(ErrorMessage = "La categoría es obligatoria")]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "El proveedor es obligatorio")]
        public int ProveedorId { get; set; }

        // Propiedades de navegación
        public virtual Categoria Categoria { get; set; } = null!;
        public virtual Proveedor Proveedor { get; set; } = null!;

        // Métodos de utilidad
        public bool TieneStock()
        {
            return Stock > 0;
        }

        public bool TieneStockSuficiente(int cantidadSolicitada)
        {
            return Stock >= cantidadSolicitada;
        }

        public void ReducirStock(int cantidad)
        {
            if (cantidad <= Stock)
            {
                Stock -= cantidad;
            }
        }

        public void AumentarStock(int cantidad)
        {
            Stock += cantidad;
        }

        public decimal CalcularPrecioConDescuento(decimal porcentajeDescuento)
        {
            if (porcentajeDescuento < 0 || porcentajeDescuento > 100)
                throw new ArgumentException("El porcentaje de descuento debe estar entre 0 y 100");

            return Precio * (1 - porcentajeDescuento / 100);
        }
    }
}