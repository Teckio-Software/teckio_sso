using System;
using System.Collections.Generic;

namespace SistemaERP.Model;

public partial class CatalogoMenu
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public string CodigoMenu { get; set; } = null!;

    public virtual ICollection<CatalogoSeccion> CatalogoSeccions { get; set; } = new List<CatalogoSeccion>();

    public virtual ICollection<MenuEmpresa> MenuEmpresas { get; set; } = new List<MenuEmpresa>();
}
