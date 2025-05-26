using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TVPFarmacia.MVVM.Base;

namespace TVPFarmacia.Backend.Modelos;

[Table("permisos")]
public partial class Permiso : PropertyChangedDataError
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(100)]
    public string Descripcion { get; set; } = null!;

    [ForeignKey("PermisoId")]
    [InverseProperty("Permisos")]
    public virtual ICollection<Role> Rols { get; set; } = new List<Role>();
}
