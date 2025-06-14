﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TVPFarmacia.Backend.Modelos;
using TVPFarmacia.MVVM.Base;

namespace TPVFarmacia.Backend.Modelos;

[Table("ofertas")]
public partial class Oferta : PropertyChangedDataError
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(100)]
    [Required]
    public string Descripcion { get; set; } = null!;

    [Column(TypeName = "date")]
    public DateTime OfertaInicio { get; set; }

    [Column(TypeName = "date")]
    public DateTime OfertaFin { get; set; }

    [StringLength(100)]
    public string? Fichero { get; set; }

    [Required]
    public int DescuentoPctj { get; set; }

    [InverseProperty("Oferta")]
    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
