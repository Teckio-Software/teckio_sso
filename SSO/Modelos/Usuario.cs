using SistemaERP.Model.Gastos;
using SSO.Modelos;
using System;
using System.Collections.Generic;

namespace SistemaERP.Model;

public partial class Usuario
{
    public int Id { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string NombreUsuario { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public bool Activo { get; set; }

    public string? IdAspNetUser { get; set; }

    public string? Apaterno { get; set; }

    public string? Amaterno { get; set; }

    public virtual ICollection<UsuarioActividad> UsuarioActividads { get; set; } = new List<UsuarioActividad>();

    public virtual ICollection<UsuarioClasificacionCfdi> UsuarioClasificacionCfdis { get; set; } = new List<UsuarioClasificacionCfdi>();

    public virtual ICollection<UsuarioCorporativo> UsuarioCorporativos { get; set; } = new List<UsuarioCorporativo>();

    public virtual ICollection<UsuarioDivision> UsuarioDivisions { get; set; } = new List<UsuarioDivision>();

    public virtual ICollection<UsuarioEmpresaPorDefecto> UsuarioEmpresaPorDefectos { get; set; } = new List<UsuarioEmpresaPorDefecto>();

    public virtual ICollection<UsuarioEmpresa> UsuarioEmpresas { get; set; } = new List<UsuarioEmpresa>();

    public virtual ICollection<UsuarioPertenecienteACliente> UsuarioPertenecienteAclientes { get; set; } = new List<UsuarioPertenecienteACliente>();

    public virtual ICollection<UsuarioProveedor> UsuarioProveedors { get; set; } = new List<UsuarioProveedor>();
    public virtual ICollection<UsuarioGastos> UsuarioGastos { get; set; } = new List<UsuarioGastos>();
    public virtual ICollection<UsuarioAutoFac> UsuarioAutoFac { get; set; } = new List<UsuarioAutoFac>();
    public virtual ICollection<UsuarioProveedorFormasPagoXempresa> UsuarioProveedorFormasPagoXempresas { get; set; } = new List<UsuarioProveedorFormasPagoXempresa>();
    public virtual ICollection<UsuarioProyecto> UsuarioProyectos { get; set; } = new List<UsuarioProyecto>();

    public virtual ICollection<UsuarioSeccion> UsuarioSeccions { get; set; } = new List<UsuarioSeccion>();

    public virtual ICollection<UsuariosUltimaSeccion> UsuariosUltimaSeccions { get; set; } = new List<UsuariosUltimaSeccion>();
    public virtual ICollection<RolProyectoEmpresaUsuario> RolProyectoEmpresaUsuarios { get; set; } = new List<RolProyectoEmpresaUsuario>();
}
