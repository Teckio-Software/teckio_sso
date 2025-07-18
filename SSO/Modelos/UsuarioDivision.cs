using System;
using System.Collections.Generic;

namespace SistemaERP.Model;

public partial class UsuarioDivision
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdDivision { get; set; }

    public virtual Division IdDivisionNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
