using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TPVTFG.Backend.Modelos;

[Table("productos")]
[Index("Categoria", Name = "CategoriaID_idx")]
[Index("OfertaId", Name = "OfertaID_idx")]
public partial class Producto
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(100)]
    public string Descripcion { get; set; } = null!;

    [Precision(10)]
    public decimal Precio { get; set; }

    [StringLength(100)]
    public string? Ubicacion { get; set; }

    public int Cantidad { get; set; }

    public int? Categoria { get; set; }

    [Column("OfertaID")]
    public int? OfertaId { get; set; }

    [ForeignKey("Categoria")]
    [InverseProperty("Productos")]
    public virtual Categoria? CategoriaNavigation { get; set; }

    [ForeignKey("OfertaId")]
    [InverseProperty("Productos")]
    public virtual Oferta? Oferta { get; set; }

    [InverseProperty("Producto")]
    public virtual ICollection<VentaProducto> VentaProductos { get; set; } = new List<VentaProducto>();
}
