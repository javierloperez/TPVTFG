using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TPVTFG.Backend.Modelos;

[Table("usuarios")]
public partial class Usuario
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    public string Nombre { get; set; } = null!;

    [StringLength(50)]
    public string? Apellidos { get; set; }

    [StringLength(50)]
    public string Login { get; set; } = null!;

    [StringLength(50)]
    public string Password { get; set; } = null!;

    public int? Telefono { get; set; }

    [InverseProperty("Usuario")]
    public virtual UsuarioRole? UsuarioRole { get; set; }

    [InverseProperty("Empleado")]
    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
