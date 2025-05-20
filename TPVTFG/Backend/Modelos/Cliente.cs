using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TPVTFG.MVVM.Base;

namespace TPVTFG.Backend.Modelos;

[Table("clientes")]
public partial class Cliente : PropertyChangedDataError
{
    [Key]
    [Column("DNI")]
    public string Dni { get; set; }

    [StringLength(50)]
    public string Nombre { get; set; } = null!;

    [StringLength(50)]
    public string? Apellidos { get; set; }

    [StringLength(50)]
    public string Email { get; set; } = null!;

    [StringLength(100)]
    public string? Direccion { get; set; }

    [StringLength(2)]
    public string Activado { get; set; }

    [InverseProperty("Cliente")]
    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
