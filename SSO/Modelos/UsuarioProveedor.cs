using System;
using System.Collections.Generic;

namespace SistemaERP.Model;

public partial class UsuarioProveedor
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public string? Rfc { get; set; }

    public string NumeroProveedor { get; set; } = null!;

    public string IdentificadorFiscal { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
