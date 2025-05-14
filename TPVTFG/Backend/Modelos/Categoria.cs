using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TPVTFG.MVVM.Base;

namespace TPVTFG.Backend.Modelos;

[Table("categorias")]
[Index("Id", Name = "ID_UNIQUE", IsUnique = true)]
public partial class Categoria : PropertyChangedDataError
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("Categoria")]
    [StringLength(45)]
    public string Categoria1 { get; set; } = null!;

    [InverseProperty("CategoriaNavigation")]
    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

    [Column("RutaImagen")]
    [StringLength(200)]
    public string RutaImagen { get; set; } = null!;
}
