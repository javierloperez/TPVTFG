using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TPVFarmacia.Backend.Modelos;
using TVPFarmacia.MVVM.Base;

namespace TVPFarmacia.Backend.Modelos;

[Table("productos")]
[Index("Categoria", Name = "CategoriaID_idx")]
[Index("OfertaId", Name = "OfertaID_idx")]
public partial class Producto : PropertyChangedDataError
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(100)]
    [Required(ErrorMessage = "El campo Nombre es obligatorio")]
    public string Descripcion { get; set; } = null!;

    [Precision(10)]
    [Required(ErrorMessage = "El campo Precio es obligatorio")]
    public decimal Precio { get; set; }

    [StringLength(100)]
    [Required(ErrorMessage = "El campo ubicacion de almacén es obligatorio")]
    public string? Ubicacion { get; set; }

    [Required(ErrorMessage = "El campo cantidad es obligatorio")]
    public int Cantidad { get; set; }

    [Required(ErrorMessage = "La categoría del producto es obligatorio")]
    public int? Categoria { get; set; }

    [Column("OfertaID")]
    public int? OfertaId { get; set; }

    [Column("RutaLogo")]
    [StringLength(200)]
    [Required]
    public string RutaImagen { get; set; } = null!;

    [StringLength(2)]
    public string Activado { get; set; }

    [ForeignKey("Categoria")]
    [InverseProperty("Productos")]
    public virtual Categoria? CategoriaNavigation { get; set; }

    [ForeignKey("OfertaId")]
    [InverseProperty("Productos")]
    public virtual Oferta? Oferta { get; set; }

    [InverseProperty("Producto")]
    public virtual ICollection<VentaProducto> VentaProductos { get; set; } = new List<VentaProducto>();
}
