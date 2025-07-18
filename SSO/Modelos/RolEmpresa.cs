using System;
using System.Collections.Generic;

namespace SistemaERP.Model;

public partial class RolEmpresa
{
    public int Id { get; set; }

    public int IdRol { get; set; }

    public int IdEmpresa { get; set; }

    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    public virtual Rol IdRolNavigation { get; set; } = null!;
}
