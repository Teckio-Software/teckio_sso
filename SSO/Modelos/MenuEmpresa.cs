using System;
using System.Collections.Generic;

namespace SistemaERP.Model;

public partial class MenuEmpresa
{
    public int Id { get; set; }

    public int IdEmpresa { get; set; }

    public int IdMenu { get; set; }

    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    public virtual CatalogoMenu IdMenuNavigation { get; set; } = null!;
}
