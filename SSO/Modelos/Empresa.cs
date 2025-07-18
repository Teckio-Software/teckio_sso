using SistemaERP.Model.SSO;
using SSO.Modelos;
using System;
using System.Collections.Generic;

namespace SistemaERP.Model;

public partial class Empresa
{
    public int Id { get; set; }

    public string NombreComercial { get; set; } = null!;

    public string Rfc { get; set; } = null!;
    public string CodigoPostal { get; set; } = null!;

    public bool? Estatus { get; set; }

    public int IdCorporativo { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public string? Sociedad { get; set; }

    public string? GuidEmpresa { get; set; }

    public virtual ICollection<Division> Divisions { get; set; } = new List<Division>();
    public virtual ICollection<ParametrosEmpresaGastos> ParametrosEmpresaGastos { get; set; } = new List<ParametrosEmpresaGastos>();

    public virtual Corporativo IdCorporativoNavigation { get; set; } = null!;

    public virtual ICollection<MenuEmpresa> MenuEmpresas { get; set; } = new List<MenuEmpresa>();

    public virtual ICollection<UsuarioEmpresa> UsuarioEmpresas { get; set; } = new List<UsuarioEmpresa>();
    public virtual ICollection<UsuarioProyecto> UsuarioProyectos { get; set; } = new List<UsuarioProyecto>();
    public virtual ICollection<UsuarioEmpresaPorDefecto> UsuarioEmpresaPorDefectos { get; set; } = new List<UsuarioEmpresaPorDefecto>();
    public virtual ICollection<RolEmpresa> RolEmpresas { get; set; } = new List<RolEmpresa>();
    public virtual ICollection<UsuarioProveedorFormasPagoXempresa> UsuarioProveedorFormasPagoXempresas { get; set; } = new List<UsuarioProveedorFormasPagoXempresa>();
    public virtual ICollection<UsuariosUltimaSeccion> UsuariosUltimaSeccions { get; set; } = new List<UsuariosUltimaSeccion>();
    public virtual ICollection<UsuarioSeccion> UsuarioEmpresaSeccions { get; set; } = new List<UsuarioSeccion>();
    public virtual ICollection<UsuarioActividad> UsuariosActividades { get; set; } = new List<UsuarioActividad>();
    public virtual ICollection<ParametrosTimbrado> ParametrosTimbrados { get; set; } = new List<ParametrosTimbrado>();
    public virtual ICollection<RolProyectoEmpresaUsuario> RolProyectoEmpresaUsuarios { get; set; } = new List<RolProyectoEmpresaUsuario>();
}
