using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TVPFarmacia.MVVM.Base;

namespace TVPFarmacia.Backend.Modelos;

[Table("ventas")]
[Index("ClienteId", Name = "ClienteID")]
[Index("EmpleadoId", Name = "EmpleadosID")]
public partial class Venta : PropertyChangedDataError
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public int? Iva { get; set; }

    [Column(TypeName = "date")]
    public DateTime Fecha { get; set; }

    [Column("EmpleadoID")]
    public int EmpleadoId { get; set; }

    [StringLength(50)]
    [Required]
    public string TipoCobro { get; set; }

    [Precision(10)]
    public decimal Total { get; set; }

    [Column("ClienteID")]
    public string? ClienteId { get; set; }

    [ForeignKey("ClienteId")]
    [InverseProperty("Ventas")]
    public virtual Cliente? Cliente { get; set; }

    [ForeignKey("EmpleadoId")]
    [InverseProperty("Venta")]
    public virtual Usuario Empleado { get; set; } = null!;

    [InverseProperty("Venta")]
    public virtual ICollection<VentaProducto> VentaProductos { get; set; } = new List<VentaProducto>();
}
