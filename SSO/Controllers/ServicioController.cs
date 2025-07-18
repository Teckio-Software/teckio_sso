using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaERP.BLL.Servicios.Contrato;
using SistemaERP.DTO.Menu;

namespace SistemaERP.API.Controllers.SSO
{
    /// <summary>
    /// La idea de esto es manejarlo por interfaz para no estar consumiendolo por base de datos
    /// </summary>
    [Route("api/servicios")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "VisorPermiso")]
    public class ServicioController : ControllerBase
    {
        private readonly ICatalogoMenuService _CatalogoMenuService;
        private readonly ICatalogoSeccionService _CatalogoSeccionService;
        private readonly ICatalogoActividadService _CatalogoActividadService;
        public ServicioController(
            ICatalogoMenuService CatalogoMenuService
            , ICatalogoSeccionService CatalogoSeccionService
            , ICatalogoActividadService CatalogoActividadService
            )
        {
            _CatalogoMenuService = CatalogoMenuService;
            _CatalogoSeccionService = CatalogoSeccionService;
            _CatalogoActividadService = CatalogoActividadService;
        }

        #region obtenMenus
        /// <summary>
        /// Es para obtener los menus
        /// </summary>
        /// <returns></returns>
        [HttpGet("ObtenMenus")]
        public async Task<ActionResult<List<CatalogoMenuDTO>>> obtenMenus()
        {
            return await _CatalogoMenuService.ObtenTodos();
        }
        #endregion
    }
}
