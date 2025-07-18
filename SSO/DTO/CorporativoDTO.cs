using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.DTO.SSO
{
    public class CorporativoDTO
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int Estatus { get; set; }
        public int? IdERP { get; set; }
    }
}
