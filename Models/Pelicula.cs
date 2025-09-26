using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PeliculasApi.Models;

public partial class Peliculas
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    public string Titulo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? Imagen { get; set; }

    [StringLength(50)]
    public string? Genero { get; set; }

    public int? Anio { get; set; }
}