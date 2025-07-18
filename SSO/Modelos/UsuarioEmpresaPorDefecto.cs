using System;
using System.Collections.Generic;

namespace SistemaERP.Model;

public partial class UsuarioEmpresaPorDefecto
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdEmpresa { get; set; }

    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
