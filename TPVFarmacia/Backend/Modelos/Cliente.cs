using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TVPFarmacia.MVVM.Base;

namespace TVPFarmacia.Backend.Modelos;

[Table("clientes")]
public partial class Cliente : PropertyChangedDataError
{
    [Key]
    [Column("DNI")]
    [Required]
    public string Dni { get; set; }

    [StringLength(50)]
    [Required]
    public string Nombre { get; set; } = null!;

    [StringLength(50)]
    [Required]
    public string? Apellidos { get; set; }

    [StringLength(50)]
    [Required]
    public string Email { get; set; } = null!;

    [StringLength(100)]
    [Required]
    public string? Direccion { get; set; }

    [StringLength(2)]

    public string Activado { get; set; }

    [InverseProperty("Cliente")]
    public virtual ICollection<Venta> Ventas { get; set; } = new List<Venta>();
}
