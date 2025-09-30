using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SistemaERP.BLL.Contrato;
using SistemaERP.BLL.Servicios.Contrato;
using SistemaERP.DTO;
using SistemaERP.Model;
using SistemaERP.Model.Gastos;
using SSO.DTO;
using SSO.Servicios.Contratos;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SistemaERP.API.Controllers.SSO
{
    /// <summary>
    /// Controlador utilizado en el logueo de los usuarios
    /// </summary>
    [Route("api/cuenta")]
    [ApiController]
    public class CuentaController : ControllerBase
    {
        private readonly IUsuarioService _UsuarioService;
        private readonly IUsuarioProveedorService _UsuarioProvedorService;
        private readonly IUsuarioGastosService _UsuarioGastosService;
        private readonly IUsuarioEmpresaService _UsuarioEmpresaService;
        private readonly IUsuarioPertenecienteAClienteService _UsuarioPertenecienteClienteService;
        private readonly IEmpresaService _EmpresaService;
        private readonly UserManager<IdentityUser> zvUserManager;
        private readonly RoleManager<IdentityRole> zvRoleManager;
        private readonly IConfiguration zvConfiguration;
        private readonly SignInManager<IdentityUser> zvSignInManager;
        private readonly IMapper _Mapper;
        private readonly IErpService _RpService;
        private readonly IUsuariosUltimaSeccionService _UsuariosUltimaSeccion;
        private readonly IUsuarioProyectoService _usuarioProyectoService;
        private readonly IRolProyectoEmpresaUsuarioService _rolProyectoEmpresaUsuarioService;
        private readonly IRolActividadService _rolActividadService;
        private readonly ICatalogoActividadService _catalogoActividadService;
        private readonly ICatalogoSeccionService _catalogoSeccionService;
        private readonly IRolSeccionService _rolSeccionService;
        public CuentaController(
            UserManager<IdentityUser> zUserManager
            , RoleManager<IdentityRole> zRoleManager
            , IConfiguration zConfiguration
            , SignInManager<IdentityUser> zSignInManager
            , IMapper zMapper
            , IUsuarioService UsuarioService
            , IUsuarioProveedorService UsuarioProvedorService
            , IUsuarioGastosService UsuarioGastosService
            , IUsuarioEmpresaService UsuarioEmpresaService
            , IUsuarioPertenecienteAClienteService UsuarioPertenecienteClienteService
            , IEmpresaService EmpresaService
            , IErpService rpService
            , IUsuariosUltimaSeccionService UsuariosUltimaSeccion
            , IUsuarioProyectoService usuarioProyectoService
            , IRolProyectoEmpresaUsuarioService rolProyectoEmpresaUsuarioService
            , IRolActividadService rolActividadService
            , ICatalogoActividadService catalogoActividadService
            , ICatalogoSeccionService catalogoSeccionService
            , IRolSeccionService rolSeccionService
            )
        {
            zvUserManager = zUserManager;
            zvRoleManager = zRoleManager;
            zvConfiguration = zConfiguration;
            zvSignInManager = zSignInManager;
            _Mapper = zMapper;
            _UsuarioService = UsuarioService;
            _UsuarioProvedorService = UsuarioProvedorService;
            _UsuarioGastosService = UsuarioGastosService;
            _UsuarioEmpresaService = UsuarioEmpresaService;
            _UsuarioPertenecienteClienteService = UsuarioPertenecienteClienteService;
            _EmpresaService = EmpresaService;
            _RpService = rpService;
            _UsuariosUltimaSeccion = UsuariosUltimaSeccion;
            _usuarioProyectoService = usuarioProyectoService;
            _rolProyectoEmpresaUsuarioService = rolProyectoEmpresaUsuarioService;
            _rolActividadService = rolActividadService;
            _catalogoActividadService = catalogoActividadService;
            _catalogoSeccionService = catalogoSeccionService;
            _rolSeccionService = rolSeccionService;
        }

        #region JWT
        /// <summary>
        /// Método para construir el token que nos dara acceso a las diferentes partes del software
        /// </summary>
        /// <param name="zCredenciales">Un objeto del tipo <see cref="CredencialesUsuarioDTO"/></param>
        /// <returns>Un objeto del tipo <see cref="RespuestaAutenticacionDTO"/></returns>
        private async Task<RespuestaAutenticacionDTO>   zfConstruirToken(CredencialesUsuarioDTO zCredenciales)
        {
            //Claims: Conjunto de datos confiable acerca del usuario
            var zvUsuario = await zvUserManager.FindByNameAsync(zCredenciales.Email);
            var zvClaims = new List<Claim>()
            {
                //ejemplo
                //new Claim("nombre", "felipe")
                new Claim("username", zvUsuario!.UserName!),
                new Claim("email", zvUsuario.Email!),
                new Claim("guid", zvUsuario.Id)
            };
            var usuario = await _UsuarioService.ObtenXGuidUsuario(zvUsuario.Id);
            var usuarioNoIdentity = await _UsuarioService.ObtenXUsername(zCredenciales.Email);
            zvClaims.Add(new Claim("activo", usuarioNoIdentity.Activo == true ? "1" : "0"));
            zvClaims.Add(new Claim("idUsuario", usuarioNoIdentity.Id.ToString()));
            var claims = zvUserManager.GetClaimsAsync(zvUsuario).Result;
            var claimAdministrador = claims.FirstOrDefault(z => z.Value == "Administrador");
            if(claimAdministrador != null)
            {
                zvClaims.Add(claimAdministrador);
            }
            var claimAdministradorRoles = claims.FirstOrDefault(z => z.Value == "AdministradorRoles");
            if (claimAdministradorRoles != null)
            {
                zvClaims.Add(claimAdministradorRoles);
            }
            var rolesActivos = new List<RolProyectoEmpresaUsuarioDTO>();
            var UsuarioUltimaSecion = await _UsuariosUltimaSeccion.ObtenerXIdUsuario(usuario.Id);
            if (UsuarioUltimaSecion.Id <= 0) {
                var usuarioXPoryectoXEmpresa = await _usuarioProyectoService.ObtenXIdUsuario(usuario.Id);
                var registroDefault = usuarioXPoryectoXEmpresa.FirstOrDefault();
                if (registroDefault != null) {
                    var rolesXUsuario = await _rolProyectoEmpresaUsuarioService.ObtenXIdEmpresaYIdUsuario(registroDefault.IdEmpresa, registroDefault.IdUsuario);
                    rolesActivos = rolesXUsuario.Where(z => z.Estatus == true && z.IdProyecto == registroDefault.IdProyecto).ToList();
                }
            }
            else
            {
                var rolesXUsuario = await _rolProyectoEmpresaUsuarioService.ObtenXIdEmpresaYIdUsuario(UsuarioUltimaSecion.IdEmpresa, UsuarioUltimaSecion.IdUsuario);
                rolesActivos = rolesXUsuario.Where(z => z.Estatus == true && z.IdProyecto == UsuarioUltimaSecion.IdProyecto).ToList();
            }
            var catalogoActividades = await _catalogoActividadService.ObtenTodos();
            var catalogoSecciones = await _catalogoSeccionService.ObtenTodos();

            foreach (var rol in rolesActivos) { 
                var rolesXActividad = await _rolActividadService.ObtenTodosXIdRol(rol.IdRol);
                foreach (var actividad in rolesXActividad) {
                    var catActividad = catalogoActividades.FirstOrDefault(z => z.Id == actividad.IdActividad);
                    var Claim = new Claim(catActividad.DescripcionInterna, catActividad.CodigoActividad);
                    var existeclaim = zvClaims.Where(z => z.Value == Claim.Value);
                    if(existeclaim.Count() != 0)
                    {
                        continue;
                    }
                    zvClaims.Add(new Claim(catActividad.DescripcionInterna, catActividad.CodigoActividad));
                }
                var seccionXRol = await _rolSeccionService.ObtenTodosXIdRol(rol.IdRol);
                foreach (var seccion in seccionXRol)
                {
                    var catSeccion = catalogoSecciones.FirstOrDefault(z => z.Id == seccion.IdSeccion);
                    var Claim = new Claim(catSeccion.DescripcionInterna, catSeccion.CodigoSeccion);
                    var existeclaim = zvClaims.Where(z => z.Value == Claim.Value);
                    if (existeclaim.Count() != 0)
                    {
                        continue;
                    }
                    zvClaims.Add(new Claim(catSeccion.DescripcionInterna, catSeccion.CodigoSeccion));
                }
            }
            var zvLlave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(zvConfiguration["llavejwt"]!));
            var zvCreds = new SigningCredentials(zvLlave, SecurityAlgorithms.HmacSha256);
            var zvExpiracion = DateTime.UtcNow.AddHours(5);
            var zvToken = new JwtSecurityToken(issuer: null, audience: null, claims: zvClaims,
                expires: zvExpiracion, signingCredentials: zvCreds);
            return new RespuestaAutenticacionDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(zvToken),
                FechaExpiracion = zvExpiracion
            };
        }

        #endregion

        #region Login
        /// <summary>
        /// Método para loguearse en el software
        /// </summary>
        /// <param name="zCredenciales">Un objeto del tipo <see cref="CredencialesUsuarioDTO"/></param>
        /// <returns>Un código de <see cref="BadRequestResult"/> o un objeto del tipo <see cref="RespuestaAutenticacionDTO"/></returns>
        [HttpPost("login")]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> zfLogin([FromBody] CredencialesUsuarioDTO zCredenciales)
        {
            // Buscar usuario
            var user = await zvUserManager.FindByNameAsync(zCredenciales.Email);
            if (user == null)
            {
                return new RespuestaAutenticacionDTO
                {
                    FechaExpiracion = DateTime.Today,
                    Token = "El usuario ingresado es incorrecto."
                };
            }

            // Validar contraseña SIN emitir cookie
            var pwdCheck = await zvSignInManager.CheckPasswordSignInAsync(
                user,
                zCredenciales.Password,
                lockoutOnFailure: false
            );

            if (!pwdCheck.Succeeded)
            {
                return new RespuestaAutenticacionDTO
                {
                    FechaExpiracion = DateTime.Today,
                    Token = "La contraseña ingresada es incorrecta."
                };
            }

            // Verificar estado del usuario en tu dominio
            var usuario = await _UsuarioService.ObtenXUsername(zCredenciales.Email);
            if (!usuario.Activo)
            {
                return new RespuestaAutenticacionDTO
                {
                    FechaExpiracion = DateTime.Today,
                    Token = "UsuarioNoActivo"
                };
            }

            // (Opcional) Si quedaron cookies antiguas de Identity en el navegador,
            // puedes forzar su eliminación en la respuesta:
            // Response.Cookies.Delete(".AspNetCore.Identity.Application");

            // Emitir SOLO el JWT
            return await zfConstruirToken(zCredenciales);
        }

        /// <summary>
        /// En el front se ocupa que se carguen las empresas una vez logueado pero el método de las empresas tiene que tener una petición antes
        /// Para eso se ocupa este endpoint, para validar que este logueado y que sea previo a la petición de empresas pertenecientes
        /// </summary>
        /// <returns></returns>
        [HttpPost("RespuestaFront")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> RespuestaFront()
        {
            //var authen = HttpContext.User;
            return NoContent();
        }

        [HttpGet("actualizarClaims")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<ActionResult<RespuestaAutenticacionDTO>> ActualizarClaims()
        {
            var zCredenciales = new CredencialesUsuarioDTO();
            var authen = HttpContext.User;
            var usernameClaim = authen.Claims.FirstOrDefault()!.Value;
            if (usernameClaim == null) { 
                RespuestaAutenticacionDTO resp = new RespuestaAutenticacionDTO();
                resp.FechaExpiracion = DateTime.Today;
                resp.Token = "NoToken";
                return resp;
            }
            zCredenciales.Email = usernameClaim;
            return await zfConstruirToken(zCredenciales);
        }
        #endregion
    }
}