using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TVPFarmacia.Backend.Modelos;

public partial class TpvbdContext : DbContext
{
    public TpvbdContext()
    {
    }

    public TpvbdContext(DbContextOptions<TpvbdContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Usuario> Ofertas { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuarioRole> UsuarioRoles { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

    public virtual DbSet<VentaProducto> VentaProductos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=127.0.0.1;port=3306;database=tpvbd;user=root;password=mysql");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Dni).HasName("PRIMARY");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.CategoriaNavigation).WithMany(p => p.Productos).HasConstraintName("CategoriaID");

            entity.HasOne(d => d.Oferta).WithMany(p => p.Productos).HasConstraintName("OfertaID");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasMany(d => d.Permisos).WithMany(p => p.Rols)
                .UsingEntity<Dictionary<string, object>>(
                    "RolPermiso",
                    r => r.HasOne<Permiso>().WithMany()
                        .HasForeignKey("PermisoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("PermisosID"),
                    l => l.HasOne<Role>().WithMany()
                        .HasForeignKey("RolId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("RolesID"),
                    j =>
                    {
                        j.HasKey("RolId", "PermisoId").HasName("PRIMARY");
                        j.ToTable("rol_permisos");
                        j.HasIndex(new[] { "PermisoId" }, "PermisosID");
                        j.IndexerProperty<int>("RolId").HasColumnName("RolID");
                        j.IndexerProperty<int>("PermisoId").HasColumnName("PermisoID");
                    });
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        modelBuilder.Entity<UsuarioRole>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PRIMARY");

            entity.HasOne(d => d.Rol).WithMany(p => p.UsuarioRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RolID");

            entity.HasOne(d => d.Usuario).WithOne(p => p.UsuarioRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UsuarioID");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Ventas).HasConstraintName("ClientesID");

            entity.HasOne(d => d.Empleado).WithMany(p => p.Venta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EmpleadosID");
        });

        modelBuilder.Entity<VentaProducto>(entity =>
        {
            entity.HasKey(e => new { e.VentaId, e.ProductoId }).HasName("PRIMARY");

            entity.HasOne(d => d.Producto).WithMany(p => p.VentaProductos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ProductosID");

            entity.HasOne(d => d.Venta).WithMany(p => p.VentaProductos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("VentasID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
