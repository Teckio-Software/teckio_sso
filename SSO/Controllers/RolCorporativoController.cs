using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaERP.BLL.ProcesoSSO;
using SistemaERP.BLL.SubProcesoSSO;
using SistemaERP.DTO;
using SSO.DTO;

namespace SistemaERP.API.Controllers.SSO
{
    [Route("api/rolPorCoporativo")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "VisorPermiso")]
    public class RolCorporativoController : ControllerBase
    {
        private readonly ProcesoRol _ProcesoRol;
        private readonly ObtenRolesXCorporativo _RolXCorporativo;
        private readonly ObtenEmpresasEnRoles _EmpresasEnRoles;
        /// <summary>
        /// Constructor de las cuentas controller
        /// </summary>
        public RolCorporativoController(
            ProcesoRol ProcesoRol
            , ObtenRolesXCorporativo RolXCorporativo
            , ObtenEmpresasEnRoles EmpresasEnRoles
            )
        {
            _ProcesoRol = ProcesoRol;
            _RolXCorporativo = RolXCorporativo;
            _EmpresasEnRoles = EmpresasEnRoles;
        }
        #region RolProveedores
        /// <summary>
        /// Crea roles para diferentes empresas dentro de un mismo corporativo
        /// Crea los roles con permisos para todas las empresas
        /// Tiene la opcion de crear los roles con permisos unicos por empresa
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [HttpPost("CrearRolVariasEmpresas")]
        public async Task<ActionResult<RespuestaDTO>> CrearRolVariasEmpresas([FromBody] RolCreacionVarisEmpresasMismoCorporativoDTO parametro)
        {
            var resultado = await _ProcesoRol.ProcesoCrearRolEnVariasEmpresa(parametro);
            return resultado;
        }
        /// <summary>
        /// Obtiene los roles que están dentro de un mismo corporativo con el mismo nombre
        /// </summary>
        /// <param name="idCorporativo"></param>
        /// <returns></returns>
        [HttpGet("ObtenRolesXCorporativo/{idCorporativo:int}")]
        public async Task<ActionResult<List<RolDTO>>> ObtenRolesXCorporativo(int idCorporativo)
        {
            var resultado = await _RolXCorporativo.ProcesoObtenRolesXCorporativo(idCorporativo);
            return resultado;
        }
        /// <summary>
        /// Obtiene las empresas que tienen un rol con un mismo nombre
        /// </summary>
        /// <param name="idCorporativo"></param>
        /// <param name="idRol"></param>
        /// <returns></returns>
        [HttpGet("ObtenEmpresasQueTienenRolesActivos/{idCorporativo:int}/{idRol:int}")]
        public async Task<ActionResult<List<RolesActivosEnEmpresaDTO>>> ObtenEmpresasQueTienenRolesActivos(int idCorporativo, int idRol)
        {
            var resultado = await _EmpresasEnRoles.ObtenEmpresasConRolActivo(idCorporativo, idRol);
            return resultado;
        }
        /// <summary>
        /// Método especial
        /// Dependiendo un rol busca el id del rol respecto a esa empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idRol"></param>
        /// <returns></returns>
        [HttpGet("BuscaIdRolXOtroIdRol/{idEmpresa:int}/{idRol:int}")]
        public async Task<ActionResult<RolDTO>> BuscaIdRolXOtroIdRol(int idEmpresa, int idRol)
        {
            var resultado = await _EmpresasEnRoles.EncuentraIdRolXOtroIdRol(idEmpresa, idRol);
            return resultado;
        }
        #endregion
    }
}
