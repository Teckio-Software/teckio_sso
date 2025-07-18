using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaERP.BLL.Contrato;
using SistemaERP.BLL.ProcesoSSO;
using SistemaERP.DTO.SSO;

namespace SistemaERP.API.Controllers.SSO
{
    /// <summary>
    /// Se usa en proveedores para las formas de pago por usuario
    /// </summary>
    [Route("api/UsuarioFormaPagoPermitidaEmpresa")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsuarioFormaPagoPermitidaEmpresaController : ControllerBase
    {
        private readonly ProcesoUsuarioFormaPagoPermitidaPorEmpresa _ProcesoUsuarioFormaPagoPermitidaPorEmpresa;
        private readonly IUsuarioProveedorFormasPagoXEmpresaService _UsuarioProveedorFormasPagoService;
        public UsuarioFormaPagoPermitidaEmpresaController(
            ProcesoUsuarioFormaPagoPermitidaPorEmpresa ProcesoUsuarioFormaPagoPermitidaPorEmpresa
            , IUsuarioProveedorFormasPagoXEmpresaService UsuarioProveedorFormasPagoService
            )
        {
            _ProcesoUsuarioFormaPagoPermitidaPorEmpresa = ProcesoUsuarioFormaPagoPermitidaPorEmpresa;
            _UsuarioProveedorFormasPagoService = UsuarioProveedorFormasPagoService;
        }
        /// <summary>
        /// Para obtener las relaciones de pago en cada usuario por empresa
        /// </summary>
        /// <param name="IdUsuario"></param>
        /// <param name="IdEmpresa"></param>
        /// <returns></returns>
        [HttpGet("ObtenRelacionUsuarioEmpresaFormaPago/{IdUsuario:int}/{IdEmpresa:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UsuarioFormasPagoXEmpresaDTO>> ObtenRelacionUsuarioEmpresaFormaPago(int IdUsuario, int IdEmpresa)
        {
            return await _UsuarioProveedorFormasPagoService.ObtenXIdUsuarioEIdEmpresa(IdUsuario, IdEmpresa);
        }
        /// <summary>
        /// Para la creación de la relación de los usuarios con las empresas
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [HttpPost("CreaRelacionUsuarioEmpresaFormaPago")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<bool>> CreaRelacionUsuarioEmpresaFormaPago([FromBody] UsuarioFormasPagoXEmpresaDTO parametro)
        {
            return await _ProcesoUsuarioFormaPagoPermitidaPorEmpresa.CreaRelacionUsuarioFormaPagoEmpresa(parametro);
        }
    }
}
