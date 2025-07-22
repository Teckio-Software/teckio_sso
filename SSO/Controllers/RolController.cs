using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SistemaERP.DTO.Menu;
using SistemaERP.DTO;
using System.Security.Claims;
using AutoMapper;
using SistemaERP.BLL.Contrato;
using SistemaERP.BLL.Servicios.Contrato;
using SistemaERP.BLL.ServicioSSO;
using SistemaERP.BLL.ProcesoSSO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using SistemaERP.BLL.ContratoSSO;
using SistemaERP.BLL.SubProcesoSSO;
using SSO.DTO;

namespace SistemaERP.API.Controllers.SSO
{
    /// <summary>
    /// Para el rol por empresa
    /// </summary>
    [Route("api/rol")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RolController : ControllerBase
    {
        private readonly ICatalogoMenuService _CatalogoMenuService;
        private readonly ICatalogoSeccionService _CatalogoSeccionService;
        private readonly ICatalogoActividadService _CatalogoActividadService;
        private readonly IUsuarioSeccionService _UsuarioSeccionService;
        private readonly IUsuarioActividadService _UsuarioActividadService;
        private readonly IEmpresaService _EmpresaService;
        private readonly IRolService _RolService;
        private readonly IRolEmpresaService _RolEmpresaService;
        private readonly IRolSeccionService _RolSeccionService;
        private readonly IRolActividadService _RolActividadService;
        private readonly IUsuarioService _UsuarioService;
        private readonly IUsuarioEmpresaService _UsuarioEmpresaService;
        private readonly IMenuEmpresaService _MenuEmpresaService;
        private readonly ProcesoRol _ProcesoRol;
        private readonly ObtenRolesXCorporativo _RolXCorporativo;
        private readonly ObtenEmpresasEnRoles _EmpresasEnRoles;
        private readonly UserManager<IdentityUser> zvUserManager;
        private readonly RoleManager<IdentityRole> zvRoleManager;
        public RolController(
            UserManager<IdentityUser> zUserManager
            , RoleManager<IdentityRole> zRoleManager
            , IUsuarioSeccionService UsuarioSeccionService
            , IUsuarioActividadService UsuarioActividadService
            , ICatalogoMenuService CatalogoMenuService
            , IEmpresaService EmpresaService
            , IRolService RolService
            , IRolSeccionService RolSeccionService
            , IRolActividadService RolActividadService
            , IUsuarioService UsuarioService
            , ICatalogoSeccionService CatalogoSeccionService
            , ICatalogoActividadService CatalogoActividadService
            , IRolEmpresaService RolEmpresaService
            , IUsuarioEmpresaService UsuarioEmpresaService
            , IMenuEmpresaService MenuEmpresaService
            , ProcesoRol ProcesoRol
            , ObtenRolesXCorporativo RolXCorporativo
            , ObtenEmpresasEnRoles EmpresasEnRoles
            )
        {
            zvUserManager = zUserManager;
            zvRoleManager = zRoleManager;
            _CatalogoMenuService = CatalogoMenuService;
            _EmpresaService = EmpresaService;
            _RolService = RolService;
            _RolSeccionService = RolSeccionService;
            _RolActividadService = RolActividadService;
            _UsuarioService = UsuarioService;
            _UsuarioSeccionService = UsuarioSeccionService;
            _UsuarioActividadService = UsuarioActividadService;
            _CatalogoSeccionService = CatalogoSeccionService;
            _CatalogoActividadService = CatalogoActividadService;
            _RolEmpresaService = RolEmpresaService;
            _UsuarioEmpresaService = UsuarioEmpresaService;
            _MenuEmpresaService = MenuEmpresaService;
            _ProcesoRol = ProcesoRol;
            _RolXCorporativo = RolXCorporativo;
            _EmpresasEnRoles = EmpresasEnRoles;
        }

        #region CreaRol
        /// <summary>
        /// Para crear un rol en una sola empresa sin permisos
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [HttpPost("CrearRol")]
        [Authorize(Policy = "Administrador")]

        public async Task<ActionResult<RespuestaDTO>> crearRol([FromBody] RolCreacionUnaEmpresaDTO parametro)
        {
            var respuesta = new RespuestaDTO();
            var resultado = await _ProcesoRol.ProcesoCrearRolEnEmpresa(parametro);
            if (resultado.Id <= 0) {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se genero el rol";
                return respuesta;
            }
            respuesta.Estatus = true;
            respuesta.Descripcion = "Se genero el rol";
            return respuesta;
        }
        #endregion

        #region EditaRol
        /// <summary>
        /// Cambia el nombre del rol
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [HttpPost("editarNombreRol")]
        [Authorize(Policy = "Administrador")]

        public async Task<ActionResult<RespuestaDTO>> editarNombreRol([FromBody] RolCambiaNombre parametro)
        {
            RespuestaDTO resp = new RespuestaDTO();
            resp.Estatus = false;
            if (string.IsNullOrEmpty(parametro.DescripcionRol) || parametro.IdRol <= 0)
            {
                resp.Descripcion = "Capture todos los datos";
                return resp;
            }
            var rolNoIdentity = await _RolService.ObtenXId(parametro.IdRol);
            if (rolNoIdentity.Id <= 0 || string.IsNullOrEmpty(rolNoIdentity.IdAspNetRole))
            {
                resp.Descripcion = "Capture todos los datos";
                return resp;
            }
            var zvRole = await zvRoleManager.FindByIdAsync(rolNoIdentity.IdAspNetRole);
            if (zvRole == null)
            {
                resp.Descripcion = "Capture todos los datos";
                return resp;
            }
            var nombres = zvRole.Name.Split('-');
            if (nombres.Count() > 1)
            {
                zvRole!.Name = parametro.DescripcionRol + nombres[1];
            }
            else
            {
                zvRole!.Name = parametro.DescripcionRol;
            }
            //zvRole.NormalizedName = parametros.Name;
            var Resultado = await zvRoleManager.UpdateAsync(zvRole!);
            if (Resultado == IdentityResult.Success)
            {
                //busca los roles asociados al id del identity rol
                var rolesDTO = await _RolService.ObtenXIdAspNetRol(zvRole.Id);
                for (int i = 0; i < rolesDTO.Count; i++)
                {
                    //Actualizamos los nombres de los roles que mostramos al usuario
                    rolesDTO[i].Nombre = parametro.DescripcionRol;
                    await _RolService.Editar(rolesDTO[i]);
                }
                resp.Estatus = true;
                resp.Descripcion = "Todo ok";
                return resp;
            }
            resp.Descripcion = "Algo salió mal";
            return resp;
        }
        #endregion

        #region QuitarPermisosARol
        /// <summary>
        /// Para quitar los permisos de una seccion a un rol
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [HttpPost("quitarPermisoSeccionARol")]
        [Authorize(Policy = "Administrador")]

        public async Task<ActionResult> quitarPermisoSeccionARol([FromBody] RolMenuEstructuraDTO parametro)
        {
            if (parametro.IdRol <= 0
                || parametro.IdSeccion <= 0)
            {
                return BadRequest("Capture todos los datos");
            }
            var rolNoIdentity = await _RolService.ObtenXId(parametro.IdRol);
            if (rolNoIdentity.Id <= 0)
            {
                return BadRequest("Capture todos los datos");
            }
            var rolEmpresa = await _RolEmpresaService.ObtenXIdRol(rolNoIdentity.Id);
            if (rolEmpresa.Count() <= 0)
            {
                return NoContent();
            }
            var empresa = await _EmpresaService.ObtenXId(rolEmpresa[0].IdEmpresa);
            if (empresa.Id <= 0)
            {
                return NoContent();
            }
            var zvRole = await zvRoleManager.FindByIdAsync(rolNoIdentity.IdAspNetRole);
            if (zvRole == null)
            {
                return BadRequest();
            }
            var seccion = await _CatalogoSeccionService.ObtenXId(parametro.IdSeccion);
            if (seccion.Id <= 0)
            {
                return BadRequest();
            }
            //Vemos si hay relación del menu con la sección y el rol
            var rolSeccion = await _RolSeccionService.ObtenTodosXIdSeccion(seccion.Id);
            if (rolSeccion.Count() <= 0)
            {
                return Ok();
            }
            var respuesta1 = await _RolSeccionService.Eliminar(rolSeccion[0].Id);
            var claims = await zvRoleManager.GetClaimsAsync(zvRole);
            var claimEliminar = seccion.DescripcionInterna + "-" + empresa.Id;
            var claimMenuEmpresa = claims.Where(z => z.Type == claimEliminar).ToList();
            IdentityResult resultado2 = new IdentityResult();
            if (claimMenuEmpresa.Count() > 0)
            {
                resultado2 = await zvRoleManager.RemoveClaimAsync(zvRole, claimMenuEmpresa[0]);
            }
            if (respuesta1.Estatus == false && !resultado2.Succeeded)
            {
                return BadRequest();
            }

            var usuariosNoIdentity = await _UsuarioEmpresaService.ObtenXIdRol(rolNoIdentity.Id);
            foreach (var usuarioNoIdentity in usuariosNoIdentity)
            {
                var usuario = await _UsuarioService.ObtenXIdUsuario(usuarioNoIdentity.IdUsuario);
                var usuarioIdentity = await zvUserManager.FindByIdAsync(usuario.IdAspNetUser!);
                var claimsUsuario = await zvUserManager.GetClaimsAsync(usuarioIdentity!);
                //Obtenemos la sección
                var seccionPorUsuario = await _UsuarioSeccionService.ObtenXIdUsuario(usuario.Id);
                seccionPorUsuario = seccionPorUsuario.Where(z => z.IdEmpresa == usuarioNoIdentity.IdEmpresa).ToList();
                var seccionesPorUsuarioFiltrado = seccionPorUsuario.Where(z => z.IdSeccion == parametro.IdSeccion).ToList();
                if (seccionesPorUsuarioFiltrado.Count() > 0)
                {
                    var resultadoSeccion = await _UsuarioSeccionService.DesactivarPermisoUsuario(seccionesPorUsuarioFiltrado[0].Id);
                }
                var claimAEliminar = claimsUsuario.Where(z => z.Type == claimEliminar).ToList();
                if (claimAEliminar.Count() > 0)
                {
                    var resultadoClaimRemove = await zvUserManager.RemoveClaimAsync(usuarioIdentity!, claimAEliminar[0]);
                }
                
            }
            return Ok();
        }
        /// <summary>
        /// Para quitar los permisos de una actividad a un rol
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [HttpPost("quitarPermisoActividadARol")]
        [Authorize(Policy = "Administrador")]

        public async Task<ActionResult> quitarPermisoActividadARol([FromBody] RolMenuEstructuraDTO parametro)
        {
            if (parametro.IdRol <= 0 
                || parametro.IdActividad <= 0)
            {
                return BadRequest("Capture todos los datos");
            }
            //Obtenemos el rol por empresa (no identity)
            var rolNoIdentity = await _RolService.ObtenXId(parametro.IdRol);
            if (rolNoIdentity.Id <= 0)
            {
                return BadRequest("Capture todos los datos");
            }
            //Obtenemos el rol del login (identity)
            var zvRole = await zvRoleManager.FindByIdAsync(rolNoIdentity.IdAspNetRole);
            if (zvRole == null)
            {
                return BadRequest();
            }
            //Obtenemos la actividad para ocupar su nombre y su codigo
            var actividad = await _CatalogoActividadService.ObtenXId(parametro.IdActividad);
            if (actividad.Id <= 0)
            {
                return BadRequest();
            }
            var rolEmpresa = await _RolEmpresaService.ObtenXIdRol(rolNoIdentity.Id);
            if (rolEmpresa.Count() <= 0)
            {
                return NoContent();
            }
            var empresa = await _EmpresaService.ObtenXId(rolEmpresa[0].IdEmpresa);
            if (empresa.Id <= 0)
            {
                return NoContent();
            }
            var rolActividad = await _RolActividadService.ObtenTodosXIdRolXIdActividad(rolNoIdentity.Id, actividad.Id);
            if (rolActividad.Count() <= 0)
            {
                return Ok();
            }
            //Eliminamos la actividad de la relación con los roles
            var respuesta1 = await _RolActividadService.Eliminar(rolActividad[0].Id);
            var claims = await zvRoleManager.GetClaimsAsync(zvRole);
            var claimEliminar = claims.Where(z => z.Type == actividad.DescripcionInterna + "-" + rolEmpresa[0].IdEmpresa).ToList();
            IdentityResult resultado2 = new IdentityResult();
            if (claimEliminar.Count() > 0)
            {
                resultado2 = await zvRoleManager.RemoveClaimAsync(zvRole, claimEliminar.FirstOrDefault()!);
            }
            if (respuesta1.Estatus == false && !resultado2.Succeeded)
            {
                return BadRequest();
            }
            //Ahora vamos con los usuarios
            //Buscamos los usuarios por empresa
            var usuariosFiltrados = await _UsuarioEmpresaService.ObtenXIdRol(rolNoIdentity.Id);
            //Buscamos los usuarios que tienen esa actividad
            var usuariosActividadesSinFiltrar = await _UsuarioActividadService.ObtenXIdActividad(actividad.Id);
            foreach (var usuarioFiltrado in usuariosFiltrados)
            {
                var usuario = await _UsuarioService.ObtenXIdUsuario(usuarioFiltrado.IdUsuario);
                //Por cada usuario buscamos 
                var usuarioIdentity = await zvUserManager.FindByIdAsync(usuario.IdAspNetUser!);
                var claimsUsuarios = await zvUserManager.GetClaimsAsync(usuarioIdentity!);
                var claimFiltrado = claimsUsuarios.Where(z => z.Type == actividad.DescripcionInterna + "-" + rolEmpresa[0].IdEmpresa).ToList();
                if (claimFiltrado.Count() > 0)
                {
                    var resultado3 = await zvUserManager.RemoveClaimAsync(usuarioIdentity!, claimFiltrado.FirstOrDefault()!);
                }
                var usuariosActividadesFiltrado = usuariosActividadesSinFiltrar.Where(z => z.IdUsuario == usuarioFiltrado.IdUsuario).ToList();
                if (usuariosActividadesFiltrado.Count() > 0)
                {
                    var resultado = await _UsuarioActividadService.DesactivarPermisoUsuario(usuariosActividadesFiltrado[0].Id);
                }
            }
            return Ok();
        }

        #endregion
        /// <summary>
        /// Obtiene los roles dentro de una empresa
        /// </summary>
        /// <param name="IdEmpresa"></param>
        /// <returns></returns>
        [HttpGet("obtenRolesXEmpresa/{IdEmpresa:int}")]
        [Authorize(Policy = "AdministradorYAdminRoles")]

        public async Task<ActionResult<List<RolDTO>>> obtenRolesXEmpresa(int IdEmpresa)
        {
            List<RolDTO> roles = new List<RolDTO>();
            var rolesEmpresas = await _RolEmpresaService.ObtenXIdEmpresa(IdEmpresa);
            foreach (var rolEmpresa in rolesEmpresas)
            {
                var rol = await _RolService.ObtenXId(rolEmpresa.IdRol);
                roles.Add(rol);
            }
            return roles;
        }
        /// <summary>
        /// Obtiene los menus/servicios dentro de una empresa
        /// </summary>
        /// <param name="IdEmpresa"></param>
        /// <returns></returns>
        [HttpGet("ObtenMenusXIdEmpresa/{IdEmpresa:int}")]
        [Authorize(Policy = "Administrador")]

        public async Task<ActionResult<List<CatalogoMenuDTO>>> ObtenMenusXRol(int IdEmpresa)
        {
            List<CatalogoMenuDTO> menusReturn = new List<CatalogoMenuDTO>();
            var menusEmpresas = await _MenuEmpresaService.ObtenXIdEmpresa(IdEmpresa);
            var menus = await _CatalogoMenuService.ObtenTodos();
            foreach (var menuEmpresa in menusEmpresas)
            {
                var menuFiltrado = menus.Where(z => z.Id == menuEmpresa.IdMenu).ToList();
                if (menuFiltrado.Count() > 0)
                {
                    menusReturn.Add(menuFiltrado[0]);
                }
            }
            return menusReturn;
        }
        /// <summary>
        /// Obtiene las secciones activas por rol y empresa
        /// </summary>
        /// <param name="IdRol"></param>
        /// <param name="IdMenu"></param>
        /// <returns></returns>
        [HttpGet("rolSeccionEstructura/{IdRol:int}/{IdMenu:int}")]
        [Authorize(Policy = "Administrador")]

        public async Task<ActionResult<List<RolMenuEstructuraDTO>>> rolSeccionEstructura(int IdRol, int IdMenu)
        {
            List<RolMenuEstructuraDTO> estructura = new List<RolMenuEstructuraDTO>();
            var roles = await _RolService.ObtenXId(IdRol);
            var secciones = await _CatalogoSeccionService.ObtenTodosXIdMenu(IdMenu);
            var rolSecciones = await _RolSeccionService.ObtenTodosXIdRol(IdRol);
            foreach (var seccion in secciones)
            {
                var activoSeccion = rolSecciones.Where(z => z.IdSeccion == seccion.Id).ToList();
                estructura.Add(new RolMenuEstructuraDTO()
                {
                    IdRol = IdRol,
                    IdMenu = IdMenu,
                    IdSeccion = seccion.Id,
                    IdActividad = 0,
                    TipoMenu = 3,
                    Descripcion = seccion.Descripcion!,
                    EsActivo = activoSeccion.Count() > 0 ? true : false
                });
            }
            return estructura;
        }
        /// <summary>
        /// Obtiene las actividades activas por rol y empresa
        /// </summary>
        /// <param name="IdRol"></param>
        /// <param name="IdSeccion"></param>
        /// <returns></returns>
        [HttpGet("rolActividadEstructura/{IdRol:int}/{IdSeccion:int}")]
        [Authorize(Policy = "Administrador")]

        public async Task<ActionResult<List<RolMenuEstructuraDTO>>> rolActividadEstructura(int IdRol, int IdSeccion)
        {
            List<RolMenuEstructuraDTO> estructura = new List<RolMenuEstructuraDTO>();
            var roles = await _RolService.ObtenXId(IdRol);
            var actividades = await _CatalogoActividadService.ObtenTodosXIdSeccion(IdSeccion);
            var rolActividades = await _RolActividadService.ObtenTodosXIdRol(IdRol);
            foreach (var Actividad in actividades)
            {
                var activoActividad = rolActividades.Where(z => z.IdActividad == Actividad.Id).ToList();
                estructura.Add(new RolMenuEstructuraDTO()
                {
                    IdRol = IdRol,
                    IdMenu = 0,
                    IdSeccion = Actividad.IdSeccion,
                    IdActividad = Actividad.Id,
                    TipoMenu = 3,
                    Descripcion = Actividad.Descripcion!,
                    EsActivo = activoActividad.Count() > 0 ? true : false
                });
            }
            return estructura;
        }

        #region AutorizaSeccionARol
        /// <summary>
        /// Le da permiso de una seccion a un rol dentro de una empresa junto con los usuarios asociados a ese rol
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [HttpPost("autorizaSeccionARol")]
        [Authorize(Policy = "Administrador")]

        public async Task<ActionResult> autorizaSeccionARol([FromBody] RolSeccionDTO parametro)
        {
            await _ProcesoRol.autorizaSeccionARol(parametro);
            return NoContent();
        }
        /// <summary>
        /// Le da permiso de una actividad a un rol dentro de una empresa junto con los usuarios asociados a ese rol
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [HttpPost("autorizaActividadARol")]
        [Authorize(Policy = "Administrador")]

        public async Task<ActionResult> autorizaActividadARol([FromBody] RolActividadDTO parametro)
        {
            var actividad = await _CatalogoActividadService.ObtenXId(parametro.IdActividad);
            if (actividad.Id <= 0)
            {
                return NoContent();
            }
            var rolNoIdentity = await _RolService.ObtenXId(parametro.IdRol);
            if (rolNoIdentity.Id <= 0)
            {
                return NoContent();
            }
            var rolIdentity = await zvRoleManager.FindByIdAsync(rolNoIdentity.IdAspNetRole);
            if (rolIdentity == null)
            {
                return NoContent();
            }
            var rolEmpresa = await _RolEmpresaService.ObtenXIdRol(rolNoIdentity.Id);
            if (rolEmpresa.Count() <= 0)
            {
                return NoContent();
            }
            var empresa = await _EmpresaService.ObtenXId(rolEmpresa[0].IdEmpresa);
            if (empresa.Id <= 0)
            {
                return NoContent();
            }
            var claims = await zvRoleManager.GetClaimsAsync(rolIdentity);
            Claim claimAgregar;
            if (actividad.EsActividadUnica)
            {
                claimAgregar = new Claim(actividad.DescripcionInterna, actividad.CodigoActividad);
            }
            else
            {
                claimAgregar = new Claim(actividad.DescripcionInterna + "-" + empresa.Id, actividad.CodigoActividad + "-" + empresa.GuidEmpresa);
            }
            var claimActividadEmpresa = claims.Where(z => z.Type == claimAgregar.Type).ToList();
            if (claimActividadEmpresa.Count() <= 0)
            {
                var resultadoAddClaim = await zvRoleManager.AddClaimAsync(rolIdentity, claimAgregar);
            }
            var existeRolMenuSeccionActividad = await _RolActividadService.ObtenTodosXIdRolXIdActividad(rolNoIdentity.Id, actividad.Id);
            if (existeRolMenuSeccionActividad.Count() <= 0)
            {
                var resultado = await _RolActividadService.CrearYObtener(new RolActividadDTO()
                {
                    IdRol = rolNoIdentity.Id,
                    IdActividad = actividad.Id
                });
            }
            //Como uno o varios usuarios estan ligados a un rol se le agregan a los usuarios el nuevo permiso
            var usuariosNoIdentity = await _UsuarioEmpresaService.ObtenXIdRol(rolNoIdentity.Id);
            foreach (var usuarioNoIdentity in usuariosNoIdentity)
            {
                var usuario = await _UsuarioService.ObtenXIdUsuario(usuarioNoIdentity.IdUsuario);
                var usuarioIdentity = await zvUserManager.FindByIdAsync(usuario.IdAspNetUser!);
                if (usuarioIdentity == null)
                {
                    continue;
                }
                var claimsUsuario = await zvUserManager.GetClaimsAsync(usuarioIdentity!);
                var claimsUsuarioFiltradoActividad = claimsUsuario.Where(z => z.Type == actividad.DescripcionInterna + "-" + empresa.Id).ToList();
                if (claimsUsuarioFiltradoActividad.Count() <= 0)
                {
                    var resultadoAddClaim = await zvUserManager.AddClaimAsync(usuarioIdentity, new Claim(actividad.DescripcionInterna + "-" + empresa.Id, actividad.CodigoActividad + "-" + empresa.GuidEmpresa));
                }
                var usuarioActividad = await _UsuarioActividadService.ActivarPermisoUsuario(new UsuarioActividadDTO()
                {
                    IdUsuario = usuarioNoIdentity.IdUsuario,
                    EsActivo = true,
                    IdActividad = actividad.Id,
                    IdEmpresa = empresa.Id,
                });
            }

            return NoContent();
        }
        #endregion
    }
}
