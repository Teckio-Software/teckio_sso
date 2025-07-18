using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaERP.BLL.ContratoSSO;
using SistemaERP.BLL.Servicios.Contrato;
using SistemaERP.BLL.SubProcesoSSO;
using SistemaERP.DTO;
using SistemaERP.DTO.Menu;

namespace SistemaERP.API.Controllers.SSO
{
    /// <summary>
    /// La idea de esto es manejarlo por interfaz para no estar consumiendolo por base de datos
    /// I don't remember what is used
    /// </summary>
    [Route("api/servicios")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "VisorPermiso")]
    public class ServiciosController : ControllerBase
    {
        private readonly EstructuraServicios _EstructuraServicios;
        public ServiciosController(
            EstructuraServicios EstructuraServicios
            )
        {
            _EstructuraServicios = EstructuraServicios;
        }
        /// <summary>
        /// Obtiene la estructura de los menus (servicios) con sus secciones y sus actividades relacionadas.
        /// </summary>
        /// <returns></returns>
        [HttpGet("serviciosEstructura")]
        public async Task<ActionResult<List<CatalogoSeccionConMenuDTO>>> ObtenEstructuraSerivicios()
        {
            var estructura = await _EstructuraServicios.ObtenEstructuraSerivicios();
            return estructura;
        }
    }
}
