using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SistemaERP.BLL.Contrato;
using SistemaERP.BLL.ProcesoSSO;
using SistemaERP.BLL.Servicios.Contrato;
using SistemaERP.BLL.ServicioSSO;
using SistemaERP.DTO;
using SistemaERP.DTO.Menu;
using SistemaERP.DTO.SSO;
using SistemaERP.Model.Gastos;
using SSO.DTO;
using SSO.Servicios.Contratos;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace SistemaERP.API.Controllers.SSO
{
    /// <summary>
    /// En este controlador esta todo lo relacionado a los usuarios
    /// </summary>
    [Route("api/usuario")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Administrador")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioSeccionService _UsuarioMenuSeccionService;
        private readonly IUsuarioPertenecienteAClienteService _UsuarioPertenecienteClienteService;
        private readonly IUsuarioActividadService _UsuarioMenuSeccionActividadService;
        private readonly IRolService _RolService;
        private readonly IRolEmpresaService _RolEmpresaService;
        private readonly ICatalogoMenuService _CatalogoMenuService;
        private readonly ICatalogoSeccionService _CatalogoSeccionService;
        private readonly ICatalogoActividadService _CatalogoActividadService;
        private readonly IUsuarioService _UsuarioService;
        private readonly IEmpresaService _EmpresaService;
        private readonly IUsuarioEmpresaService _UsuarioEmpresaService;
        private readonly IUsuarioProveedorService _UsuarioProveedorService;
        private readonly IUsuarioGastosService _UsuarioGastosService;
        private readonly ProcesoUsuarioCreacion _ProcesoUsuarioCreacion;
        private readonly UserManager<IdentityUser> _UserManager;
        private readonly IConfiguration _Configuration;
        private readonly UserManager<IdentityUser> zvUserManager;
        private readonly RoleManager<IdentityRole> zvRoleManager;
        private readonly IMapper _Mapper;
        private readonly SignInManager<IdentityUser> zvSignInManager;
        private readonly IRolProyectoEmpresaUsuarioService _RolProyectoEmpresaUsuarioService;
        public UsuarioController(
            UserManager<IdentityUser> zUserManager
            , RoleManager<IdentityRole> zRoleManager
            , IMapper zMapper
            , IUsuarioSeccionService UsuarioMenuSeccionService
            , IUsuarioActividadService UsuarioMenuSeccionActividadService
            , IRolService RolService
            , IRolEmpresaService RolEmpresaService
            , ICatalogoMenuService CatalogoMenuService
            , IUsuarioService UsuarioService
            , IEmpresaService EmpresaService
            , IUsuarioEmpresaService UsuarioEmpresaService
            , IUsuarioProveedorService UsuarioProveedorService
            , IUsuarioGastosService UsuarioGastosService
            , ProcesoUsuarioCreacion ProcesoUsuarioCreacion
            , ICatalogoSeccionService CatalogoSeccionService
            , ICatalogoActividadService CatalogoActividadService
            , UserManager<IdentityUser> UserManager
            , IConfiguration Configuration
            , SignInManager<IdentityUser> zSignInManager
            , IUsuarioPertenecienteAClienteService usuarioPertenecienteAClienteService
            , IRolProyectoEmpresaUsuarioService rolProyectoEmpresaUsuarioService
            )
        {
            zvUserManager = zUserManager;
            zvRoleManager = zRoleManager;
            _Mapper = zMapper;
            _RolService = RolService;
            _RolEmpresaService = RolEmpresaService;
            _CatalogoMenuService = CatalogoMenuService;
            _UsuarioService = UsuarioService;
            _UsuarioMenuSeccionService = UsuarioMenuSeccionService;
            _UsuarioMenuSeccionActividadService = UsuarioMenuSeccionActividadService;
            _EmpresaService = EmpresaService;
            _UsuarioEmpresaService = UsuarioEmpresaService;
            _UsuarioProveedorService = UsuarioProveedorService;
            _UsuarioGastosService = UsuarioGastosService;
            _ProcesoUsuarioCreacion = ProcesoUsuarioCreacion;
            _CatalogoSeccionService = CatalogoSeccionService;
            _CatalogoActividadService = CatalogoActividadService;
            _UserManager = UserManager;
            _Configuration = Configuration;
            _UsuarioPertenecienteClienteService = usuarioPertenecienteAClienteService;
            zvSignInManager = zSignInManager;
            _RolProyectoEmpresaUsuarioService = rolProyectoEmpresaUsuarioService;

        }


        #region EditarDatosUsuario
        /// <summary>
        /// Obtiene el tipo de usuario si es de gastos, proveedores o administrativo
        /// </summary>
        /// <param name="IdUsuario"></param>
        /// <returns></returns>
        [HttpGet("obtenTipoUsuario/{IdUsuario:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "VisorPermiso")]
        public async Task<ActionResult<RespuestaDTO>> obtenUsuario(int IdUsuario)
        {
            RespuestaDTO respuestaDTO = new RespuestaDTO();
            respuestaDTO.Estatus = true;
            var usuario = await _UsuarioService.ObtenXIdUsuario(IdUsuario);
            var esUsuarioProveedor = await _UsuarioProveedorService.ObtenXIdUsuario(usuario.Id);
            if (esUsuarioProveedor.Count() > 0)
            {
                respuestaDTO.Descripcion = "proveedor";
                return respuestaDTO;
            }
            var esUsuarioGastos = await _UsuarioGastosService.ObtenXIdUsuario(usuario.Id);
            if (esUsuarioGastos.id > 0)
            {
                respuestaDTO.Descripcion = "gastos";
                return respuestaDTO;
            }
            respuestaDTO.Descripcion = "normal";
            return respuestaDTO;
        }
        /// <summary>
        /// Obtiene los datos de un usuario
        /// </summary>
        /// <param name="IdUsuario"></param>
        /// <returns></returns>
        [HttpGet("obtenDatosUsuario/{IdUsuario:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "VisorPermiso")]
        public async Task<ActionResult<UsuarioDTO>> obtenDatosUsuario(int IdUsuario)
        {
            var usuario = await _UsuarioService.ObtenXIdUsuario(IdUsuario);
            return usuario;
        }
        #endregion
        
        /// <summary>
        /// Para cambiarle la contraseña a un usuario con el permiso de administrador
        /// </summary>
        /// <param name="zContrasenias">Un objeto del tipo <see cref="ReestablecerContraseniaDTO"/></param>
        /// <returns>Un objeto del tipo <see cref="RespuestaAutenticacionDTO"/></returns>
        [HttpPost("actualizaInfoUsuario")]
        public async Task<ActionResult<RespuestaDTO>> actualizaInfoUsuario([FromBody] UsuarioDTO parametro)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            if (
                parametro.Id <= 0
                || string.IsNullOrEmpty(parametro.Correo)
                || string.IsNullOrEmpty(parametro.NombreCompleto)
                || string.IsNullOrEmpty(parametro.Apaterno)
                || string.IsNullOrEmpty(parametro.Amaterno)
                || string.IsNullOrEmpty(parametro.NombreUsuario)
                )
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "Capture toda la información";
                return respuesta;
            }
            var respuesta2 = Regex.IsMatch(parametro.Correo,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            if (!respuesta2)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "Capture un correo electrónico válido";
                return respuesta;
            }
            var resultado = await _UsuarioService.Editar(parametro);
            var usuario = await _UsuarioService.ObtenXIdUsuario(parametro.Id);
            var usuarioIdentity = await zvUserManager.FindByIdAsync(usuario.IdAspNetUser);
            usuarioIdentity.NormalizedEmail = parametro.Correo;
            usuarioIdentity.Email = parametro.Correo;
            usuarioIdentity.UserName = parametro.NombreUsuario;
            usuarioIdentity.NormalizedUserName = parametro.NombreUsuario;
            await zvUserManager.UpdateAsync(usuarioIdentity);
            respuesta.Estatus = true;
            respuesta.Descripcion = "Usuario actualizado";
            return respuesta;
        }
        /// <summary>
        /// Para crear un usuario normal o usuario administrativo
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [HttpPost("creaUsuarioBase")]
        public async Task<ActionResult<RespuestaDTO>> creaUsuarioBase([FromBody] UsuarioCreacionBaseDTO2 parametro)
        {
            var user = HttpContext.User;
            var claims = user.Claims.ToList();
            var username = claims.Where(z => z.Type == "username").ToList();
            RespuestaDTO respuesta = new RespuestaDTO();
            
            if (
                string.IsNullOrEmpty(parametro.Correo)
                || string.IsNullOrEmpty(parametro.NombreCompleto)
                || string.IsNullOrEmpty(parametro.APaterno)
                || string.IsNullOrEmpty(parametro.AMaterno)
                || string.IsNullOrEmpty(parametro.NombreUsuario)
                )
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "Capture toda la información";
                return respuesta;
            }
            var respuesta2 = Regex.IsMatch(parametro.Correo,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            if (!respuesta2)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "Capture un correo electrónico válido";
                return respuesta;
            }
            respuesta.Estatus = await _ProcesoUsuarioCreacion.CreaUsuarioNormal(parametro, claims);
            respuesta.Descripcion = "Usuario creado";
            return respuesta;
        }
        /// <summary>
        /// Para activar un usuario dentro de la aplicación
        /// </summary>
        /// <param name="IdUsuario"></param>
        /// <returns></returns>
        [HttpPost("activaUsuario/{IdUsuario:int}")]
        public async Task<ActionResult<RespuestaDTO>> ActivaUsuario(int IdUsuario)
        {
            var user = HttpContext.User;
            var claims = user.Claims.ToList();
            var username = claims.Where(z => z.Type == "username").ToList();
            RespuestaDTO respuesta = new RespuestaDTO();
            respuesta = await _UsuarioService.Activar(IdUsuario);
            //respuesta.Descripcion = "Usuario creado";
            return respuesta;
        }
        [HttpPost("desactivaUsuario/{IdUsuario:int}")]
        public async Task<ActionResult<RespuestaDTO>> DesactivaUsuario(int IdUsuario)
        {
            var user = HttpContext.User;
            var claims = user.Claims.ToList();
            var username = claims.Where(z => z.Type == "username").ToList();
            RespuestaDTO respuesta = new RespuestaDTO();
            respuesta = await _UsuarioService.Desactivar(IdUsuario);
            //respuesta.Descripcion = "Usuario creado";
            return respuesta;
        }

        #region editarContraseñaUsuario
        /// <summary>
        /// Para cambiarle la contraseña a un usuario con el permiso de administrador
        /// </summary>
        /// <param name="zContrasenias">Un objeto del tipo <see cref="ReestablecerContraseniaDTO"/></param>
        /// <returns>Un objeto del tipo <see cref="RespuestaAutenticacionDTO"/></returns>
        [HttpPost("reestableceContrasenia")]
        public async Task<ActionResult<RespuestaDTO>> zfReestablecerContrasenia([FromBody] ReestablecerContraseniaDTO zContrasenias)
        {
            var user = HttpContext.User;
            var claims = user.Claims.ToList();
            var role = HttpContext.User.IsInRole("Administrador");
            var esVisor = claims.Where(z => z.Type == "VisorCorporativo").Where(z => z.Value == "Codigo-J05tg8!").ToList();
            var esRfacil = claims.Where(z => z.Type == "role").Where(z => z.Value == "Administrador").ToList();
            RespuestaDTO resp = new RespuestaDTO();
            resp.Estatus = false;
            if (esVisor.Count() <= 0 && !role)
            {
                resp.Descripcion = "No cuenta con permisos";
                return resp;
            }
            if (string.IsNullOrEmpty(zContrasenias.idUsuario)
                || string.IsNullOrEmpty(zContrasenias.nuevaContrasenia)
                || string.IsNullOrEmpty(zContrasenias.nuevaContraseniaConfirma))
            {
                resp.Descripcion = "Capture todos los campos";
                return resp;
            }
            if (zContrasenias.nuevaContrasenia != zContrasenias.nuevaContraseniaConfirma)
            {
                resp.Descripcion = "Las contraseñas no coinciden";
                return resp;
            }
            var zvUsuario = await zvUserManager.FindByIdAsync(zContrasenias.idUsuario);
            if (zvUsuario == null)
            {
                resp.Descripcion = "Algo salió mal, consulte a soporte";
                return resp;
            }
            await zvUserManager.RemovePasswordAsync(zvUsuario);
            var zvResultado = await zvUserManager.AddPasswordAsync(zvUsuario, zContrasenias.nuevaContrasenia);

            if (zvResultado.Succeeded)
            {
                resp.Estatus = true;
                resp.Descripcion = "Contraseña actualizada para ese usuario";
                return resp;
            }
            else
            {
                resp.Descripcion = "Algo salió mal, consulte a soporte";
                return resp;
            }
        }
        /// <summary>
        /// Para cambiar la contraseña de un usuario (no hay forma de recuperar una contraseña)
        /// </summary>
        /// <param name="zCredenciales">Un objeto del tipo <see cref="CredencialesUsuarioDTO"/></param>
        /// <returns></returns>
        [HttpPost("cambiarContraseniaUsuario")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "VisorPermiso")]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> zfCambiarContraseniaUsuario([FromBody] CredencialesUsuarioDTO zCredenciales)
        {
            var zvUsuario = await zvUserManager.FindByEmailAsync(zCredenciales.Email);
            if (zvUsuario == null)
            {
                return NoContent();
            }
            await zvUserManager.ChangePasswordAsync(zvUsuario, zvUsuario.PasswordHash!, zCredenciales.Password);
            return NoContent();
        }
        #endregion

        #region CambiarRolUsuario
        /// <summary>
        /// Para cambiar el rol de un usuario por empresa
        /// </summary>
        /// <param name="parametro">Un objeto del tipo <see cref="CambiarRolAUsuarioEnEmpresaDTO"/></param>
        /// <returns>Un objeto de <seealso cref="NoContentResult"/></returns>
        [HttpPost("cambiarPermisosUsuarioXPerfil")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "VisorPermiso")]
        public async Task<ActionResult<RespuestaDTO>> AsignarQuitarClaimsAUsuario([FromBody] CambiarRolAUsuarioEnEmpresaDTO parametro)
        {
            var authen = HttpContext.User;

            var usernameClaim = authen.Claims.FirstOrDefault()!.Value;
            var token = await zfConstruirToken(usernameClaim);
            RespuestaDTO respuesta = new RespuestaDTO();
            var usuariosNoIdentity = await _UsuarioService.ObtenXIdUsuario(parametro.IdUsuario);
            if (usuariosNoIdentity.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No existe el usuario con ese nombre de usuario";
                return respuesta;
            }
            var zvUsuario = await zvUserManager.FindByIdAsync(usuariosNoIdentity.IdAspNetUser);
            if (zvUsuario == null)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No existe el usuario con ese nombre de usuario";
                return respuesta;
            }
            var rolNoIdentity = await _RolService.ObtenXId(parametro.IdRol);
            if (rolNoIdentity.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No existe el rol";
                return respuesta;
            }
            var rolIdentity = await zvRoleManager.FindByIdAsync(rolNoIdentity.IdAspNetRole);
            if (rolIdentity == null)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No existe el rol";
                return respuesta;
            }
            //Obtenemos los roles que mostramos al usuario
            var rolesEnEmpresa = await _RolEmpresaService.ObtenXIdRol(parametro.IdRol);
            if (rolesEnEmpresa.Count <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No existe el rol";
                return respuesta;
            }
            var empresa = await _EmpresaService.ObtenXId(parametro.IdEmpresa);
            if (empresa.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No existe la empresa";
                return respuesta;
            }
            var usuarioEmpresa = await _UsuarioEmpresaService.ObtenXIdUsuarioXIdEmpresa(usuariosNoIdentity.Id, empresa.Id);

            if (usuarioEmpresa.Count() <= 0)
            {
                //Lo agregamos al rol nuevo
                await zvUserManager.AddToRoleAsync(zvUsuario, rolIdentity.Name!);
                //Obtenemos los permisos del rol nuevo
                var rolClaimsNuevosPrimerRol = await zvRoleManager.GetClaimsAsync(rolIdentity);
                //Agregamos los nuevos claims del nuevo usuario
                var resultado3ClaimsNuevosPrimerRol = await zvUserManager.AddClaimsAsync(zvUsuario, rolClaimsNuevosPrimerRol);
                var resultado = await _UsuarioEmpresaService.Activar(new UsuarioEmpresaDTO()
                {
                    IdUsuario = usuariosNoIdentity.Id,
                    IdEmpresa = empresa.Id,
                    Activo = true,
                    IdRol = rolNoIdentity.Id,
                });
                respuesta.Estatus = true;
                respuesta.Descripcion = "Rol cambiado exitosamente";
                return respuesta;
            }
            else
            {
                //Tenemos el Id del rol
                var rolViejoNoIdentity = await _RolService.ObtenXId(usuarioEmpresa[0].IdRol);
                var rolViejoIdentity = await zvRoleManager.FindByIdAsync(rolViejoNoIdentity.IdAspNetRole);
                if (rolViejoIdentity != null)
                {
                    //Obtenemos los claims del rol (identity) por la empresa
                    var claimsRolViejoIdentity = await zvRoleManager.GetClaimsAsync(rolViejoIdentity!);
                    //Le quitamos los claims (permisos al usuario)
                    var resultado1Usuario = await zvUserManager.RemoveClaimsAsync(zvUsuario, claimsRolViejoIdentity);
                    var resultado2RolRemovido = await zvUserManager.RemoveFromRoleAsync(zvUsuario, rolViejoIdentity!.Name!);
                }
                //Lo agregamos al rol nuevo
                await zvUserManager.AddToRoleAsync(zvUsuario, rolIdentity.Name!);
                //Obtenemos los permisos del rol nuevo
                var rolClaimsNuevos = await zvRoleManager.GetClaimsAsync(rolIdentity);
                //Agregamos los nuevos claims del nuevo usuario
                var resultado3ClaimsNuevos = await zvUserManager.AddClaimsAsync(zvUsuario, rolClaimsNuevos);
                respuesta.Estatus = await _UsuarioEmpresaService.CambiarRol(new UsuarioEmpresaDTO()
                {
                    Id = usuarioEmpresa[0].Id,
                    IdUsuario = usuariosNoIdentity.Id,
                    IdEmpresa = usuarioEmpresa[0].IdEmpresa,
                    IdRol = rolNoIdentity.Id
                });
                if (respuesta.Estatus)
                {
                    respuesta.Descripcion = "Rol cambiado exitosamente";
                    return respuesta;
                }
                respuesta.Descripcion = "algo salio mal en el cambio del rol";
                return respuesta;
            }
        }
        #endregion

        #region ConsultaUsuarios
        /// <summary>
        /// Obtiene la relación de los usuarios con las empresas dentro de un corporativo
        /// </summary>
        /// <param name="IdCorporativo"></param>
        /// <returns></returns>
        [HttpGet("usuariosEstructuraPorRolXEmpresa/{IdCorporativo:int}")]
        public async Task<ActionResult<List<UsuarioEstructuraCorporativoDTO>>> ObtenUsuariosCompletoXIdEmpresa(int IdCorporativo)
        {
            List<UsuarioEstructuraCorporativoDTO> usuarioEstructura = new List<UsuarioEstructuraCorporativoDTO>();
            var usuarios = await _UsuarioService.ObtenTodos();
            //var usuariosDeCliente = await 
            var empresas = await _EmpresaService.ObtenXIdCorporativo(IdCorporativo);
            foreach (var usuario in usuarios)
            {
                List<UsuarioEmpresaEstructura> empresaslista = new List<UsuarioEmpresaEstructura>();
                foreach (var empresa in empresas)
                {
                    var usuarioEmpresa = await _UsuarioEmpresaService.ObtenXIdUsuarioXIdEmpresa(usuario.Id, empresa.Id);
                    empresaslista.Add(new UsuarioEmpresaEstructura()
                    {
                        IdEmpresa = empresa.Id,
                        NombreEmpresa = empresa.NombreComercial,
                        ActivoEmpresa = usuarioEmpresa.Count() > 0 ? usuarioEmpresa[0].Activo : false
                    });
                }
                usuarioEstructura.Add(new UsuarioEstructuraCorporativoDTO()
                {
                    IdUsuario = usuario.Id,
                    //IdRol = usuario.IdRol,
                    NombreUsuario = usuario.NombreUsuario,
                    EsActivo = usuario.Activo,
                    //Empresas = empresaslista
                });
            }
            return usuarioEstructura;
        }
        /// <summary>
        /// Para traer los usuarios con sus menus dependientes de los roles
        /// </summary>
        /// <param name="IdEmpresa">Identificador de la empresa</param>
        /// <returns></returns>
        [HttpGet("usuariosEstructuraXEmpresa/{IdEmpresa:int}")]
        public async Task<ActionResult<List<UsuarioRolMenuEstructuraDTO>>> ObtenUsuariosMenuCompletoXIdEmpresa(int IdEmpresa)
        {
            List<UsuarioRolMenuEstructuraDTO> UsuarioEstructuraDTOs = new List<UsuarioRolMenuEstructuraDTO>();
            var usuarios = await _UsuarioService.ObtenXIdEmpresa(IdEmpresa);
            foreach (var usuario in usuarios)
            {
                //var usuarioMenus = await _UsuarioMenuService.ObtenMenusXIdUsuario(usuario.Id);
                var usuarioMenuSecciones = await _UsuarioMenuSeccionService.ObtenXIdUsuario(usuario.Id);
                var usuarioMenuSeccionActividades = await _UsuarioMenuSeccionActividadService.ObtenXIdUsuario(usuario.Id);
                var Menus = await _CatalogoMenuService.ObtenTodos();
                //var role = await _RolService.ObtenXId(usuario.IdRol);
                List<UsuarioRolMenuEstructuraDTO> usuarioRolMenuEstructura = new List<UsuarioRolMenuEstructuraDTO>();
                foreach (var menu1 in Menus)
                {
                    var secciones = await _CatalogoSeccionService.ObtenTodosXIdMenu(menu1.Id);
                    //var menuUsuarioActivo = usuarioMenus.Where(z => z.IdMenu == menu1.Id).ToList();
                    List<UsuarioRolMenuEstructuraDTO> usuarioRolMenuSeccionEstructura = new List<UsuarioRolMenuEstructuraDTO>();
                    foreach (var seccion1 in secciones)
                    {
                        var actividades = await _CatalogoActividadService.ObtenTodosXIdSeccion(seccion1.Id);
                        var seccionUsuarioActivo = usuarioMenuSecciones.Where(z => z.IdSeccion == seccion1.Id).ToList();
                        List<UsuarioRolMenuEstructuraDTO> usuarioRolMenuSeccionActividadEstructura = new List<UsuarioRolMenuEstructuraDTO>();
                        foreach (var actividad1 in actividades)
                        {
                            var actividadUsuarioActivo = usuarioMenuSeccionActividades.Where(z => z.IdActividad == actividad1.Id).ToList();
                            usuarioRolMenuSeccionActividadEstructura.Add(new UsuarioRolMenuEstructuraDTO()
                            {
                                IdUsuario = usuario.Id,
                                IdEmpresa = IdEmpresa,
                                //IdRol = usuario.IdRol,
                                IdMenu = menu1.Id,
                                IdSeccion = seccion1.Id,
                                IdActividad = actividad1.Id,
                                TipoMenu = 1,
                                Descripcion = actividad1.Descripcion,
                                EsActivo = actividadUsuarioActivo.Count > 0 ? actividadUsuarioActivo.FirstOrDefault()!.EsActivo : false,
                                //Estructura = usuarioRolMenuEstructura
                            });
                        }
                        usuarioRolMenuSeccionEstructura.Add(new UsuarioRolMenuEstructuraDTO()
                        {
                            IdUsuario = usuario.Id,
                            IdEmpresa = IdEmpresa,
                            //IdRol = usuario.IdRol,
                            IdMenu = menu1.Id,
                            IdSeccion = seccion1.Id,
                            IdActividad = 0,
                            TipoMenu = 1,
                            Descripcion = seccion1.Descripcion,
                            EsActivo = seccionUsuarioActivo.Count > 0 ? seccionUsuarioActivo.FirstOrDefault()!.EsActivo : false,
                            Estructura = usuarioRolMenuSeccionActividadEstructura
                        });
                    }
                    usuarioRolMenuEstructura.Add(new UsuarioRolMenuEstructuraDTO()
                    {
                        IdUsuario = usuario.Id,
                        IdEmpresa = IdEmpresa,
                        //IdRol = usuario.IdRol,
                        IdMenu = menu1.Id,
                        IdSeccion = 0,
                        IdActividad = 0,
                        TipoMenu = 1,
                        Descripcion = menu1.Descripcion,
                        //EsActivo = menuUsuarioActivo.Count > 0 ? menuUsuarioActivo.FirstOrDefault()!.EsActivo : false,
                        Estructura = usuarioRolMenuSeccionEstructura
                    });
                }
                UsuarioEstructuraDTOs.Add(new UsuarioRolMenuEstructuraDTO()
                {
                    IdUsuario = usuario.Id,
                    IdEmpresa = IdEmpresa,
                    //IdRol = usuario.IdRol,
                    IdMenu = 0,
                    IdSeccion = 0,
                    IdActividad = 0,
                    TipoMenu = 1,
                    Descripcion = usuario.NombreUsuario,
                    EsActivo = usuario.Activo,
                    Estructura = usuarioRolMenuEstructura
                });
            }



            return UsuarioEstructuraDTOs;
        }
        #endregion
        /// <summary>
        /// Es para el alta de un usuario dentro de una BD operativa a travez de una autenticación rapida
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        private async Task<RespuestaAutenticacionDTO> zfConstruirToken(string Username)
        {
            //Claims: Conjunto de datos confiable acerca del usuario
            var zvUsuario = await _UserManager.FindByNameAsync(Username);
            var zvClaims = new List<Claim>()
            {
                //ejemplo
                //new Claim("nombre", "felipe")
                new Claim("username", zvUsuario!.UserName!),
                new Claim("email", zvUsuario.Email!)
                //new Claim(ClaimTypes.Email)
            };
            var zvClaimsDB = await _UserManager.GetClaimsAsync(zvUsuario!);
            zvClaims.AddRange(zvClaimsDB);
            var zvLlave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration["llavejwt"]!));
            var zvCreds = new SigningCredentials(zvLlave, SecurityAlgorithms.HmacSha256);
            var zvExpiracion = DateTime.UtcNow.AddSeconds(60);
            var zvToken = new JwtSecurityToken(issuer: null, audience: null, claims: zvClaims,
                expires: zvExpiracion, signingCredentials: zvCreds);
            return new RespuestaAutenticacionDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(zvToken),
                FechaExpiracion = zvExpiracion
            };
        }

        #region Validacion de Usuario

        [HttpGet("ValidarUsuario/{username}")]
        public async Task<ActionResult<RespuestaDTO>> ValidarUsuario(string username)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            if (username != null)
            {
                var Usuario = await _UsuarioService.ObtenXUsername(username);
                if (Usuario.Id > 0)
                {
                    respuesta.Estatus = true;
                    respuesta.Descripcion = "Usuario Existente";
                    return respuesta;
                }
                else
                {
                    return respuesta;

                }
            }
            else
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "Usuario no encontrado";
                return respuesta;

            }
            
        }

        [HttpGet("UsuarioPerteneceClienteXIdUsuario/{IdUsuario:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<UsuarioPertenecienteAClienteDTO> UsuarioPerteneceClienteXIdUsuario(int IdUsuario)
        {
            var usuarioCliente = await _UsuarioPertenecienteClienteService.ObtenXIdUsuario(IdUsuario);
            return usuarioCliente;
        }


        [HttpGet("ObtenerUsuario/{username}")]
        public async Task<ActionResult<UsuarioDTO>> ObtenerUsuario(string username)
        {
            if (username != null)
            {
                var Usuario = await _UsuarioService.ObtenXUsername(username);
                if (Usuario.Id > 0)
                {
                    return Usuario;
                }
                else
                {
                    return new UsuarioDTO(); 
                }
            }
            else
            {
                return new UsuarioDTO();

            }
        }

        [HttpGet("ObtenTodos")]
        public async Task<List<UsuarioDTO>> ObtenerTodos()
        {
            var Usuarios = await _UsuarioService.ObtenTodos();
            if (Usuarios.Count > 0)
            {
                return Usuarios;
            }
            else
            {
                return new List<UsuarioDTO>();
            }
        }


        [HttpGet("ObtenerUsuarioXidUsuario/{id:int}")]
        public async Task<ActionResult<UsuarioDTO>> ObtenerUsuarioxid(int id)
        {
            UsuarioDTO UsuarioBaseDTOs = new  UsuarioDTO();
            if (id != null)
            {
                var Usuario = await _UsuarioService.ObtenXIdUsuario(id);
                UsuarioBaseDTOs = Usuario;
                if (Usuario.Id > 0)
                {
                    return Usuario;
                }
                else
                {
                    return UsuarioBaseDTOs; 
                }
            }
            else
            {
                return UsuarioBaseDTOs;

            }

        }

        [HttpPost("ValidarUsuarioXEmpresa")]
        public async Task<ActionResult<RespuestaDTO>> ValidarUsuarioXEmpresa([FromBody] CredencialesUsuarioDTO zCredenciales)
        {
            RespuestaDTO resp = new RespuestaDTO();

            try
            {
                var zvResultado = await zvSignInManager.PasswordSignInAsync(zCredenciales.Email, zCredenciales.Password,
                isPersistent: false, lockoutOnFailure: false);
                if (zvResultado.Succeeded)
                {
                    var usuario = await _UsuarioService.ObtenXUsername(zCredenciales.Email);

                    if (usuario != null)
                    {
                        if (usuario.Activo)
                        {
                            var usuarioEmpresa = await _UsuarioEmpresaService.ObtenXIdUsuario(usuario.Id);

                            if (usuarioEmpresa.Count > 0)
                            {
                                foreach (var user in usuarioEmpresa)
                                {
                                    var Empresa = await _EmpresaService.ObtenXId(user.IdEmpresa);

                                    if (Empresa != null)
                                    {
                                        if (Empresa.Estatus.HasValue && Empresa.Estatus.Value)
                                        {
                                            resp.Estatus = true;
                                            resp.Descripcion = "Empresa Activa";
                                        }
                                        else
                                        {
                                            resp.Estatus = false;
                                            resp.Descripcion = "Empresa Inactiva";
                                        }
                                    }
                                    else
                                    {
                                        resp.Estatus = false;
                                        resp.Descripcion = "Empresa no encontrada";
                                    }
                                }
                            }
                        }
                        else
                        {
                            resp.Estatus = true;
                            resp.Descripcion = "Usuario Inactivo";
                            return resp;
                        }
                    }
                    else
                    {
                        resp.Estatus = false;
                        resp.Descripcion = "Usuario no econtrado";
                    }
                    return resp;
                }
                else
                {
                    resp.Estatus = false;
                    resp.Descripcion = "Usuario o Contraseña incorrectos";

                    return resp;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al ValidarUsuarioXEmpresa: {ex.Message}");
            }
        }
        

        [HttpGet("ValidarUsuarioGastos/{username}")]
        public async Task<ActionResult<RespuestaDTO>> ValidarUsuarioGastos(string username)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            if (username != null)
            {
                var Usuario = await _UsuarioService.ObtenXUsername(username);
                var UsuarioGastos = await _UsuarioGastosService.ObtenXIdUsuario(Usuario.Id);
                if (UsuarioGastos.id > 0)
                {
                    respuesta.Estatus = true;
                    respuesta.Descripcion = "Usuario Existente";
                    return respuesta;
                }
                else
                {
                    return respuesta;

                }
            }
            else
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "Usuario no encontrado";
                return respuesta;

            }

        }


        [HttpGet("ValidarUsuarioEmpresa/{username}/{idEmpresa}")]
        public async Task<ActionResult<RespuestaDTO>> ValidarUsuarioEmpresa(string username, int idEmpresa)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            if (username != null)
            {
                var Usuario = await _UsuarioService.ObtenXUsername(username);
                var UsuarioEmpresa = await _UsuarioEmpresaService.ObtenXIdUsuario(Usuario.Id);
                if (UsuarioEmpresa.Count > 0)
                {
                    if (UsuarioEmpresa.Where(z => z.IdEmpresa == idEmpresa).Count() > 0)
                    {
                        respuesta.Estatus = true;
                        respuesta.Descripcion = "Usuario Existente";
                        return respuesta;
                    }
                    else
                    {
                        respuesta.Estatus = false;
                        respuesta.Descripcion = "Usuario no encontrado";
                        return respuesta;

                    }

                }
                else
                {
                    respuesta.Estatus = false;
                    respuesta.Descripcion = "Usuario no encontrado";
                    return respuesta;

                }
            }
            else
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "Usuario no encontrado";
                return respuesta;

            }

        }


        #endregion

        #region RolesTeckio

        [HttpPost("CrearPrimerUsuario")]
        public async Task PrimerUsuarioCreacion()
        {
            string adminRole = "Administrador";
            if (!await zvRoleManager.RoleExistsAsync(adminRole))
            {
                await zvRoleManager.CreateAsync(new IdentityRole(adminRole));
            }

            string adminEmail = "cflores@procomi.com";
            string adminPassword = "Aa123456789!";

            var adminUser = await zvUserManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await zvUserManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await zvUserManager.AddToRoleAsync(adminUser, adminRole);
                }
                else
                {
                    // Maneja errores aquí si es necesario
                    throw new Exception("No se pudo crear el usuario administrador: " +
                                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }

        [HttpPost("AsignarRolesPorProyecto")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Administrador")]
        public async Task<ActionResult<RespuestaDTO>> AsignarRolUsuarioProyectoEmpresa([FromBody] AsignarRolAUsuarioEnEmpresaPorPoryectoDTO registro)
        {
            var authen = HttpContext.User;
            var usernameClaim = authen.Claims.FirstOrDefault()!.Value;
            var token = await zfConstruirToken(usernameClaim);
            RespuestaDTO respuesta = new RespuestaDTO();
            var usuariosNoIdentity = await _UsuarioService.ObtenXIdUsuario(registro.IdUsuario);
            if (usuariosNoIdentity.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No existe el usuario con ese nombre de usuario";
                return respuesta;
            }
            var zvUsuario = await zvUserManager.FindByIdAsync(usuariosNoIdentity.IdAspNetUser);
            if (zvUsuario == null)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No existe el usuario con ese nombre de usuario";
                return respuesta;
            }
            var rolNoIdentity = await _RolService.ObtenXId(registro.IdRol);
            if (rolNoIdentity.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No existe el rol";
                return respuesta;
            }
            var rolIdentity = await zvRoleManager.FindByIdAsync(rolNoIdentity.IdAspNetRole);
            if (rolIdentity == null)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No existe el rol";
                return respuesta;
            }
            var rolesEnEmpresa = await _RolEmpresaService.ObtenXIdRol(registro.IdRol);
            if (rolesEnEmpresa.Count <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No existe el rol";
                return respuesta;
            }
            var empresa = await _EmpresaService.ObtenXId(registro.IdEmpresa);
            if(empresa.Id <= 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No existe la empresa";
                return respuesta;
            }
            var usuarioEmpresa = await _UsuarioEmpresaService.ObtenXIdUsuarioXIdEmpresa(usuariosNoIdentity.Id, empresa.Id);
            if(usuarioEmpresa.Count() <= 0)
            {
                //Lo agregamos al rol nuevo
                await zvUserManager.AddToRoleAsync(zvUsuario, rolIdentity.Name!);
                //Obtenemos los permisos del rol nuevo
                var rolClaimsNuevosPrimerRol = await zvRoleManager.GetClaimsAsync(rolIdentity);
                //Agregamos los nuevos claims del nuevo usuario
                var resultado3ClaimsNuevosPrimerRol = await zvUserManager.AddClaimsAsync(zvUsuario, rolClaimsNuevosPrimerRol);
                var resultado = await _UsuarioEmpresaService.Activar(new UsuarioEmpresaDTO()
                {
                    IdUsuario = usuariosNoIdentity.Id,
                    IdEmpresa = empresa.Id,
                    Activo = true,
                    IdRol = rolNoIdentity.Id,
                });
                if(resultado.Estatus == true)
                {
                    var NuevaRelacion = new RolProyectoEmpresaUsuarioDTO();
                    NuevaRelacion.IdProyecto = registro.IdProyecto;
                    NuevaRelacion.IdEmpresa = registro.IdEmpresa;
                    NuevaRelacion.IdRol = registro.IdRol;
                    NuevaRelacion.IdUsuario = registro.IdUsuario;
                    NuevaRelacion.Estatus = true;
                    var NuevaRelacionCreada = await _RolProyectoEmpresaUsuarioService.Crear(NuevaRelacion);
                    if (NuevaRelacionCreada.Id != 0)
                    {
                        respuesta.Estatus = true;
                        respuesta.Descripcion = "Rol asignado correctamente";
                    }
                    else
                    {
                        respuesta.Estatus = false;
                        respuesta.Descripcion = "Ocurrio un error al asignar el rol al usuario";
                    }
                }
                else
                {
                    respuesta.Estatus = false;
                    respuesta.Descripcion = "Ocurrio un error al asignar el rol al usuario";
                }
                return respuesta;
            }
            else
            {
                var ObtenerRelaciones = await _RolProyectoEmpresaUsuarioService.ObtenXIdEmpresaYIdUsuario(registro.IdEmpresa, registro.IdUsuario);
                var relacionesFiltradas = ObtenerRelaciones.Where(z => z.IdProyecto == registro.IdProyecto).ToList();
                var existeRol = relacionesFiltradas.Where(z => z.IdRol == registro.IdRol).ToList();
                if(existeRol.Count > 0)
                {
                    existeRol[0].Estatus = !existeRol[0].Estatus;
                    var RolActivado = await _RolProyectoEmpresaUsuarioService.ActivarDesactivar(existeRol[0]);
                    if(RolActivado.Estatus == true)
                    {
                        var rolesUsuario = await _UserManager.GetRolesAsync(zvUsuario);
                        var rolAsignado = rolesUsuario.Where(z => z == rolIdentity.Name).ToList();
                        if(rolAsignado.Count <= 0)
                        {
                            await zvUserManager.AddToRoleAsync(zvUsuario, rolIdentity.Name!);
                            var claimsRol = await zvRoleManager.GetClaimsAsync(rolIdentity);
                            var claimsUsuario = await zvUserManager.GetClaimsAsync(zvUsuario!);
                            var claimsParaAsignar = new List<Claim>();
                            foreach(var claim in claimsRol)
                            {
                                var claimAsignadoAUsuario = claimsUsuario.Where(z => z == claim).ToList();
                                if(claimAsignadoAUsuario.Count <= 0)
                                {
                                    claimsParaAsignar.Add(claim);
                                }
                            }
                            var claimsAsignados = await zvUserManager.AddClaimsAsync(zvUsuario, claimsParaAsignar);
                        }
                    }
                }
                else
                {
                    var NuevaRelacion = new RolProyectoEmpresaUsuarioDTO();
                    NuevaRelacion.IdProyecto = registro.IdProyecto;
                    NuevaRelacion.IdEmpresa = registro.IdEmpresa;
                    NuevaRelacion.IdRol = registro.IdRol;
                    NuevaRelacion.IdUsuario = registro.IdUsuario;
                    NuevaRelacion.Estatus = true;
                    var NuevaRelacionCreada = await _RolProyectoEmpresaUsuarioService.Crear(NuevaRelacion);
                    var rolesUsuario = await _UserManager.GetRolesAsync(zvUsuario);
                    var rolAsignado = rolesUsuario.Where(z => z == rolIdentity.Name).ToList();
                    if (rolAsignado.Count <= 0)
                    {
                        await zvUserManager.AddToRoleAsync(zvUsuario, rolIdentity.Name!);
                        var claimsRol = await zvRoleManager.GetClaimsAsync(rolIdentity);
                        var claimsUsuario = await zvUserManager.GetClaimsAsync(zvUsuario!);
                        var claimsParaAsignar = new List<Claim>();
                        foreach (var claim in claimsRol)
                        {
                            var claimAsignadoAUsuario = claimsUsuario.Where(z => z == claim).ToList();
                            if (claimAsignadoAUsuario.Count <= 0)
                            {
                                claimsParaAsignar.Add(claim);
                            }
                        }
                        var claimsAsignados = await zvUserManager.AddClaimsAsync(zvUsuario, claimsParaAsignar);
                    }
                }
                return respuesta;
            }
        }

        [HttpPost("obtenerRelacionesRolProyectoUsuario")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Administrador")]
        public async Task<List<RolProyectoEmpresaUsuarioDTO>> obtenXIdUsuarioXIdProyectoXIdEmpresa(AsignarRolAUsuarioEnEmpresaPorPoryectoDTO registro)
        {
            var roles = await _RolService.ObtenTodos();
            var relaciones = await _RolProyectoEmpresaUsuarioService.ObtenXIdEmpresaYIdUsuario(registro.IdEmpresa, registro.IdUsuario);
            var relacionesFiltradas = relaciones.Where(z => z.IdProyecto == registro.IdProyecto).ToList();
            foreach (var rolXProyecto in relacionesFiltradas) {
                var rol = roles.FirstOrDefault(z => z.Id == rolXProyecto.IdRol);
                rolXProyecto.DescripcionRol = rol.Nombre;
            }
            return relacionesFiltradas;
        }
        #endregion
    }
}
