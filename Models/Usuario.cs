using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PeliculasApi.Models;

[Index("Email", Name = "UX_Usuarios_Email", IsUnique = true)]
public partial class Usuario
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [StringLength(150)]
    public string Email { get; set; } = null!;

    public string? Imagen { get; set; }

    [StringLength(20)]
    public string Rol { get; set; } = null!;

    [StringLength(200)]
    public string Contraseña { get; set; } = null!;
}
