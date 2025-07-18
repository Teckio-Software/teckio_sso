using System;
using System.Collections.Generic;

namespace SistemaERP.Model;

public partial class ClasificacionesCfdi
{
    public int Id { get; set; }

    public string Codigo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public int IdCorporacion { get; set; }

    public virtual Corporativo IdCorporacionNavigation { get; set; } = null!;

    public virtual ICollection<UsuarioClasificacionCfdi> UsuarioClasificacionCfdis { get; set; } = new List<UsuarioClasificacionCfdi>();
}
