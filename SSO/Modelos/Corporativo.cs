using System;
using System.Collections.Generic;

namespace SistemaERP.Model;

public partial class Corporativo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public bool? Estatus { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<ClasificacionesCfdi> ClasificacionesCfdis { get; set; } = new List<ClasificacionesCfdi>();

    public virtual ICollection<Empresa> Empresas { get; set; } = new List<Empresa>();
    public virtual ICollection<Erpcorporativo> Erpcorporativos { get; set; } = new List<Erpcorporativo>();

    public virtual ICollection<UsuarioCorporativo> UsuarioCorporativos { get; set; } = new List<UsuarioCorporativo>();
    public virtual ICollection<UsuarioPertenecienteACliente> UsuarioPertenecienteAclientes { get; set; } = new List<UsuarioPertenecienteACliente>();
}
