using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TPVTFG.Backend.Modelos;

[PrimaryKey("VentaId", "ProductoId")]
[Table("venta_productos")]
[Index("ProductoId", Name = "ProductosID")]
public partial class VentaProducto
{
    [Key]
    [Column("VentaID")]
    public int VentaId { get; set; }

    [Key]
    [Column("ProductoID")]
    public int ProductoId { get; set; }

    public int Cantidad { get; set; }

    public double? Precio { get; set; }

    [ForeignKey("ProductoId")]
    [InverseProperty("VentaProductos")]
    public virtual Producto Producto { get; set; } = null!;

    [ForeignKey("VentaId")]
    [InverseProperty("VentaProductos")]
    public virtual Venta Venta { get; set; } = null!;
}
