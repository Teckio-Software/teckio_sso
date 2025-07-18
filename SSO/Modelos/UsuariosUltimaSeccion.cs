using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.Model;

public partial class UsuariosUltimaSeccion{
    public int Id { get; set; }

    public int IdProyecto { get; set; }

    public int IdEmpresa { get; set; }

    public int IdUsuario { get; set; }

    public virtual Empresa IdEmpresaNavigation { get; set; } = null;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null;

}

