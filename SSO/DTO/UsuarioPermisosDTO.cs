using SistemaERP.DTO.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.DTO.SSO
{
    public class UsuarioPermisosDTO
    {
    }
    /// <summary>
    /// Para la creación de los permisos para un usuario
    /// </summary>
    public class UsuarioRolEmpresaPermisosCreacionDTO
    {
        public int IdEmpresa { get; set; }
        /// <summary>
        /// Este puede no ser el rol verdadero, se debe buscar en base a la empresa
        /// </summary>
        public int IdRol { get; set; }
        /// <summary>
        /// Se debe de comparar con la lista base de las secciones
        /// </summary>
        public List<CatalogoSeccionConMenuDTO> ListaSecciones { get; set; }
    }
}
