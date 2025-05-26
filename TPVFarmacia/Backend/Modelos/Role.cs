using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TVPFarmacia.MVVM.Base;

namespace TVPFarmacia.Backend.Modelos;

[Table("roles")]
[Index("Id", Name = "ID_UNIQUE", IsUnique = true)]
public partial class Role : PropertyChangedDataError
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    public string Nombre { get; set; } = null!;

    [InverseProperty("Rol")]
    public virtual ICollection<UsuarioRole> UsuarioRoles { get; set; } = new List<UsuarioRole>();

    [ForeignKey("RolId")]
    [InverseProperty("Rols")]
    public virtual ICollection<Permiso> Permisos { get; set; } = new List<Permiso>();
}
