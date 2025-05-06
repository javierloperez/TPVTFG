using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TPVTFG.Backend.Modelos;

[Table("usuario_roles")]
[Index("RolId", Name = "RolID")]
[Index("UsuarioId", Name = "UsuarioID_UNIQUE", IsUnique = true)]
public partial class UsuarioRole
{
    [Key]
    [Column("UsuarioID")]
    public int UsuarioId { get; set; }

    [Column("RolID")]
    public int RolId { get; set; }

    [ForeignKey("RolId")]
    [InverseProperty("UsuarioRoles")]
    public virtual Role Rol { get; set; } = null!;

    [ForeignKey("UsuarioId")]
    [InverseProperty("UsuarioRole")]
    public virtual Usuario Usuario { get; set; } = null!;
}
