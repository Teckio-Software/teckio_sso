using SistemaERP.BLL.Contrato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.BLL.ProcesoSSO
{
   public class ProcesoUsuariosUltimaSeccion
    {
        private readonly IUsuariosUltimaSeccionService _UsuariosUltimaSeccion;


        public ProcesoUsuariosUltimaSeccion(
            IUsuariosUltimaSeccionService usuariosUltimaSeccion
            )
        {
            _UsuariosUltimaSeccion = usuariosUltimaSeccion;
        }

        


        


    }
}
