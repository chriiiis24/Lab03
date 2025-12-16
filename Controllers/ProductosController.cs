using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventarioTechSolutions.Data;
using InventarioTechSolutions.Models;

namespace InventarioTechSolutions.Controllers
{
    public class ProductosController : Controller
    {
        private readonly InventarioTechSolutionsDbContext _context;

        public ProductosController(InventarioTechSolutionsDbContext context)
        {
            _context = context;
        }

        // GET: /Productos?search=algo
        public async Task<IActionResult> Index(string? search)
        {
            search = search?.Trim();

            IQueryable<Producto> query = _context.Productos.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.Nombre.Contains(search));
            }

            query = query.OrderBy(p => p.Nombre);

            ViewData["Search"] = search;

            var lista = await query.ToListAsync();
            return View(lista);
        }

        // GET: /Productos/Editar/5
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var producto = await _context.Productos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.IdProducto == id);

            if (producto == null)
                return NotFound();

            return View(producto);
        }

        // GET: /Productos/Eliminar/5
        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null) return NotFound();

            var producto = await _context.Productos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.IdProducto == id);

            if (producto == null) return NotFound();

            return View(producto);
        }

        // GET: /Productos/Estadisticas
        public async Task<IActionResult> Estadisticas()
        {
            var baseQuery = _context.Productos.AsNoTracking();

            var vm = new EstadisticasViewModel
            {
                // 1) Lista del más caro al más barato
                ProductosOrdenadosPorPrecio = await baseQuery
                    .OrderByDescending(p => p.Precio)
                    .ToListAsync(),

                // 2) Promedio (si no hay productos, queda 0)
                PromedioPrecio = (await baseQuery
                    .Select(p => (decimal?)p.Precio)
                    .AverageAsync()) ?? 0m,

                // 3) Valor total inventario (si no hay productos, queda 0)
                ValorTotalInventario = (await baseQuery
                    .Select(p => (decimal?)(p.Precio * p.CantidadStock))
                    .SumAsync()) ?? 0m,

                // 4) Stock crítico: menos de 5
                ProductosStockCritico = await baseQuery
                    .Where(p => p.CantidadStock < 5)
                    .OrderBy(p => p.CantidadStock)
                    .ThenBy(p => p.Nombre)
                    .ToListAsync()
            };

            return View(vm);
        }

        // POST: /Productos/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            TempData["Msg"] = "Producto eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /Productos/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Producto model)
        {
            if (id != model.IdProducto)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            var productoDb = await _context.Productos.FirstOrDefaultAsync(p => p.IdProducto == id);

            if (productoDb == null)
                return NotFound();

            // Solo se actualiza lo que pide el paso 2:
            productoDb.Precio = model.Precio;
            productoDb.CantidadStock = model.CantidadStock;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
