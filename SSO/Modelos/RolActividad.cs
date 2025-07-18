using System;
using System.Collections.Generic;

namespace SistemaERP.Model;

public partial class RolActividad
{
    public int Id { get; set; }


    public int? IdActividad { get; set; }

    public int? IdRol { get; set; }

    public virtual CatalogoActividad? IdActividadNavigation { get; set; }

    public virtual Rol? IdRolNavigation { get; set; }

}
