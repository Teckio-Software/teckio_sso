using System;
using System.Collections.Generic;

namespace SistemaERP.Model;

/// <summary>
/// Relación entre la organización y los usuarios
/// </summary>
public partial class UsuarioEmpresa
{
    /// <summary>
    /// id de la organizacion
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Id de las organizaciones
    /// </summary>
    public int IdEmpresa { get; set; }

    /// <summary>
    /// Id de usuario
    /// </summary>
    public int IdUsuario { get; set; }

    public bool Activo { get; set; }

    public int? IdRol { get; set; }

    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
    public virtual Rol? IdRolNavigation { get; set; }
}
