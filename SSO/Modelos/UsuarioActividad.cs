using System;
using System.Collections.Generic;

namespace SistemaERP.Model;

public partial class UsuarioActividad
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdActividad { get; set; }
    public int IdEmpresa { get; set; }

    public bool EsActivo { get; set; }

    public virtual CatalogoActividad IdActividadNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}
