using System;
using System.Collections.Generic;

namespace InventarioTechSolutions.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public int CantidadStock { get; set; }
}
