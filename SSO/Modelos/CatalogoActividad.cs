using System;
using System.Collections.Generic;

namespace SistemaERP.Model;

public partial class CatalogoActividad
{
    public int Id { get; set; }

    public int IdSeccion { get; set; }

    public string Descripcion { get; set; } = null!;

    public string CodigoActividad { get; set; } = null!;
    public string? DescripcionInterna { get; set; }
    public bool EsActividadUnica { get; set; }

    public virtual CatalogoSeccion IdSeccionNavigation { get; set; } = null!;

    public virtual ICollection<RolActividad> RolActividads { get; set; } = new List<RolActividad>();

    public virtual ICollection<UsuarioActividad> UsuarioActividads { get; set; } = new List<UsuarioActividad>();
}
