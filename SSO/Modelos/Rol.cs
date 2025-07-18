using System;
using System.Collections.Generic;
using SSO.Modelos;

namespace SistemaERP.Model;

public partial class Rol
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public string? IdAspNetRole { get; set; }

    public virtual ICollection<RolActividad> RolActividads { get; set; } = new List<RolActividad>();

    public virtual ICollection<RolSeccion> RolSeccions { get; set; } = new List<RolSeccion>();

    public virtual ICollection<RolEmpresa> RolEmpresas { get; set; } = new List<RolEmpresa>();

    public virtual ICollection<UsuarioEmpresa> UsuarioEmpresas { get; set; } = new List<UsuarioEmpresa>();
    public virtual ICollection<RolProyectoEmpresaUsuario> RolProyectoEmpresaUsuarios { get; set; } = new List<RolProyectoEmpresaUsuario>();
}
