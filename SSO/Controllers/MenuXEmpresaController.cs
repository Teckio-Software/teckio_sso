using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaERP.BLL.ContratoSSO;
using SistemaERP.BLL.Servicios.Contrato;
using SistemaERP.DTO;
using SistemaERP.DTO.Menu;
using SSO.DTO;

namespace SistemaERP.API.Controllers.SSO
{
    /// <summary>
    /// Cuando una empresa contrata un servicio se le tiene que asignar o quitar dependiendo
    /// </summary>
    [Route("api/menusXEmpresa")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Administrador")]
    public class MenuXEmpresaController : ControllerBase
    {
        private readonly ICatalogoMenuService _CatalogoMenuService;
        private readonly ICatalogoSeccionService _CatalogoSeccionService;
        private readonly ICatalogoActividadService _CatalogoActividadService;
        private readonly IMenuEmpresaService _MenuEmpresaService;
        private readonly IMapper _Mapper;
        public MenuXEmpresaController(
            IMapper zMapper
            , ICatalogoMenuService CatalogoMenuService
            , ICatalogoSeccionService CatalogoSeccionService
            , ICatalogoActividadService CatalogoActividadService
            , IMenuEmpresaService MenuEmpresaService
            , ICatalogoMenuService menuService
            , ICatalogoSeccionService SeccionService
            , ICatalogoActividadService ActividadService
            )
        {
            _Mapper = zMapper;
            _MenuEmpresaService = MenuEmpresaService;
            _CatalogoMenuService = CatalogoMenuService;
            _CatalogoSeccionService = CatalogoSeccionService;
            _CatalogoActividadService = CatalogoActividadService;
        }

        /// <summary>
        /// Para consultar los servicios que tienen disponibles las empresas
        /// </summary>
        /// <param name="IdEmpresa"></param>
        /// <returns></returns>
        [HttpGet("menusActivosEstructura/{IdEmpresa:int}")]
        public async Task<ActionResult<List<MenuEstructuraDTO>>> ObtenMenusActivosCompletoEstructuraXIdEmpresa(int IdEmpresa)
        {
            List<MenuEstructuraDTO> menusEstructura = new List<MenuEstructuraDTO>();
            if (IdEmpresa <= 0)
            {
                return menusEstructura;
            }
            var catalogosMenus = await _CatalogoMenuService.ObtenTodos();
            var menusEmpresa = await _MenuEmpresaService.ObtenXIdEmpresa(IdEmpresa);
            for (int i = 0; i < catalogosMenus.Count; i++)
            {
                var ListaEsActivoMenu = menusEmpresa.Where(z => z.IdMenu == catalogosMenus[i].Id).ToList();
                List<MenuEstructuraDTO> seccionesEstrcutura = new List<MenuEstructuraDTO>();
                var secciones = new List<CatalogoSeccionDTO>();
                if (ListaEsActivoMenu.Count() > 0)
                {
                    secciones = await _CatalogoSeccionService.ObtenTodosXIdMenu(catalogosMenus[i].Id);
                }

                var catalogoSecciones = await _CatalogoSeccionService.ObtenTodosXIdMenu(catalogosMenus[i].Id);
                for (int j = 0; j < catalogoSecciones.Count; j++)
                {
                    var ListaEsActivoSeccion = secciones.Where(z => z.CodigoSeccion == catalogoSecciones[j].CodigoSeccion);
                    var catalogoActividades = await _CatalogoActividadService.ObtenTodosXIdSeccion(catalogoSecciones[j].Id);
                    List<MenuEstructuraDTO> actividadesEstrcutura = new List<MenuEstructuraDTO>();
                    var actividades = new List<CatalogoActividadDTO>();
                    if (ListaEsActivoSeccion.Count() > 0)
                    {
                        actividades = await _CatalogoActividadService.ObtenTodosXIdSeccion(catalogoSecciones[j]!.Id);
                    }
                    for (int k = 0; k < catalogoActividades.Count; k++)
                    {
                        var ListaEsActivoActividad = actividades.Where(z => z.CodigoActividad == catalogoActividades[k].CodigoActividad);
                        actividadesEstrcutura.Add(new MenuEstructuraDTO()
                        {
                            IdMenu = catalogosMenus[i].Id,
                            IdSeccion = catalogoSecciones[j].Id,
                            IdActividad = catalogoActividades[k].Id,
                            TipoMenu = 3,
                            Descripcion = catalogoActividades[k].Descripcion!,
                            EsAutorizado = ListaEsActivoActividad.Count() > 0 ? true : false,
                            Estructura = new List<MenuEstructuraDTO>()
                        });
                    }
                    //menus[i].ListaSecciones[j].ListaActividades = actividades;
                    seccionesEstrcutura.Add(new MenuEstructuraDTO()
                    {
                        IdMenu = catalogosMenus[i].Id,
                        IdSeccion = catalogoSecciones[j].Id,
                        IdActividad = 0,
                        TipoMenu = 2,
                        Descripcion = catalogoSecciones[j].Descripcion!,
                        EsAutorizado = ListaEsActivoSeccion.Count() > 0 ? true : false,
                        Estructura = actividadesEstrcutura
                    });
                }
                menusEstructura.Add(new MenuEstructuraDTO()
                {
                    IdMenu = catalogosMenus[i].Id,
                    IdSeccion = 0,
                    IdActividad = 0,
                    TipoMenu = 1,
                    Descripcion = catalogosMenus[i].Descripcion!,
                    EsAutorizado = ListaEsActivoMenu.Count() > 0 ? true : false,
                    Estructura = seccionesEstrcutura
                });
            }

            return menusEstructura;
        }
        /// <summary>
        /// Para activar un servicio en una empresa
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [HttpPost("activaMenuAEmpresa")]
        public async Task<ActionResult<RespuestaDTO>> ActivaMenuAEmpresa([FromBody] MenuEmpresaDTO parametro)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var existeMenuEmpresa = await _MenuEmpresaService.ObtenXIdEmpresaYMenu(parametro.IdEmpresa, parametro.IdMenu);
            if (existeMenuEmpresa.Id > 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "Ya existe la relacion menu empresa";
                return respuesta;
            }
            var resultadoMenuEmpresa = await _MenuEmpresaService.Crear(parametro);
            if (resultadoMenuEmpresa.Id > 0)
            {
                respuesta.Estatus = true;
                respuesta.Descripcion = "Relacion menu y empresa creada";
                return respuesta;
            }
            else
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "Algo salió mal";
                return respuesta;
            }
        }
        /// <summary>
        /// Para desactivar un menu a una empresa
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [HttpPost("desactivaMenuAEmpresa")]
        public async Task<ActionResult<RespuestaDTO>> DesactivaMenuAEmpresa([FromBody] MenuEmpresaDTO parametro)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var existeMenuEmpresa = await _MenuEmpresaService.ObtenXIdEmpresaYMenu(parametro.IdEmpresa, parametro.IdMenu);
            if (existeMenuEmpresa.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No existe la relacion menu empresa";
                return respuesta;
            }
            var resultadoMenuEmpresa = await _MenuEmpresaService.Eliminar(existeMenuEmpresa.Id);
            if (resultadoMenuEmpresa)
            {
                respuesta.Estatus = true;
                respuesta.Descripcion = "Relacion menu y empresa eliminada";
                return respuesta;
            }
            else
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "Algo salió mal";
                return respuesta;
            }
        }
    }
}
