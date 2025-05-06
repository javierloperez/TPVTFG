using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TPVTFG.Backend.Modelos;

[Table("categorias")]
[Index("Id", Name = "ID_UNIQUE", IsUnique = true)]
public partial class Categoria
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("Categoria")]
    [StringLength(45)]
    public string Categoria1 { get; set; } = null!;

    [InverseProperty("CategoriaNavigation")]
    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
