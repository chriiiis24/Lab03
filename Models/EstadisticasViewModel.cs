using System.Collections.Generic;

namespace InventarioTechSolutions.Models
{
    public class EstadisticasViewModel
    {
        // 1) Reporte de precios (caro -> barato)
        public List<Producto> ProductosOrdenadosPorPrecio { get; set; } = new();

        // 2) Promedio de precio
        public decimal PromedioPrecio { get; set; }

        // 3) Valor total del inventario (Precio * Stock)
        public decimal ValorTotalInventario { get; set; }

        // 4) Stock crítico (< 5)
        public List<Producto> ProductosStockCritico { get; set; } = new();
    }
}
