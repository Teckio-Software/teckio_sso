using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SistemaERP.BLL.Contrato;
using SistemaERP.BLL.ProcesoSSO;
using SistemaERP.DTO;
using SSO.DTO;
using System.Text.RegularExpressions;

namespace SistemaERP.API.Controllers.SSO
{
    /// <summary>
    /// Para la relacion de los usuarios con el sistema de gastos
    /// </summary>
    [Route("api/usuarioGastos")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsuarioGastosController : ControllerBase
    {
        private readonly UserManager<IdentityUser> zvUserManager;
        private readonly IEmpresaService _empresaServicio;
        private readonly IUsuarioService _UsuarioService;
        private readonly IUsuarioEmpresaService _UsuarioEmpresaService;
        private readonly ProcesoRelacionUsuarioEmpresa _ProcesoUsuarioEmpresa;
        private readonly ProcesoUsuarioCreacion _ProcesoUsuarioCreacion;
        private readonly IUsuarioCorporativoService _UsuarioCorporativoService;
        private readonly IUsuarioGastosService _usuarioGastosService;

        public UsuarioGastosController(
            IEmpresaService empresaServicio
            , IUsuarioService usuarioService
            , UserManager<IdentityUser> userManager
            , IUsuarioEmpresaService usuarioEmpresaService
            , ProcesoRelacionUsuarioEmpresa ProcesoUsuarioEmpresa
            , ProcesoUsuarioCreacion ProcesoUsuarioCreacion
            , IUsuarioCorporativoService UsuarioCorporativoService
            , IUsuarioGastosService usuarioGastosService
            )
        {
            zvUserManager = userManager;
            _empresaServicio = empresaServicio;
            _UsuarioService = usuarioService;
            _UsuarioEmpresaService = usuarioEmpresaService;
            _ProcesoUsuarioEmpresa = ProcesoUsuarioEmpresa;
            _ProcesoUsuarioCreacion = ProcesoUsuarioCreacion;
            _UsuarioCorporativoService = UsuarioCorporativoService;
            _usuarioGastosService = usuarioGastosService;
        }
        /// <summary>
        /// Para obtener la relación de los usuarios-gastos por el id del usuario
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpGet("ObtenerUsuarioGastosxidUsuario/{idUsuario}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<UsuarioGastosDTO> ObtenerUsuarioGastosxId(int idUsuario)
        {
            UsuarioGastosDTO usuarioGastosDTO = new UsuarioGastosDTO();
            var authen = HttpContext.User;
            var zvUsernameClaim = authen.Claims.FirstOrDefault()!.Value;

            var usuario = await _usuarioGastosService.ObtenXIdUsuario(idUsuario);
            usuarioGastosDTO = usuario;

            return usuarioGastosDTO;
        }
        /// <summary>
        /// Crea la relación del usuario-gasto (tabla)
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [HttpPost("creaRelacionUsuarioEmpresaGastos")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "VisorPermiso")]
        public async Task<ActionResult<RespuestaDTO>> CrearRelacionEmpresas(UsuarioGastosEnVariasEmpresasCreacionDTO parametro)
        {
            RespuestaDTO resp = new RespuestaDTO();
            var authen = HttpContext.User;
            var zvUsernameClaim = authen.Claims.FirstOrDefault()!.Value;
            bool ProcesoUsuarioEmpresa = false;
            var usuario = await _UsuarioService.ObtenXUsername(zvUsernameClaim);
            if (usuario.Id > 0)
            {
                var usuarioCorporativo = await _UsuarioCorporativoService.ObtenXIdUsuario(usuario.Id);
                ProcesoUsuarioEmpresa = await _ProcesoUsuarioCreacion.CreaUsuarioGastos(parametro, usuarioCorporativo.IdCorporativo);
            }
            resp.Estatus = ProcesoUsuarioEmpresa;
            if (ProcesoUsuarioEmpresa)
            {
                resp.Descripcion = "Usuario gastos creado";
            }
            else
            {
                resp.Descripcion = "Algo salió mal";
            }
            return resp;
        }
        /// <summary>
        /// Actualiza la información complementaria del usuario
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [HttpPut("ActualizaUsuarioGastos")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "VisorPermiso")]
        public async Task<ActionResult<RespuestaDTO>> ActualizaUsuarioGastos(UsuarioGastosConsultaDTO parametro)
        {
            RespuestaDTO resp = new RespuestaDTO();
            resp.Estatus = false;
            var authen = HttpContext.User;
            var zvUsernameClaim = authen.Claims.FirstOrDefault()!.Value;
            if (
                parametro.Id <= 0
                || string.IsNullOrEmpty(parametro.Correo)
                || string.IsNullOrEmpty(parametro.NombreCompleto)
                || string.IsNullOrEmpty(parametro.NombreUsuario)
                || string.IsNullOrEmpty(parametro.numeroEmpleado)
                )
            {
                resp.Estatus = false;
                resp.Descripcion = "Capture toda la información";
                return resp;
            }
            var usuario = await _UsuarioService.ObtenXUsername(parametro.NombreUsuario);
            if (usuario.Id <= 0)
            {
                resp.Descripcion = "No existe un usuario";
                return resp;
            }

            var respuesta2 = Regex.IsMatch(parametro.Correo,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            if (!respuesta2)
            {
                resp.Estatus = false;
                resp.Descripcion = "Capture un correo electrónico válido";
                return resp;
            }
            
            usuario.Correo = parametro.Correo;
            usuario.NombreUsuario = parametro.NombreUsuario;
            usuario.NombreCompleto = parametro.NombreCompleto;
            usuario.Apaterno = parametro.Apaterno;
            usuario.Amaterno = parametro.Amaterno;
            var resultado = await _UsuarioService.Editar(usuario);
            var usuarioIdentity = await zvUserManager.FindByIdAsync(usuario.IdAspNetUser);
            usuarioIdentity.NormalizedEmail = parametro.Correo;
            usuarioIdentity.Email = parametro.Correo;
            usuarioIdentity.UserName = parametro.NombreUsuario;
            usuarioIdentity.NormalizedUserName = parametro.NombreUsuario;
            await zvUserManager.UpdateAsync(usuarioIdentity);

            var usuarioGastos = await _usuarioGastosService.ObtenXIdUsuario(usuario.Id);
            if (usuarioGastos.id > 0)
            {
                resp.Estatus = await _usuarioGastosService.Editar(new UsuarioGastosDTO()
                {
                    id = usuarioGastos.id,
                    numeroEmpleado = parametro.numeroEmpleado,
                    idUsuario = usuario.Id,
                    fecha_alta = parametro.fecha_alta,
                    fecha_baja = parametro.fecha_baja,
                    estatus = parametro.estatus,
                });
                    if (resp.Estatus)
                    {
                        resp.Descripcion = "Usuario Gastos editado";
                        return resp;
                    }
                    else
                    {
                        resp.Descripcion = "Algo salió mal en la edición del usuario gastos";
                        return resp;
                    }
            }
            return resp;

        }
        /// <summary>
        /// Obtiene la información de la tabla de usuarios-gastos por el usuario
        /// </summary>
        /// <param name="IdUsuario"></param>
        /// <returns></returns>
        [HttpGet("ObtenInfoUsuarioGastos/{IdUsuario:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UsuarioGastosConsultaDTO>> ObtenInfoUsuarioGastos(int IdUsuario)
        {
            UsuarioGastosConsultaDTO usuarioGastosConsultaDTO = new UsuarioGastosConsultaDTO();
            var authen = HttpContext.User;
            var claims = authen.Claims.ToList();
            var zvUsernameClaim = authen.Claims.FirstOrDefault()!.Value;
            var usuario = await _UsuarioService.ObtenXIdUsuario(IdUsuario);
            if (usuario.Id <= 0)
            {
                return usuarioGastosConsultaDTO;
            }
            var usuarioGastos = await _usuarioGastosService.ObtenXIdUsuario(IdUsuario);
            if (usuarioGastos.id <= 0)
            {
                return usuarioGastosConsultaDTO;
            }
            usuarioGastosConsultaDTO.Id = usuario.Id;
            usuarioGastosConsultaDTO.IdAspNetUser = usuario.IdAspNetUser;
            usuarioGastosConsultaDTO.NombreUsuario = usuario.NombreUsuario;
            usuarioGastosConsultaDTO.NombreCompleto = usuario.NombreCompleto;
            usuarioGastosConsultaDTO.Apaterno = usuario.Apaterno;
            usuarioGastosConsultaDTO.Amaterno = usuario.Amaterno;
            usuarioGastosConsultaDTO.Activo = usuario.Activo;
            usuarioGastosConsultaDTO.Correo = usuario.Correo;
            usuarioGastosConsultaDTO.numeroEmpleado = usuarioGastos.numeroEmpleado;
            return usuarioGastosConsultaDTO;
        }
        [HttpGet("ObtenUsuarioGastosxId/{IdUsuario:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UsuarioGastosDTO>> ObtenUsuarioGastosId(int IdUsuario)
        {
            UsuarioGastosDTO usuarioGastosConsultaDTO = new UsuarioGastosDTO();
            var authen = HttpContext.User;
            var claims = authen.Claims.ToList();
            var zvUsernameClaim = authen.Claims.FirstOrDefault()!.Value;
            var usuario = await _UsuarioService.ObtenXIdUsuario(IdUsuario);
            if (usuario.Id <= 0)
            {
                return usuarioGastosConsultaDTO;
            }
            var usuarioGastos = await _usuarioGastosService.ObtenXId(IdUsuario);
            if (usuarioGastos.id <= 0)
            {
                return usuarioGastosConsultaDTO;
            }
           
            return usuarioGastos;
        }
    }
}
