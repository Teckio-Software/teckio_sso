using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.Model.SSO
{
    public class ParametrosEmpresaGastos
    {
        public int id { get; set; }
        public int idEmpresa { get; set; }
        public string Permiso { get; set; }
        public string Valor { get; set; }
        public bool Estatus { get; set; }
        public bool EditablexUsuario { get; set; }

        public virtual Empresa idEmpresaNavigation { get; set; } = null!;

    }
}
