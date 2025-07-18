using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SistemaERP.BLL.Contrato;
using SistemaERP.BLL.Servicios.Contrato;
using SistemaERP.DTO;
using SistemaERP.DTO.Menu;
using SSO.DTO;
using System.Data;
using System.Security.Claims;

namespace SistemaERP.API.Controllers.SSO
{
    /// <summary>
    /// Controlador para darle permisos especiales a los usuarios por empresa
    /// </summary>
    [Route("api/UsuarioSeccion")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
    public class UsuarioSeccionController : ControllerBase
    {
        private readonly IUsuarioSeccionService _UsuarioMenuSeccionService;
        private readonly IUsuarioActividadService _UsuarioMenuSeccionActividadService;
        private readonly ICatalogoMenuService _CatalogoMenuService;
        private readonly ICatalogoSeccionService _CatalogoSeccionService;
        private readonly ICatalogoActividadService _CatalogoActividadService;
        private readonly IRolService _RolService;
        private readonly IRolSeccionService _RolMenuSeccionService;
        private readonly IRolActividadService _RolMenuSeccionActividadService;
        private readonly IUsuarioService _UsuarioService;
        private readonly IEmpresaService _EmpresaServicio;
        private readonly UserManager<IdentityUser> zvUserManager;
        private readonly RoleManager<IdentityRole> zvRoleManager;
        private readonly IConfiguration zvConfiguration;
        private readonly SignInManager<IdentityUser> zvSignInManager;
        private readonly IMapper _Mapper;

        public UsuarioSeccionController(
            UserManager<IdentityUser> zUserManager
            , RoleManager<IdentityRole> zRoleManager
            , IConfiguration zConfiguration
            , SignInManager<IdentityUser> zSignInManager
            , IMapper zMapper
            , IUsuarioSeccionService UsuarioMenuSeccionService
            , IUsuarioActividadService UsuarioMenuSeccionActividadService
            , ICatalogoMenuService CatalogoMenuService
            , ICatalogoSeccionService CatalogoSeccionService
            , ICatalogoActividadService CatalogoActividadService
            , IEmpresaService EmpresaServicio
            , IRolService RolService
            , IRolSeccionService RolMenuSeccionService
            , IRolActividadService RolMenuSeccionActividadService
            , IUsuarioService UsuarioService)
        {
            zvUserManager = zUserManager;
            zvRoleManager = zRoleManager;
            zvConfiguration = zConfiguration;
            zvSignInManager = zSignInManager;
            _Mapper = zMapper;
            _EmpresaServicio = EmpresaServicio;
            _RolService = RolService;
            _RolMenuSeccionService = RolMenuSeccionService;
            _RolMenuSeccionActividadService = RolMenuSeccionActividadService;
            _UsuarioService = UsuarioService;
            _UsuarioMenuSeccionService = UsuarioMenuSeccionService;
            _UsuarioMenuSeccionActividadService = UsuarioMenuSeccionActividadService;
            _CatalogoMenuService = CatalogoMenuService;
            _CatalogoSeccionService = CatalogoSeccionService;
            _CatalogoActividadService = CatalogoActividadService;
        }

        #region DesactivarPermisoUsuario
        /// <summary>
        /// Si tu quitas un permiso de sección no tienes permisos para ver la pantalla
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [HttpPost("quitarPermisoSeccionAUsuario")]
        public async Task<ActionResult<RespuestaDTO>> quitarPermisoSeccionAUsuario([FromBody] CambiaPermisoSeccion parametro)
        {
            RespuestaDTO resp = new RespuestaDTO();
            resp.Estatus = false;
            resp.Descripcion = "Algo salió mal";
            var usuarioEmpresa = await _UsuarioService.ObtenXIdUsuario(parametro.IdUsuario);
            if (usuarioEmpresa.Id <= 0)
            {
                return resp;
            }
            var seccion = await _CatalogoSeccionService.ObtenXId(parametro.IdSeccion);
            if (seccion.Id <= 0)
            {
                return resp;
            }
            //var actividades = await _CatalogoActividadService.ObtenTodosXIdSeccion(parametro.IdSeccion);
            //if (actividades.Count() <= 0)
            //{
            //    return NoContent();
            //}
            var usuarioIdentity = await zvUserManager.FindByIdAsync(usuarioEmpresa.IdAspNetUser!);
            if (usuarioIdentity == null)
            {
                return resp;
            }
            var claimsUsuarioIdentity = await zvUserManager.GetClaimsAsync(usuarioIdentity!);
            string CodigoDistintivoSeccion = seccion.Descripcion + "-" + parametro.IdEmpresa;
            string CodigoValorSeccion = seccion.CodigoSeccion + "-" + parametro.IdEmpresa;
            var claimAQuitarSeccion = claimsUsuarioIdentity.Where(z => z.ValueType == CodigoDistintivoSeccion).Where(z => z.Value == CodigoValorSeccion).ToList();
            bool respuestaIdentityBool = false;
            if (claimAQuitarSeccion.Count() > 0)
            {
                var respuestaIdentity = await zvUserManager.RemoveClaimAsync(usuarioIdentity!, claimAQuitarSeccion.FirstOrDefault()!);
                respuestaIdentityBool = respuestaIdentity.Succeeded;
            }
            //var actividadesAQuitar = await _UsuarioMenuSeccionActividadService.ObtenXIdUsuario(usuarioEmpresa.Id);
            //for (int i = 0; i < actividades.Count; i++)
            //{
            //    var actividadAQuitar = actividadesAQuitar.Where(z => z.IdActividad == actividades[i].Id).ToList();
            //    if (actividadAQuitar.Count() > 0)
            //    {
            //        string CodigoDistintivoActividad = actividades[i].Descripcion + "-" + parametro.IdEmpresa;
            //        string CodigoValorActividad = actividades[i].CodigoActividad + "-" + parametro.IdEmpresa;
            //        var claimAQuitarActividad = claimsUsuarioIdentity.Where(z => z.ValueType == CodigoDistintivoActividad).Where(z => z.Value == CodigoValorActividad).ToList();
            //        if (claimAQuitarActividad.Count() > 0)
            //        {
            //            await zvUserManager.RemoveClaimAsync(usuarioIdentity!, claimAQuitarActividad.FirstOrDefault()!);
            //        }
            //        await _UsuarioMenuSeccionActividadService.DesactivarPermisoUsuario(actividadAQuitar.FirstOrDefault()!.Id);
            //    }
            //}
            var seccionesParaQuitar = await _UsuarioMenuSeccionService.ObtenXIdUsuario(usuarioEmpresa.Id);
            var seccionParaQuitar = seccionesParaQuitar.Where(z => z.IdSeccion == seccion.Id).ToList();
            if (seccionParaQuitar.Count() > 0)
            {
                var resultado = await _UsuarioMenuSeccionService.DesactivarPermisoUsuario(seccionParaQuitar.FirstOrDefault()!.Id);
                if (resultado.Estatus == true && respuestaIdentityBool)
                {
                    return resultado;
                }
            }
            return resp;
        }
        /// <summary>
        /// Para quitar el permiso de un usuario en la base de datos a una empresa en específico
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [HttpPost("quitarPermisoActividadAUsuario")]
        public async Task<ActionResult<RespuestaDTO>> quitarPermisoActividadAUsuario([FromBody] CambiaPermisoActividad parametro)
        {
            RespuestaDTO resp = new RespuestaDTO();
            resp.Estatus = false;
            resp.Descripcion = "Algo salió mal";
            var usuarioEmpresa = await _UsuarioService.ObtenXIdUsuario(parametro.IdUsuario);
            if (usuarioEmpresa.Id <= 0)
            {
                return resp;
            }
            var actividad = await _CatalogoActividadService.ObtenXId(parametro.IdActividad);
            if (actividad.Id <= 0)
            {
                return resp;
            }
            var usuarioIdentity = await zvUserManager.FindByIdAsync(usuarioEmpresa.IdAspNetUser!);
            if (usuarioIdentity == null)
            {
                return resp;
            }
            var claimsUsuarioIdentity = await zvUserManager.GetClaimsAsync(usuarioIdentity!);
            string actividadCodigoDistintivo = actividad.Descripcion + "-" + parametro.IdEmpresa;
            string actividadCodigoValor = actividad.CodigoActividad + "-" + parametro.IdEmpresa;
            var claimAQuitar = claimsUsuarioIdentity.Where(z => z.ValueType == actividadCodigoDistintivo).Where(z => z.Value == actividadCodigoValor).ToList();
            bool respuestaIdentityBool = false;
            if (claimAQuitar.Count() > 0)
            {
                var respuestaIdentity = await zvUserManager.RemoveClaimAsync(usuarioIdentity!, claimAQuitar.FirstOrDefault()!);
                respuestaIdentityBool = respuestaIdentity.Succeeded;
            }
            var actividadesParaQuitar = await _UsuarioMenuSeccionActividadService.ObtenXIdUsuario(usuarioEmpresa.Id);
            var actividadParaQuitar = actividadesParaQuitar.Where(z => z.IdActividad == actividad.Id).ToList();
            if (actividadParaQuitar.Count() > 0)
            {
                var resultado = await _UsuarioMenuSeccionActividadService.DesactivarPermisoUsuario(actividadParaQuitar.FirstOrDefault()!.Id);
                if (resultado.Estatus == true && respuestaIdentityBool)
                {
                    return resultado;
                }
            }
            return resp;
        }
        #endregion

        #region ActivarPermisoUsuario
        /// <summary>
        /// Para activarle la vista a un usuario en especifico dentro de una empresa
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [HttpPost("asignarPermisoSeccionAUsuario")]
        public async Task<ActionResult<RespuestaDTO>> asignarPermisoSeccionAUsuario([FromBody] CambiaPermisoSeccion parametro)
        {
            RespuestaDTO resp = new RespuestaDTO();
            resp.Estatus = false;
            resp.Descripcion = "Algo salió mal";
            var usuarioEmpresa = await _UsuarioService.ObtenXIdUsuario(parametro.IdUsuario);
            if (usuarioEmpresa.Id <= 0)
            {
                return resp;
            }
            var seccion = await _CatalogoSeccionService.ObtenXId(parametro.IdSeccion);
            if (seccion.Id <= 0)
            {
                return resp;
            }
            var menu = await _CatalogoMenuService.ObtenXId(seccion.IdMenu);
            if (menu.Id <= 0 )
            {
                return resp;
            }
            var usuarioIdentity = await zvUserManager.FindByIdAsync(usuarioEmpresa.IdAspNetUser!);
            if (usuarioIdentity == null)
            {
                return resp;
            }
            var claimsUsuarioIdentity = await zvUserManager.GetClaimsAsync(usuarioIdentity!);
            string CodigoDistintivoSeccion = seccion.Descripcion + "-" + parametro.IdEmpresa;
            string CodigoValorSeccion = seccion.CodigoSeccion + "-" + parametro.IdEmpresa;
            var existeClaimSeccion = claimsUsuarioIdentity.Where(z => z.ValueType == CodigoDistintivoSeccion).ToList();
            bool respuestaIdentityBool = false;
            if (existeClaimSeccion.Count() <= 0)
            {
                var respuestaIdentity = await zvUserManager.AddClaimAsync(usuarioIdentity!, new Claim(CodigoDistintivoSeccion, CodigoValorSeccion));
                respuestaIdentityBool = respuestaIdentity.Succeeded;
            }
            var resultadoSeccionRelacionUsuario = await _UsuarioMenuSeccionService.ActivarPermisoUsuario(new UsuarioSeccionDTO()
            {
                IdSeccion = seccion.Id,
                IdUsuario = usuarioEmpresa.Id,
                EsActivo = true,
                IdEmpresa = parametro.IdEmpresa,
            });
            if (respuestaIdentityBool == true && resultadoSeccionRelacionUsuario.Estatus == true)
            {
                return resultadoSeccionRelacionUsuario;
            }
            return resp;
        }
        /// <summary>
        /// Para asignar un permiso de una actividad a un usuario dentro de una empresa
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [HttpPost("asignarPermisoActividadAUsuario")]
        public async Task<ActionResult<RespuestaDTO>> asignarPermisoActividadAUsuario([FromBody] CambiaPermisoActividad parametro)
        {
            RespuestaDTO resp = new RespuestaDTO();
            resp.Estatus = false;
            resp.Descripcion = "Algo salió mal";
            //Buscamos que la información proporcionada sea veridica
            var usuarioEmpresa = await _UsuarioService.ObtenXIdUsuario(parametro.IdUsuario);
            if (usuarioEmpresa.Id <= 0)
            {
                return resp;
            }
            var actividad = await _CatalogoActividadService.ObtenXId(parametro.IdActividad);
            if (actividad.Id <= 0)
            {
                return resp;
            }
            //var seccion = await _CatalogoSeccionService.ObtenXId(actividad.Id);
            //if (seccion.Id <= 0)
            //{
            //    return NoContent();
            //}
            //var menu = await _CatalogoMenuService.ObtenXId(seccion.IdMenu);
            //if (menu.Id <= 0 )
            //{
            //    return NoContent();
            //}
            var usuarioIdentity = await zvUserManager.FindByIdAsync(usuarioEmpresa.IdAspNetUser!);
            if (usuarioIdentity == null)
            {
                return resp;
            }
            //Buscamos los claims del usuario
            var claimsUsuarioIdentity = await zvUserManager.GetClaimsAsync(usuarioIdentity!);
            //string CodigoDistintivoSeccion = seccion.Descripcion + "-" + parametro.IdEmpresa;
            //string CodigoValorSeccion = seccion.CodigoSeccion + "-" + parametro.IdEmpresa;
            //var existeClaimSeccion = claimsUsuarioIdentity.Where(z => z.ValueType == CodigoDistintivoSeccion).ToList();
            //if (existeClaimSeccion.Count() <= 0)
            //{
            //    await zvUserManager.AddClaimAsync(usuarioIdentity!, new Claim(CodigoDistintivoSeccion, CodigoValorSeccion));
            //}
            string CodigoDistintivoActividad = actividad.Descripcion + "-" + parametro.IdEmpresa;
            string CodigoValorActividad = actividad.CodigoActividad + "-" + parametro.IdEmpresa;
            var existeClaimActividad = claimsUsuarioIdentity.Where(z => z.ValueType == CodigoDistintivoActividad).ToList();
            bool respuestaIdentityBool = false;
            if (existeClaimActividad.Count() <= 0)
            {
                var respuestaIdentity = await zvUserManager.AddClaimAsync(usuarioIdentity!, new Claim(CodigoDistintivoActividad, CodigoValorActividad));
                respuestaIdentityBool = respuestaIdentity.Succeeded;
            }
            //Vemos si hay una relacion con la seccion (porque tiene que ir amarrado)
            //RespuestaDTO resultadoSeccionRelacionUsuario = await _UsuarioMenuSeccionService.ActivarPermisoUsuario(new UsuarioSeccionDTO()
            //{
            //    IdSeccion = seccion.Id,
            //    IdUsuario = usuarioEmpresa.Id,
            //    EsActivo = true,
            //    IdEmpresa = parametro.IdEmpresa,
            //});
            //Por último checamos si existe la relación con la actividad
            RespuestaDTO resultadoActividadRelacionUsuario = await _UsuarioMenuSeccionActividadService.ActivarPermisoUsuario(new UsuarioActividadDTO()
            {
                IdActividad = actividad.Id,
                IdUsuario = usuarioEmpresa.Id,
                EsActivo = true,
                IdEmpresa = parametro.IdEmpresa
            });
            if (respuestaIdentityBool == true && resultadoActividadRelacionUsuario.Estatus == true)
            {
                return resultadoActividadRelacionUsuario;
            }
            return resp;
        }
        #endregion
    }
}
