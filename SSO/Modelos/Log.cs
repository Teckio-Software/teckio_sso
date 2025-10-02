using SistemaERP.Model;
using SistemaERP.Models;
using System;
using System.Collections.Generic;

namespace SistemaERP.Model;

public partial class LogRegistro
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public string Nivel { get; set; } = null!;

    public string Metodo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string DbContext { get; set; } = null!;

    public int IdUsuario { get; set; }

    public int IdEmpresa { get; set; }
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

}
