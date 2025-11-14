namespace PreparacionExamen.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        // Relación: Una categoría puede tener muchos productos
        public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}