using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.DTO.SSO
{
    public class ParametrosEmpresaGastosDTO
    {
        public int id { get; set; }
        public int idEmpresa { get; set; }
        public string Permiso { get; set; }
        public string Valor { get; set; }
        public bool Estatus { get; set; }
        public bool EditablexUsuario { get; set; }
    }
}
