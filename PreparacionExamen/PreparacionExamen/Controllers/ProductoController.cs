using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PreparacionExamen.Context;
using PreparacionExamen.Models;
using PreparacionExamen.DTOs;

namespace PreparacionExamen.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoResponseDto>>> GetProductos()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .Select(p => new ProductoResponseDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    DescripcionCorta = p.DescripcionCorta,
                    Precio = p.Precio,
                    Stock = p.Stock,
                    Categoria = p.Categoria.Nombre,
                    Proveedor = p.Proveedor.RazonSocial
                })
                .ToListAsync();

            return productos;
        }

        // GET: api/Productos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoResponseDto>> GetProducto(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .Where(p => p.Id == id)
                .Select(p => new ProductoResponseDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    DescripcionCorta = p.DescripcionCorta,
                    Precio = p.Precio,
                    Stock = p.Stock,
                    Categoria = p.Categoria.Nombre,
                    Proveedor = p.Proveedor.RazonSocial
                })
                .FirstOrDefaultAsync();

            if (producto == null)
            {
                return NotFound();
            }

            return producto;
        }

        // POST: api/Productos
        [HttpPost]
        public async Task<ActionResult<ProductoResponseDto>> PostProducto(ProductoDto productoDto)
        {
            // Validar que la categoría y proveedor existen
            var categoria = await _context.Categorias.FindAsync(productoDto.CategoriaId);
            var proveedor = await _context.Proveedores.FindAsync(productoDto.ProveedorId);

            if (categoria == null || proveedor == null)
            {
                return BadRequest("La categoría o el proveedor no existen");
            }

            var producto = new Producto
            {
                Nombre = productoDto.Nombre,
                DescripcionCorta = productoDto.DescripcionCorta,
                Precio = productoDto.Precio,
                Stock = productoDto.Stock,
                CategoriaId = productoDto.CategoriaId,
                ProveedorId = productoDto.ProveedorId
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            var responseDto = new ProductoResponseDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                DescripcionCorta = producto.DescripcionCorta,
                Precio = producto.Precio,
                Stock = producto.Stock,
                Categoria = categoria.Nombre,
                Proveedor = proveedor.RazonSocial
            };

            return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, responseDto);
        }

        // PUT: api/Productos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, ProductoDto productoDto)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            // Validar que la categoría y proveedor existen
            var categoria = await _context.Categorias.FindAsync(productoDto.CategoriaId);
            var proveedor = await _context.Proveedores.FindAsync(productoDto.ProveedorId);

            if (categoria == null || proveedor == null)
            {
                return BadRequest("La categoría o el proveedor no existen");
            }

            producto.Nombre = productoDto.Nombre;
            producto.DescripcionCorta = productoDto.DescripcionCorta;
            producto.Precio = productoDto.Precio;
            producto.Stock = productoDto.Stock;
            producto.CategoriaId = productoDto.CategoriaId;
            producto.ProveedorId = productoDto.ProveedorId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Productos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Productos/ordenados-por-categoria
        [HttpGet("ordenados-por-categoria")]
        public async Task<ActionResult<IEnumerable<ProductoResponseDto>>> GetProductosOrdenadosPorCategoria()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .OrderBy(p => p.Categoria.Nombre)
                .ThenBy(p => p.Nombre)
                .Select(p => new ProductoResponseDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    DescripcionCorta = p.DescripcionCorta,
                    Precio = p.Precio,
                    Stock = p.Stock,
                    Categoria = p.Categoria.Nombre,
                    Proveedor = p.Proveedor.RazonSocial
                })
                .ToListAsync();

            return productos;
        }

        // GET: api/Productos/buscar-por-nombre/{nombre}
        [HttpGet("buscar-por-nombre/{nombre}")]
        public async Task<ActionResult<IEnumerable<ProductoResponseDto>>> GetProductosPorNombre(string nombre)
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .Where(p => p.Nombre.Contains(nombre))
                .Select(p => new ProductoResponseDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    DescripcionCorta = p.DescripcionCorta,
                    Precio = p.Precio,
                    Stock = p.Stock,
                    Categoria = p.Categoria.Nombre,
                    Proveedor = p.Proveedor.RazonSocial
                })
                .ToListAsync();

            return productos;
        }

        // GET: api/Productos/por-proveedor/{proveedorId}
        [HttpGet("por-proveedor/{proveedorId}")]
        public async Task<ActionResult<IEnumerable<ProductoResponseDto>>> GetProductosPorProveedor(int proveedorId)
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .Where(p => p.ProveedorId == proveedorId)
                .Select(p => new ProductoResponseDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    DescripcionCorta = p.DescripcionCorta,
                    Precio = p.Precio,
                    Stock = p.Stock,
                    Categoria = p.Categoria.Nombre,
                    Proveedor = p.Proveedor.RazonSocial
                })
                .ToListAsync();

            return productos;
        }

        // GET: api/Productos/por-categoria/{categoriaId}
        [HttpGet("por-categoria/{categoriaId}")]
        public async Task<ActionResult<IEnumerable<ProductoResponseDto>>> GetProductosPorCategoria(int categoriaId)
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .Where(p => p.CategoriaId == categoriaId)
                .Select(p => new ProductoResponseDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    DescripcionCorta = p.DescripcionCorta,
                    Precio = p.Precio,
                    Stock = p.Stock,
                    Categoria = p.Categoria.Nombre,
                    Proveedor = p.Proveedor.RazonSocial
                })
                .ToListAsync();

            return productos;
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}