using System;
using System.Collections.Generic;

namespace SistemaERP.Model;

public partial class RolSeccion
{
    public int Id { get; set; }

    public int? IdSeccion { get; set; }

    public int? IdRol { get; set; }

    public virtual Rol? IdRolNavigation { get; set; }

    public virtual CatalogoSeccion? IdSeccionNavigation { get; set; }

}
