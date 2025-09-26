using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.Models;

namespace PeliculasApi.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Peliculas> Peliculas { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // vacío, o como fallback
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Peliculas>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pelicula__3214EC0708629F75");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuarios__3214EC076A69BA13");

            entity.Property(e => e.Rol).HasDefaultValue("usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
