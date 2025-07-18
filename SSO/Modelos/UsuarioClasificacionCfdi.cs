using System;
using System.Collections.Generic;

namespace SistemaERP.Model;

public partial class UsuarioClasificacionCfdi
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdClasificacionCfdi { get; set; }

    public virtual ClasificacionesCfdi IdClasificacionCfdiNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
