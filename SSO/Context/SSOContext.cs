using GuardarArchivos.DTO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaERP.Model;
using SistemaERP.Model.Gastos;
using SistemaERP.Model.SSO;
using SistemaERP.Models;
using SSO.Modelos;

namespace SistemaERP.DAL.DBContext;

public partial class SSOContext : IdentityDbContext
{
    /// <summary>
    /// El SSO context es el encargado de mapear toda la información de la BD encargada del logueo, alta y permisos de los usuarios dentro de uno o varios corporativos
    /// El SSO debe de estar aislado de los otros sistemas
    /// </summary>
    public SSOContext()
    {
    }

    public SSOContext(DbContextOptions<SSOContext> options)
    : base(options)
    {
        var connectionString = this.Database.GetDbConnection().ConnectionString;
    }

    public virtual DbSet<CatalogoActividad> CatalogoActividads { get; set; }

    public virtual DbSet<CatalogoMenu> CatalogoMenus { get; set; }

    public virtual DbSet<CatalogoSeccion> CatalogoSeccions { get; set; }

    public virtual DbSet<ClasificacionesCfdi> ClasificacionesCfdis { get; set; }

    public virtual DbSet<Corporativo> Corporativos { get; set; }

    public virtual DbSet<Division> Divisions { get; set; }

    public virtual DbSet<Empresa> Empresas { get; set; }

    public virtual DbSet<MenuEmpresa> MenuEmpresas { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<RolActividad> RolActividads { get; set; }

    public virtual DbSet<RolSeccion> RolSeccions { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuarioActividad> UsuarioActividads { get; set; }

    public virtual DbSet<UsuarioClasificacionCfdi> UsuarioClasificacionCfdis { get; set; }

    public virtual DbSet<UsuarioCorporativo> UsuarioCorporativos { get; set; }

    public virtual DbSet<UsuarioDivision> UsuarioDivisions { get; set; }

    public virtual DbSet<UsuarioEmpresa> UsuarioEmpresas { get; set; }
    public virtual DbSet<UsuarioEmpresaPorDefecto> UsuarioEmpresaPorDefectos { get; set; }

    public virtual DbSet<UsuarioPertenecienteACliente> UsuarioPertenecienteAclientes { get; set; }

    public virtual DbSet<UsuarioGastos> UsuarioGastos { get; set; }
    public virtual DbSet<UsuarioAutoFac> UsuarioAutoFac { get; set; }
    public virtual DbSet<ParametrosEmpresaGastos> ParametrosEmpresaGastos { get; set; }


    public virtual DbSet<UsuarioProveedor> UsuarioProveedores { get; set; }
    public virtual DbSet<UsuarioProveedorFormasPagoXempresa> UsuarioProveedorFormasPagoXempresas { get; set; }
    public virtual DbSet<UsuarioProyecto> UsuarioProyectos { get; set; }

    public virtual DbSet<UsuarioSeccion> UsuarioSeccions { get; set; }

    public virtual DbSet<UsuariosUltimaSeccion> UsuariosUltimaSeccions { get; set; }
    public virtual DbSet<Archivo> TblArchivos { get; set; }

    public virtual DbSet<Erp> Rps { get; set; }
    public virtual DbSet<Erpcorporativo> Rpempresas { get; set; }
    public virtual DbSet<RolProyectoEmpresaUsuario> RolProyectoEmpresaUsuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ParametrosTimbrado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Archivos__3214EC07B88EC560");

            entity.ToTable("ParametrosTimbrado");

            entity.Property(e => e.FechaExpedicion).HasColumnType("datetime");
            entity.Property(e => e.FechaVigencia).HasColumnType("datetime");
            entity.Property(e => e.Predeterminado).HasColumnName("predeterminado");
            entity.Property(e => e.KeyPassword).HasMaxLength(550);

            entity.HasOne(d => d.ContenidoCER).WithMany(p => p.ArchivoCer)
                .HasForeignKey(d => d.IdArchivoCer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ArchivosEmpresa_Archivo");

            entity.HasOne(d => d.ContenidoKEY).WithMany(p => p.ArchivoKey)
                .HasForeignKey(d => d.IdArchivoKey)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ArchivosEmpresa_Archivo2");

            entity.HasOne(d => d.Empresa).WithMany(p => p.ParametrosTimbrados)
                .HasForeignKey(d => d.IdEmpresa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ArchivosEmpresa_Empresa");
        });

        modelBuilder.Entity<CatalogoActividad>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Catalogo__3214EC072D27B809");

            entity.ToTable("CatalogoActividad");

            entity.Property(e => e.CodigoActividad).HasMaxLength(100);
            entity.Property(e => e.Descripcion).HasMaxLength(100);
            entity.Property(e => e.DescripcionInterna).HasMaxLength(100);

            entity.HasOne(d => d.IdSeccionNavigation).WithMany(p => p.CatalogoActividads)
                .HasForeignKey(d => d.IdSeccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CatalogoA__IdSec__2F10007B");
        });

        modelBuilder.Entity<CatalogoMenu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Catalogo__3214EC07164452B1");

            entity.ToTable("CatalogoMenu");

            entity.Property(e => e.CodigoMenu).HasMaxLength(100);
            entity.Property(e => e.Descripcion).HasMaxLength(100);
        });

        modelBuilder.Entity<CatalogoSeccion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Catalogo__3214EC071A14E395");

            entity.ToTable("CatalogoSeccion");

            entity.Property(e => e.CodigoSeccion).HasMaxLength(100);
            entity.Property(e => e.Descripcion).HasMaxLength(100);
            entity.Property(e => e.DescripcionInterna).HasMaxLength(100);

            entity.HasOne(d => d.IdMenuNavigation).WithMany(p => p.CatalogoSeccions)
                .HasForeignKey(d => d.IdMenu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CatalogoS__IdMen__1BFD2C07");
        });

        modelBuilder.Entity<ClasificacionesCfdi>(entity =>
        {
            entity.ToTable("ClasificacionesCFDI");

            entity.Property(e => e.Codigo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCorporacionNavigation).WithMany(p => p.ClasificacionesCfdis)
                .HasForeignKey(d => d.IdCorporacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClasificacionesCFDI_Corporaciones");
        });

        modelBuilder.Entity<Corporativo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Corporat__3214EC0700551192");

            entity.ToTable("Corporativo");

            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Division>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Division__3214EC07160F4887");

            entity.ToTable("Division");

            entity.Property(e => e.Descripcion).HasMaxLength(100);

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Divisions)
                .HasForeignKey(d => d.IdEmpresa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Division__IdEmpr__1DB06A4F");
        });

        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Empresa__3214EC070519C6AF");

            entity.ToTable("Empresa");

            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GuidEmpresa).HasMaxLength(50);
            entity.Property(e => e.NombreComercial)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Rfc)
                .HasMaxLength(13)
                .IsUnicode(false);
            entity.Property(e => e.CodigoPostal)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Sociedad)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCorporativoNavigation).WithMany(p => p.Empresas)
                .HasForeignKey(d => d.IdCorporativo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Empresa__IdCorpo__07F6335A");
        });

        modelBuilder.Entity<MenuEmpresa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MenuEmpr__3214EC074BAC3F29");

            entity.ToTable("MenuEmpresa");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.MenuEmpresas)
                .HasForeignKey(d => d.IdEmpresa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MenuEmpre__IdEmp__4D94879B");

            entity.HasOne(d => d.IdMenuNavigation).WithMany(p => p.MenuEmpresas)
                .HasForeignKey(d => d.IdMenu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MenuEmpre__IdMen__4E88ABD4");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rol__3214EC070AD2A005");

            entity.ToTable("Rol");

            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdAspNetRole).HasMaxLength(450);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RolActividad>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RolSecci__3214EC0734C8D9D1");

            entity.ToTable("RolActividad");

            entity.HasOne(d => d.IdActividadNavigation).WithMany(p => p.RolActividads)
                .HasForeignKey(d => d.IdActividad)
                .HasConstraintName("FK__Rol_Menu_Seccion_Actividad____IdActividad_20231204");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.RolActividads)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Rol_IdRol_20240125");
        });

        modelBuilder.Entity<RolEmpresa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_RolEmpresa_2024_02_16");

            entity.ToTable("RolEmpresa");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.RolEmpresas)
                .HasForeignKey(d => d.IdEmpresa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Empresa_2024_02_16");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.RolEmpresas)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rol_2024_02_16");
        });

        modelBuilder.Entity<RolProyectoEmpresaUsuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RolProye__3214EC07C0C00780");

            entity.ToTable("RolProyectoEmpresaUsuario");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.RolProyectoEmpresaUsuarios)
                .HasForeignKey(d => d.IdEmpresa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RolProyec__IdEmp__160F4887");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.RolProyectoEmpresaUsuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RolProyec__IdRol__17036CC0");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.RolProyectoEmpresaUsuarios)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RolProyec__IdUsu__17F790F9");
        });

        modelBuilder.Entity<RolSeccion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RolSecci__3214EC0721B6055D");

            entity.ToTable("RolSeccion");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.RolSeccions)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__RolSeccio__IdRol__73BA3083");

            entity.HasOne(d => d.IdSeccionNavigation).WithMany(p => p.RolSeccions)
                .HasForeignKey(d => d.IdSeccion)
                .HasConstraintName("FK__Rol_Menu__Seccion____IdSeccion_20231204");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuario__3214EC073A81B327");

            entity.ToTable("Usuario");

            entity.Property(e => e.Amaterno)
                .HasMaxLength(100)
                .HasColumnName("AMaterno");
            entity.Property(e => e.Apaterno)
                .HasMaxLength(100)
                .HasColumnName("APaterno");
            entity.Property(e => e.Correo).HasMaxLength(150);
            entity.Property(e => e.IdAspNetUser).HasMaxLength(450);
            entity.Property(e => e.NombreCompleto).HasMaxLength(100);
            entity.Property(e => e.NombreUsuario).HasMaxLength(100);
        });

        modelBuilder.Entity<UsuarioActividad>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UsuarioM__3214EC0744FF419A");

            entity.ToTable("UsuarioActividad");

            entity.HasOne(d => d.IdActividadNavigation).WithMany(p => p.UsuarioActividads)
                .HasForeignKey(d => d.IdActividad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuario_Menu_Seccion_Actividad____IdActividad_20231204");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuarioActividads)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsuarioMe__IdUsu__47DBAE45");
            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.UsuariosActividades)
                .HasForeignKey(d => d.IdEmpresa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioActividad_Empresa");
        });

        modelBuilder.Entity<UsuarioClasificacionCfdi>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Usuarios_ClasificacionesCFDI");

            entity.ToTable("UsuarioClasificacionCFDI");

            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");

            entity.HasOne(d => d.IdClasificacionCfdiNavigation).WithMany(p => p.UsuarioClasificacionCfdis)
                .HasForeignKey(d => d.IdClasificacionCfdi)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_ClasificacionesCFDI_ClasificacionesCFDI");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuarioClasificacionCfdis)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_ClasificacionesCFDI_Usuarios");
        });

        modelBuilder.Entity<UsuarioCorporativo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuarios__3214EC075DCAEF64");

            entity.ToTable("UsuarioCorporativo");

            entity.HasOne(d => d.IdCorporativoNavigation).WithMany(p => p.UsuarioCorporativos)
                .HasForeignKey(d => d.IdCorporativo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuarios___idCor__5FB337D6");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuarioCorporativos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuarios___idUsu__60A75C0F");
        });

        modelBuilder.Entity<UsuarioDivision>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UsuarioD__3214EC0719DFD96B");

            entity.ToTable("UsuarioDivision");

            entity.HasOne(d => d.IdDivisionNavigation).WithMany(p => p.UsuarioDivisions)
                .HasForeignKey(d => d.IdDivision)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsuarioDi__IdDiv__1CBC4616");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuarioDivisions)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsuarioDi__IdUsu__1BC821DD");
        });

        modelBuilder.Entity<UsuarioEmpresa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Usuarios_Empresas");

            entity.ToTable("UsuarioEmpresa", tb => tb.HasComment("Relación entre la organización y los usuarios"));

            entity.Property(e => e.Id)
                .HasComment("id de la organizacion")
                .HasColumnName("id");
            entity.Property(e => e.IdEmpresa).HasComment("Id de las organizaciones");
            entity.Property(e => e.IdUsuario).HasComment("Id de usuario");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.UsuarioEmpresas)
                .HasForeignKey(d => d.IdEmpresa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Empresas_Empresa");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.UsuarioEmpresas)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK_UsuarioEmpresa_Rol_2024_02_16");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuarioEmpresas)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Empresas_Usuario");
        });

        modelBuilder.Entity<UsuarioEmpresaPorDefecto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UsuarioE__3214EC072BFE89A6");

            entity.ToTable("UsuarioEmpresaPorDefecto");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.UsuarioEmpresaPorDefectos)
                .HasForeignKey(d => d.IdEmpresa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsuarioEm__IdEmp__2EDAF651");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuarioEmpresaPorDefectos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsuarioEm__IdUsu__2DE6D218");
        });

        modelBuilder.Entity<UsuarioPertenecienteACliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UsuarioP__3214EC0731B762FC");

            entity.ToTable("UsuarioPertenecienteACliente");

            entity.HasOne(d => d.IdCorporativoNavigation).WithMany(p => p.UsuarioPertenecienteAclientes)
                .HasForeignKey(d => d.IdCorporativo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsuarioPe__IdCor__3493CFA7");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuarioPertenecienteAclientes)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsuarioPe__IdUsu__339FAB6E");
        });

        modelBuilder.Entity<UsuarioProveedor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UsuarioP__3214EC0725518C17");

            entity.ToTable("UsuarioProveedor");

            entity.Property(e => e.IdentificadorFiscal).HasMaxLength(30);
            entity.Property(e => e.Rfc).HasMaxLength(13);
            entity.Property(e => e.NumeroProveedor).HasMaxLength(10);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuarioProveedors)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsuarioPr__IdUsu__2739D489");
        });

        modelBuilder.Entity<UsuarioProveedorFormasPagoXempresa>(entity =>
        {
            entity.ToTable("UsuarioProveedorFormasPagoXEmpresa");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.UsuarioProveedorFormasPagoXempresas)
                .HasForeignKey(d => d.IdEmpresa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Empresa_UsuarioProveedorFormasPagoXEmpresa");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuarioProveedorFormasPagoXempresas)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_UsuarioProveedorFormasPagoXEmpresa");
        });

        modelBuilder.Entity<UsuarioGastos>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__UsuarioG__3213E83F55F4C372");

            entity.Property(e => e.id).HasColumnName("id");
            entity.Property(e => e.apellidoMaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellidoMaterno");
            entity.Property(e => e.apellidoPaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellidoPaterno");
            entity.Property(e => e.estatus).HasColumnName("estatus");
            entity.Property(e => e.fecha_alta)
                .HasColumnType("datetime")
                .HasColumnName("fecha_alta");
            entity.Property(e => e.fecha_baja)
                .HasColumnType("datetime")
                .HasColumnName("fecha_baja");
            entity.Property(e => e.idUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.numeroEmpleado)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("numeroEmpleado");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuarioGastos)
                .HasForeignKey(d => d.idUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuario");
        });

        modelBuilder.Entity<ParametrosEmpresaGastos>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__Parametr__3213E83F82D2E83F");

            entity.Property(e => e.id).HasColumnName("id");
            entity.Property(e => e.idEmpresa).HasColumnName("idEmpresa");
            entity.Property(e => e.Permiso).IsUnicode(false);
            entity.Property(e => e.Valor).IsUnicode(false);

            entity.HasOne(d => d.idEmpresaNavigation).WithMany(p => p.ParametrosEmpresaGastos)
                .HasForeignKey(d => d.idEmpresa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ParametrosEmpresaGastos");
        });

        modelBuilder.Entity<UsuarioAutoFac>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__UsuarioA__3213E83FB2EDD867");

            entity.ToTable("UsuarioAutoFactura");

            entity.Property(e => e.id).HasColumnName("id");
            entity.Property(e => e.apellidoMaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellidoMaterno");
            entity.Property(e => e.apellidoPaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellidoPaterno");
            entity.Property(e => e.estatus).HasColumnName("estatus");
            entity.Property(e => e.fechaAlta)
                .HasColumnType("datetime")
                .HasColumnName("fechaAlta");
            entity.Property(e => e.fechaBaja)
                .HasColumnType("datetime")
                .HasColumnName("fechaBaja");
            entity.Property(e => e.idUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuarioAutoFac)
                .HasForeignKey(d => d.idUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuario_UsuarioAutoFa");
        });


        modelBuilder.Entity<UsuarioProyecto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UsuarioP__3214EC07208CD6FA");

            entity.ToTable("UsuarioProyecto");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.UsuarioProyectos)
                .HasForeignKey(d => d.IdEmpresa)
                .HasConstraintName("FK__UsuarioPr__IdEmp__29221CFB");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuarioProyectos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsuarioPr__IdUsu__22751F6C");
        });

        modelBuilder.Entity<UsuarioSeccion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UsuarioM__3214EC073F466844");

            entity.ToTable("UsuarioSeccion");

            entity.HasOne(d => d.IdSeccionNavigation).WithMany(p => p.UsuarioSeccions)
                .HasForeignKey(d => d.IdSeccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuario_Menu__Seccion____IdSeccion_20231204");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuarioSeccions)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsuarioMe__IdUsu__4222D4EF");
            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.UsuarioEmpresaSeccions)
                .HasForeignKey(d => d.IdEmpresa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioSeccion_Empresa");
        });

        modelBuilder.Entity<UsuariosUltimaSeccion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuarios__3214EC076AEFE058");

            entity.ToTable("UsuariosUltimaSeccion");

            entity.Property(e=> e.IdProyecto).HasColumnType("int");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.UsuariosUltimaSeccions)
            .HasForeignKey(d => d.IdEmpresa)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_UsuariosUltimaS_IdEmpresa_20240719");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuariosUltimaSeccions)
            .HasForeignKey(d => d.IdUsuario)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_UsuariosUltimaS_IdUsuario_20240719"); 
        }
         );
        modelBuilder.Entity<Archivo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Archivos__3214EC0711B70751");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ruta).IsUnicode(false);
        });

        modelBuilder.Entity<Erp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_RPS");

            entity.ToTable("ERP");

            entity.Property(e => e.NombreErp)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("NombreERP");
        });

        modelBuilder.Entity<Erpcorporativo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ERPEmpresa");

            entity.ToTable("ERPCorporativo");

            entity.HasOne(d => d.IdCorporativoNavigation).WithMany(p => p.Erpcorporativos)
                .HasForeignKey(d => d.IdCorporativo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ERPCorpor__IdCor__498EEC8D");

            entity.HasOne(d => d.IdErpNavigation).WithMany(p => p.Erpcorporativos)
                .HasForeignKey(d => d.IdErp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ERPCorpor__IdErp__4A8310C6");
        });

        base.OnModelCreating(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

