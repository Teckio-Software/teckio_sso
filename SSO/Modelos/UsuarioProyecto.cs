using System;
using System.Collections.Generic;

namespace SistemaERP.Model;

public partial class UsuarioProyecto
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }
    public int IdEmpresa { get; set; }

    public int IdProyecto { get; set; }
    public bool Estatus { get; set; }
    public virtual Empresa? IdEmpresaNavigation { get; set; }
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
