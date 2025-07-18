using System;
using System.Collections.Generic;

namespace SistemaERP.Model;

public partial class CodigosRespuestum
{
    public int Id { get; set; }

    public string Codigo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;
}
