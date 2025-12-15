using System;
using System.Collections.Generic;
using InventarioTechSolutions.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioTechSolutions.Data;

public partial class InventarioTechSolutionsDbContext : DbContext
{
    public InventarioTechSolutionsDbContext()
    {
    }

    public InventarioTechSolutionsDbContext(DbContextOptions<InventarioTechSolutionsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Producto> Productos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__Producto__09889210600832D8");

            entity.HasIndex(e => e.Nombre, "IX_Productos_Nombre");

            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
