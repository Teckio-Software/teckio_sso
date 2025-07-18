using System;
using System.Collections.Generic;

namespace SistemaERP.Model;

public partial class UsuarioCorporativo
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdCorporativo { get; set; }

    public virtual Corporativo IdCorporativoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
