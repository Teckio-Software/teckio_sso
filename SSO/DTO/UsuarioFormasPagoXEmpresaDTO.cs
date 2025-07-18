using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.DTO.SSO
{
    public class UsuarioFormasPagoXEmpresaDTO
    {
        public int Id { get; set; }

        public int IdUsuario { get; set; }

        public int IdEmpresa { get; set; }

        public bool EsPue { get; set; }

        public bool EsPpd { get; set; }
    }
}
