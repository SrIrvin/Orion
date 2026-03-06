using Microsoft.EntityFrameworkCore;
using Orión.Application.Interfaces;
using Orión.Domain.Entities;

namespace Orión.Infrastructure.Persistence;

public class OrionDbContext : DbContext, IOrionDbContext
{
    public OrionDbContext() { }

    public OrionDbContext(DbContextOptions<OrionDbContext> options) : base(options) { }

    public DbSet<Maquinaria> Maquinarias { get; set; } = null!;
    public DbSet<NivelCritico> NivelesCriticos { get; set; } = null!;
    public DbSet<Ubicacion> Ubicaciones { get; set; } = null!;
    public DbSet<Componente> Componentes { get; set; } = null!;
    public DbSet<TipoComponente> TiposComponentes { get; set; } = null!;
    public DbSet<EstadoComponente> EstadosComponentes { get; set; } = null!;
    public DbSet<Tecnico> Tecnicos { get; set; } = null!;
    public DbSet<Turno> Turnos { get; set; } = null!;
    public DbSet<SolicitudServicio> SolicitudesServicios { get; set; } = null!;
    public DbSet<TipoMantenimiento> TiposMantenimiento { get; set; } = null!;
    public DbSet<EstadoSolicitud> EstadosSolicitudes { get; set; } = null!;
    public DbSet<Usuario> Usuarios { get; set; } = null!;
    public DbSet<Proveedor> Proveedores { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Se recomienda no tener la cadena de conexión aquí. 
        // Se inyecta a través de DbContextOptions en App.xaml.cs
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1. NivelCritico
        modelBuilder.Entity<NivelCritico>(entity =>
        {
            entity.ToTable("NivelCritico");
            entity.HasKey(e => e.IdNivelCritico);
            entity.Property(e => e.IdNivelCritico).HasColumnName("ID_NivelCritico").ValueGeneratedNever();
            entity.Property(e => e.Descripcion).HasColumnName("Descripcion").HasMaxLength(20).IsRequired();
            entity.HasIndex(e => e.Descripcion).IsUnique();
        });

        // 2. Ubicacion
        modelBuilder.Entity<Ubicacion>(entity =>
        {
            entity.ToTable("Ubicacion");
            entity.HasKey(e => e.IdUbicacion);
            entity.Property(e => e.IdUbicacion).HasColumnName("ID_Ubicacion").ValueGeneratedOnAdd();
            entity.Property(e => e.NumeroNave).HasColumnName("Numero_Nave").IsRequired();
            entity.HasIndex(e => e.NumeroNave).IsUnique();
        });

        // 3. TipoComponente
        modelBuilder.Entity<TipoComponente>(entity =>
        {
            entity.ToTable("TipoComponente");
            entity.HasKey(e => e.IdTipoComponente);
            entity.Property(e => e.IdTipoComponente).HasColumnName("ID_TipoComponente").ValueGeneratedOnAdd();
            entity.Property(e => e.NombreTipo).HasColumnName("Nombre_Tipo").HasMaxLength(100).IsRequired();
            entity.HasIndex(e => e.NombreTipo).IsUnique();
        });

        // 4. EstadoComponente
        modelBuilder.Entity<EstadoComponente>(entity =>
        {
            entity.ToTable("EstadoComponente");
            entity.HasKey(e => e.IdEstado);
            entity.Property(e => e.IdEstado).HasColumnName("ID_Estado").ValueGeneratedNever();
            entity.Property(e => e.DescripcionEstado).HasColumnName("Descripcion_Estado").HasMaxLength(50).IsRequired();
            entity.HasIndex(e => e.DescripcionEstado).IsUnique();
        });

        // 5. Turno
        modelBuilder.Entity<Turno>(entity =>
        {
            entity.ToTable("Turno");
            entity.HasKey(e => e.IdTurno);
            entity.Property(e => e.IdTurno).HasColumnName("ID_Turno").ValueGeneratedNever();
            entity.Property(e => e.DescripcionTurno).HasColumnName("Descripcion_Turno").HasMaxLength(20).IsRequired();
            entity.HasIndex(e => e.DescripcionTurno).IsUnique();
        });

        // 6. TipoMantenimiento
        modelBuilder.Entity<TipoMantenimiento>(entity =>
        {
            entity.ToTable("TipoMantenimiento");
            entity.HasKey(e => e.IdTipoMantto);
            entity.Property(e => e.IdTipoMantto).HasColumnName("ID_TipoMantto").ValueGeneratedNever();
            entity.Property(e => e.DescripcionTipo).HasColumnName("Descripcion_Tipo").HasMaxLength(20).IsRequired();
            entity.HasIndex(e => e.DescripcionTipo).IsUnique();
        });

        // 7. EstadoSolicitud
        modelBuilder.Entity<EstadoSolicitud>(entity =>
        {
            entity.ToTable("EstadoSolicitud");
            entity.HasKey(e => e.IdEstadoSolicitud);
            entity.Property(e => e.IdEstadoSolicitud).HasColumnName("ID_EstadoSolicitud").ValueGeneratedNever();
            entity.Property(e => e.DescripcionEstado).HasColumnName("Descripcion_Estado").HasMaxLength(25).IsRequired();
            entity.HasIndex(e => e.DescripcionEstado).IsUnique();
        });

        // 8. Maquinaria
        modelBuilder.Entity<Maquinaria>(entity =>
        {
            entity.ToTable("Maquinaria");
            entity.HasKey(e => e.IdMaquinaria);
            entity.Property(e => e.IdMaquinaria).HasColumnName("ID_Maquinaria").HasMaxLength(30);
            entity.Property(e => e.NombreMaquina).HasColumnName("Nombre_Maquina").HasMaxLength(100).IsRequired();
            entity.Property(e => e.Tipo).HasColumnName("Tipo").HasMaxLength(50);
            entity.Property(e => e.Marca).HasColumnName("Marca").HasMaxLength(50);
            entity.Property(e => e.Modelo).HasColumnName("Modelo").HasMaxLength(50);
            entity.Property(e => e.FechaInstalacion).HasColumnName("Fecha_Instalacion");
            entity.Property(e => e.IdNivelCritico).HasColumnName("ID_NivelCritico");
            entity.Property(e => e.IdUbicacion).HasColumnName("ID_Ubicacion");
            entity.Property(e => e.Activo).HasColumnName("Activo").HasDefaultValue(true);

            entity.HasOne(d => d.NivelCritico)
                .WithMany(p => p.Maquinarias)
                .HasForeignKey(d => d.IdNivelCritico)
                .HasConstraintName("FK_Maquinaria_NivelCritico");

            entity.HasOne(d => d.Ubicacion)
                .WithMany(p => p.Maquinarias)
                .HasForeignKey(d => d.IdUbicacion)
                .HasConstraintName("FK_Maquinaria_Ubicacion");
        });

        // 9. Tecnico
        modelBuilder.Entity<Tecnico>(entity =>
        {
            entity.ToTable("Tecnico");
            entity.HasKey(e => e.IdPersonal);
            entity.Property(e => e.IdPersonal).HasColumnName("ID_Personal").ValueGeneratedNever();
            entity.Property(e => e.NombreApellido).HasColumnName("Nombre_Apellido").HasMaxLength(50).IsRequired();
            entity.Property(e => e.Especialidad).HasColumnName("Especialidad").HasMaxLength(50);
            entity.Property(e => e.IdTurno).HasColumnName("ID_Turno");
            entity.Property(e => e.Activo).HasColumnName("Activo").HasDefaultValue(true);

            entity.HasOne(d => d.Turno)
                .WithMany(p => p.Tecnicos)
                .HasForeignKey(d => d.IdTurno)
                .HasConstraintName("FK_Tecnico_Turno");
        });

        // 10. Componente
        modelBuilder.Entity<Componente>(entity =>
        {
            entity.ToTable("Componente");
            entity.HasKey(e => e.IdComponente);
            entity.Property(e => e.IdComponente).HasColumnName("ID_Componente").HasMaxLength(30);
            entity.Property(e => e.IdMaquinaria).HasColumnName("ID_Maquinaria").HasMaxLength(30).IsRequired();
            entity.Property(e => e.NombreComponente).HasColumnName("Nombre_Componente").HasMaxLength(100).IsRequired();
            entity.Property(e => e.IdTipoComponente).HasColumnName("ID_TipoComponente");
            entity.Property(e => e.Marca).HasColumnName("Marca").HasMaxLength(50);
            entity.Property(e => e.NumeroSerie).HasColumnName("Numero_Serie").HasMaxLength(50);
            entity.Property(e => e.EspecificacionesTecnicas).HasColumnName("Especificaciones_Tecnicas").HasMaxLength(50);
            entity.Property(e => e.FechaUltimoCambio).HasColumnName("Fecha_Ultimo_Cambio");
            entity.Property(e => e.IdEstado).HasColumnName("ID_Estado");
            entity.Property(e => e.Activo).HasColumnName("Activo").HasDefaultValue(true);

            entity.HasOne(d => d.Maquinaria)
                .WithMany(p => p.Componentes)
                .HasForeignKey(d => d.IdMaquinaria)
                .HasConstraintName("FK_Componente_Maquinaria");

            entity.HasOne(d => d.TipoComponente)
                .WithMany(p => p.Componentes)
                .HasForeignKey(d => d.IdTipoComponente)
                .HasConstraintName("FK_Componente_TipoComponente");

            entity.HasOne(d => d.EstadoComponente)
                .WithMany(p => p.Componentes)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK_Componente_EstadoComponente");
        });

        // 11. Solicitud_Servicio
        modelBuilder.Entity<SolicitudServicio>(entity =>
        {
            entity.ToTable("Solicitud_Servicio");
            entity.HasKey(e => e.IdSS);
            entity.Property(e => e.IdSS).HasColumnName("ID_SS").ValueGeneratedOnAdd();
            entity.Property(e => e.IdMaquinaria).HasColumnName("ID_Maquinaria").HasMaxLength(15).IsRequired();
            entity.Property(e => e.IdTipoMantto).HasColumnName("ID_TipoMantto").IsRequired();
            entity.Property(e => e.DescripcionFalla).HasColumnName("Descripcion_Falla").HasMaxLength(200);
            entity.Property(e => e.FechaApertura).HasColumnName("Fecha_Apertura").IsRequired();
            entity.Property(e => e.FechaCierre).HasColumnName("Fecha_Cierre");
            entity.Property(e => e.IdPersonal).HasColumnName("ID_Personal");
            entity.Property(e => e.IdEstadoSolicitud).HasColumnName("ID_EstadoSolicitud").IsRequired();

            entity.HasOne(d => d.Maquinaria)
                .WithMany(p => p.SolicitudesServicio)
                .HasForeignKey(d => d.IdMaquinaria)
                .HasConstraintName("FK_Solicitud_Maquinaria");

            entity.HasOne(d => d.TipoMantenimiento)
                .WithMany(p => p.SolicitudesServicio)
                .HasForeignKey(d => d.IdTipoMantto)
                .HasConstraintName("FK_Solicitud_TipoMantenimiento");

            entity.HasOne(d => d.Tecnico)
                .WithMany(p => p.SolicitudesServicio)
                .HasForeignKey(d => d.IdPersonal)
                .HasConstraintName("FK_Solicitud_Tecnico");

            entity.HasOne(d => d.EstadoSolicitud)
                .WithMany(p => p.SolicitudesServicio)
                .HasForeignKey(d => d.IdEstadoSolicitud)
                .HasConstraintName("FK_Solicitud_EstadoSolicitud");
        });

        // 12. Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario");
            entity.HasKey(e => e.IdUsuario);
            entity.Property(e => e.IdUsuario).HasColumnName("ID_Usuario").ValueGeneratedOnAdd();
            entity.Property(e => e.NombreUsuario).HasColumnName("Nombre_Usuario").HasMaxLength(50).IsRequired();
            entity.Property(e => e.PasswordHash).HasColumnName("Password_Hash").IsRequired();
            entity.Property(e => e.Email).HasColumnName("Email").HasMaxLength(100);
            entity.Property(e => e.Rol).HasColumnName("Rol").HasMaxLength(20).IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("Fecha_Creacion");
            entity.Property(e => e.LastPasswordChange).HasColumnName("Ultimo_Cambio_Pass");
            entity.Property(e => e.RequiresPasswordChange).HasColumnName("Requiere_Cambio_Pass").HasDefaultValue(false);
            entity.Property(e => e.Activo).HasColumnName("Activo").HasDefaultValue(true);

            entity.HasIndex(e => e.NombreUsuario).IsUnique();
        });

        // 13. Proveedor
        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.ToTable("Proveedor");
            entity.HasKey(e => e.IdProveedor);
            entity.Property(e => e.IdProveedor).HasColumnName("ID_Proveedor").ValueGeneratedOnAdd();
            entity.Property(e => e.Nombre).HasColumnName("Nombre").HasMaxLength(100).IsRequired();
            entity.Property(e => e.RUC).HasColumnName("RUC").HasMaxLength(20);
            entity.Property(e => e.Telefono).HasColumnName("Telefono").HasMaxLength(20);
            entity.Property(e => e.Email).HasColumnName("Email").HasMaxLength(100);
            entity.Property(e => e.Direccion).HasColumnName("Direccion").HasMaxLength(200);
            entity.Property(e => e.Activo).HasColumnName("Activo").HasDefaultValue(true);
        });

        // Relación Componente -> Proveedor
        modelBuilder.Entity<Componente>()
            .HasOne(c => c.Proveedor)
            .WithMany(p => p.Componentes)
            .HasForeignKey(c => c.IdProveedor)
            .HasConstraintName("FK_Componente_Proveedor");

        // =====================================================
        // SEED DATA (Datos iniciales de catálogos)
        // =====================================================

        // NivelCritico
        modelBuilder.Entity<NivelCritico>().HasData(
            new NivelCritico { IdNivelCritico = 1, Descripcion = "Baja" },
            new NivelCritico { IdNivelCritico = 2, Descripcion = "Media" },
            new NivelCritico { IdNivelCritico = 3, Descripcion = "Alta" },
            new NivelCritico { IdNivelCritico = 4, Descripcion = "Critico" }
        );

        // EstadoComponente
        modelBuilder.Entity<EstadoComponente>().HasData(
            new EstadoComponente { IdEstado = 1, DescripcionEstado = "Activo" },
            new EstadoComponente { IdEstado = 2, DescripcionEstado = "En reparacion" },
            new EstadoComponente { IdEstado = 3, DescripcionEstado = "Dado de baja" },
            new EstadoComponente { IdEstado = 4, DescripcionEstado = "En stock" }
        );

        // Turno
        modelBuilder.Entity<Turno>().HasData(
            new Turno { IdTurno = 1, DescripcionTurno = "Matutino" },
            new Turno { IdTurno = 2, DescripcionTurno = "Vespertino" },
            new Turno { IdTurno = 3, DescripcionTurno = "Nocturno" },
            new Turno { IdTurno = 4, DescripcionTurno = "Mixto" }
        );

        // TipoMantenimiento
        modelBuilder.Entity<TipoMantenimiento>().HasData(
            new TipoMantenimiento { IdTipoMantto = 1, DescripcionTipo = "Preventivo" },
            new TipoMantenimiento { IdTipoMantto = 2, DescripcionTipo = "Correctivo" }
        );

        // EstadoSolicitud
        modelBuilder.Entity<EstadoSolicitud>().HasData(
            new EstadoSolicitud { IdEstadoSolicitud = 1, DescripcionEstado = "Abierta" },
            new EstadoSolicitud { IdEstadoSolicitud = 2, DescripcionEstado = "En proceso" },
            new EstadoSolicitud { IdEstadoSolicitud = 3, DescripcionEstado = "Esperando proveedor" },
            new EstadoSolicitud { IdEstadoSolicitud = 4, DescripcionEstado = "Finalizada" },
            new EstadoSolicitud { IdEstadoSolicitud = 5, DescripcionEstado = "Cancelada" }
        );
    }
}
