using System;
using System.Collections.Generic;

namespace SistemaERP.Model;

public partial class CatalogoSeccion
{
    public int Id { get; set; }

    public int IdMenu { get; set; }

    public string Descripcion { get; set; } = null!;

    public string CodigoSeccion { get; set; } = null!;
    public string? DescripcionInterna { get; set; }
    public bool EsSeccionUnica { get; set; }

    public virtual ICollection<CatalogoActividad> CatalogoActividads { get; set; } = new List<CatalogoActividad>();

    public virtual CatalogoMenu IdMenuNavigation { get; set; } = null!;

    public virtual ICollection<RolSeccion> RolSeccions { get; set; } = new List<RolSeccion>();

    public virtual ICollection<UsuarioSeccion> UsuarioSeccions { get; set; } = new List<UsuarioSeccion>();
}
